using CycriptGUI.LibIMobileDevice;
using CycriptGUI.MainProgram;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CycriptGUI
{
    public partial class ManageForm : Form
    {
        iDevice WorkingDevice;

        public ManageForm()
        {
            InitializeComponent();
        }

        #region Events
        private void ManageForm_Load(object sender, System.EventArgs e)
        {
            WorkingDevice = StartForm.WorkingDevice;
            nameLabel.Text = $"{StartForm.WorkingDevice.Name} ({StartForm.WorkingDevice.ProductName})";

            jailbreakStatusLabel.Text = "Checking...";
            cydiaLabel.Text = "Checking...";
            openSSHLabel.Text = "Checking...";
            cycriptLabel.Text = "Checking...";

            Task.Factory.StartNew(() => checkComponents());
        }
        #endregion

        #region Main Functions
        void checkComponents()
        {
            bool[] componentsAvailable = new bool[4];

            connectToInstallationProxy();
            checkCydia(out componentsAvailable[1]);

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => applyControls(componentsAvailable)));
            }
        }

        void connectToInstallationProxy()
        {
            if (!WorkingDevice.Connect())
            {
                Alert.ShowError("Couldn't connect to the device. Please check the connection.",
                    "Device Connection Failed");
            }

            if (!InstallationProxy.Connect(WorkingDevice))
            {
                Alert.ShowError("Couldn't connect to Installation Proxy. Please check the connection to the device.",
                    "Installation Proxy Connection Failed");
            }
        }

        bool checkCydia(out bool isInstalled)
        {
            isInstalled = false;

            List<iOSApplication> appList;
            if (!InstallationProxy.GetApplications(WorkingDevice, out appList))
            {
                Alert.ShowError("Couldn't get app list from the device. Please check the connection.",
                    "Get Apps Failed");
                return false;
            }

            isInstalled = appList.Any(x => x.Name.ToLower() == "cydia"
                && x.Identifier.ToLower() == "com.saurik.cydia");

            return true;
        }
        #endregion

        #region Other Functions
        void applyControls(bool[] componentsAvailable)
        {
            jailbreakStatusLabel.Text = componentsAvailable[0] ? "Yes" : "No";
            jailbreakStatusHelpButton.Enabled = !componentsAvailable[0];

            cydiaLabel.Text = componentsAvailable[1] ? "Installed" : "Missing";
            cydiaHelpButton.Enabled = !componentsAvailable[1];

            openSSHLabel.Text = componentsAvailable[2] ? "Installed" : "Missing";
            openSSHButton.Enabled = !componentsAvailable[2];

            cycriptLabel.Text = componentsAvailable[3] ? "Installed" : "Missing";
            cycriptButton.Enabled = !componentsAvailable[3];
        }
        #endregion
    }
}
