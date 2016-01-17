using Renci.SshNet;
using System.Diagnostics;
using System.IO;

namespace CycriptGUI.MainProgram
{
    class Ssh
    {
        public static void Connect(string udid)
        {
            Process usbTunnel = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "iproxy.exe",
                    Arguments = "22 22 " + udid,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            usbTunnel.Start();
        }

        public static string[] MakeRequest(SshClient client, string command)
        {
            SshCommand sshCommand = client.CreateCommand(command);
            var asynch = sshCommand.BeginExecute();

            StreamReader streamReader = new StreamReader(sshCommand.OutputStream);
            string response = "";
            while (!asynch.IsCompleted)
            {
                string result = streamReader.ReadToEnd();
                if (result != "")
                {
                    response += result;
                }
            }

            return response.TrimEnd('\r', '\n').Split('\n');
        }
    }
}
