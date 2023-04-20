using System.Collections.Generic;
using ChoroExplorer.Models;

namespace ChoroExplorer.Regions {
    internal class LoadRegions {
    }

    internal class InitializeRegions {
        public IReadOnlyList<RegionData>    Regions { get; }

        public InitializeRegions( IReadOnlyList<RegionData> regions ) {
            Regions = regions;
        }
    }
}
