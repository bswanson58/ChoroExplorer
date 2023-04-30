﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using ChoroExplorer.ColorMapping;
using ChoroExplorer.Models;
using ChoroExplorer.Regions;
using Fluxor.Selectors;
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

        public  ObservableCollection<RegionScoreLegend> Regions { get; }
        public  LinearGradientBrush                     LegendBrush { get; }

        public MapLegendViewModel( IColorMapper colorMapper, IRegionSelectors regionSelectors ) {
            mRegionSummary = regionSelectors.RegionSummarySelector();
            mRegionSummary.StateChanged += OnRegionSummaryChanged;

            Regions = new ObservableCollection<RegionScoreLegend>();

            LegendBrush = new LinearGradientBrush { GradientStops = colorMapper.MappingColors };
        }

        private void OnRegionSummaryChanged( object ? sender, EventArgs e ) {
            Regions.Clear();
            Regions.AddRange( mRegionSummary.Value.Where( r => r.Enabled ).Select( r => new RegionScoreLegend( r )));
        }

        public void Dispose() {
            mRegionSummary.Dispose();
        }
    }
}
