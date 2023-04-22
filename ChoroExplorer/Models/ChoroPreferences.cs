using System;

namespace ChoroExplorer.Models {
    internal class ChoroPreferences {
        public  string      CurrentFactSet { get; set; }

        public ChoroPreferences() {
            CurrentFactSet = String.Empty;
        }
    }
}
