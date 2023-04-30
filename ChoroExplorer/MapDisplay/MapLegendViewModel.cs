using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using ChoroExplorer.ColorMapping;
using ChoroExplorer.Models;
using ChoroExplorer.Regions;
using Fluxor;
using Fluxor.Selectors;
using ReusableBits.Wpf.Commands;
using ReusableBits.Wpf.Platform;

namespace ChoroExplorer.MapDisplay {
    internal class RegionScoreLegend {
        public  string  RegionName { get; }
        public  int     RegionScore { get; }

        public RegionScoreLegend( RegionSummary regionSummary ) {
            RegionName = regionSummary.RegionName;
            RegionScore = (int)( regionSummary.RegionScore * 1000 );
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class MapLegendViewModel : IDisposable {
        private readonly ISelectorSubscription<IReadOnlyList<RegionSummary>>    mRegionSummary;
        private readonly IState<RegionState>            mRegionState;
        private readonly IRegionsFacade                 mRegionFacade;
        private readonly IRegionFilter                  mRegionFilter;

        public  ObservableCollection<RegionScoreLegend> Regions { get; }
        public  LinearGradientBrush                     LegendBrush { get; }

        public  DelegateCommand<RegionScoreLegend>      LegendFocus { get; }

        public MapLegendViewModel( IColorMapper colorMapper, IRegionSelectors regionSelectors, IRegionsFacade regionFacade,
                                   IRegionFilter regionFilter, IState<RegionState> regionState ) {
            mRegionFilter = regionFilter;
            mRegionState = regionState;
            mRegionFacade = regionFacade;
            mRegionSummary = regionSelectors.RegionSummarySelector();
            mRegionSummary.StateChanged += OnRegionSummaryChanged;

            Regions = new ObservableCollection<RegionScoreLegend>();
            LegendFocus = new DelegateCommand<RegionScoreLegend>( OnLegendFocus );

            LegendBrush = new LinearGradientBrush { GradientStops = colorMapper.MappingColors };
        }

        private void OnRegionSummaryChanged( object ? sender, EventArgs e ) {
            Regions.Clear();
            Regions.AddRange( 
                mRegionFilter
                    .FilterRegions( mRegionSummary.Value, mRegionState.Value.RegionFilter )
                    .Select( r => new RegionScoreLegend( r )));
        }

        private void OnLegendFocus( RegionScoreLegend ? region ) {
            if( region != null ) {
                mRegionFacade.SetFocusedRegion( region.RegionName );
            }
        }

        public void Dispose() {
            mRegionSummary.Dispose();
        }
    }
}
