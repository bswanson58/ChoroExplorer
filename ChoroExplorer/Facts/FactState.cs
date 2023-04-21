using System;
using System.Collections.Generic;
using Fluxor;
using System.Linq;
using ChoroExplorer.Models;

namespace ChoroExplorer.Facts {
    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    internal class FactState {
        public  IReadOnlyList<FactData>     Facts { get; }
        public  IReadOnlyList<FactSet>      FactSets { get; }
        public  string                      CurrentFactSet { get; }

        public FactState( IReadOnlyList<FactData> facts, IReadOnlyList<FactSet> factSets, string currentFactSet ) {
            Facts = facts;
            FactSets = factSets;
            CurrentFactSet = currentFactSet;
        }

        public static FactState Factory() => 
            new( Enumerable.Empty<FactData>().ToList(), Enumerable.Empty<FactSet>().ToList(), String.Empty );
    }

    // ReSharper disable once UnusedType.Global
    internal static class FactReducers {
        [ReducerMethod]
        public static FactState AddFact( FactState state, AddFactAction action ) =>
            new( new List<FactData>( state.Facts ) { action.Fact }, state.FactSets, state.CurrentFactSet );

        [ReducerMethod]
        public static FactState UpdateFact( FactState state, UpdateFactAction action ) {
            var facts = new List<FactData>( 
                state.Facts.Select( f => f.FactId.Equals( action.Fact.FactId ) ? action.Fact : f ));

            return new( facts, state.FactSets, state.CurrentFactSet );
        }

        [ReducerMethod]
        public static FactState AddFactSet( FactState state, AddFactSetAction action ) =>
            new( state.Facts, new List<FactSet>( state.FactSets ) { action.FactSet }, state.CurrentFactSet );

        [ReducerMethod]
        public static FactState UpdateFactSet( FactState state, UpdateFactSetAction action ) {
            var factSets = new List<FactSet>( 
                state.FactSets.Select( f => f.SetId.Equals( action.FactSet.SetId ) ? action.FactSet : f ));

            return new( state.Facts, factSets, state.CurrentFactSet );
        }

        [ReducerMethod]
        public static FactState SetCurrentFactSet( FactState state, SetCurrentFactSetAction action ) =>
            new( state.Facts, state.FactSets, action.FactSetId );
    }
}
