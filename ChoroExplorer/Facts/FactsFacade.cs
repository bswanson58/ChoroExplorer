using ChoroExplorer.Models;
using Fluxor;

namespace ChoroExplorer.Facts {
    internal interface IFactsFacade {
        void    LoadFacts();
        void    AddFact( FactData fact );
        void    UpdateFact( FactData fact );
        void    DeleteFact( FactData fact );

        void    LoadFactSets();
        void    AddFactSet( FactSet factSet );
        void    UpdateFactSet( FactSet factSet );
        void    DeleteFactSet( FactSet factSet );

        void    SetCurrentFactSet( FactSet factSet );
        void    InitializeCurrentFactSet( string factSetId );
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

        public void DeleteFact( FactData fact ) {
            mDispatcher.Dispatch( new DeleteFactAction( fact ));
        }

        public void LoadFactSets() {
            mDispatcher.Dispatch( new LoadFactSetsAction());
        }

        public void AddFactSet( FactSet factSet ) {
            mDispatcher.Dispatch( new AddFactSetAction( factSet ));
        }
        
        public void UpdateFactSet( FactSet factSet ) {
            mDispatcher.Dispatch( new UpdateFactSetAction( factSet ));
        } 

        public void DeleteFactSet( FactSet factSet ) {
            mDispatcher.Dispatch( new DeleteFactSetAction( factSet ));
        } 

        public void SetCurrentFactSet( FactSet factSet ) {
            mDispatcher.Dispatch( new SetCurrentFactSetAction( factSet ));
        }

        public void InitializeCurrentFactSet( string factSetId ) {
            mDispatcher.Dispatch( new InitializeCurrentFactSetAction( factSetId ));
        }
    }
}
