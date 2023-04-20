using ChoroExplorer.Regions;

namespace ChoroExplorer {
    internal interface IAppStartup {
        void    StartApplication();
        void    StopApplication();
    }

    internal class AppStartup : IAppStartup {
        private readonly IRegionsFacade     mRegionsFacade;

        public AppStartup( IRegionsFacade regionsFacade ) {
            mRegionsFacade = regionsFacade;
        }

        public void StartApplication() {
            mRegionsFacade.InitializeRegions();
        }

        public void StopApplication() {
        }
    }
}
