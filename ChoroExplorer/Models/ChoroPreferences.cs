using System;

namespace ChoroExplorer.Models {
    internal class ChoroPreferences {
        public  string      CurrentFactSet { get; set; }
        public  int         RegionColorTransparency { get; set; }

        public ChoroPreferences() {
            CurrentFactSet = String.Empty;
            RegionColorTransparency = 128;
        }
    }
}
