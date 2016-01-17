using System;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace CycriptGUI.LibIMobileDevice
{
    class Lockdown
    {
        public enum LockdownError
        {
            LOCKDOWN_E_SUCCESS = 0,
            LOCKDOWN_E_INVALID_ARG = -1,
            LOCKDOWN_E_INVALID_CONF = -2,
            LOCKDOWN_E_PLIST_ERROR = -3,
            LOCKDOWN_E_PAIRING_FAILED = -4,
            LOCKDOWN_E_SSL_ERROR = -5,
            LOCKDOWN_E_DICT_ERROR = -6,
            LOCKDOWN_E_NOT_ENOUGH_DATA = -7,
            LOCKDOWN_E_MUX_ERROR = -8,
            LOCKDOWN_E_NO_RUNNING_SESSION = -9,
            LOCKDOWN_E_INVALID_RESPONSE = -10,
            LOCKDOWN_E_MISSING_KEY = -11,
            LOCKDOWN_E_MISSING_VALUE = -12,
            LOCKDOWN_E_GET_PROHIBITED = -13,
            LOCKDOWN_E_SET_PROHIBITED = -14,
            LOCKDOWN_E_REMOVE_PROHIBITED = -15,
            LOCKDOWN_E_IMMUTABLE_VALUE = -16,
            LOCKDOWN_E_PASSWORD_PROTECTED = -17,
            LOCKDOWN_E_USER_DENIED_PAIRING = -18,
            LOCKDOWN_E_PAIRING_DIALOG_RESPONSE_PENDING = -19,
            LOCKDOWN_E_MISSING_HOST_ID = -20,
            LOCKDOWN_E_INVALID_HOST_ID = -21,
            LOCKDOWN_E_SESSION_ACTIVE = -22,
            LOCKDOWN_E_SESSION_INACTIVE = -23,
            LOCKDOWN_E_MISSING_SESSION_ID = -24,
            LOCKDOWN_E_INVALID_SESSION_ID = -25,
            LOCKDOWN_E_MISSING_SERVICE = -26,
            LOCKDOWN_E_INVALID_SERVICE = -27,
            LOCKDOWN_E_SERVICE_LIMIT = -28,
            LOCKDOWN_E_MISSING_PAIR_RECORD = -29,
            LOCKDOWN_E_SAVE_PAIR_RECORD_FAILED = -30,
            LOCKDOWN_E_INVALID_PAIR_RECORD = -31,
            LOCKDOWN_E_MISSING_ACTIVATION_RECORD = -33,
            LOCKDOWN_E_SERVICE_PROHIBITED = -34,
            LOCKDOWN_E_ESCROW_LOCKED = -35,
            LOCKDOWN_E_UNKNOWN_ERROR = -256
        }
        const string serviceIdentifier = "com.apple.mobile.installation_proxy";

        // Connect
        #region Dll Imports
        [DllImport(LibiMobileDevice.LibimobiledeviceDllPath, CallingConvention = CallingConvention.Cdecl)]
        static extern LockdownError lockdownd_client_new_with_handshake(IntPtr devicePtr, out IntPtr lockDownClient, string label);

        [DllImport(LibiMobileDevice.LibimobiledeviceDllPath, CallingConvention = CallingConvention.Cdecl)]
        static extern LockdownError lockdownd_start_service(IntPtr lockDownClient, string identifier, out IntPtr service);
        #endregion

        public static LockdownError Start(IntPtr device, out IntPtr client, out IntPtr service)
        {
            LockdownError returnCode = lockdownd_client_new_with_handshake(device, out client, "CycriptGUI");
            service = IntPtr.Zero;
            if (returnCode != LockdownError.LOCKDOWN_E_SUCCESS)
            {
                return returnCode;
            }

            else if (client == IntPtr.Zero)
            {
                return LockdownError.LOCKDOWN_E_UNKNOWN_ERROR;
            }

            returnCode = lockdownd_start_service(client, serviceIdentifier, out service);
            if (service == IntPtr.Zero) return LockdownError.LOCKDOWN_E_UNKNOWN_ERROR;

            return returnCode;
        }

        // Working With Lockdown Service
        #region Dll Imports
        [DllImport(LibiMobileDevice.LibimobiledeviceDllPath, CallingConvention = CallingConvention.Cdecl)]
        static extern LockdownError lockdownd_get_value(IntPtr lockdownClient, string domain, string key, out IntPtr result);
        #endregion

        public static LockdownError GetProperties(IntPtr lockdownClient, out XDocument result)
        {
            IntPtr resultPlist;
            LockdownError returnCode = lockdownd_get_value(lockdownClient, null, null, out resultPlist);

            result = new XDocument();
            if (returnCode != LockdownError.LOCKDOWN_E_SUCCESS)
            {
                return returnCode;
            }

            else if (resultPlist == IntPtr.Zero)
            {
                return LockdownError.LOCKDOWN_E_UNKNOWN_ERROR;
            }

            result = LibiMobileDevice.PlistToXml(resultPlist);
            return returnCode;
        }

        // Free Clients and Services
        #region Dll Imports
        [DllImport(LibiMobileDevice.LibimobiledeviceDllPath, EntryPoint = "lockdownd_client_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern LockdownError FreeClient(IntPtr client);

        [DllImport(LibiMobileDevice.LibimobiledeviceDllPath, EntryPoint = "lockdownd_service_descriptor_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern LockdownError FreeService(IntPtr service);
        #endregion
    }
}
