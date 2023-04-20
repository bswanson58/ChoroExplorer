using System.Collections.Generic;
using ChoroExplorer.Models;

namespace ChoroExplorer.Regions {
    internal class LoadRegionsAction {
    }

    internal class InitializeRegionsAction {
        public IReadOnlyList<RegionData>    Regions { get; }

        public InitializeRegionsAction( IReadOnlyList<RegionData> regions ) {
            Regions = regions;
        }
    }

    internal class ConfigureRegionShapesAction {
        public IReadOnlyList<RegionShape>   RegionShapes { get; }

        public ConfigureRegionShapesAction( IEnumerable<RegionShape> regionShapes ) {
            RegionShapes = new List<RegionShape>( regionShapes );
        }
    }
}
