using System.Collections.Generic;
using Fluxor;
using System.Linq;
using ChoroExplorer.Models;

namespace ChoroExplorer.Facts {
    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    internal class FactState {
        public  IReadOnlyList<FactData>  Facts { get; }

        public FactState( IReadOnlyList<FactData> facts ) {
            Facts = facts;
        }

        public static FactState Factory() => 
            new( Enumerable.Empty<FactData>().ToList());
    }

    // ReSharper disable once UnusedType.Global
    internal static class FactReducers {
        [ReducerMethod]
        public static FactState AddFact( FactState state, AddFactAction action ) =>
            new( new List<FactData>( state.Facts ) { action.Fact } );

        [ReducerMethod]
        public static FactState UpdateFact( FactState state, UpdateFactAction action ) {
            var facts = new List<FactData>( state.Facts.Select( f => f.FactId.Equals( action.Fact.FactId ) ? action.Fact : f ));

            return new( facts );
        }
    }
}
