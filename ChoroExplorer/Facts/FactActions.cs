using ChoroExplorer.Models;

namespace ChoroExplorer.Facts {
    internal class LoadFactsAction {}

    internal class AddFactAction {
        public  FactData    Fact { get; }

        public AddFactAction( FactData fact ) {
            Fact = fact;
        }
    }

    internal class PopulateFactAction {
        public  FactData    Fact { get; }

        public PopulateFactAction( FactData fact ) {
            Fact = fact;
        }
    }

    internal class UpdateFactAction {
        public  FactData    Fact { get; }

        public UpdateFactAction( FactData fact ) {
            Fact = fact;
        }
    }

    internal class DeleteFactAction {
        public  FactData    Fact { get; }

        public DeleteFactAction( FactData fact ) {
            Fact = fact;
        }
    }

    internal class LoadFactSetsAction { }

    internal class AddFactSetAction {
        public  FactSet     FactSet { get; }

        public AddFactSetAction( FactSet factSet ) {
            FactSet = factSet;
        }
    }

    internal class PopulateFactSetAction {
        public  FactSet     FactSet { get; }

        public PopulateFactSetAction( FactSet factSet ) {
            FactSet = factSet;
        }
    }

    internal class UpdateFactSetAction {
        public  FactSet     FactSet { get; }

        public UpdateFactSetAction( FactSet factSet ) {
            FactSet = factSet;
        }
    }

    internal class DeleteFactSetAction {
        public  FactSet     FactSet { get; }

        public DeleteFactSetAction( FactSet factSet ) {
            FactSet = factSet;
        }
    }

    internal class SetCurrentFactSetAction {
        public  string  FactSetId { get; }

        public SetCurrentFactSetAction( FactSet factSet ) {
            FactSetId = factSet.SetId;
        }
    }

    internal class InitializeCurrentFactSetAction {
        public  string  FactSetId { get; }

        public InitializeCurrentFactSetAction( string factSetId ) {
            FactSetId = factSetId;
        }
    }
}
