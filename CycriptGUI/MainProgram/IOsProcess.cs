using System;

namespace CycriptGUI.MainProgram
{
    class iOSProcess
    {
        public string User;
        public int ProcessID;
        public double CpuUsage;
        public double MemoryUsage;
        public int VirtualMemoryUsage;
        public int ResidentSetSize;
        public string TTY;
        public string State;
        public DateTime StartTime;
        public TimeSpan CpuTime;
        public string Command;

        public iOSProcess(string User, int ProcessID, double CpuUsage, double MemoryUsage, int VirtualMemoryUsage, int ResidentSetSize,
            string TTY, string State, DateTime StartTime, TimeSpan CpuTime, string Command)
        {
            this.User = User;
            this.ProcessID = ProcessID;
            this.CpuUsage = CpuUsage;
            this.MemoryUsage = MemoryUsage;
            this.VirtualMemoryUsage = VirtualMemoryUsage;
            this.ResidentSetSize = ResidentSetSize;
            this.TTY = TTY;
            this.State = State;
            this.StartTime = StartTime;
            this.CpuTime = CpuTime;
            this.Command = Command;
        }
    }
}
