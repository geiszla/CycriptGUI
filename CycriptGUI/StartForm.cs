using CycriptGUI.LibIMobileDevice;
using CycriptGUI.MainProgram;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CycriptGUI
{
    public partial class StartForm : Form
    {
        public static iDevice WorkingDevice;

        List<iDevice> Devices = new List<iDevice>();
        List<iDevice> listedDevices = new List<iDevice>();
        bool isUpdateInProgress;

        public StartForm()
        {
            InitializeComponent();
        }

        #region Form Events
        private void StartForm_Load(object sender, EventArgs e)
        {
            //            helpBox.Text = @"This tool uses SSH connection to the iDevice to make reverse engineering easier by automating long, often used tasks.

            //Requirements
            //You need an Apple device running jaibroken iOs with the following packages installed:
            // - OpenSSH (Cydia/Telesphoreo)
            // - Cycript (Cydia/Telesphoreo)";
        }

        private void StartForm_Shown(object sender, EventArgs e)
        {
            selectDeviceBox.SelectedIndex = 0;

            Task.Factory.StartNew(() => refreshDeviceList());

            // Subscribe to device event
            bool success = LibiMobileDevice.SubscribeDeviceEvent();
            if (!success)
            {
                Alert.ShowError("Couldn't subscribe to device change event. You will have to refresh the device list manually.",
                    "Device Event Subscription Failed");
                return;
            }
            LibiMobileDevice.DeviceStateChanged += LibiMobileDevice_DeviceStateChanged;
        }

        private void StartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LibiMobileDevice.UnsubscribeDeviceEvent();
        }
        #endregion

        #region Other Events
        private void selectDeviceBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listedDevices.Count != 0)
            {
                iDevice selectedDevice = listedDevices[selectDeviceBox.SelectedIndex];
                setPropertyLabels(selectedDevice);
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            // Handle exceptional cases
            if (listedDevices != null && listedDevices.Count != 0)
            {
                WorkingDevice = Devices.Where(x => x.IsConnected == true).ToList()[selectDeviceBox.SelectedIndex];
            }

            else if (isUpdateInProgress)
            {
                Alert.ShowError("Please wait, while the program finishes searching for devices!", "Search In Progress");
                return;
            }

            else
            {
                Alert.ShowError("Couldn't find any iOS device. Please check the connection and try again!", "No device detected");
                return;
            }
            
            Cursor = Cursors.AppStarting;

            // Connect to the device
            if (!WorkingDevice.Connect())
            {
                Alert.ShowError("Couldn't connect to the device. Please check the connection.",
                    "Installation Proxy Connection Failed");
            }

            // Connect to Installation Proxy
            if (!InstallationProxy.Connect(WorkingDevice))
            {
                Alert.ShowError("Couldn't connect to Installation Proxy. Please check the connection to the device.",
                    "Installation Proxy Connection Failed");
                return;
            }

            Cursor = Cursors.Default;

            // If everything's OK, set up and start app selection form
            Form selectAppForm = new SelectForm() { Owner = this };
            selectAppForm.Location = new Point(
                Location.X + (Width - selectAppForm.Width) / 2,
                Location.Y + (Height - selectAppForm.Height) / 2);
            selectAppForm.FormClosing += (s, args) =>
                Location = new Point(
                    selectAppForm.Location.X + (selectAppForm.Width - Width) / 2,
                    selectAppForm.Location.Y + (selectAppForm.Height - Height) / 2);
            selectAppForm.Closed += (s, args) => { Show(); };

            Hide();
            selectAppForm.Show();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            Alert.ShowNotImplementedMessage();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => refreshDeviceList());
        }

        private void manageButton_Click(object sender, EventArgs e)
        {
            WorkingDevice = listedDevices[selectDeviceBox.SelectedIndex];
            ManageForm manageForm = new ManageForm();
            manageForm.ShowDialog();
        }

        private void LibiMobileDevice_DeviceStateChanged(object sender, EventArgs e)
        {
            // Initialize refresh
            if (isUpdateInProgress) return;
            initializeOrFinalizeRefresh(true);

            iDeviceEventArgs eventArgs = (iDeviceEventArgs)e;

            // If the device was removed refresh the list again for WiFi connection
            if (eventArgs.EventType == iDeviceEventArgs.iDeviceEventType.IDEVICE_DEVICE_REMOVE)
            {
                Task.Factory.StartNew(() => refreshDeviceList());
                return;
            }

            else if (eventArgs.EventType == iDeviceEventArgs.iDeviceEventType.IDEVICE_DEVICE_ADD)
            {
                // If the device was added connect to it
                IntPtr devicePtr;
                if (!LibiMobileDevice.NewDevice(out devicePtr, eventArgs.Udid))
                {
                    Alert.ShowError($"Couldn't connect to the device with UDID '{eventArgs.Udid}'. Please check the connection.",
                        "Connection Failed");
                    initializeOrFinalizeRefresh(false);
                    return;
                }

                // Get device properties
                iDeviceHandle senderHandle = new iDeviceHandle(devicePtr, eventArgs.Udid);

                iDevice newDevice;
                if (!(senderHandle.GetIDeviceFromHandle(out newDevice)) || newDevice == null)
                {
                    Alert.ShowError($"Couldn't get properties of the device with UDID '{senderHandle.Udid}'. Please check the connection.",
                        "Get Properties Failed.");
                    initializeOrFinalizeRefresh(false);
                    return;
                }

                // Remove and add the new device to the list
                Devices.RemoveAll(x => x.Udid == eventArgs.Udid);
                Devices.Add(newDevice);
            }

            initializeOrFinalizeRefresh(false, true);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Label targetLabel = (Label)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            string copyString = "";
            Regex nameRegex = new Regex("([a-z]*)TitleLabel");
            if (nameRegex.IsMatch(targetLabel.Name))
            {
                string namePrefix = nameRegex.Match(targetLabel.Name).Groups[1].ToString();
                copyString = ((Label)Controls.Find(namePrefix + "Label", true).FirstOrDefault()).Text;
            }

            else
            {
                copyString = targetLabel.Text;
            }

            Clipboard.SetText(copyString);
        }
        #endregion

        #region Update Device List Functions
        void refreshDeviceList()
        {
            initializeOrFinalizeRefresh(true);

            // Get new devices
            List<iDevice> deviceList;
            if (!LibiMobileDevice.GetDeviceList(out deviceList))
            {
                Alert.ShowError("Couldn't get device list. Please try again.", "Device Search Failed");
                initializeOrFinalizeRefresh(false);
                return;
            }

            // Remove duplicates
            deviceList = deviceList.GroupBy(x => x.Udid).Select(x => x.First()).ToList();

            // If the list haven't changed finalize refresh
            if (deviceList != null && deviceList.Count != 0 && listedDevices.Count == deviceList.Count &&
                deviceList.Join(listedDevices, x => x.Udid, y => y.Udid, (x, y) => x).Count() == deviceList.Count)
            {
                initializeOrFinalizeRefresh(false);
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
            Devices = deviceList.Union(Devices, new iDeviceEqualityComparer()).ToList();

            initializeOrFinalizeRefresh(false, true);
        }

        void initializeOrFinalizeRefresh(bool isInitialization, bool isApply = false)
        {
            if (isInitialization) isUpdateInProgress = true;

            // Enable/disable controls and apply device list to combo box
            if (InvokeRequired)
            {
                if (isApply)
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        applyDeviceList();
                    }));
                }

                BeginInvoke(new MethodInvoker(() =>
                {
                    setControls(isInitialization);
                }));
            }

            else
            {
                if (isApply) applyDeviceList();
                setControls(isInitialization);
            }

            if (!isInitialization) isUpdateInProgress = false;
        }

        void applyDeviceList()
        {
            // Determine currently selected device
            string selectedUdid = listedDevices.Count != 0 ? listedDevices[selectDeviceBox.SelectedIndex].Udid : null;

            // Clear combo box and if no device is connected, set controls
            selectDeviceBox.Items.Clear();

            List<iDevice> connectedDevices = Devices.Where(x => x.IsConnected).ToList();
            if (connectedDevices.Count == 0)
            {
                listedDevices.Clear();

                selectDeviceBox.Items.Add("No iDevice Has Been Detected...");
                selectDeviceBox.SelectedIndex = 0;

                nameLabel.Text = "N/A";
                modelLabel.Text = "N/A";
                iosVersionLabel.Text = "N/A";
                capacityLabel.Text = "N/A";
                serialNumberLabel.Text = "N/A";
                udidLabel.Text = "N/A";
                statusLabel.Text = "Offline";
                phoneNumberLabel.Text = "N/A";
                regionLabel.Text = "N/A";
                cpuLabel.Text = "N/A";

                return;
            }

            // Else refresh device combo box
            listedDevices = connectedDevices;
            foreach (iDevice currDevice in connectedDevices)
            {
                // Add devices to combo box
                string currItemString = $"{currDevice.Name} ({currDevice.ProductName})";

                int sameNameCount = connectedDevices.Where(x => x.Name == currDevice.Name).Count();
                int sameProductCount = connectedDevices.Where(x => x.ProductType == currDevice.ProductType).Count();
                if (sameNameCount > 1 && sameProductCount > 1)
                {
                    currItemString += $" ({ currDevice.SerialNumber})";
                }

                selectDeviceBox.Items.Add(currItemString);
            }

            // If last selected index can't be determined, set selected index to 0
            iDevice selectedDevice;
            if (listedDevices.Count == 0 || selectedUdid == null)
            {
                selectedDevice = connectedDevices[0];
                selectDeviceBox.SelectedIndex = 0;
            }

            // Else set the currently selected index to last selected index
            else
            {
                selectedDevice = connectedDevices.Where(x => x.Udid == selectedUdid).FirstOrDefault();

                if (selectedDevice == null) selectedDevice = connectedDevices[0];

                int udidIndex = connectedDevices.IndexOf(selectedDevice);
                selectDeviceBox.SelectedIndex = udidIndex != -1 ? udidIndex : 0;
            }

            // Apply device properties to the group box
            setPropertyLabels(selectedDevice);
        }

        void setControls(bool isInitialization)
        {
            // If this is finalization, check if there's device connected
            // and set controls accordingly
            if (!isInitialization)
            {
                
                bool isNoDeviceConnected = Devices.Where(x => x.IsConnected).Count() == 0;

                // Enable/disable controls
                selectDeviceBox.Enabled = !isNoDeviceConnected;
                manageButton.Enabled = !isNoDeviceConnected;
                propertiesGroup.Enabled = !isNoDeviceConnected;
                newSessionButton.Enabled = !isNoDeviceConnected;
            }

            refreshButton.Enabled = !isInitialization;
        }

        void setPropertyLabels(iDevice selectedDevice)
        {
            nameLabel.Text = selectedDevice.Name;
            modelLabel.Text = $"{selectedDevice.ProductName}  ({selectedDevice.ProductType})";
            iosVersionLabel.Text = selectedDevice.iOSVersion;
            serialNumberLabel.Text = selectedDevice.SerialNumber;
            udidLabel.Text = selectedDevice.Udid;
            statusLabel.Text = selectedDevice.IsConnected ? "Online" : "Offline";
            phoneNumberLabel.Text = selectedDevice.PhoneNumber != null ? selectedDevice.PhoneNumber : "N/A";
            regionLabel.Text = selectedDevice.Region;
            cpuLabel.Text = selectedDevice.CPUArchitecture;
            capacityLabel.Text = "Loading...";

            double storageCapacity;
            if (!AFC.GetCapacity(selectedDevice, out storageCapacity))
            {
                capacityLabel.Text = "N/A";
                return;
            }
            
            double standardCapacity = Math.Pow(2, Math.Floor(Math.Log(storageCapacity, 2)) + 1);
            capacityLabel.Text = $"{storageCapacity} GB ({standardCapacity} GB)";
        }

        #endregion

        #region Other Functions
        
        #endregion
    }
}