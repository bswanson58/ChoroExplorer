using ChoroExplorer.Models;
using ReusableBits.Wpf.ViewModelSupport;
using System;

namespace ChoroExplorer.Facts.FactList {
    internal class FactViewModel : PropertyChangeBase {
        private readonly Action<FactViewModel>  mOnParameterChange;

        public  FactData        Data { get; }
        public  FactParameters  Parameters { get; private set; }

        public  string          FactId => Data.FactId;
        public  string          FactKey => Data.FactKey;
        public  string          FactName => Data.FactName;

        public FactViewModel( FactData data, Action<FactViewModel> onChange ) {
            Data = data;
            Parameters = new FactParameters( data );

            mOnParameterChange = onChange;
        }

        public void SetParameters( FactParameters parameters ) {
            Parameters = parameters;

            RaisePropertyChanged( () => IsEnabled );
            RaisePropertyChanged( () => Weight );
        }

        public bool IsEnabled {
            get => Parameters.Enabled;
            set {
                Parameters = Parameters.With( value );

                mOnParameterChange.Invoke( this );
            }
        }

        public int Weight {
            get => Parameters.Weight;
            set {
                Parameters = Parameters.With( value );

                mOnParameterChange.Invoke( this );
            }
        }
    }
}
