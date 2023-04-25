using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using ChoroExplorer.Dialogs;
using ChoroExplorer.Facts.FactEditor;
using ChoroExplorer.Facts.FactSetEditor;
using ChoroExplorer.Filters;
using ChoroExplorer.Filters.FilterEditor;
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
        private readonly IFiltersFacade             mFilterFacade;
        private readonly ILogger<FactListViewModel> mLogger;
        private readonly ISelectorSubscription<IReadOnlyList<FactSet>>      mFactSetsSelector;
        private readonly ISelectorSubscription<IReadOnlyList<FactData>>     mFactsSelector;
        private readonly ISelectorSubscription<IReadOnlyList<FilterData>>   mFilterSelector;
        private FactSet ?                           mCurrentFactSet;


        public  ObservableCollection<FactViewModel>     Facts { get; }
        public  ObservableCollection<FilterViewModel>   Filters { get; }
        public  ObservableCollection<FactSet>           FactSets { get; }

        public  ICommand                                AddFactSet { get; }
        public  ICommand                                EditFactSet { get; }
        public  ICommand                                DeleteFactSet { get; }

        public  ICommand                                AddFact { get; }
        public  DelegateCommand<FactViewModel>          EditFact { get; }
        public  DelegateCommand<FactViewModel>          DeleteFact {  get; }

        public  ICommand                                AddFilter { get; }
        public  DelegateCommand<FilterViewModel>        EditFilter { get; }
        public  DelegateCommand<FilterViewModel>        DeleteFilter {  get; }

        public FactListViewModel( IDialogService dialogService, IFactsFacade factsFacade, IFiltersFacade filterFacade,
                                  IState<FactState> factState, IEnvironment environment,
                                  IFactSelectors  factSelectors, ILogger<FactListViewModel> logger ) {
            mDialogService = dialogService;
            mEnvironment = environment;
            mLogger = logger;
            mFactState = factState;
            mFactsFacade = factsFacade;
            mFilterFacade = filterFacade;

            mCurrentFactSet = null;

            Facts = new ObservableCollection<FactViewModel>();
            Filters = new ObservableCollection<FilterViewModel>();
            FactSets = new ObservableCollection<FactSet>();

            AddFact = new DelegateCommand( OnAddFact );
            EditFact = new DelegateCommand<FactViewModel>( OnEditFact );
            DeleteFact = new DelegateCommand<FactViewModel>( OnDeleteFact );

            AddFilter = new DelegateCommand( OnAddFilter );
            EditFilter = new DelegateCommand<FilterViewModel>( OnEditFilter );
            DeleteFilter = new DelegateCommand<FilterViewModel>( OnDeleteFilter );

            AddFactSet = new DelegateCommand( OnAddFactSet );
            EditFactSet = new DelegateCommand( OnEditFactSet );
            DeleteFactSet = new DelegateCommand( OnDeleteFactSet );

            LoadFacts();
            LoadFilters();
            LoadFactSets();

            mFactSetsSelector = factSelectors.FactSetsSelector();
            mFactSetsSelector.StateChanged += OnFactSetsChanged;
            mFactsSelector = factSelectors.FactsSelector();
            mFactsSelector.StateChanged += OnFactsChanged;
            mFilterSelector = factSelectors.FiltersSelector();
            mFilterSelector.StateChanged += OnFiltersChanged;
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
            SetFilterParameters();
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

        private void SetFilterParameters() {
            if( CurrentFactSet != null ) {
                foreach( var filter in Filters ) {
                    var parameters = CurrentFactSet.Filters.FirstOrDefault( f => f.FilterId.Equals( filter.FilterId )) ?? 
                                     new FilterParameters( filter.Data );

                    filter.SetParameters( parameters );
                }
            }
        }

        private void OnAddFactSet() {
            mDialogService.ShowDialog<FactSetEditorView>( result => {
                if( result.Result.Equals( ButtonResult.Ok )) {
                    var factSet = result.Parameters.GetValue<FactSet>( FactSetEditorViewModel.cFactSet );

                    if( factSet != null ) {
                        mFactsFacade.AddFactSet( factSet );

                        CurrentFactSet = factSet;
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

        private void OnDeleteFactSet() {
            if( CurrentFactSet != null ) {
                var parameters = new DialogParameters{
                    { ConfirmationDialogViewModel.cTitle, "Confirm Deletion" },
                    { ConfirmationDialogViewModel.cMessage, $"Do you want to delete fact set '{CurrentFactSet.SetName}'?" }
                };

                mDialogService.ShowDialog<ConfirmationDialog>( parameters, result => {
                    if( result.Result.Equals( ButtonResult.Ok )) {
                        var nextCurrent = FactSets.FirstOrDefault( s => !s.SetId.Equals( CurrentFactSet.SetId ));

                        mFactsFacade.DeleteFactSet( CurrentFactSet );

                        if( nextCurrent != null ) {
                            CurrentFactSet = nextCurrent;
                        }

                    }
                });
            }
        }

        private void OnFactSetsChanged( object ? sender, EventArgs e ) {
            LoadFactSets();
        }

        private void OnFiltersChanged( object ? sender, EventArgs e ) {
            LoadFilters();
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

        private void OnDeleteFact( FactViewModel ? fact ) {
            if( fact != null ) {
                var parameters = new DialogParameters{
                    { ConfirmationDialogViewModel.cTitle, "Confirm Deletion" },
                    { ConfirmationDialogViewModel.cMessage, $"Do you want to delete fact '{fact.FactName}'?" }
                };

                mDialogService.ShowDialog<ConfirmationDialog>( parameters, result => {
                    if( result.Result.Equals( ButtonResult.Ok )) {
                        mFactsFacade.DeleteFact( fact.Data );
                    }
                });
            }
        }

        private void OnAddFilter() {
            mDialogService.ShowDialog<FilterEditorView>( result => {
                if( result.Result.Equals( ButtonResult.Ok )) {
                    try {
                        var filterData = result.Parameters.GetValue<FilterData>( FilterEditorViewModel.cFilterData );

                        if( filterData != null ) {
                            mFilterFacade.AddFilter( filterData );
                        }
                    }
                    catch( Exception ex ) {
                        mLogger.LogError( ex, String.Empty );
                    }
                }
            });
        }

        private void OnEditFilter( FilterViewModel ? filter ) {
            if( filter != null ) {
                var parameters = new DialogParameters{{ FilterEditorViewModel.cFilterData, filter.Data }};

                mDialogService.ShowDialog<FilterEditorView>( parameters, result => {
                    if( result.Result.Equals( ButtonResult.Ok )) {
                        try {
                            var filterData = result.Parameters.GetValue<FilterData>( FilterEditorViewModel.cFilterData );

                            if( filterData != null ) {
                                mFilterFacade.UpdateFilter( filterData );
                            }
                        }
                        catch( Exception ex ) {
                            mLogger.LogError( ex, String.Empty );
                        }
                    }
                });
            }
        }

        private void OnDeleteFilter( FilterViewModel ? filter ) {
            if( filter != null ) { }
        }

        private void LoadFacts() {
            Facts.Clear();

            foreach( var fact in mFactState.Value.Facts.OrderBy( p => p.FactName )) {
                Facts.Add( new FactViewModel( fact, OnFactParameterChanged ));
            }
        }

        private void OnFactParameterChanged( FactViewModel viewModel ) {
            if( CurrentFactSet != null ) {
                var parameters = Facts.Where( f => !f.Parameters.IsDefault ).Select( f => f.Parameters );

                mFactsFacade.UpdateFactSet( CurrentFactSet.With( parameters ));
            }
        }

        private void LoadFilters() {
            Filters.Clear();

            foreach( var filter in mFactState.Value.Filters.OrderBy(  p => p.FilterName )) {
                Filters.Add( new FilterViewModel( filter, OnFilterParameterChanged ));
            }
        }

        private void OnFilterParameterChanged( FilterViewModel viewModel ) {
            if( CurrentFactSet != null ) {
                var parameters = Filters.Where( f => f.Parameters.Enabled ).Select( f => f.Parameters );

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
            mFilterSelector.Dispose();
        }
    }
}
