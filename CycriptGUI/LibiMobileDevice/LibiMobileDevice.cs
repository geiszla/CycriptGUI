using CycriptGUI.MainProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CycriptGUI.LibIMobileDevice
{
    class LibiMobileDevice
    {
        public const string LIBIMOBILEDEVICE_DLL_PATH = @"libimobiledevice.dll";
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
        [DllImport(LIBIMOBILEDEVICE_DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern iDeviceError idevice_new(out IntPtr devicePtr, string udid);

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
            DeviceStateChanged?.Invoke(null, e);
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

        public static bool GetDeviceList(out List<iDevice> deviceList)
        {
            List<iDeviceHandle> devices = new List<iDeviceHandle>();
            iDeviceError returnCode = searchForDevices(out devices);

            deviceList = new List<iDevice>();

            if (returnCode == iDeviceError.IDEVICE_E_NO_DEVICE) return true;
            if (returnCode != iDeviceError.IDEVICE_E_SUCCESS) return false;

            foreach (iDeviceHandle currDeviceHandle in devices)
            {
                iDevice newDevice;
                bool success = currDeviceHandle.GetIDeviceFromHandle(out newDevice);
                currDeviceHandle.Dispose();

                if (!success || newDevice == null) continue;

                deviceList.Add(newDevice);
            }

            return true;
        }

        static iDeviceError searchForDevices(out List<iDeviceHandle> devices)
        {
            IntPtr devicesPtr;
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
                if (!NewDevice(out currDevice, currUdid)) continue;
                devices.Add(new iDeviceHandle(currDevice, currUdid));
                i = i + 4;
            }

            idevice_device_list_free(devicesPtr);
            return returnCode;
        }

        public static bool NewDevice(out IntPtr devicePtr, string udid)
        {
            iDeviceError returnCode = idevice_new(out devicePtr, udid);
            if (returnCode != iDeviceError.IDEVICE_E_SUCCESS || devicePtr == IntPtr.Zero)
            {
                return false;
            }

            return true;

            // Must free device after using
        }
        #endregion

        #region Other Functions
        static void eventCallback(IntPtr deviceEventPtr)
        {
            iDeviceEvent deviceEvent = (iDeviceEvent)Marshal.PtrToStructure(deviceEventPtr, typeof(iDeviceEvent));
            OnDeviceStateChanged(new iDeviceEventArgs(deviceEvent));
        }

        public static List<string> PtrToStringList(IntPtr listPtr, int skip)
        {
            List<string> stringList = new List<string>();
            if (Marshal.ReadInt32(listPtr) != 0)
            {
                string currString;
                int i = skip * 4;
                while ((currString = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(listPtr, i))) != null)
                {
                    stringList.Add(currString);
                    i = i + 4;
                }
            }

            return stringList;
        }
        #endregion

        // Free Device
        #region Dll Imports
        [DllImport(LIBIMOBILEDEVICE_DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        static extern iDeviceError idevice_device_list_free(IntPtr devicesPtr);

        [DllImport(LIBIMOBILEDEVICE_DLL_PATH, EntryPoint = "idevice_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern iDeviceError FreeDevice(IntPtr devicePtr);
        #endregion
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
