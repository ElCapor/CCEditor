namespace CCEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.materialMultiLineTextBox1 = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            this.startBtn = new MaterialSkin.Controls.MaterialButton();
            this.languageBtn = new MaterialSkin.Controls.MaterialButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "CC Editor Alpha 0.1\r\nCopyright @ WitchcraftAI 2023";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // materialMultiLineTextBox1
            // 
            this.materialMultiLineTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialMultiLineTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.materialMultiLineTextBox1.Depth = 0;
            this.materialMultiLineTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialMultiLineTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialMultiLineTextBox1.Location = new System.Drawing.Point(453, 75);
            this.materialMultiLineTextBox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialMultiLineTextBox1.Name = "materialMultiLineTextBox1";
            this.materialMultiLineTextBox1.Size = new System.Drawing.Size(341, 358);
            this.materialMultiLineTextBox1.TabIndex = 1;
            this.materialMultiLineTextBox1.Text = "";
            // 
            // startBtn
            // 
            this.startBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.startBtn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.startBtn.Depth = 0;
            this.startBtn.HighEmphasis = true;
            this.startBtn.Icon = null;
            this.startBtn.Location = new System.Drawing.Point(9, 121);
            this.startBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.startBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(67, 36);
            this.startBtn.TabIndex = 2;
            this.startBtn.Text = "Start";
            this.startBtn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.startBtn.UseAccentColor = false;
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // languageBtn
            // 
            this.languageBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.languageBtn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.languageBtn.Depth = 0;
            this.languageBtn.HighEmphasis = true;
            this.languageBtn.Icon = null;
            this.languageBtn.Location = new System.Drawing.Point(9, 167);
            this.languageBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.languageBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.languageBtn.Name = "languageBtn";
            this.languageBtn.Size = new System.Drawing.Size(172, 36);
            this.languageBtn.TabIndex = 3;
            this.languageBtn.Text = "Open File";
            this.languageBtn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.languageBtn.UseAccentColor = false;
            this.languageBtn.UseVisualStyleBackColor = true;
            this.languageBtn.Click += new System.EventHandler(this.languageBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.languageBtn);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.materialMultiLineTextBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private MaterialSkin.Controls.MaterialMultiLineTextBox materialMultiLineTextBox1;
        private MaterialSkin.Controls.MaterialButton startBtn;
        private MaterialSkin.Controls.MaterialButton languageBtn;
    }
}

