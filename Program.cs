using System;
using System.Windows.Forms;

namespace EbayPhotoMatchTest
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());  // <-- starts our form
        }
    }
}
