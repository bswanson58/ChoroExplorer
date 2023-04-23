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
        public  IReadOnlyList<RegionColor>      RegionColors { get; }
        public  int                             RegionColorTransparency { get; }

        public RegionState( IReadOnlyList<RegionData> regions, IReadOnlyList<RegionSummary> scores,
                            IReadOnlyList<RegionColor> regionColors, int regionColorTransparency ) {
            Regions = regions;
            RegionScores = scores;
            RegionColors = regionColors;
            RegionColorTransparency = regionColorTransparency;
        }

        public static RegionState Factory() => 
            new( Enumerable.Empty<RegionData>().ToList(), 
                 Enumerable.Empty<RegionSummary>().ToList(),
                 Enumerable.Empty<RegionColor>().ToList(),
                 128 );
    }

    // Reducers

    // ReSharper disable once UnusedType.Global
    internal static class RegionReducers {
        [ReducerMethod]
        public static RegionState InitializeRegions( RegionState state, InitializeRegionsAction action ) =>
            new( action.Regions, state.RegionScores, state.RegionColors, state.RegionColorTransparency );

        [ReducerMethod]
        public static RegionState UpdateRegionScores( RegionState state, UpdateRegionScoresAction action ) =>
            new( state.Regions, action.Scores, state.RegionColors, state.RegionColorTransparency );

        [ReducerMethod]
        public static RegionState UpdateRegionColors( RegionState state, UpdateRegionColorsAction action ) =>
            new( state.Regions, state.RegionScores, action.RegionColors, state.RegionColorTransparency );

        [ReducerMethod]
        public static RegionState SetRegionColorTransparency( RegionState state, SetRegionColorTransparencyAction action ) =>
            new( state.Regions, state.RegionScores, state.RegionColors, action.ColorTransparency );
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
