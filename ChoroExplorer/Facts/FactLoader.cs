using ChoroExplorer.Models;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Facts {
    internal static class FactLoader {
        public static FactData LoadFact( string path ) {
            var factData = JsonObjectSerializer.Read<FactData>( path );

            return factData ?? new FactData();
        }

        public static void SaveFact( FactData factData, string path ) {
            JsonObjectSerializer.Write( path, factData );
        }
    }
}
