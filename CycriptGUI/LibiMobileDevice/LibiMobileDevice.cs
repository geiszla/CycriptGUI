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
        public static extern iDeviceError NewDevice(out IntPtr devicePtr, string udid);

        [DllImport(LibimobiledeviceDllPath, CallingConvention = CallingConvention.Cdecl)]
        static extern iDeviceError idevice_get_device_list(out IntPtr devicesPtr, out int count);

        [DllImport(LibimobiledeviceDllPath, CallingConvention = CallingConvention.Cdecl)]
        static extern iDeviceError idevice_get_udid(IntPtr devicePtr, out string udid);
        #endregion

        public static iDeviceError GetDeviceList(out List<iDevice> deviceList)
        {
            List<ResultDevice> devices = new List<ResultDevice>();
            IntPtr devicesPtr;
            iDeviceError returnCode = searchForDevices(out devices, out devicesPtr);

            deviceList = new List<iDevice>();
            if (returnCode != iDeviceError.IDEVICE_E_SUCCESS)
            {
                return returnCode;
            }

            foreach (ResultDevice currDevice in devices)
            {
                IntPtr lockdownService;
                IntPtr lockdownClient;
                Lockdown.LockdownError lockdownReturnCode = Lockdown.Start(currDevice.Handle, out lockdownClient, out lockdownService);

                if (lockdownReturnCode != Lockdown.LockdownError.LOCKDOWN_E_SUCCESS)
                {
                    idevice_free(currDevice.Handle);
                    continue;
                }

                XDocument deviceProperties;
                lockdownReturnCode = Lockdown.GetProperties(lockdownClient, out deviceProperties);
                if (lockdownReturnCode != Lockdown.LockdownError.LOCKDOWN_E_SUCCESS || deviceProperties == default(XDocument))
                {
                    lockdownReturnCode = Lockdown.FreeService(lockdownService);
                    lockdownReturnCode = Lockdown.FreeClient(lockdownClient);
                    idevice_free(currDevice.Handle);
                    continue;
                }

                IEnumerable<XElement> keys = deviceProperties.Descendants("dict").Descendants("key");
                deviceList.Add(new iDevice(
                    IntPtr.Zero,
                    keys.Where(x => x.Value == "UniqueDeviceID").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                    keys.Where(x => x.Value == "SerialNumber").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                    keys.Where(x => x.Value == "DeviceName").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                    keys.Where(x => x.Value == "ProductType").Select(x => (x.NextNode as XElement).Value).FirstOrDefault()
                    ));

                Lockdown.FreeService(lockdownService);
                Lockdown.FreeClient(lockdownClient);
                idevice_free(currDevice.Handle);
            }

            return returnCode;
        }

        static iDeviceError searchForDevices(out List<ResultDevice> devices, out IntPtr devicesPtr)
        {
            int count;
            iDeviceError returnCode = idevice_get_device_list(out devicesPtr, out count);

            devices = new List<ResultDevice>();
            if (returnCode != iDeviceError.IDEVICE_E_SUCCESS)
            {
                return returnCode;
            }

            else if (devicesPtr == IntPtr.Zero)
            {
                return iDeviceError.IDEVICE_E_UNKNOWN_ERROR;
            }

            else if (Marshal.ReadInt32(devicesPtr) == 0)
            {
                return iDeviceError.IDEVICE_E_NO_DEVICE;
            }

            string currUdid;
            int i = 0;
            while ((currUdid = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(devicesPtr, i))) != null
                && devices.Count(x => x.Udid == currUdid) == 0)
            {
                IntPtr currDevice;
                returnCode = NewDevice(out currDevice, currUdid);
                devices.Add(new ResultDevice(currDevice, currUdid));
                i = i + 4;
            }

            idevice_device_list_free(devicesPtr);

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
            XDocument resultXml;
            try
            {
                resultXml = XDocument.Parse(resultString);
            }

            catch
            {
                resultXml = new XDocument();
            }

            return resultXml;
        }

        // Free Device
        #region Dll Imports
        [DllImport(LibimobiledeviceDllPath, CallingConvention = CallingConvention.Cdecl)]
        static extern iDeviceError idevice_device_list_free(IntPtr devicesPtr);

        [DllImport(LibimobiledeviceDllPath, CallingConvention = CallingConvention.Cdecl)]
        static extern iDeviceError idevice_free(IntPtr devicePtr);
        #endregion

        public static void FreeDevice(iDevice device)
        {
            Lockdown.FreeClient(device.LockdownClient);
            device.LockdownClient = IntPtr.Zero;

            idevice_free(device.Handle);
            device.Handle = IntPtr.Zero;
        }
    }
}
