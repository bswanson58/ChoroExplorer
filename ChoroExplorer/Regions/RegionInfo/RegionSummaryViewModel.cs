using System;
using ChoroExplorer.Models;
using Fluxor.Selectors;
using ReusableBits.Wpf.ViewModelSupport;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChoroExplorer.Facts;
using ReusableBits.Wpf.Platform;

namespace ChoroExplorer.Regions.RegionInfo {
    internal class FactScoreViewModel {
        public  string  FactName { get; }
        public  int     FactScore { get; }

        public FactScoreViewModel( FactData fact, FactScore score ) {
            FactName = fact.FactName;
            FactScore = (int)( score.Score * 100 );
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class RegionSummaryViewModel : PropertyChangeBase, IDisposable {
        private readonly ISelectorSubscription<IReadOnlyList<RegionSummary>> mRegionSummarySelector;
        private readonly ISelectorSubscription<IReadOnlyList<FactData>>      mFactDataSelector;
        private readonly ISelectorSubscription<string>                       mFocusedRegionSelector;

        public  ObservableCollection<FactScoreViewModel>    Facts { get; }

        public  string      RegionName { get; private set; }
        public  int         RegionRank { get; private set; }
        public  int         RegionCount { get; private set; }
        public  int         RegionScore { get; private set; }

        public RegionSummaryViewModel( IFactSelectors factSelectors, IRegionSelectors regionSelectors ) {
            mRegionSummarySelector = regionSelectors.RegionSummarySelector();
            mRegionSummarySelector.StateChanged += OnRegionSummaryChanged;
            mFocusedRegionSelector = regionSelectors.FocusedRegionSelector();
            mFocusedRegionSelector.StateChanged += OnFocusedRegionChanged;
            
            mFactDataSelector = factSelectors.FactsSelector();

            Facts = new ObservableCollection<FactScoreViewModel>();

            RegionName = String.Empty;
            RegionRank = 0;
        }

        private void OnRegionSummaryChanged( object ? sender, EventArgs e ) {
            UpdateRegionSummary();
        }

        private void OnFocusedRegionChanged( object ? sender, EventArgs e ) {
            UpdateRegionSummary();
        }

        private void UpdateRegionSummary() {
            if(!String.IsNullOrWhiteSpace( mFocusedRegionSelector.Value )) {
                var scoreSortedRegions = 
                    mRegionSummarySelector.Value.Where( r => r.Enabled ).OrderByDescending( r => r.RegionScore ).ToList();
                var regionSummary = 
                    mRegionSummarySelector.Value.FirstOrDefault( r => r.RegionName.Equals( mFocusedRegionSelector.Value ));

                if( regionSummary != null ) {
                    RegionName = regionSummary.RegionName;
                    RegionRank = scoreSortedRegions.IndexOf( regionSummary ) + 1;
                    RegionCount = scoreSortedRegions.Count;
                    RegionScore = (int)( regionSummary.RegionScore * 100 );

                    Facts.Clear();
                    Facts.AddRange( BuildFactScores( regionSummary.FactScores, mFactDataSelector.Value ));
                }
            }

            RaiseAllPropertiesChanged();
        }

        private IEnumerable<FactScoreViewModel> BuildFactScores( IEnumerable<FactScore> scores, IEnumerable<FactData> facts ) =>
            from score in scores
            join fact in facts
                on score.FactId equals fact.FactId
            select new FactScoreViewModel( fact, score );

        public void Dispose() {
            mRegionSummarySelector.Dispose();
            mFocusedRegionSelector.Dispose();
            mFactDataSelector.Dispose();
        }
    }
}
