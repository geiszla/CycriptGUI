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
        public const string LIBIMOBILEDEVICE_DLL_PATH = @"libimobiledevice.dll";
        const string LIBPLIST_DLL_PATH = @"libplist.dll";
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
        enum iDeviceEventType
        {
            IDEVICE_DEVICE_ADD = 1,
            IDEVICE_DEVICE_REMOVE
        }

        static iDeviceEventCallback deviceEventCallback;

        #region DllImports
        [DllImport(LIBPLIST_DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        static extern void plist_to_xml(IntPtr plist, out IntPtr xml, out int length);

        [DllImport(LIBIMOBILEDEVICE_DLL_PATH, EntryPoint = "idevice_new", CallingConvention = CallingConvention.Cdecl)]
        public static extern iDeviceError NewDevice(out IntPtr devicePtr, string udid);

        [DllImport(LIBIMOBILEDEVICE_DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        static extern iDeviceError idevice_get_device_list(out IntPtr devicesPtr, out int count);

        [DllImport(LIBIMOBILEDEVICE_DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        static extern iDeviceError idevice_event_subscribe(iDeviceEventCallback callback);

        [DllImport(LIBIMOBILEDEVICE_DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        static extern iDeviceError idevice_event_unsubscribe();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void iDeviceEventCallback(IntPtr deviceEventPtr);
        #endregion

        #region Main Functions
        static void OnDeviceStateChanged(iDeviceEventArgs e)
        {
            EventHandler handler = DeviceStateChanged;
            if (handler != null) handler(null, e);
        }

        public static event EventHandler DeviceStateChanged;

        public static bool SubscribeDeviceEvent()
        {
            if (deviceEventCallback != null) return false;
            deviceEventCallback = (IntPtr deviceEventPtr) =>
            {
                iDeviceEvent deviceEvent = (iDeviceEvent)Marshal.PtrToStructure(deviceEventPtr, typeof(iDeviceEvent));
                OnDeviceStateChanged(new iDeviceEventArgs(deviceEvent));
            };

            iDeviceError returnCode = idevice_event_subscribe(deviceEventCallback);
            return returnCode == iDeviceError.IDEVICE_E_SUCCESS;
        }

        public static void UnsubscribeDeviceEvent()
        {
            idevice_event_unsubscribe();
            deviceEventCallback = null;
        }

        public static iDeviceError GetDeviceList(out List<iDevice> deviceList)
        {
            List<iDeviceHandle> devices = new List<iDeviceHandle>();
            IntPtr devicesPtr;
            iDeviceError returnCode = searchForDevices(out devices, out devicesPtr);

            deviceList = new List<iDevice>();
            if (returnCode != iDeviceError.IDEVICE_E_SUCCESS)
            {
                return returnCode;
            }

            foreach (iDeviceHandle currDeviceHandle in devices)
            {
                iDevice newDevice;
                bool success = GetDeviceFromHandle(currDeviceHandle, out newDevice);

                if (!success || newDevice == null) continue;

                deviceList.Add(newDevice);
            }

            return returnCode;
        }

        public static bool GetDeviceFromHandle(iDeviceHandle currDeviceHandle, out iDevice newDevice)
        {
            IntPtr lockdownService;
            IntPtr lockdownClient;
            Lockdown.LockdownError lockdownReturnCode = Lockdown.Start(currDeviceHandle.Handle, out lockdownClient, out lockdownService);

            newDevice = null;
            if (lockdownReturnCode != Lockdown.LockdownError.LOCKDOWN_E_SUCCESS)
            {
                idevice_free(currDeviceHandle.Handle);
                return false;
            }

            XDocument deviceProperties;
            lockdownReturnCode = Lockdown.GetProperties(lockdownClient, out deviceProperties);
            if (lockdownReturnCode != Lockdown.LockdownError.LOCKDOWN_E_SUCCESS || deviceProperties == default(XDocument))
            {
                lockdownReturnCode = Lockdown.FreeService(lockdownService);
                lockdownReturnCode = Lockdown.FreeClient(lockdownClient);
                idevice_free(currDeviceHandle.Handle);
                return false;
            }

            Lockdown.FreeService(lockdownService);
            Lockdown.FreeClient(lockdownClient);
            idevice_free(currDeviceHandle.Handle);

            IEnumerable<XElement> keys = deviceProperties.Descendants("dict").Descendants("key");
            newDevice = new iDevice(
                IntPtr.Zero,
                keys.Where(x => x.Value == "UniqueDeviceID").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                keys.Where(x => x.Value == "SerialNumber").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                keys.Where(x => x.Value == "DeviceName").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                keys.Where(x => x.Value == "ProductType").Select(x => (x.NextNode as XElement).Value).FirstOrDefault()
            );

            return true;
        }

        static iDeviceError searchForDevices(out List<iDeviceHandle> devices, out IntPtr devicesPtr)
        {
            int count;
            iDeviceError returnCode = idevice_get_device_list(out devicesPtr, out count);

            devices = new List<iDeviceHandle>();
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
                devices.Add(new iDeviceHandle(currDevice, currUdid));
                i = i + 4;
            }

            idevice_device_list_free(devicesPtr);
            return returnCode;
        }
        #endregion

        #region Other Functions
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

        static void eventCallback(IntPtr deviceEventPtr)
        {
            iDeviceEvent deviceEvent = (iDeviceEvent)Marshal.PtrToStructure(deviceEventPtr, typeof(iDeviceEvent));
            OnDeviceStateChanged(new iDeviceEventArgs(deviceEvent));
        }
        #endregion

        // Free Device
        #region Dll Imports
        [DllImport(LIBIMOBILEDEVICE_DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        static extern iDeviceError idevice_device_list_free(IntPtr devicesPtr);

        [DllImport(LIBIMOBILEDEVICE_DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
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

    #region iDevice Event

    public struct iDeviceEvent
    {
        public int EventType;
        public string Udid;
    }

    public class iDeviceEventArgs : EventArgs
    {
        public enum iDeviceEventType
        {
            IDEVICE_DEVICE_ADD = 1,
            IDEVICE_DEVICE_REMOVE
        }

        public iDeviceEventArgs(iDeviceEvent iDeviceEvent)
        {
            EventType = (iDeviceEventType)iDeviceEvent.EventType;
            Udid = iDeviceEvent.Udid;
        }

        public iDeviceEventType EventType;
        public string Udid;
    }
    #endregion
}
