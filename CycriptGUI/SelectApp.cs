using CycriptGUI.LibIMobileDevice;
using CycriptGUI.MainProgram;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CycriptGUI
{
    public partial class selectApp : Form
    {
        static CancellationTokenSource cancellationTokenSource;
        public List<iOSApplication> AppList;

        public selectApp()
        {
            InitializeComponent();
        }

        private void SelectApp_Shown(object sender, EventArgs e)
        {
            searchInSelect.SelectedIndex = 0;
            cancellationTokenSource = new CancellationTokenSource();
            Task getApplicationsTask = Task.Factory.StartNew(() => { updateAppList(); }, cancellationTokenSource.Token);
        }

        #region Other Events
        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            applySelectAppLayout();
        }

        private void searchInSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AppList != null)
            {
                applySelectAppLayout();
            }          
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            Task getApplicationsTask = Task.Factory.StartNew(() => { updateAppList(); }, cancellationTokenSource.Token);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void selectApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }
        #endregion

        #region Functions
        DataTable createDataTable(List<iOSApplication> inputList)
        {
            DataTable newTable = new DataTable();
            newTable.Columns.Add("Name", typeof(string));
            newTable.Columns.Add("Version", typeof(string));
            newTable.Columns.Add("Identifier", typeof(string));

            // Sorting list of applications
            inputList.Sort(new Comparison<iOSApplication>((x, y) => string.Compare(x.Name, y.Name)));
            foreach (iOSApplication currApp in inputList)
            {
                newTable.Rows.Add(currApp.Name, currApp.Version, currApp.Identifier);
            }

            return newTable;
        }

        void applySelectAppLayout()
        {
            // Applying layout after application list update
            loadingLabel.Visible = false;
            loadingBar.Visible = false;
            searchBox.Visible = true;
            searchInLabel.Visible = true;
            searchInSelect.Visible = true;
            searchLabel.Visible = true;
            resultsTable.Visible = true;
            resultsTable.Enabled = true;
            analyzeButton.Visible = true;
            cancelButton.Visible = true;
            refreshButton.Visible = true;
            refreshButton.Enabled = true;

            searchBox.Focus();
            int selectedIndex = searchInSelect.SelectedIndex;

            // Determining application type for search and searching in properties of found applications
            string appType = selectedIndex == 0 ? "User" : (selectedIndex == 1 ? "System" : (selectedIndex == 2 ? "Any" : "None"));
            List<iOSApplication> resultList = new List<iOSApplication>();
            if (appType != "None")
            {
                resultList = AppList.Where(x => x.Type == appType || appType == "Any").Where(x => x.Name.ToLower().Contains(searchBox.Text.ToLower())
                  || x.Identifier.ToLower().Contains(searchBox.Text.ToLower()) || x.ExecutableName.ToLower().Contains(searchBox.Text.ToLower())
                  || searchBox.Text.Contains(".") && x.Version.ToLower().Contains(searchBox.Text.ToLower())).ToList();
            }

            // Setting up result table
            resultsTable.DataSource = createDataTable(resultList);
            resultsTable.Columns[1].Width = 65;
        }

        void updateAppList()
        {
            // Changing layout for updating
            BeginInvoke(new MethodInvoker(delegate()
            {
                loadingBar.Value = 0;
                loadingBar.PerformStep();
                resultsTable.DataSource = createDataTable(new List<iOSApplication>());
                resultsTable.Enabled = false;
                loadingBar.Visible = true;
                refreshButton.Enabled = false;
            }));

            // Getting applications
            InstallationProxy.GetApplications(StartForm.WorkingDevice.InstallationProxyClient, this, out AppList);

            // Changing layout after updating
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(applySelectAppLayout));
            }

            else
            {
                applySelectAppLayout();
            }
        }
        #endregion
    }
}