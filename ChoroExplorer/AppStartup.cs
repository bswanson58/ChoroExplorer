using ChoroExplorer.Facts;
using ChoroExplorer.Regions;

namespace ChoroExplorer {
    internal interface IAppStartup {
        void    StartApplication();
        void    StopApplication();
    }

    internal class AppStartup : IAppStartup {
        private readonly IFactsFacade       mFactsFacade;
        private readonly IRegionsFacade     mRegionsFacade;

        public AppStartup( IRegionsFacade regionsFacade, IFactsFacade factsFacade ) {
            mRegionsFacade = regionsFacade;
            mFactsFacade = factsFacade;
        }

        public void StartApplication() {
            mRegionsFacade.InitializeRegions();
            mFactsFacade.LoadFacts();
            mFactsFacade.LoadFactSets();
        }

        public void StopApplication() {
        }
    }
}
