using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImgEnc = System.Drawing.Imaging.Encoder;

namespace EbayPhotoMatchTest
{
    public partial class Form1 : Form
    {
        private const string CLIENT_ID = "Client ID";
        private const string CLIENT_SECRET = "Secret ID";

        // === Model for one search result ===
        private class Hit
        {
            public string ItemId { get; set; }
            public string Title { get; set; }
            public string Price { get; set; }
            public string WebUrl { get; set; }
            public string ImageUrl { get; set; }
            public override string ToString() => string.IsNullOrWhiteSpace(Price) ? Title : $"{Title} — ${Price}";
        }

        // one list per column
        private readonly List<Hit>[] _hits = { new(), new(), new(), new(), new() };

        // token cache
        private string _cachedToken;
        private DateTime _tokenExpiryUtc = DateTime.MinValue;

        public Form1()
        {
            InitializeComponent();

            // wire button


            // wire all 5 result lists
            HookList(lstImg1, 0);
            HookList(lstImg2, 1);
            HookList(lstImg3, 2);
            HookList(lstImg4, 3);
            HookList(lstImg5, 4);

            // make the log box useful
            textBox1.Multiline = true;
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.WordWrap = false;
        }

        private void HookList(ListBox lst, int idx)
        {
            lst.SelectedIndexChanged += (s, e) => ShowPreview(idx);
            lst.DoubleClick += (s, e) => CopyOpen(idx);
        }

        // ===========================
        // Main action: select images
        // ===========================
        private async void BtnTest_Click(object sender, EventArgs e)

        {
            try
            {
                using var dlg = new OpenFileDialog
                {
                    Filter = "Images|*.jpg;*.jpeg;*.png",
                    Multiselect = true
                };
                if (dlg.ShowDialog() != DialogResult.OK) return;

                var files = dlg.FileNames.Take(5).ToArray(); // up to 5
                textBox1.Text = $"Selected {files.Length} image(s)\r\n";
                for (int i = 0; i < 5; i++)
                    SetImage(GetSrcPic(i), null);

                // show the newly selected source photos
                for (int i = 0; i < files.Length; i++)
                    SetImage(GetSrcPic(i), LoadImageUnlocked(files[i]));
                var token = await GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    textBox1.AppendText("OAuth token error.\r\n");
                    return;
                }

                // clear old UI
                for (int i = 0; i < 5; i++)
                {
                    _hits[i].Clear();
                    GetList(i).DataSource = null;
                    GetPic(i).Image = null;
                    GetTxt(i).Clear();

                }

                // run at most 2 searches in parallel to avoid timeouts
                var sem = new SemaphoreSlim(2);
                var tasks = new List<Task>();
                for (int col = 0; col < files.Length; col++)
                {
                    int ci = col;
                    tasks.Add(Task.Run(async () =>
                    {
                        await sem.WaitAsync();
                        try { await SearchOneImage(token, files[ci], ci); }
                        finally { sem.Release(); }
                    }));
                }
                await Task.WhenAll(tasks);

                textBox1.AppendText("Done.\r\n");
            }
            catch (Exception ex)
            {
                textBox1.Text = ex.ToString();
            }
        }

        // Search for one image and bind into column 'ci'
        private async Task SearchOneImage(string token, string path, int ci)
        {
            try
            {
                var bytes = ShrinkToJpeg(path, 1200, 85L);
                var base64 = Convert.ToBase64String(bytes);

                using var http = NewHttp(token);
                var body = new StringContent($"{{\"image\":\"{base64}\"}}", Encoding.UTF8, "application/json");
                var resp = await http.PostAsync("https://api.ebay.com/buy/browse/v1/item_summary/search_by_image?limit=5", body);
                var json = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    AppendLog($"[Image {ci + 1}] API ERROR {resp.StatusCode}\r\n{json}\r\n");
                    return;
                }

                var list = new List<Hit>();
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("itemSummaries", out var items))
                {
                    foreach (var it in items.EnumerateArray())
                    {
                        string price = "";
                        if (it.TryGetProperty("price", out var p) && p.ValueKind == JsonValueKind.Object &&
                            p.TryGetProperty("value", out var pv))
                            price = pv.GetString() ?? "";

                        var hit = new Hit
                        {
                            ItemId = it.GetProperty("itemId").GetString(),
                            Title = it.GetProperty("title").GetString(),
                            Price = price,
                            WebUrl = it.TryGetProperty("itemWebUrl", out var wu) ? wu.GetString() : "",
                            ImageUrl = it.TryGetProperty("image", out var img) && img.TryGetProperty("imageUrl", out var iu)
                                       ? iu.GetString() : ""
                        };
                        list.Add(hit);
                    }
                }

