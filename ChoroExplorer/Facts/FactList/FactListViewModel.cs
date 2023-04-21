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
    internal class FactViewModel {
        public  FactData        Data { get; }
        public  FactParameters  Parameters { get; }

        public  string          FactKey => Data.FactKey;
        public  string          FactName => Data.FactName;

        public FactViewModel( FactData data ) {
            Data = data;
            Parameters = new FactParameters();
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class FactListViewModel : PropertyChangeBase {
        private readonly IDialogService             mDialogService;
        private readonly IEnvironment               mEnvironment;
        private readonly IState<FactState>          mFactState;
        private readonly IFactsFacade               mFactsFacade;
        private readonly ILogger<FactListViewModel> mLogger;
        private FactSet ?                           mCurrentFactSet;

        public  ObservableCollection<FactViewModel> Facts { get; }
        public  ObservableCollection<FactSet>       FactSets { get; }

        public  ICommand                            AddFact { get; }
        public  DelegateCommand<FactViewModel>      EditFact { get; }
        public  DelegateCommand<FactViewModel>      DeleteFact {  get; }

        public FactListViewModel( IDialogService dialogService, IFactsFacade factsFacade, IState<FactState> factState,
                                  IEnvironment environment, ILogger<FactListViewModel> logger ) {
            mDialogService = dialogService;
            mEnvironment = environment;
            mLogger = logger;
            mFactState = factState;
            mFactsFacade = factsFacade;

            mCurrentFactSet = null;

            Facts = new ObservableCollection<FactViewModel>();
            FactSets = new ObservableCollection<FactSet>();
            mFactState.StateChanged += OnFactStateChanged;

            AddFact = new DelegateCommand( OnAddFact );
            EditFact = new DelegateCommand<FactViewModel>( OnEditFact );
            DeleteFact = new DelegateCommand<FactViewModel>( OnDeleteFact );

            LoadFacts();
            LoadFactSets();
        }

        public FactSet ? CurrentFactSet {
            get => mCurrentFactSet;
            set {
                mCurrentFactSet = value;

                if( value != null ) {
                    mFactsFacade.SetCurrentFactSet( value );
                }
            }
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

        private void OnEditFact( FactViewModel ? factSet ) {
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

        private void OnDeleteFact( FactViewModel ? factSet ) { }

        private void LoadFacts() {
            Facts.Clear();

            foreach( var fact in mFactState.Value.Facts ) {
                Facts.Add( new FactViewModel( fact ));
            }
        }

        private void LoadFactSets() {
            FactSets.Clear();

            foreach( var factSet in mFactState.Value.FactSets ) {
                FactSets.Add( factSet );
            }
        }
    }
}
