using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChoroExplorer.Models {
    internal class FactValue {
        public  string                  RegionId { get; }
        public  double                  Value { get; }
        
        public FactValue() {
            RegionId = String.Empty;
            Value = 0.0D;
        }

        [JsonConstructor]
        public FactValue( string regionId, double value ) {
            RegionId = regionId;
            Value = value;
        }
    }

    internal class FactData {
        public  string                  FactId { get; }
        public  string                  FactKey { get; }
        public  string                  FactName { get; }
        public  string                  Description { get; }
        public  string                  Source { get; }
        public  List<FactValue>         RegionFacts { get; }

        public FactData() {
            FactId = NCuid.Cuid.Generate();
            FactKey = String.Empty;
            FactName = String.Empty;
            Description = String.Empty;
            Source = String.Empty;
            RegionFacts = new List<FactValue>();
        }

        [JsonConstructor]
        public FactData( string factId, string factKey, string factName, string description, string source, 
                         List<FactValue> regionFacts ) {
            FactId = factId;
            FactKey = factKey;
            FactName = factName;
            Description = description;
            Source = source;
            RegionFacts = regionFacts;
        }
    }

    internal class FactParameters {
        public  string                  FactId { get; }
        public  double                  Weight { get; }
        public  bool                    Enabled { get; }

        public FactParameters() {
            FactId = NCuid.Cuid.Generate();
            Weight = 0.0D;
            Enabled = false;
        }

        [JsonConstructor]
        public FactParameters( string factId, double weight, bool enabled ) {
            FactId = factId;
            Weight = weight;
            Enabled = enabled;
        }
    }

    internal class FactSet {
        public  string                  SetId { get; }
        public  string                  SetName { get; }
        public  List<FactParameters>    Facts { get; }

        [JsonConstructor]
        public FactSet( string setId, string setName, List<FactParameters> facts ) {
            SetId = setId;
            SetName = setName;
            Facts = new List<FactParameters>();
        }
    }

    internal class RegionScore {
        public  string                  RegionId { get; }
        public  double                  Score { get; }
    }
}
