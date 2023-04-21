using ChoroExplorer.Models;

namespace ChoroExplorer.Facts {
    internal class LoadFactsAction {}

    internal class AddFactAction {
        public  FactData    Fact { get; }

        public AddFactAction( FactData fact ) {
            Fact = fact;
        }
    }

    internal class UpdateFactAction {
        public  FactData    Fact { get; }

        public UpdateFactAction( FactData fact ) {
            Fact = fact;
        }
    }
}
