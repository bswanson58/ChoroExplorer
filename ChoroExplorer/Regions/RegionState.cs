using System.Collections.Generic;
using System.Linq;
using ChoroExplorer.Models;
using Fluxor;
using Fluxor.Selectors;

namespace ChoroExplorer.Regions {
    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    internal class RegionState {
        public  IReadOnlyList<RegionData>       Regions { get; }
        public  IReadOnlyList<RegionSummary>    RegionScores { get; }

        public RegionState( IReadOnlyList<RegionData> regions, IReadOnlyList<RegionSummary> scores ) {
            Regions = regions;
            RegionScores = scores;
        }

        public static RegionState Factory() => 
            new( Enumerable.Empty<RegionData>().ToList(), Enumerable.Empty<RegionSummary>().ToList());
    }

    // Reducers

    // ReSharper disable once UnusedType.Global
    internal static class RegionReducers {
        [ReducerMethod]
        public static RegionState InitializeRegions( RegionState state, InitializeRegionsAction action ) =>
            new( action.Regions, state.RegionScores );
    }

    // Selectors

    internal interface IRegionSelectors {
        ISelectorSubscription<IReadOnlyList<RegionData>>  RegionsSelector();
    }

    internal class RegionSelectors : IRegionSelectors {
        private readonly IStore                 mStore;
        private readonly Selector<RegionState>  mRegionStateSelector;

        public RegionSelectors( IStore store ) {
            mStore = store;
            mRegionStateSelector = SelectorFactory.CreateFeatureSelector<RegionState>();
        }

        public ISelectorSubscription<IReadOnlyList<RegionData>> RegionsSelector() =>
            mStore.SubscribeSelector( SelectorFactory.CreateSelector( mRegionStateSelector, state => state.Regions ));

    }
}
