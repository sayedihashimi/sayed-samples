namespace Activities.Design
{
    partial class VersionDialog
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
            this.major = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.minor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.build = new System.Windows.Forms.TextBox();
            this.revision = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // major
            // 
            this.major.Location = new System.Drawing.Point(12, 12);
            this.major.Name = "major";
            this.major.Size = new System.Drawing.Size(33, 20);
            this.major.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = ".";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = ".";
            // 
            // minor
            // 
            this.minor.Location = new System.Drawing.Point(63, 12);
            this.minor.Name = "minor";
            this.minor.Size = new System.Drawing.Size(33, 20);
            this.minor.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(149, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = ".";
            // 
            // build
            // 
            this.build.Location = new System.Drawing.Point(114, 12);
            this.build.Name = "build";
            this.build.Size = new System.Drawing.Size(33, 20);
            this.build.TabIndex = 4;
            // 
            // revision
            // 
            this.revision.Location = new System.Drawing.Point(165, 12);
            this.revision.Name = "revision";
            this.revision.Size = new System.Drawing.Size(33, 20);
            this.revision.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(12, 43);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(93, 43);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // VersionDialog
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(210, 78);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.revision);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.build);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.minor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.major);
            this.Name = "VersionDialog";
            this.Text = "VersionDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox major;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox minor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox build;
        private System.Windows.Forms.TextBox revision;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}