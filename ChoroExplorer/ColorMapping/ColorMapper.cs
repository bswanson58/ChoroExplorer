using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;

namespace ChoroExplorer.ColorMapping {
    internal interface IColorMapper {
        GradientStopCollection  MappingColors { get; }

        Color   MapColor( double value );
        Color   MapColor( double value, byte alpha );
        Color   DisabledRegionColor( byte alpha );
    }

    internal class ColorMapper : IColorMapper {
        private readonly GradientStopCollection mGradientStops;

        public  GradientStopCollection MappingColors => mGradientStops;

        public ColorMapper() {
            mGradientStops = new GradientStopCollection {
                new ( Colors.Red, 0.0 ),
                new ( Colors.Yellow, 0.5 ),
                new ( Colors.Green, 1.0 )
            };
        }

        public Color MapColor( double value ) =>
            GetColorByOffset( mGradientStops, value );

        public Color MapColor( double value, byte alpha ) {
            var color = MapColor( value );

            return new Color { R = color.R, G = color.G, B = color.B, A = alpha };
        }

        public Color DisabledRegionColor( byte alpha ) =>
            new Color { R = 128, G = 128, B = 128, A = alpha };

        private static Color GetColorByOffset( GradientStopCollection collection, double offset ) {
            var stops = collection.OrderBy(x => x.Offset).ToArray();

            if( offset <= 0 ) {
                return stops[0].Color;
            }

            if( offset >= 1 ) {
                return stops[^1].Color;
            }

            GradientStop ? left = stops[0], right = null;

            foreach( GradientStop stop in stops ) {
                if( stop.Offset >= offset ) {
                    right = stop;
                    break;
                }
                left = stop;
            }

            Debug.Assert( right != null );

            offset = Math.Round(( offset - left.Offset ) / ( right.Offset - left.Offset ), 2 );
            var a = (byte)(( right.Color.A - left.Color.A ) * offset + left.Color.A );
            var r = (byte)(( right.Color.R - left.Color.R ) * offset + left.Color.R );
            var g = (byte)(( right.Color.G - left.Color.G ) * offset + left.Color.G );
            var b = (byte)(( right.Color.B - left.Color.B ) * offset + left.Color.B );

            return Color.FromArgb( a, r, g, b );
        }
    }
}
