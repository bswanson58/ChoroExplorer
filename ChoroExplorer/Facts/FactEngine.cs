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
            mCurrentFactsSelector = mFactSelectors.CurrentFactSelector();
            mCurrentFactsSelector.StateChanged += OnCurrentFactChanged;

            mRegionsSelector = mRegionSelectors.RegionsSelector();
            mRegionsSelector.StateChanged += OnRegionsChanged;
        }

        public void StopFactEngine() {
            if( mFactsSelector != null ) {
                mFactsSelector.StateChanged -= OnFactsChanged;
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

        private void UpdateRegionScores() {
            if(( mFactSetsSelector != null ) &&
               ( mFactsSelector != null ) &&
               ( mRegionsSelector != null ) &&
               (!String.IsNullOrWhiteSpace( mCurrentFactsSelector?.Value ))) {
                var factSet = mFactSetsSelector.Value.FirstOrDefault( s => s.SetId.Equals( mCurrentFactsSelector.Value ));

                if( factSet != null ) {
                    var regionSummaries = new List<RegionSummary>();
                    var regionFactScores = CalculateFactScores( factSet );
                    var regionScores = CollateRegionScores( regionFactScores );

                    foreach( var region in regionScores ) {
                        var regionDetail = mRegionsSelector.Value.FirstOrDefault( r => r.RegionId.Equals( region.RegionId ));

                        if( regionDetail != null ) {
                            var scores = regionFactScores
                                .Where( s => s.RegionId.Equals( region.RegionId ))
                                .Select( s => new FactScore( s.FactId, s.Score ))
                                .ToList();

                            regionSummaries.Add( 
                                new RegionSummary( regionDetail.RegionId, regionDetail.RegionName, region.Score, scores ));
                        }
                    }

                    mRegionsFacade.UpdateRegionScores( regionSummaries );
                }
            }
        }

        private IList<RegionScore> CollateRegionScores( IList<RegionFactScore> scores ) =>
            scores
                .GroupBy( s => s.RegionId )
                .Select( g => new RegionScore( g.Key, g.Sum( v => v.Score )))
                .ToList();

        private IList<RegionFactScore> CalculateFactScores( FactSet factSet ) {
            var scores = new List<RegionFactScore>();

            if( mFactsSelector != null ) {
                var activeIds = factSet.Facts.Select( f => f.FactId ).ToList();
                var activeFacts = mFactsSelector.Value.Where( f => activeIds.Contains( f.FactId )).ToList();

                foreach( var fact in activeFacts ) {
                    var factContext = 
                        new FactContext( fact.FactId, fact.RegionFacts.Min( f => f.Value ), fact.RegionFacts.Max( f => f.Value ));

                    foreach( var region in fact.RegionFacts ) {
                        scores.Add( CalculateFactValue( factContext, region ));
                    }
                }
            }

            return scores;
        }

        private RegionFactScore CalculateFactValue( FactContext context, FactValue value ) {
            var factRange = context.MaximumFactValue - context.MinimumFactValue;
            var score = ( value.Value - context.MinimumFactValue ) / factRange;

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
