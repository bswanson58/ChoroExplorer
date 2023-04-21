using ChoroExplorer.Models;
using Fluxor;

namespace ChoroExplorer.Facts {
    internal interface IFactsFacade {
        void    LoadFacts();
        void    AddFact( FactData fact );
        void    UpdateFact( FactData fact );
    }

    internal class FactsFacade : IFactsFacade {
        private readonly IDispatcher    mDispatcher;

        public FactsFacade( IDispatcher dispatcher ) {
            mDispatcher = dispatcher;
        }

        public void LoadFacts() {
            mDispatcher.Dispatch( new LoadFactsAction());
        }

        public void AddFact( FactData fact ) {
            mDispatcher.Dispatch( new AddFactAction( fact ));
        }

        public void UpdateFact( FactData fact ) {
            mDispatcher.Dispatch( new UpdateFactAction( fact ));
        }
    }
}
