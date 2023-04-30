using System.Collections.ObjectModel;
using System.Linq;
using ChoroExplorer.Models;
using ChoroExplorer.Regions;
using ReusableBits.Wpf.ViewModelSupport;

namespace ChoroExplorer.MapDisplay {
    internal class MapFilterOption {
        public  string          FilterName { get; }
        public  eRegionFilter   Filter { get; }

        public MapFilterOption( eRegionFilter filter, string filterName ) {
            Filter = filter;
            FilterName = filterName;
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class MapFilterViewModel : PropertyChangeBase {
        private readonly IRegionsFacade     mRegionsFacade;
        private MapFilterOption ?           mSelectedFilter;

        public ObservableCollection<MapFilterOption>    Filters { get; }

        public MapFilterViewModel( IRegionsFacade regionsFacade ) {
            mRegionsFacade = regionsFacade;
            Filters = new ObservableCollection<MapFilterOption> {
                new ( eRegionFilter.AllRegions, "All Regions" ),
                new ( eRegionFilter.Top10, "Top 10 Regions" ),
                new ( eRegionFilter.Bottom10, "Bottom 10 Regions" )
            };

            SelectedFilter = Filters.First();
        }

        public MapFilterOption ? SelectedFilter {
            get => mSelectedFilter;
            set {
                mSelectedFilter = value;

                if( mSelectedFilter != null ) {
                    mRegionsFacade.SetRegionFilter( mSelectedFilter.Filter );
                }

                RaisePropertyChanged( () => SelectedFilter );
            }
        }
    }
}
