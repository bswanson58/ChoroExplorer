using ChoroExplorer.Facts;
using ChoroExplorer.Models;
using ChoroExplorer.Regions;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer {
    internal interface IAppStartup {
        void    StartApplication();
        void    StopApplication();
    }

    internal class AppStartup : IAppStartup {
        private readonly IFactsFacade       mFactsFacade;
        private readonly IRegionsFacade     mRegionsFacade;
        private readonly IPreferences       mPreferences;

        public AppStartup( IRegionsFacade regionsFacade, IFactsFacade factsFacade, IPreferences preferences ) {
            mRegionsFacade = regionsFacade;
            mFactsFacade = factsFacade;
            mPreferences = preferences;
        }

        public void StartApplication() {
            var preferences = mPreferences.Load<ChoroPreferences>();

            mFactsFacade.InitializeCurrentFactSet( preferences.CurrentFactSet );

            mRegionsFacade.InitializeRegions();

            mFactsFacade.LoadFacts();
            mFactsFacade.LoadFactSets();
        }

        public void StopApplication() {
        }
    }
}
