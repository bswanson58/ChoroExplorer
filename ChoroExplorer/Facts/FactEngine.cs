using System;
using ChoroExplorer.Models;
using System.Collections.Generic;
using System.Linq;
using ChoroExplorer.Regions;
using Fluxor.Selectors;

namespace ChoroExplorer.Facts {
    internal interface IFactEngine {
        void    StartFactEngine();
        void    StopFactEngine();
    }

    internal class FactEngine : IFactEngine, IDisposable {
        private readonly IFactSelectors     mFactSelectors;
        private readonly IRegionSelectors   mRegionSelectors;
        private readonly IRegionsFacade     mRegionsFacade;

        private ISelectorSubscription<IReadOnlyList<RegionData>> ?  mRegionsSelector;
        private ISelectorSubscription<IReadOnlyList<FactSet>> ?     mFactSetsSelector;
        private ISelectorSubscription<IReadOnlyList<FilterData>> ?  mFiltersSelector;
        private ISelectorSubscription<IReadOnlyList<FactData>> ?    mFactsSelector;
        private ISelectorSubscription<String> ?                     mCurrentFactsSelector;

        public FactEngine( IFactSelectors factSelectors, IRegionSelectors regionSelectors, IRegionsFacade regionsFacade ) {
            mFactSelectors = factSelectors;
            mRegionSelectors = regionSelectors;
            mRegionsFacade = regionsFacade;
        }

        public void StartFactEngine() {
            mFactsSelector = mFactSelectors.FactsSelector();
            mFactsSelector.StateChanged += OnFactsChanged;
            mFactSetsSelector = mFactSelectors.FactSetsSelector();
            mFactSetsSelector.StateChanged += OnFactSetsChanged;
            mFiltersSelector = mFactSelectors.FiltersSelector();
            mFiltersSelector.StateChanged += OnFiltersChanged;
            mCurrentFactsSelector = mFactSelectors.CurrentFactSelector();
            mCurrentFactsSelector.StateChanged += OnCurrentFactChanged;

            mRegionsSelector = mRegionSelectors.RegionsSelector();
            mRegionsSelector.StateChanged += OnRegionsChanged;
        }

        public void StopFactEngine() {
            if( mFactsSelector != null ) {
                mFactsSelector.StateChanged -= OnFactsChanged;
            }
            if( mFiltersSelector != null ) {
                mFiltersSelector.StateChanged -= OnFiltersChanged;
            }
            if( mFactSetsSelector != null ) {
                mFactSetsSelector.StateChanged -= OnFactSetsChanged;
            }
            if( mCurrentFactsSelector != null ) {
                mCurrentFactsSelector.StateChanged -= OnCurrentFactChanged;
            }
            if( mRegionsSelector != null ) {
                mRegionsSelector.StateChanged -= OnRegionsChanged;
            }
        }

        private void OnRegionsChanged( object ? sender, EventArgs e ) {
            UpdateRegionScores();
        }

        private void OnCurrentFactChanged( object ? sender, EventArgs e ) {
            UpdateRegionScores();
        }

        private void OnFactSetsChanged( object ? sender, EventArgs e ) {
            UpdateRegionScores();
        }

        private void OnFactsChanged( object ? sender, EventArgs e ) {
            UpdateRegionScores();
        }

        private void OnFiltersChanged( object ? sender, EventArgs e ) {
            UpdateRegionScores();
        }

        private void UpdateRegionScores() {
            if(( mFactSetsSelector != null ) &&
               ( mFactsSelector != null ) &&
               ( mFiltersSelector != null ) &&
               ( mRegionsSelector != null ) &&
               (!String.IsNullOrWhiteSpace( mCurrentFactsSelector?.Value ))) {
                var factSet = mFactSetsSelector.Value.FirstOrDefault( s => s.SetId.Equals( mCurrentFactsSelector.Value ));

                if( factSet != null ) {
                    var filters = CombineFilters( factSet.Filters ).ToList();
                    var regionSummaries = new List<RegionSummary>();
                    var regionFactScores = CalculateFactScores( factSet, filters );
                    var regionScores = CollateRegionScores( regionFactScores );

                    foreach( var region in regionScores ) {
                        var regionDetail = mRegionsSelector.Value.FirstOrDefault( r => r.RegionId.Equals( region.RegionId ));

                        if( regionDetail != null ) {
                            var scores = regionFactScores
                                .Where( s => s.RegionId.Equals( region.RegionId ))
                                .Select( s => new FactScore( s.FactId, s.Score ))
                                .ToList();

                            regionSummaries.Add( 
                                new RegionSummary( regionDetail.RegionId, regionDetail.RegionName, 
                                                   region.Score, region.Enabled, scores ));
                        }
                    }

                    mRegionsFacade.UpdateRegionScores( regionSummaries );
                }
            }
        }

