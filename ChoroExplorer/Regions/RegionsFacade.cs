using System.Collections.Generic;
using ChoroExplorer.Models;
using Fluxor;

namespace ChoroExplorer.Regions {
    internal interface IRegionsFacade {
        void    InitializeRegions();
        void    UpdateRegionScores( IReadOnlyList<RegionSummary> scores );
        void    SetRegionColorTransparency( int transparency );
    }

    internal class RegionsFacade : IRegionsFacade {
        private readonly IDispatcher    mDispatcher;

        public RegionsFacade( IDispatcher dispatcher ) {
            mDispatcher = dispatcher;
        }

        public void InitializeRegions() {
            mDispatcher.Dispatch( new LoadRegionsAction());
        }

        public void UpdateRegionScores( IReadOnlyList<RegionSummary> scores ) {
            mDispatcher.Dispatch( new UpdateRegionScoresAction( scores ));
        }

        public void SetRegionColorTransparency( int transparency ) {
            mDispatcher.Dispatch( new SetRegionColorTransparencyAction( transparency ));
        }
    }
}
