using ChoroExplorer.Models;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Facts {
    internal static class FactSetLoader {
        public static FactSet ? LoadFactSet( string path ) =>
            JsonObjectSerializer.Read<FactSet>( path );

        public static void SaveFactSet( FactSet factSet, string path ) =>
            JsonObjectSerializer.Write( path, factSet );
    }
}
