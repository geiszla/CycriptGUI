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
            helpBox.Text = @"This tool uses SSH connection to the iDevice to make reverse engineering easier by automating long, often used tasks.

Requirements
You need an Apple device running jaibroken iOs with the following packages installed:
 - OpenSSH (Cydia/Telesphoreo)
 - Cycript (Cydia/Telesphoreo)";
        }

        private void StartForm_Shown(object sender, EventArgs e)
        {
            selectDeviceBox.SelectedIndex = 0;

            Task.Factory.StartNew(() => refreshDeviceList());

            // Subscribe to device event
            bool success = LibiMobileDevice.SubscribeDeviceEvent();
            if (!success)
            {
                showError("Couldn't subscribe to device change event. You will have to refresh the device list manually.",
                    "Device event subscription failed");
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
        private void connectButton_Click(object sender, EventArgs e)
        {
            // Handle exceptional cases
            if (listedDevices != null && listedDevices.Count != 0)
            {
                WorkingDevice = Devices.Where(x => x.IsConnected == true).ToList()[selectDeviceBox.SelectedIndex];
            }

            else if (isUpdateInProgress)
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

            // Attempt to connect to Installation Proxy
            if (!connectToInstallationProxy()) return;

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

        private void loadButton_Click(object sender, EventArgs e)
        {
            showNotImplementedMessage();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => refreshDeviceList());
        }

        private void manageButton_Click(object sender, EventArgs e)
        {
            showNotImplementedMessage();
        }

        private void LibiMobileDevice_DeviceStateChanged(object sender, EventArgs e)
        {
            // Initialize refresh
            if (isUpdateInProgress) return;
            initializeOrFinalizeRefresh(true);

            // Remove current device from the device list
            iDeviceEventArgs eventArgs = (iDeviceEventArgs)e;
            Devices.RemoveAll(x => x.Udid == eventArgs.Udid);

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
                LibiMobileDevice.iDeviceError returnCode = LibiMobileDevice.NewDevice(out devicePtr, eventArgs.Udid);
                if (returnCode != LibiMobileDevice.iDeviceError.IDEVICE_E_SUCCESS || devicePtr == IntPtr.Zero)
                {
                    showError("Couldn't connect to the device with UDID \'" + eventArgs.Udid + "\'. Please check the connection.",
                        "Connection failed");
                    initializeOrFinalizeRefresh(false);
                    return;
                }

                // Get device properties
                iDeviceHandle senderHandle = new iDeviceHandle(devicePtr, eventArgs.Udid);

                iDevice newDevice;
                if (!LibiMobileDevice.GetDeviceFromHandle(senderHandle, out newDevice) || newDevice == null)
                {
                    showError("Couldn't get properties of the device with UDID \'" + senderHandle.Udid + "\'. Please check the connection.",
                        "Get properties failed.");
                    initializeOrFinalizeRefresh(false);
                    return;
                }

                // Add new device to the list
                Devices.Add(newDevice);
            }

            initializeOrFinalizeRefresh(false, true);
        }
        #endregion

        #region Update Device List
        void refreshDeviceList()
        {
            initializeOrFinalizeRefresh(true);

            // Get new devices and remove duplicates
            List<iDevice> deviceList;
            LibiMobileDevice.iDeviceError returnCode = LibiMobileDevice.GetDeviceList(out deviceList);
            if (returnCode != LibiMobileDevice.iDeviceError.IDEVICE_E_SUCCESS
                && returnCode != LibiMobileDevice.iDeviceError.IDEVICE_E_NO_DEVICE)
            {
                showError("Couldn't get device list. Please try again.", "Device search failed");
                initializeOrFinalizeRefresh(false);
                return;
            }

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

        void applyDeviceList()
        {
            // Determine currently selected device
            string selectedUdid = listedDevices.Count != 0 ? listedDevices[selectDeviceBox.SelectedIndex].Udid : null;

            // Clear combo box and if no device is connected, change its text to error message
            selectDeviceBox.Items.Clear();

            List<iDevice> connectedDevices = Devices.Where(x => x.IsConnected).ToList();
            if (connectedDevices.Count == 0)
            {
                listedDevices.Clear();

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

            isUpdateInProgress = false;
        }

        void initializeOrFinalizeRefresh(bool isInitialization, bool isApply = false)
        {
            if (isInitialization) isUpdateInProgress = true;

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    refreshButton.Enabled = !isInitialization;
                    manageButton.Enabled = !isInitialization;
                }));

                if (isApply)
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        applyDeviceList();
                    }));
                }
            }

            else
            {
                refreshButton.Enabled = !isInitialization;
                manageButton.Enabled = !isInitialization;

                if (isApply) applyDeviceList();
            }

            if (!isInitialization) isUpdateInProgress = false;
        }
        #endregion

        #region Other Functions
        bool connectToInstallationProxy()
        {
            Cursor = Cursors.AppStarting;

            // Connect to the iDevice
            iDevice workingDevice = StartForm.WorkingDevice;
            LibiMobileDevice.iDeviceError deviceReturnCode = LibiMobileDevice.NewDevice(out workingDevice.Handle, workingDevice.Udid);
            if (deviceReturnCode != LibiMobileDevice.iDeviceError.IDEVICE_E_SUCCESS)
            {
                showError("Couldn't connect to " + workingDevice.Name + ". Please check the connection.",
                    "Connection failed");
                return false;
            }

            // Start Installation Proxy service
            Lockdown.LockdownError lockdownReturnCode = Lockdown.Start(
                workingDevice.Handle,
                out workingDevice.LockdownClient,
                out workingDevice.InstallationProxyService);
            if (lockdownReturnCode != Lockdown.LockdownError.LOCKDOWN_E_SUCCESS)
            {
                showError("Couldn't start Installation Proxy. Please check the connection to the device.",
                    "Installation Proxy start failed");
                return false;
            }

            // Connect to Installation Proxy service
            InstallationProxy.InstproxyError installProxyReturnCode = InstallationProxy.Connect(
                workingDevice.Handle,
                workingDevice.InstallationProxyService,
                out workingDevice.InstallationProxyClient);
            if (installProxyReturnCode != InstallationProxy.InstproxyError.INSTPROXY_E_SUCCESS)
            {
                showError("Couldn't connect to Installation Proxy. Please check the connection to the device.",
                    "Installation Proxy connection failed");
                return false;
            }

            Cursor = Cursors.Default;
            return true;
        }

        void showNotImplementedMessage()
        {
            MessageBox.Show("This feature hasn't been implemented yet.", "Not implemented",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void showError(string errorString, string errorTitle)
        {
            MessageBox.Show(errorString, "Error: " + errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion
    }
}