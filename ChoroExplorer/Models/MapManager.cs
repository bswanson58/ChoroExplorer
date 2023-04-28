using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Utilities;
using Mapsui;
using System.Collections.Generic;
using System.Linq;

namespace ChoroExplorer.Models {
    internal interface IMapManager {
        Map     Map { get; }

        void    SetMapCenter( int longitude, int latitude );
        void    RefreshMap();

        void    ConfigureRegionLayers( IEnumerable<RegionShape> regions );
        void    UpdateRegionColors( IReadOnlyList<RegionColor> regions );

        string  FindRegionForPosition( Point position );
    }

    internal class MapManager : IMapManager {
        public Map  Map { get; }

        public MapManager() {
            Map = new Map();
            Map.Layers.Add( OpenStreetMap.CreateTileLayer());

            SetMapCenter( -98, 39 ); // rough center of the contiguous states.
        }

        public void SetMapCenter( int longitude, int latitude ) {
            var center = SphericalMercator.FromLonLat( longitude, latitude );

            Map.Home = n => n.NavigateTo( center, Map.Resolutions[5]);
        }

        public void RefreshMap() {
            var layer = Map.Layers.FirstOrDefault();

            if( layer != null ) {
                layer.DataHasChanged();
            }
        }

        public void ConfigureRegionLayers( IEnumerable<RegionShape> regions ) {
            var layers = CreateLayers( regions );

            foreach( var layer in layers ) {
                Map.Layers.Add( layer );
            }
        }

        private IEnumerable<Layer> CreateLayers( IEnumerable<RegionShape> regions ) {
            var retValue = new List<Layer>();

            foreach( var regionShape in regions ) {
                var polygon = new Polygon( new LinearRing( regionShape.ShapePoints ));
                var polyList = new List<Polygon> { polygon };

                retValue.Add( new Layer( regionShape.Name ) {
                    Tag = polygon,
                    DataSource = new MemoryProvider( polyList ),
                    Style = new VectorStyle {
                        Fill = new Brush( new Color( 0, 0, 0, 16 )),
                        Outline = new Pen {
                            Color = Color.Orange,
                            Width = 1,
                            PenStyle = PenStyle.Solid,
                            PenStrokeCap = PenStrokeCap.Round
                        }
                    }
                });
            }

            return retValue;
        }

        public void UpdateRegionColors( IReadOnlyList<RegionColor> regions ) {
            foreach( var region in regions ) {
                var layer = Map.Layers.FindLayer( region.RegionName ).FirstOrDefault();

                if( layer?.Style is VectorStyle style ) {
                    var mapColor = new Color( region.Color.R, region.Color.G, region.Color.B, region.Color.A );

                    style.Fill = new Brush( mapColor );

                    layer.DataHasChanged();
                }
            }
        }

        public string FindRegionForPosition( Point position ) {
            foreach( var layer in Map.Layers.Where( l => l.Id > 0 )) {
                if( layer.Tag is Polygon polygon ) {
                    if( polygon.Contains( position )) {
                        return layer.Name;
                    }
                }
            }

            return string.Empty;
        }
    }
}
