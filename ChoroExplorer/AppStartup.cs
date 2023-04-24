using ChoroExplorer.Facts;
using ChoroExplorer.Filters;
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
        private readonly IFiltersFacade     mFiltersFacade;
        private readonly IRegionsFacade     mRegionsFacade;
        private readonly IFactEngine        mFactEngine;
        private readonly IPreferences       mPreferences;

        public AppStartup( IRegionsFacade regionsFacade, IFactsFacade factsFacade, IFiltersFacade filtersFacade,
                           IFactEngine factEngine, IPreferences preferences ) {
            mRegionsFacade = regionsFacade;
            mFactsFacade = factsFacade;
            mFiltersFacade = filtersFacade;
            mFactEngine = factEngine;
            mPreferences = preferences;
        }

        public void StartApplication() {
            var preferences = mPreferences.Load<ChoroPreferences>();

            mFactsFacade.InitializeCurrentFactSet( preferences.CurrentFactSet );
            mRegionsFacade.SetRegionColorTransparency( preferences.RegionColorTransparency );

            mRegionsFacade.InitializeRegions();

            mFactsFacade.LoadFacts();
            mFiltersFacade.LoadFilters();
            mFactsFacade.LoadFactSets();

            mFactEngine.StartFactEngine();
        }

        public void StopApplication() {
            mFactEngine.StopFactEngine();
        }
    }
}
