namespace EbayPhotoMatchTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnTest = new Button();
            textBox1 = new TextBox();
            tlpResults = new TableLayoutPanel();
            lstImg5 = new ListBox();
            txtImg5 = new TextBox();
            txtImg4 = new TextBox();
            txtImg3 = new TextBox();
            txtImg2 = new TextBox();
            picImg5 = new PictureBox();
            picImg4 = new PictureBox();
            picImg3 = new PictureBox();
            picImg2 = new PictureBox();
            lstImg4 = new ListBox();
            lstImg3 = new ListBox();
            lstImg2 = new ListBox();
            lstImg1 = new ListBox();
            picImg1 = new PictureBox();
            txtImg1 = new TextBox();
            tlpResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picImg5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picImg4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picImg3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picImg2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picImg1).BeginInit();
            SuspendLayout();
            // 
            // btnTest
            // 
            btnTest.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnTest.Location = new Point(12, 33);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(133, 60);
            btnTest.TabIndex = 0;
            btnTest.Text = "Test Photo Match";
            btnTest.UseVisualStyleBackColor = true;
            btnTest.Click += BtnTest_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(179, 33);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new Size(947, 60);
            textBox1.TabIndex = 1;
            // 
            // tlpResults
            // 
            tlpResults.ColumnCount = 5;
            tlpResults.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpResults.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpResults.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 213F));
            tlpResults.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tlpResults.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tlpResults.Controls.Add(lstImg5, 4, 0);
            tlpResults.Controls.Add(txtImg5, 4, 2);
            tlpResults.Controls.Add(txtImg4, 3, 2);
            tlpResults.Controls.Add(txtImg3, 2, 2);
            tlpResults.Controls.Add(txtImg2, 1, 2);
            tlpResults.Controls.Add(picImg5, 4, 1);
            tlpResults.Controls.Add(picImg4, 3, 1);
            tlpResults.Controls.Add(picImg3, 2, 1);
            tlpResults.Controls.Add(picImg2, 1, 1);
            tlpResults.Controls.Add(lstImg4, 3, 0);
            tlpResults.Controls.Add(lstImg3, 2, 0);
            tlpResults.Controls.Add(lstImg2, 1, 0);
            tlpResults.Controls.Add(lstImg1, 0, 0);
            tlpResults.Controls.Add(picImg1, 0, 1);
            tlpResults.Controls.Add(txtImg1, 0, 2);
            tlpResults.Location = new Point(56, 110);
            tlpResults.Name = "tlpResults";
            tlpResults.RowCount = 3;
            tlpResults.RowStyles.Add(new RowStyle(SizeType.Percent, 40.766552F));
            tlpResults.RowStyles.Add(new RowStyle(SizeType.Percent, 59.233448F));
            tlpResults.RowStyles.Add(new RowStyle(SizeType.Absolute, 262F));
            tlpResults.Size = new Size(1070, 592);
            tlpResults.TabIndex = 2;
            // 
            // lstImg5
            // 
            lstImg5.Dock = DockStyle.Fill;
            lstImg5.FormattingEnabled = true;
            lstImg5.ItemHeight = 15;
            lstImg5.Location = new Point(852, 3);
            lstImg5.Name = "lstImg5";
            lstImg5.Size = new Size(215, 128);
            lstImg5.TabIndex = 15;
            // 
            // txtImg5
            // 
            txtImg5.Dock = DockStyle.Fill;
            txtImg5.Location = new Point(852, 332);
            txtImg5.Multiline = true;
            txtImg5.Name = "txtImg5";
            txtImg5.ReadOnly = true;
            txtImg5.ScrollBars = ScrollBars.Vertical;
            txtImg5.Size = new Size(215, 257);
            txtImg5.TabIndex = 14;
            // 
            // txtImg4
            // 
            txtImg4.Dock = DockStyle.Fill;
            txtImg4.Location = new Point(652, 332);
            txtImg4.Multiline = true;
            txtImg4.Name = "txtImg4";
            txtImg4.ReadOnly = true;
            txtImg4.ScrollBars = ScrollBars.Vertical;
            txtImg4.Size = new Size(194, 257);
            txtImg4.TabIndex = 13;
            // 
            // txtImg3
            // 
            txtImg3.Dock = DockStyle.Fill;
            txtImg3.Location = new Point(439, 332);
            txtImg3.Multiline = true;
            txtImg3.Name = "txtImg3";
            txtImg3.ReadOnly = true;
            txtImg3.ScrollBars = ScrollBars.Vertical;
            txtImg3.Size = new Size(207, 257);
            txtImg3.TabIndex = 12;
            // 
            // txtImg2
            // 
            txtImg2.Dock = DockStyle.Fill;
            txtImg2.Location = new Point(221, 332);
            txtImg2.Multiline = true;
            txtImg2.Name = "txtImg2";
            txtImg2.ReadOnly = true;
            txtImg2.ScrollBars = ScrollBars.Vertical;
            txtImg2.Size = new Size(212, 257);
            txtImg2.TabIndex = 11;
            // 
            // picImg5
            // 
            picImg5.Dock = DockStyle.Fill;
            picImg5.Location = new Point(852, 137);
            picImg5.Name = "picImg5";
            picImg5.Size = new Size(215, 189);
            picImg5.TabIndex = 9;
            picImg5.TabStop = false;
            // 
            // picImg4
            // 
            picImg4.Dock = DockStyle.Fill;
            picImg4.Location = new Point(652, 137);
            picImg4.Name = "picImg4";
            picImg4.Size = new Size(194, 189);
            picImg4.TabIndex = 8;
            picImg4.TabStop = false;
            // 
            // picImg3
            // 
            picImg3.Dock = DockStyle.Fill;
            picImg3.Location = new Point(439, 137);
            picImg3.Name = "picImg3";
            picImg3.Size = new Size(207, 189);
            picImg3.TabIndex = 7;
            picImg3.TabStop = false;
            // 
            // picImg2
            // 
            picImg2.Dock = DockStyle.Fill;
            picImg2.Location = new Point(221, 137);
            picImg2.Name = "picImg2";
            picImg2.Size = new Size(212, 189);
            picImg2.TabIndex = 6;
            picImg2.TabStop = false;
            // 
            // lstImg4
            // 
            lstImg4.Dock = DockStyle.Fill;
            lstImg4.FormattingEnabled = true;
            lstImg4.ItemHeight = 15;
            lstImg4.Location = new Point(652, 3);
            lstImg4.Name = "lstImg4";
            lstImg4.Size = new Size(194, 128);
            lstImg4.TabIndex = 3;
            // 
            // lstImg3
            // 
            lstImg3.Dock = DockStyle.Fill;
            lstImg3.FormattingEnabled = true;
            lstImg3.ItemHeight = 15;
            lstImg3.Location = new Point(439, 3);
            lstImg3.Name = "lstImg3";
            lstImg3.Size = new Size(207, 128);
            lstImg3.TabIndex = 2;
            // 
            // lstImg2
            // 
            lstImg2.Dock = DockStyle.Fill;
            lstImg2.FormattingEnabled = true;
            lstImg2.ItemHeight = 15;
            lstImg2.Location = new Point(221, 3);
            lstImg2.Name = "lstImg2";
            lstImg2.Size = new Size(212, 128);
            lstImg2.TabIndex = 1;
            // 
            // lstImg1
            // 
            lstImg1.Dock = DockStyle.Fill;
            lstImg1.FormattingEnabled = true;
            lstImg1.ItemHeight = 15;
            lstImg1.Location = new Point(3, 3);
            lstImg1.Name = "lstImg1";
            lstImg1.Size = new Size(212, 128);
            lstImg1.TabIndex = 0;
            // 
            // picImg1
            // 
            picImg1.Dock = DockStyle.Fill;
            picImg1.Location = new Point(3, 137);
            picImg1.Name = "picImg1";
            picImg1.Size = new Size(212, 189);
            picImg1.TabIndex = 5;
            picImg1.TabStop = false;
            // 
            // txtImg1
            // 
            txtImg1.Dock = DockStyle.Fill;
            txtImg1.Location = new Point(3, 332);
            txtImg1.Multiline = true;
            txtImg1.Name = "txtImg1";
            txtImg1.ReadOnly = true;
            txtImg1.ScrollBars = ScrollBars.Vertical;
            txtImg1.Size = new Size(212, 257);
            txtImg1.TabIndex = 10;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1156, 714);
            Controls.Add(tlpResults);
            Controls.Add(textBox1);
            Controls.Add(btnTest);
            Name = "Form1";
            Text = "Form1";
            tlpResults.ResumeLayout(false);
            tlpResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picImg5).EndInit();
            ((System.ComponentModel.ISupportInitialize)picImg4).EndInit();
            ((System.ComponentModel.ISupportInitialize)picImg3).EndInit();
            ((System.ComponentModel.ISupportInitialize)picImg2).EndInit();
            ((System.ComponentModel.ISupportInitialize)picImg1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnTest;
        private TextBox textBox1;
        private TableLayoutPanel tlpResults;
        private ListBox lstImg4;
        private ListBox lstImg3;
        private ListBox lstImg2;
        private ListBox lstImg1;
        private TextBox txtImg4;
        private TextBox txtImg3;
        private TextBox txtImg2;
        private PictureBox picImg5;
        private PictureBox picImg4;
        private PictureBox picImg3;
        private PictureBox picImg2;
        private PictureBox picImg1;
        private TextBox txtImg1;
        private TextBox txtImg5;
        private ListBox lstImg5;
    }
}
