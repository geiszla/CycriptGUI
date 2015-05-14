using CycriptGUI.LibIMobileDevice;
using CycriptGUI.MainProgram;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;

 namespace CycriptGUI
{
    public partial class StartForm : Form
    {
        #region Global Variables
        public static IDevice WorkingDevice;

        //public static string userName;
        //public static string password;

        List<IDevice> Devices = new List<IDevice>();
        List<IDevice> listedDevices = new List<IDevice>();
        string selectedUdid;
        Task updateTask;
        #endregion

        public StartForm()
        {
            InitializeComponent();
        }

        #region Form Events
        private void StartForm_Load(object sender, EventArgs e)
        {
            helpBox.Text = @"This tool uses SSH connection to the iDevice to make reverse engineering easier by automating long, often used tasks.

Requirements
You need an Apple device running jaibroken iOs with the following packages installed:
 - OpenSSH (Cydia/Telesphoreo)
 - Cycript (Cydia/Telesphoreo)";
        }

        private void StartForm_Shown(object sender, EventArgs e)
        {
                selectDeviceBox.SelectedIndex = 0;
                updateTask = Task.Factory.StartNew(() => { updateDeviceList(); });
        }
        #endregion

        #region Other Events
        private void connectButton_Click(object sender, EventArgs e)
        {
            // Handling exceptional cases
            if (listedDevices != null && listedDevices.Count != 0)
            {
                WorkingDevice = Devices.Where(x => x.Connected == true).ToList()[selectDeviceBox.SelectedIndex];
            }

            else if (!updateTask.IsCompleted)
            {
                MessageBox.Show("Please wait, while the program finishes searching for devices!", "Search in progress",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                MessageBox.Show("Couldn't find any iOs device. Please check the connection and try again!", "No device detected",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If sverything's OK, connectiong to the device
            LibiMobileDevice.iDeviceError deviceReturnCode = LibiMobileDevice.NewDevice(out WorkingDevice.Handle, WorkingDevice.Udid);

            Lockdown.LockdownError lockdownReturnCode = Lockdown.Start(
                WorkingDevice.Handle,
                out WorkingDevice.LockdownClient,
                out WorkingDevice.InstallationProxyService);

            InstallationProxy.InstproxyError installProxyReturnCode = InstallationProxy.Connect(
                WorkingDevice.Handle,
                WorkingDevice.InstallationProxyService,
                out WorkingDevice.InstallationProxyClient);

            //userName = userNameBox.Text;
            //password = passwordBox.Text;

            // Setting up and starting app selection form
            Form selectAppForm = new selectApp() { Owner = this };
            selectAppForm.Location = new Point(
                this.Location.X + (this.Width - selectAppForm.Width) / 2,
                this.Location.Y + (this.Height - selectAppForm.Height) / 2);
            selectAppForm.FormClosing += (s, args) =>
                this.Location = new Point(
                    selectAppForm.Location.X + (selectAppForm.Width - this.Width) / 2,
                    selectAppForm.Location.Y + (selectAppForm.Height - this.Height) / 2);
            selectAppForm.Closed += (s, args) => this.Show();

            this.Hide();
            selectAppForm.Show();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            refreshButton.Enabled = false;
            Task updateTask = Task.Factory.StartNew(() => { updateDeviceList(); });
        }

        private void manageButton_Click(object sender, EventArgs e)
        {
            Ssh.Connect(listedDevices[selectDeviceBox.SelectedIndex].Udid);
        }
        #endregion

        #region Update Device List Functions
        void updateDeviceList()
        {
            List<IDevice> deviceList;
            LibiMobileDevice.iDeviceError getDeviceReturnCode = LibiMobileDevice.GetDeviceList(out deviceList);

            // Setting found devices "Connected" property to "true", and the others' to "false"
            foreach (IDevice currDevice in Devices)
            {
                currDevice.Connected = false;
                foreach (IDevice currDevice2 in deviceList)
                {
                    if (currDevice.Udid == currDevice2.Udid)
                    {
                        currDevice.Connected = true;
                    }
                }
            }

            // Adding new devices to the list
            List<IDevice> noDuplicatesList = deviceList.Where(x => Devices.Where(y => y.Udid == x.Udid).Count() == 0).ToList();
            if (noDuplicatesList != null)
            {
                Devices.AddRange(noDuplicatesList);
            }

            BeginInvoke(new MethodInvoker(applyDeviceList));
        }

        void applyDeviceList()
        {
            // Determining currently selected device
            selectedUdid = listedDevices.Count() != 0 ? listedDevices[selectDeviceBox.SelectedIndex].Udid : null;

            // Refreshing device combo box
            selectDeviceBox.Items.Clear();
            if (Devices.Count != 0)
            {
                listedDevices = new List<IDevice>();
                foreach (IDevice currDevice in Devices)
                {
                    // If device is connected, adding it to the combo box and setting its "index" property to its index in the combo box.
                    if (currDevice.Connected == true)
                    {
                        string currItemString = currDevice.Name + " (" + currDevice.ProductName + ")";
                        if (Devices.Where(x => x.Name == currDevice.Name).Count() > 1 && Devices.Where(x => x.ProductType == currDevice.ProductType).Count() > 1)
                        {
                            currItemString += " (" + currDevice.SerialNumber + ")";
                        }

                        selectDeviceBox.Items.Add(currItemString);
                        listedDevices.Add(currDevice);
                    }
                }

                if (listedDevices.Count() != 0)
                {
                    // If there was device selected before refresh, setting the currently selected index to its index
                    if (selectedUdid != null)
                    {
                        int udidIndex = Devices.Where(x => x.Connected == true).ToList().IndexOf(Devices.Where(x => x.Udid == selectedUdid).FirstOrDefault());
                        selectDeviceBox.SelectedIndex = udidIndex != -1 ? udidIndex : 0;
                    }

                    else
                    {
                        selectDeviceBox.SelectedIndex = 0;
                    }

                    manageButton.Enabled = true;
                }
            }

            // If no devices connected, displaying message in combo box
            if (Devices.Count() != 0 && listedDevices.Count() == 0 || Devices.Count() == 0)
            {
                selectedUdid = null;

                selectDeviceBox.Items.Add("No iDevice Has Been Detected...");
                selectDeviceBox.SelectedIndex = 0;
                manageButton.Enabled = false;
            }

            refreshButton.Enabled = true;
        }
        #endregion

        #region New USB Device Event
        private const int WM_DEVICECHANGE = 0x0219;
        bool eventHappened = false;
        System.Timers.Timer newEventTimer;
        protected override void WndProc(ref Message m)
        {
            // Getting device change event and starting a new timer
            if (m.Msg == WM_DEVICECHANGE && m.WParam.ToInt32() == 0x0007)
            {
                if (newEventTimer == null)
                {
                    newEventTimer = new System.Timers.Timer(500);
                    newEventTimer.Elapsed += newEventTimer_Elapsed;
                    newEventTimer.Start();
                }

                else if (newEventTimer.Enabled == false)
                {
                    newEventTimer.Start();
                }

                else
                {
                    eventHappened = true;
                }
            }

            base.WndProc(ref m);
        }

        void newEventTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // If there was a change since timer started, then starting timer again, else stopping timer and updating device list
            // (used to avoid too much device list update due to multiple connection messages)
            if (eventHappened == true)
            {
                eventHappened = false;
            }

            else
            {
                newEventTimer.Stop();
                updateDeviceList();
            }
        }
        #endregion
    }
}