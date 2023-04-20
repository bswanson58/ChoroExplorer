using ReusableBits.Platform.Interfaces;

namespace ReusableBits.Platform.Preferences {
    public interface IEnvironment {
        string		ApplicationName();
        string      EnvironmentName();

        string		ApplicationDirectory();
        string		LogFileDirectory();
        string		PreferencesDirectory();
        string      FactsDirectory();
    }

    public class OperatingEnvironment : IEnvironment {
        private readonly IApplicationConstants  mApplicationConstants;

        public OperatingEnvironment( IApplicationConstants applicationConstants ) {
            mApplicationConstants = applicationConstants;
        }

        public string ApplicationName() => mApplicationConstants.ApplicationName;

        public string EnvironmentName() => Environment.MachineName;

        public string ApplicationDirectory() {
            var retValue = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData ),
                mApplicationConstants.CompanyName,
                mApplicationConstants.ApplicationName );

            if(!Directory.Exists( retValue )) {
                Directory.CreateDirectory( retValue );
            }

            return( retValue );
        }

        public string FactsDirectory() {
            var retValue = Path.Combine( ApplicationDirectory(), mApplicationConstants.FactDirectory );

            if(!Directory.Exists( retValue )) {
                Directory.CreateDirectory( retValue );
            }

            return( retValue );
        }

        public string LogFileDirectory() {
            var retValue = Path.Combine( ApplicationDirectory(), mApplicationConstants.LogFileDirectory );

            if(!Directory.Exists( retValue )) {
                Directory.CreateDirectory( retValue );
            }

            return( retValue );
        }

        public string PreferencesDirectory() {
            var retValue = Path.Combine( ApplicationDirectory(), mApplicationConstants.ConfigurationDirectory );

            if(!Directory.Exists( retValue )) {
                Directory.CreateDirectory( retValue );
            }

            return( retValue );
        }
    }
}
