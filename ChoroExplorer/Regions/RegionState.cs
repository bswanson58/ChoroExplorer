using System.Collections.Generic;
using System.Linq;
using ChoroExplorer.Models;
using Fluxor;

namespace ChoroExplorer.Regions {
    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    internal class RegionState {
        public  IReadOnlyList<RegionData>   Regions { get; }

        public RegionState( IReadOnlyList<RegionData> regions ) {
            Regions = regions;
        }

        public static RegionState Factory() => 
            new( Enumerable.Empty<RegionData>().ToList());
    }

    // ReSharper disable once UnusedType.Global
    internal static class RegionReducers {
        [ReducerMethod]
        public static RegionState InitializeRegions( RegionState state, InitializeRegions action ) =>
            new( action.Regions );
    }
}
