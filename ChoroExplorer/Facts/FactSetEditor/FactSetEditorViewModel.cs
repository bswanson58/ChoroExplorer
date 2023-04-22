using System;
using ChoroExplorer.Models;
using ReusableBits.Wpf.DialogService;

namespace ChoroExplorer.Facts.FactSetEditor {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class FactSetEditorViewModel : DialogAwareBase {
        public  const string    cFactSet = "fact set";

        private FactSet ?   mFactSet;
        private string      mFactSetName;

        public FactSetEditorViewModel() {
            mFactSetName = String.Empty;

            Title = "Fact Set";
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mFactSet = parameters.GetValue<FactSet>( cFactSet ) ?? FactSet.UnnamedSet;

            mFactSetName = mFactSet.SetName;

            RaiseAllPropertiesChanged();
        }

        public string FactSetName {
            get => mFactSetName;
            set {
                mFactSetName = value;

                Ok.RaiseCanExecuteChanged();
            }
        }

        protected override DialogParameters CreateClosingParameters() =>
            new DialogParameters { { cFactSet, mFactSet!.With( mFactSetName )} };

        protected override bool CanAccept() =>
            !String.IsNullOrWhiteSpace( mFactSetName );
    }
}
