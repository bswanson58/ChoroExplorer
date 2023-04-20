using Fluxor;

namespace ChoroExplorer.Regions {
    internal interface IRegionsFacade {
        void    InitializeRegions();
    }

    internal class RegionsFacade : IRegionsFacade {
        private readonly IDispatcher    mDispatcher;

        public RegionsFacade( IDispatcher dispatcher ) {
            mDispatcher = dispatcher;
        }

        public void InitializeRegions() {
            mDispatcher.Dispatch( new LoadRegionsAction());
        }
    }
}
