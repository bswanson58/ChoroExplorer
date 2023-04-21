using ChoroExplorer.Models;
using Fluxor;

namespace ChoroExplorer.Facts {
    internal interface IFactsFacade {
        void    LoadFacts();
        void    AddFact( FactData fact );
        void    UpdateFact( FactData fact );

        void    LoadFactSets();
        void    UpdateFactSet( FactSet factSet );

        void    SetCurrentFactSet( FactSet factSet );
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

        public void LoadFactSets() {
            mDispatcher.Dispatch( new LoadFactSetsAction());
        }

        public void UpdateFactSet( FactSet factSet ) {
            mDispatcher.Dispatch( new UpdateFactSetAction( factSet ));
        } 

        public void SetCurrentFactSet( FactSet factSet ) {
            mDispatcher.Dispatch( new SetCurrentFactSetAction( factSet ));
        }
    }
}
