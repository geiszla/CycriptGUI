namespace CycriptGUI
{
    partial class StartForm
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
            this.helpBox = new System.Windows.Forms.TextBox();
            this.newButton = new System.Windows.Forms.Button();
            this.userNameBox = new System.Windows.Forms.TextBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.loadButton = new System.Windows.Forms.Button();
            this.manageButton = new System.Windows.Forms.Button();
            this.selectDeviceGroup = new System.Windows.Forms.GroupBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.selectDeviceBox = new System.Windows.Forms.ComboBox();
            this.selectDeviceGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // helpBox
            // 
            this.helpBox.Location = new System.Drawing.Point(12, 97);
            this.helpBox.Multiline = true;
            this.helpBox.Name = "helpBox";
            this.helpBox.ReadOnly = true;
            this.helpBox.Size = new System.Drawing.Size(375, 143);
            this.helpBox.TabIndex = 1;
            // 
            // newButton
            // 
            this.newButton.Location = new System.Drawing.Point(59, 246);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(110, 23);
            this.newButton.TabIndex = 0;
            this.newButton.Text = "New Session";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // userNameBox
            // 
            this.userNameBox.Location = new System.Drawing.Point(67, 50);
            this.userNameBox.Name = "userNameBox";
            this.userNameBox.Size = new System.Drawing.Size(120, 20);
            this.userNameBox.TabIndex = 4;
            this.userNameBox.Text = "root";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(255, 50);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.PasswordChar = '•';
            this.passwordBox.Size = new System.Drawing.Size(114, 20);
            this.passwordBox.TabIndex = 5;
            // 
            // userNameLabel
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Location = new System.Drawing.Point(3, 53);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(58, 13);
            this.userNameLabel.TabIndex = 6;
            this.userNameLabel.Text = "Username:";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(193, 53);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(56, 13);
            this.passwordLabel.TabIndex = 7;
            this.passwordLabel.Text = "Password:";
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(222, 246);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(107, 23);
            this.loadButton.TabIndex = 9;
            this.loadButton.Text = "Load Session";
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // manageButton
            // 
            this.manageButton.Enabled = false;
            this.manageButton.Location = new System.Drawing.Point(294, 18);
            this.manageButton.Name = "manageButton";
            this.manageButton.Size = new System.Drawing.Size(75, 23);
            this.manageButton.TabIndex = 10;
            this.manageButton.Text = "Manage";
            this.manageButton.UseVisualStyleBackColor = true;
            this.manageButton.Click += new System.EventHandler(this.manageButton_Click);
            // 
            // selectDeviceGroup
            // 
            this.selectDeviceGroup.Controls.Add(this.passwordLabel);
            this.selectDeviceGroup.Controls.Add(this.refreshButton);
            this.selectDeviceGroup.Controls.Add(this.userNameLabel);
            this.selectDeviceGroup.Controls.Add(this.selectDeviceBox);
            this.selectDeviceGroup.Controls.Add(this.userNameBox);
            this.selectDeviceGroup.Controls.Add(this.manageButton);
            this.selectDeviceGroup.Controls.Add(this.passwordBox);
            this.selectDeviceGroup.Location = new System.Drawing.Point(12, 13);
            this.selectDeviceGroup.Name = "selectDeviceGroup";
            this.selectDeviceGroup.Size = new System.Drawing.Size(375, 78);
            this.selectDeviceGroup.TabIndex = 11;
            this.selectDeviceGroup.TabStop = false;
            this.selectDeviceGroup.Text = "SSH Login";
            // 
            // refreshButton
            // 
            this.refreshButton.Enabled = false;
            this.refreshButton.Image = global::CycriptGUI.Properties.Resources.refresh;
            this.refreshButton.Location = new System.Drawing.Point(258, 18);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(30, 23);
            this.refreshButton.TabIndex = 11;
            this.refreshButton.Text = "   ";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // selectDeviceBox
            // 
            this.selectDeviceBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectDeviceBox.FormattingEnabled = true;
            this.selectDeviceBox.Items.AddRange(new object[] {
            "Searching for iDevices..."});
            this.selectDeviceBox.Location = new System.Drawing.Point(6, 19);
            this.selectDeviceBox.Name = "selectDeviceBox";
            this.selectDeviceBox.Size = new System.Drawing.Size(245, 21);
            this.selectDeviceBox.TabIndex = 0;
            // 
            // StartForm
            // 
            this.AcceptButton = this.newButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 276);
            this.Controls.Add(this.selectDeviceGroup);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.helpBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cycript GUI";
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.Shown += new System.EventHandler(this.StartForm_Shown);
            this.selectDeviceGroup.ResumeLayout(false);
            this.selectDeviceGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox helpBox;
        private System.Windows.Forms.Button newButton;
        private System.Windows.Forms.TextBox userNameBox;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button manageButton;
        private System.Windows.Forms.GroupBox selectDeviceGroup;
        private System.Windows.Forms.ComboBox selectDeviceBox;
        private System.Windows.Forms.Button refreshButton;

    }
}

