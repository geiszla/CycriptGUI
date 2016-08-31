using CycriptGUI.LibIMobileDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CycriptGUI.MainProgram
{
    abstract public class Device
    {
        public IntPtr Handle;
        public string Udid;

        public bool Connect()
        {
            return LibiMobileDevice.NewDevice(out Handle, Udid);

            // Must free device after using
        }

        #region Dispose
        ~Device()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            LibiMobileDevice.FreeDevice(Handle);
            Handle = IntPtr.Zero;
        }
        #endregion
    }

    public class iDevice : Device, IDisposable
    {
        #region Device Hardware
        Dictionary<string, string> DeviceHardware = new Dictionary<string, string>()
        {
            { "iPhone1,1", "iPhone 1G" },
            { "iPhone1,2", "iPhone 3G" },
            { "iPhone2,1", "iPhone 3GS" },
            { "iPhone3,1", "iPhone 4" },
            { "iPhone3,2", "iPhone 4" },
            { "iPhone3,3", "iPhone 4" },
            { "iPhone4,1", "iPhone 4S" },
            { "iPhone5,1", "iPhone 5" },
            { "iPhone5,2", "iPhone 5" },
            { "iPhone5,3", "iPhone 5C" },
            { "iPhone5,4", "iPhone 5C" },
            { "iPhone6,1", "iPhone 5S" },
            { "iPhone6,2", "iPhone 5S" },
            { "iPhone7,1", "iPhone 6 Plus" },
            { "iPhone7,2", "iPhone 6" },
            { "iPhone8,1", "iPhone 6S" },
            { "iPhone8,2", "iPhone 6S Plus" },
            { "iPhone8,4", "iPhone SE" },

            { "iPod1,1", "iPod Touch 1G" },
            { "iPod2,1", "iPod Touch 2G" },
            { "iPod3,1", "iPod Touch 3G" },
            { "iPod4,1", "iPod Touch 4G" },
            { "iPod5,1", "iPod Touch 5G" },
            { "iPod7,1", "iPod Touch 6G" },

            { "iPad1,1", "iPad 1G" },
            { "iPad2,1", "iPad 2" },
            { "iPad2,2", "iPad 2" },
            { "iPad2,3", "iPad 2" },
            { "iPad2,4", "iPad 2" },
            { "iPad3,1", "iPad 3G" },
            { "iPad3,2", "iPad 3G" },
            { "iPad3,3", "iPad 3G" },
            { "iPad3,4", "iPad 4G" },
            { "iPad3,5", "iPad 4G" },
            { "iPad3,6", "iPad 4G" },
            { "iPad4,1", "iPad Air" },
            { "iPad4,2", "iPad Air" },
            { "iPad4,3", "iPad Air" },
            { "iPad5,3", "iPad Air 2" },
            { "iPad5,4", "iPad Air 2" },

            { "iPad2,5", "iPad Mini 1G" },
            { "iPad2,6", "iPad Mini 1G" },
            { "iPad2,7", "iPad Mini 1G" },
            { "iPad4,4", "iPad Mini 2" },
            { "iPad4,5", "iPad Mini 2" },
            { "iPad4,6", "iPad Mini 2" },
            { "iPad4,7", "iPad Mini 3" },
            { "iPad4,8", "iPad Mini 3" },
            { "iPad4,9", "iPad Mini 3" },
            { "iPad5,1", "iPad Mini 4" },
            { "iPad5,2", "iPad Mini 4" },

            { "iPad6,7", "iPad Pro" },
            { "iPad6,8", "iPad Pro" },

            { "AppleTV2,1", "Apple TV 2G" },
            { "AppleTV3,1", "Apple TV 3G" },
            { "AppleTV3,2", "Apple TV 3G Rev A" },
            { "AppleTV5,3", "Apple TV 4G" }
        };
        #endregion

        public string SerialNumber;
        public string Name;
        public string ProductType;
        public string ProductName;
        public string iOSVersion;
        public string Capacity;
        public string PhoneNumber;
        public string Region;
        public string CPUArchitecture;

        public IntPtr LockdownClient;
        public IntPtr InstallationProxyService;
        public IntPtr InstallationProxyClient;

        public bool IsConnected;

        public iDevice(IntPtr handle, string udid, string serialNumber, string name, string productType,
            string iosVersion, string phoneNumber, string region, string cpuArchitecture)
        {
            IsConnected = true;
            Handle = handle;
            Udid = udid;
            SerialNumber = serialNumber;
            Name = name;
            ProductType = productType;
            ProductName = DeviceHardware[productType];
            iOSVersion = iosVersion;
            PhoneNumber = phoneNumber;
            Region = region;
            CPUArchitecture = cpuArchitecture;
        }

        #region Dispose
        ~iDevice()
        {
            Dispose(false);
        }

        public new void Dispose()
        {
            Dispose(true);
        }

        protected new virtual void Dispose(bool disposing)
        {
            InstallationProxy.FreeClient(InstallationProxyClient);
            InstallationProxyClient = IntPtr.Zero;

            Lockdown.FreeService(InstallationProxyService);
            InstallationProxyService = IntPtr.Zero;

            Lockdown.FreeClient(LockdownClient);
            LockdownClient = IntPtr.Zero;

            base.Dispose(false);
        }
        #endregion
    }

    class iDeviceHandle : Device
    {
        public iDeviceHandle(IntPtr handle, string udid)
        {
            Handle = handle;
            Udid = udid;
        }

        public bool GetIDeviceFromHandle(out iDevice newDevice)
        {
            IntPtr lockdownService;
            IntPtr lockdownClient;
            Lockdown.LockdownError lockdownReturnCode = Lockdown.Start(Handle, out lockdownClient, out lockdownService);

            newDevice = null;
            if (lockdownReturnCode != Lockdown.LockdownError.LOCKDOWN_E_SUCCESS)
            {
                return false;
            }

            XDocument deviceProperties;
            lockdownReturnCode = Lockdown.GetProperties(lockdownClient, out deviceProperties);

            Lockdown.FreeService(lockdownService);
            Lockdown.FreeClient(lockdownClient);

            if (lockdownReturnCode != Lockdown.LockdownError.LOCKDOWN_E_SUCCESS || deviceProperties == default(XDocument))
            {
                return false;
            }

            IEnumerable<XElement> keys = deviceProperties.Descendants("dict").Descendants("key");
            newDevice = new iDevice(
                IntPtr.Zero,
                keys.Where(x => x.Value == "UniqueDeviceID").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                keys.Where(x => x.Value == "SerialNumber").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                keys.Where(x => x.Value == "DeviceName").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                keys.Where(x => x.Value == "ProductType").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                keys.Where(x => x.Value == "ProductVersion").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                keys.Where(x => x.Value == "PhoneNumber").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                keys.Where(x => x.Value == "RegionInfo").Select(x => (x.NextNode as XElement).Value).FirstOrDefault(),
                keys.Where(x => x.Value == "CPUArchitecture").Select(x => (x.NextNode as XElement).Value).FirstOrDefault()
            );

            return true;
        }
    }

    public class iDeviceEqualityComparer : IEqualityComparer<iDevice>
    {
        public bool Equals(iDevice device1, iDevice device2)
        {
            return device1.Udid == device2.Udid
                || device1.SerialNumber == device2.SerialNumber;
        }

        public int GetHashCode(iDevice device)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + device.Udid.GetHashCode();
                hash = hash * 23 + device.SerialNumber.GetHashCode();

                return hash;
            }
        }
    }
}
