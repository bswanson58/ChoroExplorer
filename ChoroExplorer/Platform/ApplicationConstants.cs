using ReusableBits.Platform.Interfaces;

namespace ChoroExplorer.Platform {
    internal class ApplicationConstants : IApplicationConstants {
        public string       ApplicationName { get; }
        public string       CompanyName { get; }
        public string       ConfigurationDirectory { get; }
        public string       FactDirectory { get; }
        public string       LogFileDirectory { get; }

        public ApplicationConstants() {
            ApplicationName = "ChoroExplorer";
            CompanyName = "Secret Squirrel Software";
            ConfigurationDirectory = "Configuration";
            FactDirectory = "Facts";
            LogFileDirectory = "Logs";
        }
    }
}
