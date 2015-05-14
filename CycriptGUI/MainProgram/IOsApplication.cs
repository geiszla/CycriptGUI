namespace CycriptGUI.MainProgram
{
    public class IOsApplication
    {
        public string Type;
        public string Name;
        public string Version;
        public string Identifier;
        public string ExecutableName;

        public IOsApplication(string type, string name, string version, string identifier, string executableName)
        {
            this.Type = type;
            this.Name = name;
            this.Version = version;
            this.Identifier = identifier;
            this.ExecutableName = executableName;
        }
    }
}