                // bind to UI
                _hits[ci] = list;
                Invoke(new Action(() =>
                {
                    var lst = GetList(ci);
                    lst.DataSource = list.ToList(); // copy
                    if (list.Count > 0)
                    {
                        lst.SelectedIndex = 0;
                        ShowPreview(ci);
                    }
                    else
                    {
                        GetTxt(ci).Text = "(No results)";
                    }
                }));
            }
            catch (Exception ex)
            {
                AppendLog($"[Image {ci + 1}] {ex}\r\n");
            }
        }

        // When a result is selected in column ci
        private async void ShowPreview(int ci)
        {
            var lst = GetList(ci);
            if (lst.SelectedItem is not Hit h) return;

            // load image
            var pic = GetPic(ci);
            pic.Image = null;
            if (!string.IsNullOrEmpty(h.ImageUrl))
            {
                try { pic.LoadAsync(h.ImageUrl); } catch { /* ignore */ }
            }

            // ensure description (lazy fetch details once)
            string desc = "";
            var txt = GetTxt(ci);
            // if text already has a Description line, keep it, otherwise fetch details
            if (!txt.Text.StartsWith("Title:", StringComparison.OrdinalIgnoreCase))
            {
                desc = await GetDescription(h.ItemId);
            }
            else
            {
                // parse existing to avoid extra calls while user is scrolling results
                var lines = txt.Text.Split(new[] { "\r\n" }, StringSplitOptions.None);
                var dln = lines.FirstOrDefault(l => l.StartsWith("Description:", StringComparison.OrdinalIgnoreCase));
                if (dln != null) desc = dln.Substring("Description:".Length).Trim();
                if (string.IsNullOrWhiteSpace(desc)) desc = await GetDescription(h.ItemId);
            }

            txt.Text = $"Title: {h.Title}\r\nPrice: {h.Price}\r\nDescription: {desc}\r\nURL: {h.WebUrl}";
        }

        // Double-click → copy & open URL
        private void CopyOpen(int ci)
        {
            var lst = GetList(ci);
            if (lst.SelectedItem is not Hit h || string.IsNullOrWhiteSpace(h.WebUrl)) return;

            Clipboard.SetText(h.WebUrl);
            AppendLog($"URL copied (Image {ci + 1}).\r\n");
        }


        // ===== Details call for description (lazy) =====
        private readonly Dictionary<string, string> _descCache = new();

        private async Task<string> GetDescription(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return "";
            if (_descCache.TryGetValue(itemId, out var cached)) return cached;

            try
            {
                var token = await GetToken();
                if (string.IsNullOrEmpty(token)) return "";

                using var http = NewHttp(token);
                var resp = await http.GetAsync($"https://api.ebay.com/buy/browse/v1/item/{itemId}");
                var json = await resp.Content.ReadAsStringAsync();
                if (!resp.IsSuccessStatusCode) return "";

                using var doc = JsonDocument.Parse(json);
                string desc = "";
                if (doc.RootElement.TryGetProperty("shortDescription", out var sd) &&
                    sd.ValueKind == JsonValueKind.String)
                    desc = sd.GetString();

                // sometimes description is empty; try itemSpecifics as a fallback
                if (string.IsNullOrWhiteSpace(desc) &&
                    doc.RootElement.TryGetProperty("itemSpecifics", out var specs) &&
                    specs.ValueKind == JsonValueKind.Array)
                {
                    var sb = new StringBuilder();
                    foreach (var s in specs.EnumerateArray())
                    {
                        if (s.TryGetProperty("name", out var n) && s.TryGetProperty("value", out var v) &&
                            v.ValueKind == JsonValueKind.Array)
                        {
                            sb.Append(n.GetString()).Append(": ").Append(string.Join(", ", v.EnumerateArray().Select(x => x.GetString()))).Append("; ");
                        }
                    }
                    desc = sb.ToString();
                }

                desc ??= "";
                _descCache[itemId] = desc;
                return desc;
            }
            catch { return ""; }
        }

        // ===== Token + HTTP =====
        private async Task<string> GetToken()
        {
            if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpiryUtc)
                return _cachedToken;

            using var httpAuth = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            var creds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{CLIENT_ID}:{CLIENT_SECRET}"));
            httpAuth.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", creds);
            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", "https://api.ebay.com/oauth/api_scope")
            });
            var tokRes = await httpAuth.PostAsync("https://api.ebay.com/identity/v1/oauth2/token", form);
            var tokJson = await tokRes.Content.ReadAsStringAsync();
            if (!tokRes.IsSuccessStatusCode) return null;

            var root = JsonDocument.Parse(tokJson).RootElement;
            _cachedToken = root.GetProperty("access_token").GetString();
            var secs = root.TryGetProperty("expires_in", out var ex) ? ex.GetInt32() : 5400;
            _tokenExpiryUtc = DateTime.UtcNow.AddSeconds(secs - 120); // small buffer
            return _cachedToken;
        }

        private static HttpClient NewHttp(string token)
        {
            var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            http.DefaultRequestHeaders.Add("X-EBAY-C-MARKETPLACE-ID", "EBAY_US");
            http.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            return http;
        }

        // ===== UI helpers =====
        private ListBox GetList(int i) => i switch
        {
            0 => lstImg1,
            1 => lstImg2,
            2 => lstImg3,
            3 => lstImg4,
            _ => lstImg5
        };
        private PictureBox GetPic(int i) => i switch
        {
            0 => picImg1,
            1 => picImg2,
            2 => picImg3,
            3 => picImg4,
            _ => picImg5
        };
        private TextBox GetTxt(int i) => i switch
        {
            0 => txtImg1,
            1 => txtImg2,
            2 => txtImg3,
            3 => txtImg4,
            _ => txtImg5
        };
        // === Source (selected) photos ===
        private PictureBox GetSrcPic(int i) => i switch
        {
            0 => srcPic1,
            1 => srcPic2,
            2 => srcPic3,
            3 => srcPic4,
            _ => srcPic5
        };

        // Load without locking the file (clone the bitmap)
        private static Image LoadImageUnlocked(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var tmp = Image.FromStream(fs);
            return new Bitmap(tmp);
        }

        // Assign and dispose old image to avoid leaks
        private static void SetImage(PictureBox pb, Image img)
        {
            var old = pb.Image;
            pb.Image = img;
            old?.Dispose();
        }

        private void AppendLog(string s) => Invoke(new Action(() => textBox1.AppendText(s)));

        // shrink to JPEG for faster upload
        private static byte[] ShrinkToJpeg(string path, int maxWidth, long quality)
        {
            using var src = new Bitmap(path);
            int w = src.Width, h = src.Height;
            if (w > maxWidth) { h = h * maxWidth / w; w = maxWidth; }
            using var resized = new Bitmap(src, w, h);
            using var ms = new MemoryStream();
            var enc = ImageCodecInfo.GetImageEncoders().First(e => e.MimeType == "image/jpeg");
            var p = new EncoderParameters(1);
            p.Param[0] = new EncoderParameter(ImgEnc.Quality, quality);
            resized.Save(ms, enc, p);
            return ms.ToArray();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
