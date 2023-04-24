using ChoroExplorer.Models;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Filters {
    internal static class FilterLoader {
        public static FilterData LoadFilter( string path ) {
            var filterData = JsonObjectSerializer.Read<FilterData>( path );

            return filterData ?? new FilterData();
        }

        public static void SaveFilter( FilterData filterData, string path ) {
            JsonObjectSerializer.Write( path, filterData );
        }
    }
}
