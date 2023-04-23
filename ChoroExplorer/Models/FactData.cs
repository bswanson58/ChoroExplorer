using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Windows.Media;

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
        public  int                     Weight { get; }
        public  bool                    Enabled { get; }

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

    [DebuggerDisplay("FactSet:{SetId}")]
    internal class FactSet {
        public  string                  SetId { get; }
        public  string                  SetName { get; }
        public  List<FactParameters>    Facts { get; }

        [JsonConstructor]
        public FactSet( string setId, string setName, List<FactParameters> facts ) {
            SetId = setId;
            SetName = setName;
            Facts = facts;
        }

        public FactSet With( string setName ) =>
            new FactSet( SetId, setName, Facts );

        public FactSet With( IEnumerable<FactParameters> parameters ) =>
            new FactSet( SetId, SetName, new List<FactParameters>( parameters ));

        public static FactSet UnnamedSet =>
            new FactSet( NCuid.Cuid.Generate(), "Unnamed", new List<FactParameters>());
    }

    internal class FactContext {
        public  string      FactId { get; }
        public  double      MinimumFactValue { get; }
        public  double      MaximumFactValue { get; }

        public FactContext( string factId, double minimumFactValue, double maximumFactValue ) {
            FactId = factId;
            MinimumFactValue = minimumFactValue;
            MaximumFactValue = maximumFactValue;
        }
    }

    internal class RegionFactScore {
        public  string  FactId { get; }
        public  string  RegionId { get; }
        public  double  Score { get; }

        public RegionFactScore( string factId, string regionId, double score ) {
            FactId = factId;
            RegionId = regionId;
            Score = Math.Max( 0.0, Math.Min( 1.0, score ));
        }
    }

    internal class RegionScore {
        public  string  RegionId { get; }
        public  double  Score { get; }

        public RegionScore( string regionId, double score ) {
            RegionId = regionId;
            Score = Math.Max( 0.0, Math.Min( 1.0, score ));
        }
    }

    internal class FactScore {
        public  string  FactId { get; }
        public  double  Score { get; }

        public FactScore( string factId, double score ) {
            FactId = factId;
            Score = score;
        }
    }

    [DebuggerDisplay("Region:{RegionName}")]
    internal class RegionSummary {
        public  string              RegionId { get; }
        public  string              RegionName {  get; }
        public  double              RegionScore { get; }
        public  IList<FactScore>    FactScores { get; }

        public RegionSummary( string regionId, string regionName, double regionScore, IList<FactScore> factScores ) {
            RegionId = regionId;
            RegionName = regionName;
            RegionScore = regionScore;
            FactScores = factScores;
        }
    }

    internal class RegionColor {
        public  string      RegionName {  get; }
        public  Color       Color { get; }

        public RegionColor( string regionName, Color color ) {
            RegionName = regionName;
            Color = color;
        }
    }
}
