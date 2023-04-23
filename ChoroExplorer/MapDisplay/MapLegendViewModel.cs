using System.Windows.Media;
using ChoroExplorer.ColorMapping;

namespace ChoroExplorer.MapDisplay {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class MapLegendViewModel {

        public  LinearGradientBrush     LegendBrush { get; }

        public MapLegendViewModel( IColorMapper colorMapper ) {
            LegendBrush = new LinearGradientBrush { GradientStops = colorMapper.MappingColors };
        }
    }
}
