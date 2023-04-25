using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ChoroExplorer.Models {
    internal class FactValue {
        public  string      RegionId { get; }
        public  double      Value { get; }
        
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

    [DebuggerDisplay("FactData:{FactKey}")]
    internal class FactData {
        public  string                  FactId { get; }
        public  string                  FactKey { get; }
        public  string                  FactName { get; }
        public  string                  Description { get; }
        public  string                  Source { get; }
        public  bool                    ReverseScore { get; }
        public  List<FactValue>         RegionFacts { get; }

        public FactData() {
            FactId = NCuid.Cuid.Generate();
            FactKey = String.Empty;
            FactName = String.Empty;
            Description = String.Empty;
            Source = String.Empty;
            ReverseScore = false;
            RegionFacts = new List<FactValue>();
        }

        [JsonConstructor]
        public FactData( string factId, string factKey, string factName, string description, string source, bool reverseScore, 
                         List<FactValue> regionFacts ) {
            FactId = factId;
            FactKey = factKey;
            FactName = factName;
            Description = description;
            Source = source;
            ReverseScore = reverseScore;
            RegionFacts = regionFacts;
        }
    }

    internal class FactParameters {
        public  string                  FactId { get; }
        public  int                     Weight { get; }
        public  bool                    Enabled { get; }

        public  bool                    IsDefault => Weight.Equals( 1 ) && Enabled.Equals( false );

        public FactParameters( FactData forData ) {
            FactId = forData.FactId;
            Weight = 1;
            Enabled = false;
        }

        [JsonConstructor]
        public FactParameters( string factId, int weight, bool enabled ) {
            FactId = factId;
            Weight = weight;
            Enabled = enabled;
        }

        public FactParameters With( bool enabledState ) =>
            new FactParameters( FactId, Weight, enabledState );

        public FactParameters With( int weight ) =>
            new FactParameters( FactId, weight, Enabled );
    }
}
