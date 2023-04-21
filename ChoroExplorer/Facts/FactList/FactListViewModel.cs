using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using ChoroExplorer.Facts.FactEditor;
using ChoroExplorer.Models;
using ChoroExplorer.Platform;
using Fluxor;
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
        private readonly IState<FactState>          mFactState;
        private readonly IFactsFacade               mFactsFacade;
        private readonly ILogger<FactListViewModel> mLogger;

        public  ObservableCollection<FactSet>       Facts { get; }

        public  ICommand                            AddFact { get; }
        public  DelegateCommand<FactSet>            EditFact { get; }
        public  DelegateCommand<FactSet>            DeleteFact {  get; }

        public FactListViewModel( IDialogService dialogService, IFactsFacade factsFacade, IState<FactState> factState,
                                  IEnvironment environment, ILogger<FactListViewModel> logger ) {
            mDialogService = dialogService;
            mEnvironment = environment;
            mLogger = logger;
            mFactState = factState;
            mFactsFacade = factsFacade;

            Facts = new ObservableCollection<FactSet>();
            mFactState.StateChanged += OnFactStateChanged;

            AddFact = new DelegateCommand( OnAddFact );
            EditFact = new DelegateCommand<FactSet>( OnEditFact );
            DeleteFact = new DelegateCommand<FactSet>( OnDeleteFact );

            LoadFacts();
        }

        private void OnFactStateChanged( object ? sender, EventArgs e ) {
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
                            mFactsFacade.AddFact( factData );
                        }
                    }
                    catch( Exception ex ) {
                        mLogger.LogError( ex, String.Empty );
                    }
                }
            });
        }

        private void OnEditFact( FactSet ? factSet ) {
            if( factSet != null ) {
                var parameters = new DialogParameters{{ FactEditorViewModel.cFactData, factSet.Data }};

                mDialogService.ShowDialog<FactEditorView>( parameters, result => {
                    if( result.Result.Equals( ButtonResult.Ok )) {
                        try {
                            var factData = result.Parameters.GetValue<FactData>( FactEditorViewModel.cFactData );

                            if( factData != null ) {
                                var factPath = Path.ChangeExtension( 
                                    Path.Combine( mEnvironment.FactsDirectory(), factData.FactId ), ChoroConstants.FactExtension );

                                FactLoader.SaveFact( factData, factPath );
                                mFactsFacade.UpdateFact( factData );
                            }
                        }
                        catch( Exception ex ) {
                            mLogger.LogError( ex, String.Empty );
                        }
                    }
                });
            }
        }

        private void OnDeleteFact( FactSet ? factSet ) { }

        private void LoadFacts() {
            Facts.Clear();

            foreach( var fact in mFactState.Value.Facts ) {
                Facts.Add( new FactSet( fact ));
            }
        }
    }
}
