using System.Collections.Generic;
using System.Linq;
using ChoroExplorer.Regions;
using Fluxor;

namespace ChoroExplorer.Models {
    internal enum eRegionFilter {
        AllRegions,
        Top10,
        Bottom10
    }

    internal interface IRegionFilter {
        IEnumerable<RegionSummary> FilterRegions( IEnumerable<RegionSummary> regions, eRegionFilter filter );
    }

    internal class RegionFilter : IRegionFilter {
        private readonly IState<RegionState>    mRegionState;

        public RegionFilter( IState<RegionState> regionState ) {
            mRegionState = regionState;
        }

        public IEnumerable<RegionSummary> FilterRegions( IEnumerable<RegionSummary> regions, eRegionFilter filter ) {
            switch ( mRegionState.Value.RegionFilter ) {
                case eRegionFilter.AllRegions:
                    return regions;

                case eRegionFilter.Top10:
                    return regions.Where( r => r.Enabled ).OrderByDescending( r => r.RegionScore ).Take( 10 );

                case eRegionFilter.Bottom10:
                    return regions.Where( r => r.Enabled ).OrderBy( r => r.RegionScore ).Take( 10 );
            }

            return regions;
        }
    }
}
