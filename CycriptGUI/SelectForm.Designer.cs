using System.Windows.Forms;

namespace CycriptGUI
{
    partial class SelectForm
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
            BeginInvoke(new MethodInvoker(delegate() { base.Dispose(disposing); }));
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadingLabel = new System.Windows.Forms.Label();
            this.loadingBar = new System.Windows.Forms.ProgressBar();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.resultsTable = new System.Windows.Forms.DataGridView();
            this.analyzeButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchInLabel = new System.Windows.Forms.Label();
            this.searchInSelect = new System.Windows.Forms.ComboBox();
            this.refreshButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.resultsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // loadingLabel
            // 
            this.loadingLabel.AutoSize = true;
            this.loadingLabel.Location = new System.Drawing.Point(148, 100);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new System.Drawing.Size(81, 13);
            this.loadingLabel.TabIndex = 0;
            this.loadingLabel.Text = "Loading Apps...";
            // 
            // loadingBar
            // 
            this.loadingBar.Location = new System.Drawing.Point(59, 132);
            this.loadingBar.Maximum = 3;
            this.loadingBar.Name = "loadingBar";
            this.loadingBar.Size = new System.Drawing.Size(260, 23);
            this.loadingBar.Step = 1;
            this.loadingBar.TabIndex = 1;
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(13, 16);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(159, 20);
            this.searchBox.TabIndex = 0;
            this.searchBox.Visible = false;
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            // 
            // resultsTable
            // 
            this.resultsTable.AllowUserToAddRows = false;
            this.resultsTable.AllowUserToDeleteRows = false;
            this.resultsTable.AllowUserToOrderColumns = true;
            this.resultsTable.AllowUserToResizeRows = false;
            this.resultsTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.resultsTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.resultsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.resultsTable.Location = new System.Drawing.Point(13, 42);
            this.resultsTable.MultiSelect = false;
            this.resultsTable.Name = "resultsTable";
            this.resultsTable.ReadOnly = true;
            this.resultsTable.RowHeadersVisible = false;
            this.resultsTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.resultsTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resultsTable.Size = new System.Drawing.Size(354, 222);
            this.resultsTable.TabIndex = 3;
            this.resultsTable.Visible = false;
            // 
            // analyzeButton
            // 
            this.analyzeButton.Location = new System.Drawing.Point(59, 275);
            this.analyzeButton.Name = "analyzeButton";
            this.analyzeButton.Size = new System.Drawing.Size(97, 23);
            this.analyzeButton.TabIndex = 4;
            this.analyzeButton.Text = "Start Session";
            this.analyzeButton.UseVisualStyleBackColor = true;
            this.analyzeButton.Visible = false;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(221, 275);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(97, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Visible = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Image = global::CycriptGUI.Properties.Resources.Search1;
            this.searchLabel.Location = new System.Drawing.Point(178, 19);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(22, 13);
            this.searchLabel.TabIndex = 6;
            this.searchLabel.Text = "     ";
            this.searchLabel.Visible = false;
            // 
            // searchInLabel
            // 
            this.searchInLabel.AutoSize = true;
            this.searchInLabel.Location = new System.Drawing.Point(221, 19);
            this.searchInLabel.Name = "searchInLabel";
            this.searchInLabel.Size = new System.Drawing.Size(34, 13);
            this.searchInLabel.TabIndex = 9;
            this.searchInLabel.Text = "Type:";
            this.searchInLabel.Visible = false;
            // 
            // searchInSelect
            // 
            this.searchInSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.searchInSelect.FormattingEnabled = true;
            this.searchInSelect.Items.AddRange(new object[] {
            "User",
            "System",
            "All"});
            this.searchInSelect.Location = new System.Drawing.Point(261, 15);
            this.searchInSelect.Name = "searchInSelect";
            this.searchInSelect.Size = new System.Drawing.Size(70, 21);
            this.searchInSelect.TabIndex = 10;
            this.searchInSelect.Visible = false;
            this.searchInSelect.SelectedIndexChanged += new System.EventHandler(this.searchInSelect_SelectedIndexChanged);
            // 
            // refreshButton
            // 
            this.refreshButton.Enabled = false;
            this.refreshButton.Image = global::CycriptGUI.Properties.Resources.refresh;
            this.refreshButton.Location = new System.Drawing.Point(337, 14);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(30, 23);
            this.refreshButton.TabIndex = 12;
            this.refreshButton.Text = "   ";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Visible = false;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // SelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 305);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.searchInSelect);
            this.Controls.Add(this.searchInLabel);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.analyzeButton);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.loadingBar);
            this.Controls.Add(this.loadingLabel);
            this.Controls.Add(this.resultsTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Select Application";
            this.Shown += new System.EventHandler(this.SelectForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.resultsTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label loadingLabel;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.DataGridView resultsTable;
        private System.Windows.Forms.Button analyzeButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label searchLabel;
        public ProgressBar loadingBar;
        private Label searchInLabel;
        private ComboBox searchInSelect;
        private Button refreshButton;
    }
}