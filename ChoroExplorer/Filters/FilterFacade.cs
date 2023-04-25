using ChoroExplorer.Models;
using Fluxor;

namespace ChoroExplorer.Filters {
    internal interface IFiltersFacade {
        void    LoadFilters();
        void    AddFilter( FilterData filter );
        void    UpdateFilter( FilterData filter );
        void    DeleteFilter( FilterData filter );
    }

    internal class FiltersFacade : IFiltersFacade {
        private readonly IDispatcher    mDispatcher;

        public FiltersFacade( IDispatcher dispatcher ) {
            mDispatcher = dispatcher;
        }

        public void LoadFilters() {
            mDispatcher.Dispatch( new LoadFiltersAction());
        }

        public void AddFilter( FilterData filter ) {
            mDispatcher.Dispatch( new AddFilterAction( filter ));
        }

        public void UpdateFilter( FilterData filter ) {
            mDispatcher.Dispatch( new UpdateFilterAction( filter ));
        }

        public void DeleteFilter( FilterData filter ) {
            mDispatcher.Dispatch( new DeleteFilterAction( filter ));
        }
    }
}
