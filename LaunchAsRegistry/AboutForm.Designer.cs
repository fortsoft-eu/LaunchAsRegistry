namespace LaunchAsRegistry {
    partial class AboutForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.button = new System.Windows.Forms.Button();
            this.labelProductInfo = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelWebsite = new System.Windows.Forms.Label();
            this.panelProductInfo = new System.Windows.Forms.Panel();
            this.panelWebsite = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panelProductInfo.SuspendLayout();
            this.panelWebsite.SuspendLayout();
            this.SuspendLayout();
            // 
            // button
            // 
            resources.ApplyResources(this.button, "button");
            this.button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button.Name = "button";
            this.button.UseVisualStyleBackColor = true;
            // 
            // labelProductInfo
            // 
            resources.ApplyResources(this.labelProductInfo, "labelProductInfo");
            this.labelProductInfo.Name = "labelProductInfo";
            // 
            // pictureBox
            // 
            resources.ApplyResources(this.pictureBox, "pictureBox");
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.TabStop = false;
            // 
            // linkLabel
            // 
            resources.ApplyResources(this.linkLabel, "linkLabel");
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Tag = "";
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // labelWebsite
            // 
            resources.ApplyResources(this.labelWebsite, "labelWebsite");
            this.labelWebsite.Name = "labelWebsite";
            // 
            // panelProductInfo
            // 
            resources.ApplyResources(this.panelProductInfo, "panelProductInfo");
            this.panelProductInfo.Controls.Add(this.pictureBox);
            this.panelProductInfo.Controls.Add(this.labelProductInfo);
            this.panelProductInfo.Name = "panelProductInfo";
            // 
            // panelWebsite
            // 
            resources.ApplyResources(this.panelWebsite, "panelWebsite");
            this.panelWebsite.Controls.Add(this.labelWebsite);
            this.panelWebsite.Controls.Add(this.linkLabel);
            this.panelWebsite.Name = "panelWebsite";
            // 
            // AboutForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button;
            this.Controls.Add(this.panelWebsite);
            this.Controls.Add(this.panelProductInfo);
            this.Controls.Add(this.button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.OnFormActivated);
            this.Load += new System.EventHandler(this.OnFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panelProductInfo.ResumeLayout(false);
            this.panelProductInfo.PerformLayout();
            this.panelWebsite.ResumeLayout(false);
            this.panelWebsite.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button;
        private System.Windows.Forms.Label labelProductInfo;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelWebsite;
        private System.Windows.Forms.Panel panelProductInfo;
        private System.Windows.Forms.Panel panelWebsite;
    }
}