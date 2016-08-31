using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace CycriptGUI.LibIMobileDevice
{
    class LibPlist
    {
        const string LIBPLIST_DLL_PATH = @"libplist.dll";

        #region DLLImport
        [DllImport(LIBPLIST_DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern void plist_to_xml(IntPtr plist, out IntPtr xml, out int length);

        [DllImport(LIBPLIST_DLL_PATH, EntryPoint = "plist_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreePlist(IntPtr plistPtr);
        #endregion

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
    }
}
