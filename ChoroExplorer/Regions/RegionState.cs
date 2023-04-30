using System;
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
        public  string                          FocusRegion { get; }
        public  eRegionFilter                   RegionFilter { get; }

        public RegionState( IReadOnlyList<RegionData> regions, IReadOnlyList<RegionSummary> scores,
                            IReadOnlyList<RegionColor> regionColors,
                            int regionColorTransparency, string focusRegion, eRegionFilter regionFilter ) {
            Regions = regions;
            RegionScores = scores;
            RegionColors = regionColors;
            RegionColorTransparency = regionColorTransparency;
            FocusRegion = focusRegion;
            RegionFilter = regionFilter;
        }

        public static RegionState Factory() => 
            new( Enumerable.Empty<RegionData>().ToList(), 
                 Enumerable.Empty<RegionSummary>().ToList(),
                 Enumerable.Empty<RegionColor>().ToList(),
                 128, String.Empty, eRegionFilter.AllRegions );
    }

    // Reducers

    // ReSharper disable once UnusedType.Global
    internal static class RegionReducers {
        [ReducerMethod]
        public static RegionState InitializeRegions( RegionState state, InitializeRegionsAction action ) =>
            new( action.Regions, state.RegionScores, state.RegionColors, 
                state.RegionColorTransparency, state.FocusRegion, state.RegionFilter );

        [ReducerMethod]
        public static RegionState UpdateRegionScores( RegionState state, UpdateRegionScoresAction action ) =>
            new( state.Regions, action.Scores, state.RegionColors, 
                state.RegionColorTransparency, state.FocusRegion, state.RegionFilter );

        [ReducerMethod]
        public static RegionState UpdateRegionColors( RegionState state, UpdateRegionColorsAction action ) =>
            new( state.Regions, state.RegionScores, action.RegionColors,
                state.RegionColorTransparency, state.FocusRegion, state.RegionFilter );

        [ReducerMethod]
        public static RegionState SetRegionColorTransparency( RegionState state, SetRegionColorTransparencyAction action ) =>
            new( state.Regions, state.RegionScores, state.RegionColors,
                action.ColorTransparency, state.FocusRegion, state.RegionFilter );

        [ReducerMethod]
        public static RegionState SetFocusedRegion( RegionState state, SetFocusedRegionAction action ) =>
            new( state.Regions, state.RegionScores, state.RegionColors,
                state.RegionColorTransparency, action.FocusedRegion, state.RegionFilter );

        [ReducerMethod]
        public static RegionState SetRegionFilter( RegionState state, SetRegionFilterAction action ) =>
            new( state.Regions, state.RegionScores, state.RegionColors,
                state.RegionColorTransparency, state.FocusRegion, action.Filter );
    }

    // Selectors

    internal interface IRegionSelectors {
        ISelectorSubscription<IReadOnlyList<RegionData>>    RegionsSelector();
        ISelectorSubscription<IReadOnlyList<RegionSummary>> RegionSummarySelector();
        ISelectorSubscription<String>                       FocusedRegionSelector();
        ISelectorSubscription<eRegionFilter>                RegionFocusSelector();
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

        public ISelectorSubscription<IReadOnlyList<RegionSummary>> RegionSummarySelector() =>
            mStore.SubscribeSelector( SelectorFactory.CreateSelector( mRegionStateSelector, state => state.RegionScores ));

        public ISelectorSubscription<string> FocusedRegionSelector() =>
            mStore.SubscribeSelector( SelectorFactory.CreateSelector( mRegionStateSelector, state => state.FocusRegion ));

        public ISelectorSubscription<eRegionFilter> RegionFocusSelector() =>
            mStore.SubscribeSelector( SelectorFactory.CreateSelector( mRegionStateSelector, state => state.RegionFilter ));
    }
}
