using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using ChoroExplorer.Facts.FactEditor;
using ChoroExplorer.Facts.FactSetEditor;
using ChoroExplorer.Models;
using ChoroExplorer.Platform;
using Fluxor;
using Fluxor.Selectors;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;
using ReusableBits.Wpf.Commands;
using ReusableBits.Wpf.DialogService;
using ReusableBits.Wpf.ViewModelSupport;

namespace ChoroExplorer.Facts.FactList {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class FactListViewModel : PropertyChangeBase, IDisposable {
        private readonly IDialogService             mDialogService;
        private readonly IEnvironment               mEnvironment;
        private readonly IState<FactState>          mFactState;
        private readonly IFactsFacade               mFactsFacade;
        private readonly ILogger<FactListViewModel> mLogger;
        private readonly ISelectorSubscription<IReadOnlyList<FactSet>>  mFactSetsSelector;
        private readonly ISelectorSubscription<IReadOnlyList<FactData>> mFactsSelector;
        private FactSet ?                           mCurrentFactSet;


        public  ObservableCollection<FactViewModel> Facts { get; }
        public  ObservableCollection<FactSet>       FactSets { get; }

        public  ICommand                            AddFactSet { get; }
        public  ICommand                            EditFactSet { get; }
        public  ICommand                            AddFact { get; }
        public  DelegateCommand<FactViewModel>      EditFact { get; }
        public  DelegateCommand<FactViewModel>      DeleteFact {  get; }

        public FactListViewModel( IDialogService dialogService, IFactsFacade factsFacade, IState<FactState> factState,
                                  IFactSelectors  factSelectors, IEnvironment environment, ILogger<FactListViewModel> logger ) {
            mDialogService = dialogService;
            mEnvironment = environment;
            mLogger = logger;
            mFactState = factState;
            mFactsFacade = factsFacade;

            mCurrentFactSet = null;

            Facts = new ObservableCollection<FactViewModel>();
            FactSets = new ObservableCollection<FactSet>();

            AddFact = new DelegateCommand( OnAddFact );
            EditFact = new DelegateCommand<FactViewModel>( OnEditFact );
            DeleteFact = new DelegateCommand<FactViewModel>( OnDeleteFact );

            AddFactSet = new DelegateCommand( OnAddFactSet );
            EditFactSet = new DelegateCommand( OnEditFactSet );

            LoadFacts();
            LoadFactSets();

            mFactSetsSelector = factSelectors.FactSetsSelector();
            mFactSetsSelector.StateChanged += OnFactSetsChanged;
            mFactsSelector = factSelectors.FactsSelector();
            mFactsSelector.StateChanged += OnFactsChanged;
        }

        public FactSet ? CurrentFactSet {
            get => mCurrentFactSet;
            set {
                mCurrentFactSet = value;

                if( value != null ) {
                    mFactsFacade.SetCurrentFactSet( value );

                    UpdateCurrentFactSet();
                }
            }
        }

        private void UpdateCurrentFactSet() {
            SetFactParameters();
            RaisePropertyChanged( () => CurrentFactSet );
        }

        private void SetFactParameters() {
            if( CurrentFactSet != null ) {
                foreach( var fact in Facts ) {
                    var parameters = CurrentFactSet.Facts.FirstOrDefault( f => f.FactId.Equals( fact.FactId )) ?? 
                                     new FactParameters( fact.Data );

                    fact.SetParameters( parameters );
                }
            }
        }

        private void OnAddFactSet() {
            mDialogService.ShowDialog<FactSetEditorView>( result => {
                if( result.Result.Equals( ButtonResult.Ok )) {
                    var factSet = result.Parameters.GetValue<FactSet>( FactSetEditorViewModel.cFactSet );

                    if( factSet != null ) {
                        mFactsFacade.AddFactSet( factSet );
                    }
                }
            });
        }

        private void OnEditFactSet() {
            if( CurrentFactSet != null ) {
                var parameters = new DialogParameters{{ FactSetEditorViewModel.cFactSet, CurrentFactSet }};

                mDialogService.ShowDialog<FactSetEditorView>( parameters, result => {
                    if( result.Result.Equals( ButtonResult.Ok )) {
                        var factSet = result.Parameters.GetValue<FactSet>( FactSetEditorViewModel.cFactSet );

                        if( factSet != null ) {
                            mFactsFacade.UpdateFactSet( factSet );
                        }
                    }
                });
            }
        }

        private void OnFactSetsChanged( object ? sender, EventArgs e ) {
            LoadFactSets();
        }

        private void OnFactsChanged( object ? sender, EventArgs e ) {
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

            foreach( var fact in mFactState.Value.Facts.OrderBy( p => p.FactName )) {
                Facts.Add( new FactViewModel( fact, OnFactParameterChanged ));
            }
        }

        private void OnFactParameterChanged( FactViewModel viewModel ) {
            if( CurrentFactSet != null ) {
                var parameters = Facts.Where( f => f.Parameters.Enabled ).Select( f => f.Parameters );

                mFactsFacade.UpdateFactSet( CurrentFactSet.With( parameters ));
            }
        }

        private void LoadFactSets() {
            FactSets.Clear();

            foreach( var factSet in mFactState.Value.FactSets.OrderBy( p => p.SetName )) {
                FactSets.Add( factSet );
            }

            if( FactSets.Any()) {
                mCurrentFactSet = FactSets.FirstOrDefault( s => s.SetId.Equals( mFactState.Value.CurrentFactSet )) ??
                                  FactSets.First();

                UpdateCurrentFactSet();
            }
        }

        public void Dispose() {
            mFactSetsSelector.Dispose();
            mFactsSelector.Dispose();
        }
    }
}
