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
            this.components = new System.ComponentModel.Container();
            this.newSessionButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.manageButton = new System.Windows.Forms.Button();
            this.selectDeviceGroup = new System.Windows.Forms.GroupBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.selectDeviceBox = new System.Windows.Forms.ComboBox();
            this.propertiesGroup = new System.Windows.Forms.GroupBox();
            this.cpuLabel = new System.Windows.Forms.Label();
            this.propertyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cpuTitleLabel = new System.Windows.Forms.Label();
            this.regionLabel = new System.Windows.Forms.Label();
            this.regionTitleLabel = new System.Windows.Forms.Label();
            this.phoneNumberLabel = new System.Windows.Forms.Label();
            this.phoneNumberTitleLabel = new System.Windows.Forms.Label();
            this.serialNumberLabel = new System.Windows.Forms.Label();
            this.serialNumberTitleLabel = new System.Windows.Forms.Label();
            this.capacityLabel = new System.Windows.Forms.Label();
            this.capacityTitleLabel = new System.Windows.Forms.Label();
            this.iosVersionLabel = new System.Windows.Forms.Label();
            this.iosVersionTitleLabel = new System.Windows.Forms.Label();
            this.lastConnectedLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.lastConnectedTitleLabel = new System.Windows.Forms.Label();
            this.udidLabel = new System.Windows.Forms.Label();
            this.modelLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.udidTitleLabel = new System.Windows.Forms.Label();
            this.modelTitleLabel = new System.Windows.Forms.Label();
            this.nameTitleLabel = new System.Windows.Forms.Label();
            this.selectDeviceGroup.SuspendLayout();
            this.propertiesGroup.SuspendLayout();
            this.propertyContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // newSessionButton
            // 
            this.newSessionButton.Location = new System.Drawing.Point(58, 278);
            this.newSessionButton.Name = "newSessionButton";
            this.newSessionButton.Size = new System.Drawing.Size(110, 23);
            this.newSessionButton.TabIndex = 0;
            this.newSessionButton.Text = "New Session";
            this.newSessionButton.UseVisualStyleBackColor = true;
            this.newSessionButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(218, 278);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(107, 23);
            this.loadButton.TabIndex = 9;
            this.loadButton.Text = "Load Session";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
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
            this.selectDeviceGroup.Controls.Add(this.refreshButton);
            this.selectDeviceGroup.Controls.Add(this.selectDeviceBox);
            this.selectDeviceGroup.Controls.Add(this.manageButton);
            this.selectDeviceGroup.Location = new System.Drawing.Point(12, 13);
            this.selectDeviceGroup.Name = "selectDeviceGroup";
            this.selectDeviceGroup.Size = new System.Drawing.Size(375, 53);
            this.selectDeviceGroup.TabIndex = 11;
            this.selectDeviceGroup.TabStop = false;
            this.selectDeviceGroup.Text = "Select Device";
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
            this.selectDeviceBox.Enabled = false;
            this.selectDeviceBox.FormattingEnabled = true;
            this.selectDeviceBox.Items.AddRange(new object[] {
            "Searching for iDevices..."});
            this.selectDeviceBox.Location = new System.Drawing.Point(6, 19);
            this.selectDeviceBox.Name = "selectDeviceBox";
            this.selectDeviceBox.Size = new System.Drawing.Size(245, 21);
            this.selectDeviceBox.TabIndex = 0;
            this.selectDeviceBox.SelectedValueChanged += new System.EventHandler(this.selectDeviceBox_SelectedValueChanged);
            // 
            // propertiesGroup
            // 
            this.propertiesGroup.Controls.Add(this.cpuLabel);
            this.propertiesGroup.Controls.Add(this.cpuTitleLabel);
            this.propertiesGroup.Controls.Add(this.regionLabel);
            this.propertiesGroup.Controls.Add(this.regionTitleLabel);
            this.propertiesGroup.Controls.Add(this.phoneNumberLabel);
            this.propertiesGroup.Controls.Add(this.phoneNumberTitleLabel);
            this.propertiesGroup.Controls.Add(this.serialNumberLabel);
            this.propertiesGroup.Controls.Add(this.serialNumberTitleLabel);
            this.propertiesGroup.Controls.Add(this.capacityLabel);
            this.propertiesGroup.Controls.Add(this.capacityTitleLabel);
            this.propertiesGroup.Controls.Add(this.iosVersionLabel);
            this.propertiesGroup.Controls.Add(this.iosVersionTitleLabel);
            this.propertiesGroup.Controls.Add(this.lastConnectedLabel);
            this.propertiesGroup.Controls.Add(this.statusLabel);
            this.propertiesGroup.Controls.Add(this.lastConnectedTitleLabel);
            this.propertiesGroup.Controls.Add(this.udidLabel);
            this.propertiesGroup.Controls.Add(this.modelLabel);
            this.propertiesGroup.Controls.Add(this.nameLabel);
            this.propertiesGroup.Controls.Add(this.udidTitleLabel);
            this.propertiesGroup.Controls.Add(this.modelTitleLabel);
            this.propertiesGroup.Controls.Add(this.nameTitleLabel);
            this.propertiesGroup.Enabled = false;
            this.propertiesGroup.Location = new System.Drawing.Point(13, 72);
            this.propertiesGroup.Name = "propertiesGroup";
            this.propertiesGroup.Size = new System.Drawing.Size(368, 195);
            this.propertiesGroup.TabIndex = 12;
            this.propertiesGroup.TabStop = false;
            this.propertiesGroup.Text = "Device Properties";
            // 
            // cpuLabel
            // 
            this.cpuLabel.AutoSize = true;
            this.cpuLabel.ContextMenuStrip = this.propertyContextMenu;
            this.cpuLabel.Location = new System.Drawing.Point(306, 124);
            this.cpuLabel.Name = "cpuLabel";
            this.cpuLabel.Size = new System.Drawing.Size(27, 13);
            this.cpuLabel.TabIndex = 20;
            this.cpuLabel.Text = "N/A";
            // 
            // propertyContextMenu
            // 
            this.propertyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.propertyContextMenu.Name = "propertyContextMenu";
            this.propertyContextMenu.Size = new System.Drawing.Size(103, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // cpuTitleLabel
            // 
            this.cpuTitleLabel.AutoSize = true;
            this.cpuTitleLabel.ContextMenuStrip = this.propertyContextMenu;
            this.cpuTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cpuTitleLabel.Location = new System.Drawing.Point(191, 124);
            this.cpuTitleLabel.Name = "cpuTitleLabel";
            this.cpuTitleLabel.Size = new System.Drawing.Size(109, 13);
            this.cpuTitleLabel.TabIndex = 19;
            this.cpuTitleLabel.Text = "CPU Architecture:";
            // 
            // regionLabel
            // 
            this.regionLabel.AutoSize = true;
            this.regionLabel.ContextMenuStrip = this.propertyContextMenu;
            this.regionLabel.Location = new System.Drawing.Point(63, 124);
            this.regionLabel.Name = "regionLabel";
            this.regionLabel.Size = new System.Drawing.Size(27, 13);
            this.regionLabel.TabIndex = 18;
            this.regionLabel.Text = "N/A";
            // 
            // regionTitleLabel
            // 
            this.regionTitleLabel.AutoSize = true;
            this.regionTitleLabel.ContextMenuStrip = this.propertyContextMenu;
            this.regionTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regionTitleLabel.Location = new System.Drawing.Point(6, 124);
            this.regionTitleLabel.Name = "regionTitleLabel";
            this.regionTitleLabel.Size = new System.Drawing.Size(51, 13);
            this.regionTitleLabel.TabIndex = 17;
            this.regionTitleLabel.Text = "Region:";
            // 
            // phoneNumberLabel
            // 
            this.phoneNumberLabel.AutoSize = true;
            this.phoneNumberLabel.ContextMenuStrip = this.propertyContextMenu;
            this.phoneNumberLabel.Location = new System.Drawing.Point(106, 142);
            this.phoneNumberLabel.Name = "phoneNumberLabel";
            this.phoneNumberLabel.Size = new System.Drawing.Size(27, 13);
            this.phoneNumberLabel.TabIndex = 16;
            this.phoneNumberLabel.Text = "N/A";
            // 
            // phoneNumberTitleLabel
            // 
            this.phoneNumberTitleLabel.AutoSize = true;
            this.phoneNumberTitleLabel.ContextMenuStrip = this.propertyContextMenu;
            this.phoneNumberTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phoneNumberTitleLabel.Location = new System.Drawing.Point(6, 142);
            this.phoneNumberTitleLabel.Name = "phoneNumberTitleLabel";
            this.phoneNumberTitleLabel.Size = new System.Drawing.Size(94, 13);
            this.phoneNumberTitleLabel.TabIndex = 15;
            this.phoneNumberTitleLabel.Text = "Phone Number:";
            // 
            // serialNumberLabel
            // 
            this.serialNumberLabel.AutoSize = true;
            this.serialNumberLabel.ContextMenuStrip = this.propertyContextMenu;
            this.serialNumberLabel.Location = new System.Drawing.Point(102, 92);
            this.serialNumberLabel.Name = "serialNumberLabel";
            this.serialNumberLabel.Size = new System.Drawing.Size(27, 13);
            this.serialNumberLabel.TabIndex = 14;
            this.serialNumberLabel.Text = "N/A";
            // 
            // serialNumberTitleLabel
            // 
            this.serialNumberTitleLabel.AutoSize = true;
            this.serialNumberTitleLabel.ContextMenuStrip = this.propertyContextMenu;
            this.serialNumberTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serialNumberTitleLabel.Location = new System.Drawing.Point(6, 92);
            this.serialNumberTitleLabel.Name = "serialNumberTitleLabel";
            this.serialNumberTitleLabel.Size = new System.Drawing.Size(90, 13);
            this.serialNumberTitleLabel.TabIndex = 13;
            this.serialNumberTitleLabel.Text = "Serial Number:";
            // 
            // capacityLabel
            // 
            this.capacityLabel.AutoSize = true;
            this.capacityLabel.ContextMenuStrip = this.propertyContextMenu;
            this.capacityLabel.Location = new System.Drawing.Point(257, 41);
            this.capacityLabel.Name = "capacityLabel";
            this.capacityLabel.Size = new System.Drawing.Size(27, 13);
            this.capacityLabel.TabIndex = 12;
            this.capacityLabel.Text = "N/A";
            // 
            // capacityTitleLabel
            // 
            this.capacityTitleLabel.AutoSize = true;
            this.capacityTitleLabel.ContextMenuStrip = this.propertyContextMenu;
            this.capacityTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.capacityTitleLabel.Location = new System.Drawing.Point(191, 41);
            this.capacityTitleLabel.Name = "capacityTitleLabel";
            this.capacityTitleLabel.Size = new System.Drawing.Size(60, 13);
            this.capacityTitleLabel.TabIndex = 11;
            this.capacityTitleLabel.Text = "Capacity:";
            // 
            // iosVersionLabel
            // 
            this.iosVersionLabel.AutoSize = true;
            this.iosVersionLabel.ContextMenuStrip = this.propertyContextMenu;
            this.iosVersionLabel.Location = new System.Drawing.Point(89, 41);
            this.iosVersionLabel.Name = "iosVersionLabel";
            this.iosVersionLabel.Size = new System.Drawing.Size(27, 13);
            this.iosVersionLabel.TabIndex = 10;
            this.iosVersionLabel.Text = "N/A";
            // 
            // iosVersionTitleLabel
            // 
            this.iosVersionTitleLabel.AutoSize = true;
            this.iosVersionTitleLabel.ContextMenuStrip = this.propertyContextMenu;
            this.iosVersionTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iosVersionTitleLabel.Location = new System.Drawing.Point(6, 41);
            this.iosVersionTitleLabel.Name = "iosVersionTitleLabel";
            this.iosVersionTitleLabel.Size = new System.Drawing.Size(77, 13);
            this.iosVersionTitleLabel.TabIndex = 9;
            this.iosVersionTitleLabel.Text = "iOS Version:";
            // 
            // lastConnectedLabel
            // 
            this.lastConnectedLabel.AutoSize = true;
            this.lastConnectedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastConnectedLabel.Location = new System.Drawing.Point(96, 173);
            this.lastConnectedLabel.Name = "lastConnectedLabel";
            this.lastConnectedLabel.Size = new System.Drawing.Size(27, 13);
            this.lastConnectedLabel.TabIndex = 8;
            this.lastConnectedLabel.Text = "N/A";
            this.lastConnectedLabel.Visible = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(325, 173);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(37, 13);
            this.statusLabel.TabIndex = 7;
            this.statusLabel.Text = "Offline";
            // 
            // lastConnectedTitleLabel
            // 
            this.lastConnectedTitleLabel.AutoSize = true;
            this.lastConnectedTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastConnectedTitleLabel.Location = new System.Drawing.Point(5, 173);
            this.lastConnectedTitleLabel.Name = "lastConnectedTitleLabel";
            this.lastConnectedTitleLabel.Size = new System.Drawing.Size(85, 13);
            this.lastConnectedTitleLabel.TabIndex = 6;
            this.lastConnectedTitleLabel.Text = "Last Connected:";
            this.lastConnectedTitleLabel.Visible = false;
            // 
            // udidLabel
            // 
            this.udidLabel.AutoSize = true;
            this.udidLabel.ContextMenuStrip = this.propertyContextMenu;
            this.udidLabel.Location = new System.Drawing.Point(54, 72);
            this.udidLabel.Name = "udidLabel";
            this.udidLabel.Size = new System.Drawing.Size(27, 13);
            this.udidLabel.TabIndex = 5;
            this.udidLabel.Text = "N/A";
            // 
            // modelLabel
            // 
            this.modelLabel.AutoSize = true;
            this.modelLabel.ContextMenuStrip = this.propertyContextMenu;
            this.modelLabel.Location = new System.Drawing.Point(242, 21);
            this.modelLabel.Name = "modelLabel";
            this.modelLabel.Size = new System.Drawing.Size(27, 13);
            this.modelLabel.TabIndex = 4;
            this.modelLabel.Text = "N/A";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoEllipsis = true;
            this.nameLabel.AutoSize = true;
            this.nameLabel.ContextMenuStrip = this.propertyContextMenu;
            this.nameLabel.Location = new System.Drawing.Point(54, 21);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(27, 13);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "N/A";
            // 
            // udidTitleLabel
            // 
            this.udidTitleLabel.AutoSize = true;
            this.udidTitleLabel.ContextMenuStrip = this.propertyContextMenu;
            this.udidTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udidTitleLabel.Location = new System.Drawing.Point(6, 72);
            this.udidTitleLabel.Name = "udidTitleLabel";
            this.udidTitleLabel.Size = new System.Drawing.Size(42, 13);
            this.udidTitleLabel.TabIndex = 2;
            this.udidTitleLabel.Text = "UDID:";
            // 
            // modelTitleLabel
            // 
            this.modelTitleLabel.AutoSize = true;
            this.modelTitleLabel.ContextMenuStrip = this.propertyContextMenu;
            this.modelTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modelTitleLabel.Location = new System.Drawing.Point(191, 21);
            this.modelTitleLabel.Name = "modelTitleLabel";
            this.modelTitleLabel.Size = new System.Drawing.Size(45, 13);
            this.modelTitleLabel.TabIndex = 1;
            this.modelTitleLabel.Text = "Model:";
            // 
            // nameTitleLabel
            // 
            this.nameTitleLabel.AutoSize = true;
            this.nameTitleLabel.ContextMenuStrip = this.propertyContextMenu;
            this.nameTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameTitleLabel.Location = new System.Drawing.Point(6, 21);
            this.nameTitleLabel.Name = "nameTitleLabel";
            this.nameTitleLabel.Size = new System.Drawing.Size(43, 13);
            this.nameTitleLabel.TabIndex = 0;
            this.nameTitleLabel.Text = "Name:";
            // 
            // StartForm
            // 
            this.AcceptButton = this.newSessionButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 315);
            this.Controls.Add(this.propertiesGroup);
            this.Controls.Add(this.selectDeviceGroup);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.newSessionButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cycript GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartForm_FormClosing);
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.Shown += new System.EventHandler(this.StartForm_Shown);
            this.selectDeviceGroup.ResumeLayout(false);
            this.propertiesGroup.ResumeLayout(false);
            this.propertiesGroup.PerformLayout();
            this.propertyContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button newSessionButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button manageButton;
        private System.Windows.Forms.GroupBox selectDeviceGroup;
        private System.Windows.Forms.ComboBox selectDeviceBox;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.GroupBox propertiesGroup;
        private System.Windows.Forms.Label lastConnectedLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label lastConnectedTitleLabel;
        private System.Windows.Forms.Label udidLabel;
        private System.Windows.Forms.Label modelLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label udidTitleLabel;
        private System.Windows.Forms.Label modelTitleLabel;
        private System.Windows.Forms.Label nameTitleLabel;
        private System.Windows.Forms.Label capacityLabel;
        private System.Windows.Forms.Label capacityTitleLabel;
        private System.Windows.Forms.Label iosVersionLabel;
        private System.Windows.Forms.Label iosVersionTitleLabel;
        private System.Windows.Forms.Label serialNumberLabel;
        private System.Windows.Forms.Label serialNumberTitleLabel;
        private System.Windows.Forms.Label regionLabel;
        private System.Windows.Forms.Label regionTitleLabel;
        private System.Windows.Forms.Label phoneNumberLabel;
        private System.Windows.Forms.Label phoneNumberTitleLabel;
        private System.Windows.Forms.Label cpuLabel;
        private System.Windows.Forms.Label cpuTitleLabel;
        private System.Windows.Forms.ContextMenuStrip propertyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    }
}

