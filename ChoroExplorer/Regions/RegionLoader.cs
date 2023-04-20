using System.Collections.Generic;
using System.Linq;
using ChoroExplorer.Models;
using Newtonsoft.Json;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Regions {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class RegionList {
        public  List<RegionData>    Regions { get; set; }

        [JsonConstructor]
        public RegionList( List<RegionData> regions ) {
            Regions = regions;
        }
    }

    internal static class RegionLoader {
        public static IReadOnlyList<RegionData> LoadRegions( string dataPath ) {
            var regionList = JsonObjectSerializer.Read<RegionList>( dataPath );

            return regionList?.Regions ?? 
                   Enumerable.Empty<RegionData>().ToList();
        }
    }
}
