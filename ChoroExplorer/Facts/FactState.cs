using System;
using System.Collections.Generic;
using Fluxor;
using System.Linq;
using ChoroExplorer.Filters;
using ChoroExplorer.Models;
using Fluxor.Selectors;

namespace ChoroExplorer.Facts {
    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    internal class FactState {
        public  IReadOnlyList<FactData>     Facts { get; }
        public  IReadOnlyList<FilterData>   Filters { get; }
        public  IReadOnlyList<FactSet>      FactSets { get; }
        public  string                      CurrentFactSet { get; }

        public FactState( IReadOnlyList<FactData> facts, IReadOnlyList<FilterData> filters, IReadOnlyList<FactSet> factSets,
                          string currentFactSet ) {
            Facts = facts;
            Filters = filters;
            FactSets = factSets;
            CurrentFactSet = currentFactSet;
        }

        public static FactState Factory() => 
            new( Enumerable.Empty<FactData>().ToList(),
                 Enumerable.Empty<FilterData>().ToList(),
                 Enumerable.Empty<FactSet>().ToList(), 
                 String.Empty );
    }

    // Reducers

    // ReSharper disable once UnusedType.Global
    internal static class FactReducers {
        // Facts
        [ReducerMethod]
        public static FactState AddFact( FactState state, AddFactAction action ) =>
            new( new List<FactData>( state.Facts ) { action.Fact }, state.Filters, state.FactSets, state.CurrentFactSet );

        [ReducerMethod]
        public static FactState PopulateFact( FactState state, PopulateFactAction action ) =>
            new( new List<FactData>( state.Facts ) { action.Fact }, state.Filters, state.FactSets, state.CurrentFactSet );

        [ReducerMethod]
        public static FactState UpdateFact( FactState state, UpdateFactAction action ) {
            var facts = new List<FactData>( 
                state.Facts.Select( f => f.FactId.Equals( action.Fact.FactId ) ? action.Fact : f ));

            return new( facts, state.Filters, state.FactSets, state.CurrentFactSet );
        }

        // Filters
        [ReducerMethod]
        public static FactState AddFilter( FactState state, AddFilterAction action ) =>
            new( state.Facts, new List<FilterData>( state.Filters ) { action.Filter }, state.FactSets, state.CurrentFactSet );

        [ReducerMethod]
        public static FactState PopulateFilter( FactState state, PopulateFilterAction action ) =>
            new( state.Facts, new List<FilterData>( state.Filters ) { action.Filter }, state.FactSets, state.CurrentFactSet );

        [ReducerMethod]
        public static FactState UpdateFilter( FactState state, UpdateFilterAction action ) {
            var filters = new List<FilterData>( 
                state.Filters.Select( f => f.FilterId.Equals( action.Filter.FilterId ) ? action.Filter : f ));

            return new( state.Facts, filters, state.FactSets, state.CurrentFactSet );
        }

        // FactSets
        [ReducerMethod]
        public static FactState AddFactSet( FactState state, AddFactSetAction action ) =>
            new( state.Facts, state.Filters, new List<FactSet>( state.FactSets ) { action.FactSet }, state.CurrentFactSet );

        [ReducerMethod]
        public static FactState PopulateFactSet( FactState state, PopulateFactSetAction action ) =>
            new( state.Facts, state.Filters, new List<FactSet>( state.FactSets ) { action.FactSet }, state.CurrentFactSet );

        [ReducerMethod]
        public static FactState UpdateFactSet( FactState state, UpdateFactSetAction action ) {
            var factSets = new List<FactSet>( 
                state.FactSets.Select( f => f.SetId.Equals( action.FactSet.SetId ) ? action.FactSet : f ));

            return new( state.Facts, state.Filters, factSets, state.CurrentFactSet );
        }

        [ReducerMethod]
        public static FactState SetCurrentFactSet( FactState state, SetCurrentFactSetAction action ) =>
            new( state.Facts, state.Filters, state.FactSets, action.FactSetId );

        [ReducerMethod]
        public static FactState InitializeCurrentFactSet( FactState state, InitializeCurrentFactSetAction action ) =>
            new( state.Facts, state.Filters, state.FactSets, action.FactSetId );
    }

    // Selectors
    internal interface IFactSelectors {
        ISelectorSubscription<IReadOnlyList<FactSet>>       FactSetsSelector();
        ISelectorSubscription<IReadOnlyList<FilterData>>    FiltersSelector();
        ISelectorSubscription<IReadOnlyList<FactData>>      FactsSelector();
        ISelectorSubscription<string>                       CurrentFactSelector();
    }

    internal class FactSelectors : IFactSelectors {
        private readonly IStore                 mStore;
        private readonly Selector<FactState>    mFactStateSelector;

        public FactSelectors( IStore store ) {
            mStore = store;
            mFactStateSelector = SelectorFactory.CreateFeatureSelector<FactState>();
        }

        public ISelectorSubscription<IReadOnlyList<FactSet>> FactSetsSelector() =>
            mStore.SubscribeSelector( SelectorFactory.CreateSelector( mFactStateSelector, state => state.FactSets ));

        public ISelectorSubscription<IReadOnlyList<FactData>> FactsSelector() =>
            mStore.SubscribeSelector( SelectorFactory.CreateSelector( mFactStateSelector, state => state.Facts ));

        public ISelectorSubscription<IReadOnlyList<FilterData>> FiltersSelector() =>
            mStore.SubscribeSelector( SelectorFactory.CreateSelector( mFactStateSelector, state => state.Filters ));

        public ISelectorSubscription<string> CurrentFactSelector() =>
            mStore.SubscribeSelector( SelectorFactory.CreateSelector( mFactStateSelector, state => state.CurrentFactSet ));
    }
}
