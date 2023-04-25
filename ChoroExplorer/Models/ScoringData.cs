using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

namespace ChoroExplorer.Models {
    internal class ScoringFact {
        public  string          FactId { get; }
        public  int             FactWeight { get; }
        public  bool            ReverseScore { get; }
        public  List<FactValue> RegionFacts { get; }

        public ScoringFact( FactData factData, FactParameters parameters ) {
            FactId = factData.FactId;
            FactWeight = parameters.Weight;
            ReverseScore = factData.ReverseScore;
            RegionFacts = factData.RegionFacts;
        }
    }

    internal class FactContext {
        public  string      FactId { get; }
        public  int         FactWeight { get; }
        public  bool        ReverseScore { get; }
        public  double      MinimumFactValue { get; }
        public  double      MaximumFactValue { get; }

        public FactContext( string factId, int factWeight, bool reverseScore, double minimumFactValue, double maximumFactValue ) {
            FactId = factId;
            FactWeight = factWeight;
            ReverseScore = reverseScore;
            MinimumFactValue = minimumFactValue;
            MaximumFactValue = maximumFactValue;
        }
    }

    internal class RegionFactScore {
        public  string  FactId { get; }
        public  string  RegionId { get; }
        public  double  Score { get; }
        public  bool    Enabled { get; }

        public RegionFactScore( string factId, string regionId, double score ) {
            FactId = factId;
            RegionId = regionId;
            Score = score;
            Enabled = true;
        }

        public RegionFactScore( string factId, string regionId ) {
            FactId = factId;
            RegionId = regionId;
            Score = 0.0D;
            Enabled = false;
        }
    }

    internal class RegionScore {
        public  string  RegionId { get; }
        public  double  Score { get; }
        public  bool    Enabled { get; }

        public RegionScore( string regionId, double score, bool enabled ) {
            RegionId = regionId;
            Score = Math.Max( 0.0, Math.Min( 1.0, score ));
            Enabled = enabled;

            Debug.Assert( Score.Equals( score ));
        }
    }

    internal class FactScore {
        public  string  FactId { get; }
        public  double  Score { get; }

        public FactScore( string factId, double score ) {
            FactId = factId;
            Score = Math.Max( 0.0, Math.Min( 1.0, score ));
        }
    }

    [DebuggerDisplay("Region:{RegionName}")]
    internal class RegionSummary {
        public  string              RegionId { get; }
        public  string              RegionName {  get; }
        public  double              RegionScore { get; }
        public  bool                Enabled { get; }
        public  IList<FactScore>    FactScores { get; }

        public RegionSummary( string regionId, string regionName, double regionScore, bool enabled,
                              IList<FactScore> factScores ) {
            RegionId = regionId;
            RegionName = regionName;
            RegionScore = regionScore;
            Enabled = enabled;
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
