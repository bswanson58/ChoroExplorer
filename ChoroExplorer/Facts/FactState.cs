using System.Collections.Generic;
using Fluxor;
using System.Linq;
using ChoroExplorer.Models;

namespace ChoroExplorer.Facts {
    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    internal class FactState {
        public  IReadOnlyList<FactSet>  Facts { get; }

        public FactState( IReadOnlyList<FactSet> facts ) {
            Facts = facts;
        }

        public static FactState Factory() => 
            new( Enumerable.Empty<FactSet>().ToList());
    }
}
