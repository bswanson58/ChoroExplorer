using System;
using System.Collections.Generic;

namespace ChoroExplorer.Models {
    internal class FactInformation {
        public  string                  FactId { get; }
        public  string                  FactName { get; }
        public  string                  Description { get; }
        public  double                  Weight { get; }
        public  bool                    Enabled { get; }
        public  List<RegionFact>        RegionFacts { get; }
    }

    internal class FactParameters {
        public  string                  FactId { get; }
        public  double                  Weight { get; }
        public  bool                    Enabled { get; }
    }

    internal class RegionFact {
        public  string                  RegionId { get; }
        public  string                  FactId { get; }
        public  double                  Value { get; }
        public  Func<double, double>    CalculateScore { get; } 
    }

    internal class RegionScore {
        public  string                  RegionId { get; }
        public  double                  Score { get; }
    }

    internal class FactList : List<FactInformation> { }
}
