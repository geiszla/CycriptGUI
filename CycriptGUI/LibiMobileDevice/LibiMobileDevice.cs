using CycriptGUI.MainProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace CycriptGUI.LibIMobileDevice
{
    class LibiMobileDevice
    {
        public const string LibimobiledeviceDllPath = @"libimobiledevice.dll";
        public const string LibplistDllPath = @"libplist.dll";
        public enum iDeviceError
        {
            IDEVICE_E_SUCCESS = 0,
            IDEVICE_E_INVALID_ARG = -1,
            IDEVICE_E_UNKNOWN_ERROR = -2,
            IDEVICE_E_NO_DEVICE = -3,
            IDEVICE_E_NOT_ENOUGH_DATA = -4,
            IDEVICE_E_BAD_HEADER = -5,
            IDEVICE_E_SSL_ERROR = -6
        }

        // Get Devices
        #region DllImports
        [DllImport(LibplistDllPath, CallingConvention = CallingConvention.Cdecl)]
        static extern void plist_to_xml(IntPtr plist, out IntPtr xml, out int length);

        [DllImport(LibimobiledeviceDllPath, EntryPoint = "idevice_new", CallingConvention = CallingConvention.Cdecl)]
        public static extern iDeviceError NewDevice(out IntPtr iDevice, string udid);

        [DllImport(LibimobiledeviceDllPath, CallingConvention = CallingConvention.Cdecl)]
        static extern iDeviceError idevice_get_device_list(out IntPtr devices, out IntPtr count);

        [DllImport(LibimobiledeviceDllPath, CallingConvention = CallingConvention.Cdecl)]
        static extern iDeviceError idevice_get_udid(IntPtr device, out string udid);
        #endregion

        public static iDeviceError GetDeviceList(out List<IDevice> deviceList)
        {
            List<IDevice> devices = new List<IDevice>();
            IntPtr devicesPtr;
            iDeviceError returnCode = SearchForDevices(out devices, out devicesPtr);

            devices = devices.Where(x => devices.Skip(devices.IndexOf(x) + 1).Count(y => y.Udid == x.Udid) == 0).ToList();

            deviceList = new List<IDevice>();
            foreach (IDevice currDevice in devices)
            {
                IntPtr lockdownService;
                IntPtr lockdownClient;
                Lockdown.LockdownError lockdownReturnCode = Lockdown.Start(currDevice.Handle, out lockdownClient, out lockdownService);

                XDocument deviceProperties;
                lockdownReturnCode = Lockdown.GetProperties(lockdownClient, out deviceProperties);

                IEnumerable<XElement> keys = deviceProperties.Descendants("dict").Descendants("key");
                deviceList.Add(new IDevice(
                    IntPtr.Zero,
                    keys.Where(x => x.Value == "UniqueDeviceID").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                    keys.Where(x => x.Value == "SerialNumber").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                    keys.Where(x => x.Value == "DeviceName").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                    keys.Where(x => x.Value == "ProductType").Select(x => (x.NextNode as XElement).Value).FirstOrDefault()
                    ));

                // Freeing
                lockdownReturnCode = Lockdown.FreeService(lockdownService);
                lockdownReturnCode = Lockdown.FreeClient(lockdownClient);
                returnCode = FreeDevice(currDevice.Handle);
            }

            return returnCode;
        }

        public static iDeviceError SearchForDevices(out List<IDevice> devices, out IntPtr devicesPtr)
        {
            IntPtr countPtr;
            iDeviceError returnCode = idevice_get_device_list(out devicesPtr, out countPtr);

            devices = new List<IDevice>();
            if (Marshal.ReadInt32(devicesPtr) != 0)
            {
                string currUdid;
                int i = 0;
                while ((currUdid = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(devicesPtr, i))) != null)
                {
                    IntPtr currDevice;
                    returnCode = NewDevice(out currDevice, currUdid);
                    devices.Add(new IDevice(currDevice, currUdid));
                    i = i + 4;
                }

                FreeDevices(devicesPtr);
            }

            return returnCode;
        }

        public static XDocument PlistToXml(IntPtr plistPtr)
        {
            IntPtr xmlPtr;
            int length;
            plist_to_xml(plistPtr, out xmlPtr, out length);

            byte[] resultBytes = new byte[length];
            Marshal.Copy(xmlPtr, resultBytes, 0, length);

            string resultString = Encoding.UTF8.GetString(resultBytes);
            return XDocument.Parse(resultString);
        }

        // Free Devices
        #region Dll Imports
        [DllImport(LibimobiledeviceDllPath, EntryPoint = "idevice_device_list_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern iDeviceError FreeDevices(IntPtr devices);

        [DllImport(LibimobiledeviceDllPath, EntryPoint = "idevice_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern iDeviceError FreeDevice(IntPtr device);
        #endregion
    }
}
