using ChoroExplorer.Models;
using ReusableBits.Wpf.ViewModelSupport;
using System;
using System.Windows.Input;
using ReusableBits.Wpf.Commands;

namespace ChoroExplorer.Facts.FactList {
    internal class FactViewModel : PropertyChangeBase {
        private readonly Action<FactViewModel>  mOnParameterChange;

        public  FactData        Data { get; }
        public  FactParameters  Parameters { get; private set; }

        public  string          FactId => Data.FactId;
        public  string          FactKey => Data.FactKey;
        public  string          FactName => Data.FactName;

        public  int             FactWeight { get; set; }

        public  ICommand        IncrementWeight { get; }
        public  ICommand        DecrementWeight { get; }

        public FactViewModel( FactData data, Action<FactViewModel> onChange ) {
            Data = data;
            Parameters = new FactParameters( data );

            IncrementWeight = new DelegateCommand( OnIncrementWeight );
            DecrementWeight = new DelegateCommand( OnDecrementWeight );

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

                RaisePropertyChanged( () => Weight );
                mOnParameterChange.Invoke( this );
            }
        }

        private void OnIncrementWeight() {
            if( Weight < 100 ) {
                Weight++;
            }
        }

        private void OnDecrementWeight() {
            if( Weight > 0 ) {
                Weight--;
            }
        }
    }
}