        private IEnumerable<FilterValue> CombineFilters( IList<FilterParameters> filterParameters ) {
            if( mFiltersSelector != null ) {
                var filterList = SelectFilters( filterParameters ).ToList();

                if( filterList.Any()) {
                    var filters = filterList.First().RegionFilters;

                    if( filterList.Count > 1 ) {
                        foreach( var filter in filterList.Skip( 1 )) {
                            filters = CombineFilters( filters, filter.RegionFilters ).ToList();
                        }
                    }

                    return filters;
                }
            }

            return Enumerable.Empty<FilterValue>();
        }

        private IEnumerable<FilterData> SelectFilters( IList<FilterParameters> filterParameters ) =>
            from parameter in filterParameters
            join filter in mFiltersSelector!.Value
                on parameter.FilterId equals filter.FilterId
            select filter;

        private IEnumerable<FilterValue> CombineFilters( IEnumerable<FilterValue> filter1, IEnumerable<FilterValue> filter2 ) =>
                from f1 in filter1
                join f2 in filter2
                    on f1.RegionId equals f2.RegionId
                select new FilterValue( f1.RegionId, f1.Enabled && f2.Enabled );

        private IList<RegionScore> CollateRegionScores( IList<RegionFactScore> scores ) =>
            scores
                .GroupBy( s => s.RegionId )
                .Select( g => new RegionScore( g.Key, g.Sum( v => v.Score ), g.First().Enabled ))
                .ToList();

        private IList<RegionFactScore> CalculateFactScores( FactSet factSet, IList<FilterValue> filters ) {
            var scores = new List<RegionFactScore>();

            if( mFactsSelector != null ) {
                var activeFacts = CollectFacts( factSet.Facts ).ToList();

                foreach( var fact in activeFacts ) {
                    var enabledRegions = 
                        filters.Any() ? SelectEnabledRegions( fact.RegionFacts, filters ).ToList() : fact.RegionFacts;
                    var factContext = 
                        new FactContext( fact, enabledRegions.Min( f => f.Value ), enabledRegions.Max( f => f.Value ));

                    foreach( var region in enabledRegions ) {
                        scores.Add( CalculateFactValue( factContext, region ));
                    }

                    // Add in disabled regions
                    foreach( var region in filters.Where( f => !f.Enabled )) {
                        scores.Add( new RegionFactScore( fact.FactId, region.RegionId ));
                    }
                }
            }

            return scores;
        }

        private IEnumerable<FactData> CollectFacts( IEnumerable<FactParameters> factParameters ) =>
            from parameter in factParameters
            join fact in mFactsSelector!.Value
                on parameter.FactId equals fact.FactId
            select fact;

        private IEnumerable<FactValue> SelectEnabledRegions( IEnumerable<FactValue> regions, IEnumerable<FilterValue> filters ) =>
            from filter in filters
            join region in regions
                on filter.RegionId equals region.RegionId
            where filter.Enabled
            select region;
        

        private RegionFactScore CalculateFactValue( FactContext context, FactValue value ) {
            var factRange = context.MaximumFactValue - context.MinimumFactValue;
            var score = context.Fact.ReverseScore ? 
                    1.0 - (( value.Value - context.MinimumFactValue ) / factRange ) :
                    ( value.Value - context.MinimumFactValue ) / factRange;

            return new RegionFactScore( context.FactId, value.RegionId, score );
        }

        public void Dispose() {
            mRegionsSelector?.Dispose();
            mFactSetsSelector?.Dispose();
            mFactsSelector?.Dispose();
            mCurrentFactsSelector?.Dispose();
        }
    }
}
