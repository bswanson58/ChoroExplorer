using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChoroExplorer.Models;
using ChoroExplorer.Regions;
using Fluxor;
using ReusableBits.Wpf.DialogService;

namespace ChoroExplorer.Facts.FactEditor {
    internal class FactValueVm {
        public  string      RegionId { get; }
        public  string      RegionName { get; }
        public  double      FactValue { get; set; }

        public FactValueVm( FactValue factValue, RegionData region ) {
            RegionId = region.RegionId;
            RegionName = region.RegionName;
            FactValue = factValue.Value;
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class FactEditorViewModel : DialogAwareBase {
        public  const string                        cFactData = "fact data";

        private FactData                            mFactData;
        private readonly IReadOnlyList<RegionData>  mRegions;

        public  ObservableCollection<FactValueVm>   FactValues { get; }

        private string                              mFactKey;
        private string                              mFactName;
        public  bool                                ReverseScore { get; set; }
        public  string                              FactDescription { get; set; }
        public  string                              FactSource { get; set; }

        public FactEditorViewModel( IState<RegionState> regionState ) {
            mRegions = regionState.Value.Regions;
            mFactData = new FactData();

            mFactKey = String.Empty;
            mFactName = String.Empty;
            ReverseScore = false;
            FactDescription = String.Empty;
            FactSource = String.Empty;

            FactValues = new ObservableCollection<FactValueVm>();

            Title = "Fact Information";
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mFactData = parameters.GetValue<FactData>( cFactData ) ?? new FactData();

            FactValues.Clear();
            foreach( var region in mRegions ) {
                var factValue = mFactData.RegionFacts
                    .FirstOrDefault( d => d.RegionId.Equals( region.RegionId ), new FactValue());

                FactValues.Add( new FactValueVm( factValue, region ));
            }

            FactKey = mFactData.FactKey;
            FactName = mFactData.FactName;
            FactDescription = mFactData.Description;
            FactSource = mFactData.Source;
            ReverseScore = mFactData.ReverseScore;

            RaiseAllPropertiesChanged();
        }

        public string FactKey {
            get => mFactKey;
            set {
                mFactKey = value;

                Ok.RaiseCanExecuteChanged();
            }
        }

        public string FactName {
            get => mFactName;
            set {
                mFactName = value;

                Ok.RaiseCanExecuteChanged();
            }
        }

        protected override bool CanAccept() =>
            !String.IsNullOrWhiteSpace( mFactKey ) &&
            !String.IsNullOrWhiteSpace( mFactName );

        protected override DialogParameters CreateClosingParameters() {
            var factValues = FactValues.Select( f => new FactValue( f.RegionId, f.FactValue )).ToList();
            var factData = 
                new FactData( mFactData.FactId, mFactKey, mFactName, FactDescription, FactSource, ReverseScore, factValues );

            return new DialogParameters { { cFactData, factData } };
        }
    }
}
