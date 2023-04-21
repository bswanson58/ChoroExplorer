using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using ChoroExplorer.Facts.FactEditor;
using ChoroExplorer.Models;
using ChoroExplorer.Platform;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;
using ReusableBits.Wpf.Commands;
using ReusableBits.Wpf.DialogService;
using ReusableBits.Wpf.ViewModelSupport;

namespace ChoroExplorer.Facts.FactList {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class FactListViewModel : PropertyChangeBase {
        private readonly IDialogService             mDialogService;
        private readonly IEnvironment               mEnvironment;
        private readonly ILogger<FactListViewModel> mLogger;

        public  ObservableCollection<FactSet>       Facts { get; }

        public  ICommand                            AddFact { get; }

        public FactListViewModel( IDialogService dialogService, IEnvironment environment, ILogger<FactListViewModel> logger ) {
            mDialogService = dialogService;
            mEnvironment = environment;
            mLogger = logger;

            Facts = new ObservableCollection<FactSet>();

            AddFact = new DelegateCommand( OnAddFact );

            LoadFacts();
        }

        private void OnAddFact() {
            mDialogService.ShowDialog<FactEditorView>( result => {
                if( result.Result.Equals( ButtonResult.Ok )) {
                    try {
                        var factData = result.Parameters.GetValue<FactData>( FactEditorViewModel.cFactData );

                        if( factData != null ) {
                            var factPath = Path.ChangeExtension( 
                                Path.Combine( mEnvironment.FactsDirectory(), factData.FactId ), ChoroConstants.FactExtension );

                            FactLoader.SaveFact( factData, factPath );

                            LoadFacts();
                        }
                    }
                    catch( Exception ex ) {
                        mLogger.LogError( ex, String.Empty );
                    }
                }
            });
        }

        private void LoadFacts() {
            Facts.Clear();

            try {
                var factFiles = Directory.EnumerateFiles( mEnvironment.FactsDirectory(), $"*{ChoroConstants.FactExtension}" );

                foreach( var factFile in factFiles ) {
                    var factData = FactLoader.LoadFact( factFile );

                    Facts.Add( new FactSet( factData ));
                }
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }
        }
    }
}
