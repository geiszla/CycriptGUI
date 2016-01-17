using CycriptGUI.LibIMobileDevice;
using CycriptGUI.MainProgram;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CycriptGUI
{
    public partial class StartForm : Form
    {
        #region Global Variables
        public static iDevice WorkingDevice;

        List<iDevice> Devices = new List<iDevice>();
        List<iDevice> listedDevices = new List<iDevice>();
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
            updateTask = Task.Factory.StartNew(() => { refreshDeviceList(); });
        }
        #endregion

        #region Other Events
        private void connectButton_Click(object sender, EventArgs e)
        {
            // Handle exceptional cases
            if (listedDevices != null && listedDevices.Count != 0)
            {
                WorkingDevice = Devices.Where(x => x.IsConnected == true).ToList()[selectDeviceBox.SelectedIndex];
            }

            else if (!updateTask.IsCompleted)
            {
                MessageBox.Show("Please wait, while the program finishes searching for devices!", "Search in progress",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                MessageBox.Show("Couldn't find any iOS device. Please check the connection and try again!", "No device detected",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If everything's OK, set up and start app selection form
            Form selectAppForm = new SelectForm() { Owner = this };
            selectAppForm.Location = new Point(
                Location.X + (Width - selectAppForm.Width) / 2,
                Location.Y + (Height - selectAppForm.Height) / 2);
            selectAppForm.FormClosing += (s, args) =>
                Location = new Point(
                    selectAppForm.Location.X + (selectAppForm.Width - Width) / 2,
                    selectAppForm.Location.Y + (selectAppForm.Height - Height) / 2);
            selectAppForm.Closed += (s, args) => Show();

            Hide();
            selectAppForm.Show();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            Task updateTask = Task.Factory.StartNew(() => { refreshDeviceList(); });
        }

        private void manageButton_Click(object sender, EventArgs e)
        {
            Ssh.Connect(listedDevices[selectDeviceBox.SelectedIndex].Udid);
        }
        #endregion

        #region Update Device List Functions
        void refreshDeviceList()
        {
            BeginInvoke(new MethodInvoker(delegate { initializeOrFinalizeRefresh(true); }));

            // Get new devices and remove duplicates
            List<iDevice> deviceList;
            LibiMobileDevice.GetDeviceList(out deviceList);
            deviceList = deviceList.GroupBy(x => x.Udid).Select(x => x.First()).ToList();

            // If the list haven't changed finalize refresh
            if (deviceList.Join(listedDevices, x => x.Udid, y => y.Udid, (x, y) => x).Count() == deviceList.Count)
            {
                BeginInvoke(new MethodInvoker(delegate { initializeOrFinalizeRefresh(false); }));
                return;
            }

            // Set found devices "IsConnected" property to "true", and the others' to "false"
            foreach (iDevice currDevice in Devices)
            {
                currDevice.IsConnected = false;
                foreach (iDevice currDevice2 in deviceList)
                {
                    if (currDevice.Udid == currDevice2.Udid) currDevice.IsConnected = true;
                }
            }

            // Add new devices to the list
            List<iDevice> newDevices = deviceList.Where(x => !Devices.Any(y => y.Udid == x.Udid)).ToList();
            newDevices.ForEach(x => x.IsConnected = true);
            Devices.AddRange(newDevices);

            BeginInvoke(new MethodInvoker(() => { applyDeviceList(); initializeOrFinalizeRefresh(false); }));
        }

        void applyDeviceList()
        {
            // Determine currently selected device
            string selectedUdid = listedDevices.Count != 0 ? listedDevices[selectDeviceBox.SelectedIndex].Udid : null;

            // Clear combo box and if no devices connected, change its text to error message
            selectDeviceBox.Items.Clear();

            List<iDevice> connectedDevices = Devices.Where(x => x.IsConnected).ToList();
            if (connectedDevices.Count == 0)
            {
                selectedUdid = null;

                selectDeviceBox.Items.Add("No iDevice Has Been Detected...");
                selectDeviceBox.SelectedIndex = 0;
                manageButton.Enabled = false;

                return;
            }

            // Else refresh device combo box
            listedDevices = connectedDevices;
            foreach (iDevice currDevice in connectedDevices)
            {
                // Add devices to combo box
                string currItemString = currDevice.Name + " (" + currDevice.ProductName + ")";

                int sameNameCount = connectedDevices.Where(x => x.Name == currDevice.Name).Count();
                int sameProductCount = connectedDevices.Where(x => x.ProductType == currDevice.ProductType).Count();
                if (sameNameCount > 1 && sameProductCount > 1)
                {
                    currItemString += " (" + currDevice.SerialNumber + ")";
                }

                selectDeviceBox.Items.Add(currItemString);
            }
            
            // If last selected index can't be determined, return
            if (listedDevices.Count == 0 || selectedUdid == null)
            {
                selectDeviceBox.SelectedIndex = 0;
                return;
            }

            // Else set the currently selected index to last selected index
            int udidIndex = connectedDevices.IndexOf(connectedDevices.Where(x => x.Udid == selectedUdid).FirstOrDefault());
            selectDeviceBox.SelectedIndex = udidIndex != -1 ? udidIndex : 0;
        }

        void initializeOrFinalizeRefresh(bool isInitialization)
        {
            refreshButton.Enabled = !isInitialization;
            manageButton.Enabled = !isInitialization;
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
                refreshDeviceList();
            }
        }
        #endregion
    }
}