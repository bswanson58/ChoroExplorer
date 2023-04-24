using ChoroExplorer.Models;
using ReusableBits.Wpf.ViewModelSupport;
using System;

namespace ChoroExplorer.Facts.FactList {
    internal class FilterViewModel : PropertyChangeBase {
        private readonly Action<FilterViewModel>  mOnParameterChange;

        public  FilterData          Data { get; }
        public  FilterParameters    Parameters { get; private set; }

        public  string              FilterId => Data.FilterId;
        public  string              FilterKey => Data.FilterKey;
        public  string              FilterName => Data.FilterName;

        public FilterViewModel( FilterData data, Action<FilterViewModel> onChange ) {
            Data = data;
            Parameters = new FilterParameters( data );

            mOnParameterChange = onChange;
        }

        public void SetParameters( FilterParameters parameters ) {
            Parameters = parameters;

            RaisePropertyChanged( () => IsEnabled );
        }

        public bool IsEnabled {
            get => Parameters.Enabled;
            set {
                Parameters = Parameters.With( value );

                mOnParameterChange.Invoke( this );
            }
        }
    }
}
