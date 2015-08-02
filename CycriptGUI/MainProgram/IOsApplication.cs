namespace CycriptGUI.MainProgram
{
    public class iOSApplication
    {
        public string Type;
        public string Name;
        public string Version;
        public string Identifier;
        public string ExecutableName;

        public iOSApplication(string type, string name, string version, string identifier, string executableName)
        {
            this.Type = type;
            this.Name = name;
            this.Version = version;
            this.Identifier = identifier;
            this.ExecutableName = executableName;
        }
    }
}
