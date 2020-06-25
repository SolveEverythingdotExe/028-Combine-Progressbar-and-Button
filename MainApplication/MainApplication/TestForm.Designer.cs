namespace MainApplication
{
    partial class TestForm
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
            this.customProgressBar1 = new MainApplication.CustomProgressBar();
            this.SuspendLayout();
            // 
            // customProgressBar1
            // 
            this.customProgressBar1.DefaultText = "RUN";
            this.customProgressBar1.Font = new System.Drawing.Font("Cambria", 51F);
            this.customProgressBar1.Location = new System.Drawing.Point(69, 12);
            this.customProgressBar1.MinimumSize = new System.Drawing.Size(200, 200);
            this.customProgressBar1.Name = "customProgressBar1";
            this.customProgressBar1.Size = new System.Drawing.Size(231, 231);
            this.customProgressBar1.TabIndex = 0;
            this.customProgressBar1.TextOnSucess = "RERUN";
            this.customProgressBar1.Value = 0;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.customProgressBar1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private CustomProgressBar customProgressBar1;
    }
}