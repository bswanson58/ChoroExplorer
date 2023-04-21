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
    }

    internal class FactSet {
        public  string                  FactId { get; }
        public  FactData                Data { get; }
        public  FactParameters          Parameters { get; }

        public FactSet() {
            FactId = NCuid.Cuid.Generate();
            Data = new FactData();
            Parameters = new FactParameters();
        }

        public FactSet( FactData factData ) {
            FactId = factData.FactId;
            Data = factData;
            Parameters = new FactParameters();
        }
    }

    internal class RegionScore {
        public  string                  RegionId { get; }
        public  double                  Score { get; }
    }
}
