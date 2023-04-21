using System.Windows.Input;
using ChoroExplorer.Facts.FactEditor;
using ReusableBits.Wpf.Commands;
using ReusableBits.Wpf.DialogService;
using ReusableBits.Wpf.ViewModelSupport;

namespace ChoroExplorer.Facts.FactList {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class FactListViewModel : PropertyChangeBase {
        private readonly IDialogService     mDialogService;

        public  ICommand                    AddFact { get; }

        public FactListViewModel( IDialogService dialogService ) {
            mDialogService = dialogService;

            AddFact = new DelegateCommand( OnAddFact );
        }

        private void OnAddFact() {
            mDialogService.ShowDialog<FactEditorView>();
        }
    }
}
