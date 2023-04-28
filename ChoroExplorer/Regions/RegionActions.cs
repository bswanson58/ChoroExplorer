using System;
using System.Collections.Generic;
using ChoroExplorer.Models;

namespace ChoroExplorer.Regions {
    internal class LoadRegionsAction {
    }

    internal class InitializeRegionsAction {
        public IReadOnlyList<RegionData>    Regions { get; }

        public InitializeRegionsAction( IReadOnlyList<RegionData> regions ) {
            Regions = regions;
        }
    }

    internal class ConfigureRegionShapesAction {
        public IReadOnlyList<RegionShape>   RegionShapes { get; }

        public ConfigureRegionShapesAction( IEnumerable<RegionShape> regionShapes ) {
            RegionShapes = new List<RegionShape>( regionShapes );
        }
    }

    internal class UpdateRegionScoresAction {
        public  IReadOnlyList<RegionSummary> Scores { get; }

        public UpdateRegionScoresAction( IReadOnlyList<RegionSummary> scores ) {
            Scores = scores;
        }
    }

    internal class UpdateRegionColorsAction {
        public  IReadOnlyList<RegionColor>  RegionColors { get; }

        public UpdateRegionColorsAction( IReadOnlyList<RegionColor> regionColors ) {
            RegionColors = regionColors;
        }
    }

    internal class SetRegionColorTransparencyAction {
        public  int ColorTransparency { get; }

        public SetRegionColorTransparencyAction( int colorTransparency ) {
            ColorTransparency = Math.Min( 255, Math.Max( 0, colorTransparency ));
        }
    }

    internal class SetFocusedRegionAction {
        public  string  FocusedRegion { get; }

        public SetFocusedRegionAction( string focusedRegion ) {
            FocusedRegion = focusedRegion;
        }
    }
}
