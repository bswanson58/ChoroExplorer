using System.Collections.Generic;
using System.Text.Json.Serialization;
using Mapsui.Geometries;

namespace ChoroExplorer.Models {
    internal class RegionData {
        public  string          RegionId { get; }
        public  string          RegionName { get; }

        [JsonConstructor]
        public RegionData( string regionId, string regionName ) {
            RegionId = regionId;
            RegionName = regionName;
        }
    }

    internal class RegionShapeData {
        public  string          RegionId { get; }
        public  List<Point>     ShapeData { get; }
    }
}
