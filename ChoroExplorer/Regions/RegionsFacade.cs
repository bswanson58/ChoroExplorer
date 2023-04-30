using System.Collections.Generic;
using ChoroExplorer.Models;
using Fluxor;

namespace ChoroExplorer.Regions {
    internal interface IRegionsFacade {
        void    InitializeRegions();
        void    UpdateRegionScores( IReadOnlyList<RegionSummary> scores );
        void    SetRegionColorTransparency( int transparency );
        void    SetFocusedRegion( string region );
        void    SetRegionFilter( eRegionFilter filter );
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

        public void SetFocusedRegion( string region ) {
            mDispatcher.Dispatch( new SetFocusedRegionAction( region ));
        }

        public void SetRegionFilter( eRegionFilter filter ) {
            mDispatcher.Dispatch( new SetRegionFilterAction( filter ));
        }
    }
}
