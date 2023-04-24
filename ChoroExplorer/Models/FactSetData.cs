using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ChoroExplorer.Models {
    [DebuggerDisplay("FactSet:{SetId}")]
    internal class FactSet {
        public  string                  SetId { get; }
        public  string                  SetName { get; }
        public  List<FactParameters>    Facts { get; }
        public  List<FilterParameters>  Filters { get; }

        [JsonConstructor]
        public FactSet( string setId, string setName, List<FactParameters> ? facts, List<FilterParameters> ? filters ) {
            SetId = setId;
            SetName = setName;
            Facts = facts ?? new List<FactParameters>();
            Filters = filters ?? new List<FilterParameters>();
        }

        public FactSet With( string setName ) =>
            new FactSet( SetId, setName, Facts, Filters );

        public FactSet With( IEnumerable<FactParameters> parameters ) =>
            new FactSet( SetId, SetName, new List<FactParameters>( parameters ), Filters );

        public FactSet With( IEnumerable<FilterParameters> parameters ) =>
            new FactSet( SetId, SetName, Facts, new List<FilterParameters>( parameters ));

        public static FactSet UnnamedSet =>
            new FactSet( NCuid.Cuid.Generate(), "Unnamed", new List<FactParameters>(), new List<FilterParameters>());
    }
}
