using ChoroExplorer.Models;
using Mapsui;

namespace ChoroExplorer.MapDisplay {
    internal class MapViewModel {
        private readonly IMapManager    mMapManager;

        public  Map                     Map => mMapManager.Map;

        public MapViewModel( IMapManager mapManager ) {
            mMapManager = mapManager;
        }
    }
}
