using System;
using ChoroExplorer.Models;
using ChoroExplorer.Regions;
using Fluxor;
using ReusableBits.Wpf.DialogService;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChoroExplorer.Filters.FilterEditor {
    internal class FilterValueVm {
        public  string      RegionId { get; }
        public  string      RegionName { get; }
        public  bool        FilterValue { get; set; }

        public FilterValueVm( FilterValue filterValue, RegionData region ) {
            RegionId = region.RegionId;
            RegionName = region.RegionName;
            FilterValue = filterValue.Enabled;
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class FilterEditorViewModel : DialogAwareBase {
        public  const string                        cFilterData = "filter data";

        private FilterData                          mFilterData;
        private readonly IReadOnlyList<RegionData>  mRegions;

        public  ObservableCollection<FilterValueVm> FilterValues { get; }

        private string                              mFilterKey;
        private string                              mFilterName;
        public  string                              FilterDescription { get; set; }
        public  string                              FilterSource { get; set; }

        public FilterEditorViewModel( IState<RegionState> regionState ) {
            mRegions = regionState.Value.Regions;
            mFilterData = new FilterData();

            mFilterKey = String.Empty;
            mFilterName = String.Empty;
            FilterDescription = String.Empty;
            FilterSource = String.Empty;

            FilterValues = new ObservableCollection<FilterValueVm>();

            Title = "Filter Information";
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mFilterData = parameters.GetValue<FilterData>( cFilterData ) ?? new FilterData();

            FilterValues.Clear();
            foreach( var region in mRegions ) {
                var factValue = mFilterData.RegionFilters
                    .FirstOrDefault( d => d.RegionId.Equals( region.RegionId ), new FilterValue());

                FilterValues.Add( new FilterValueVm( factValue, region ));
            }

            FilterKey = mFilterData.FilterKey;
            FilterName = mFilterData.FilterName;
            FilterDescription = mFilterData.Description;
            FilterSource = mFilterData.Source;

            RaiseAllPropertiesChanged();
        }

        public string FilterKey {
            get => mFilterKey;
            set {
                mFilterKey = value;

                Ok.RaiseCanExecuteChanged();
            }
        }

        public string FilterName {
            get => mFilterName;
            set {
                mFilterName = value;

                Ok.RaiseCanExecuteChanged();
            }
        }

        protected override bool CanAccept() =>
            !String.IsNullOrWhiteSpace( mFilterKey ) &&
            !String.IsNullOrWhiteSpace( mFilterName );

        protected override DialogParameters CreateClosingParameters() {
            var filterValues = FilterValues.Select( f => new FilterValue( f.RegionId, f.FilterValue )).ToList();
            var filterData = new FilterData( mFilterData.FilterId, mFilterKey, mFilterName, FilterDescription, FilterSource, filterValues );

            return new DialogParameters { { cFilterData, filterData } };
        }
    }
}
