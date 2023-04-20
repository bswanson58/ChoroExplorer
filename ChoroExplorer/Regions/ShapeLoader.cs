using ChoroExplorer.Models;
using Mapsui.Projection;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Mapsui.Geometries;

namespace ChoroExplorer.Regions {
    internal static class ShapeLoader {
        public static IList<RegionShape> LoadShapes( string filePath ) {
            var xml = XDocument.Load( filePath );

            if( xml.Root != null ) {
                var stateQuery = 
                    from state in xml.Root.Descendants( "state" )
                    select new RegionShape( state.Attribute( "name" ).Value,
                        from point in state.Descendants( "point" )
                        select ParsePoint( point )
                    );

                return stateQuery.ToList();
            }

            return Enumerable.Empty<RegionShape>().ToList();
        }

        private static Point ParsePoint( XElement pointElement ) {
            var latValue = pointElement.Attribute( "lat" );
            var lonValue = pointElement.Attribute( "lng" );

            var lat = Double.Parse( latValue != null ? latValue.Value : String.Empty );
            var lon = Double.Parse( lonValue != null ? lonValue.Value : String.Empty );

            return SphericalMercator.FromLonLat( lon, lat );
        }
    }
}
