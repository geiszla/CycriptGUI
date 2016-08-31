using CycriptGUI.MainProgram;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CycriptGUI.LibIMobileDevice
{
    class AFC
    {
        internal enum AFCError
        {
            AFC_E_SUCCESS = 0,
            AFC_E_UNKNOWN_ERROR = 1,
            AFC_E_OP_HEADER_INVALID = 2,
            AFC_E_NO_RESOURCES = 3,
            AFC_E_READ_ERROR = 4,
            AFC_E_WRITE_ERROR = 5,
            AFC_E_UNKNOWN_PACKET_TYPE = 6,
            AFC_E_INVALID_ARG = 7,
            AFC_E_OBJECT_NOT_FOUND = 8,
            AFC_E_OBJECT_IS_DIR = 9,
            AFC_E_PERM_DENIED = 10,
            AFC_E_SERVICE_NOT_CONNECTED = 11,
            AFC_E_OP_TIMEOUT = 12,
            AFC_E_TOO_MUCH_DATA = 13,
            AFC_E_END_OF_DATA = 14,
            AFC_E_OP_NOT_SUPPORTED = 15,
            AFC_E_OBJECT_EXISTS = 16,
            AFC_E_OBJECT_BUSY = 17,
            AFC_E_NO_SPACE_LEFT = 18,
            AFC_E_OP_WOULD_BLOCK = 19,
            AFC_E_IO_ERROR = 20,
            AFC_E_OP_INTERRUPTED = 21,
            AFC_E_OP_IN_PROGRESS = 22,
            AFC_E_INTERNAL_ERROR = 23,
            AFC_E_MUX_ERROR = 30,
            AFC_E_NO_MEM = 31,
            AFC_E_NOT_ENOUGH_DATA = 32,
            AFC_E_DIR_NOT_EMPTY = 33,
            AFC_E_FORCE_SIGNED_TYPE = -1
        }

        #region DLL Import
        [DllImport(LibiMobileDevice.LIBIMOBILEDEVICE_DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        internal static extern AFCError afc_client_start_service(IntPtr deviceHandle, out IntPtr afcClient, string label);
        [DllImport(LibiMobileDevice.LIBIMOBILEDEVICE_DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        internal static extern AFCError afc_get_device_info(IntPtr afcClient, out IntPtr deviceInformation);
        #endregion

        public static bool GetCapacity(Device device, out double capacity)
        {
            // Must connect to device before calling

            capacity = -1;

            if (!device.Connect()) return false;

            List<string> informationList = new List<string>();

            IntPtr afcClient;
            AFCError returnCode = afc_client_start_service(device.Handle, out afcClient, "CycriptGUI");
            if (returnCode != AFCError.AFC_E_SUCCESS || afcClient == IntPtr.Zero)
            {
                return false;
            }

            IntPtr deviceInformation;
            returnCode = afc_get_device_info(afcClient, out deviceInformation);
            afc_client_free(afcClient);

            informationList = LibiMobileDevice.PtrToStringList(deviceInformation, 2);
            afc_dictionary_free(deviceInformation);

            device.Dispose();

            double byteCapacity = Convert.ToDouble(informationList[informationList.IndexOf("FSTotalBytes") + 1]);
            capacity = Math.Round(byteCapacity / (Math.Pow(2, 30)), 1);

            return true;
        }

        // Freeing
        #region Dll Import
        [DllImport(LibiMobileDevice.LIBIMOBILEDEVICE_DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        static extern AFCError afc_client_free(IntPtr client);
        [DllImport(LibiMobileDevice.LIBIMOBILEDEVICE_DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        static extern AFCError afc_dictionary_free(IntPtr dictionary);
        #endregion
    }
}
