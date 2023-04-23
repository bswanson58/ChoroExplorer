using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ChoroExplorer.Models {
    [DebuggerDisplay("Region:{RegionName}")]
    internal class RegionData {
        public  string          RegionId { get; }
        public  string          RegionName { get; }

        [JsonConstructor]
        public RegionData( string regionId, string regionName ) {
            RegionId = regionId;
            RegionName = regionName;
        }
    }
}
