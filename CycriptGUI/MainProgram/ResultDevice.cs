using System;

namespace CycriptGUI.MainProgram
{
    class ResultDevice : Device
    {
        public ResultDevice(IntPtr handle, string udid)
        {
            Handle = handle;
            Udid = udid;
        }
    }
}
