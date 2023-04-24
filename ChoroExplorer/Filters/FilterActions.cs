using ChoroExplorer.Models;

namespace ChoroExplorer.Filters {
    internal class LoadFiltersAction { }

    internal class AddFilterAction {
        public  FilterData    Filter { get; }

        public AddFilterAction( FilterData filter ) {
            Filter = filter;
        }
    }

    internal class PopulateFilterAction {
        public  FilterData    Filter { get; }

        public PopulateFilterAction( FilterData filter ) {
            Filter = filter;
        }
    }

    internal class UpdateFilterAction {
        public  FilterData    Filter { get; }

        public UpdateFilterAction( FilterData filter ) {
            Filter = filter;
        }
    }
}
