using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

namespace ChoroExplorer.Models {
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
