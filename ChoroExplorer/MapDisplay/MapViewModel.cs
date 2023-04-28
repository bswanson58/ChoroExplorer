using System.Windows.Input;
using ChoroExplorer.Models;
using ChoroExplorer.Regions;
using Mapsui;
using Mapsui.Geometries;
using Mapsui.UI.Wpf;
using ReusableBits.Wpf.Commands;
using ReusableBits.Wpf.ViewModelSupport;

namespace ChoroExplorer.MapDisplay {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class MapViewModel : PropertyChangeBase {
        private readonly IMapManager    mMapManager;
        private readonly IRegionsFacade mRegionsFacade;
        private MapControl ?            mMapControl;

        public  Map                     Map => mMapManager.Map;

        public  ICommand                CenterMap { get; }

        public MapViewModel( IMapManager mapManager, IRegionsFacade regionsFacade ) {
            mMapManager = mapManager;
            mRegionsFacade = regionsFacade;

            CenterMap = new DelegateCommand( OnCenterMap );
        }

        public void SetMapControl( MapControl mapControl ) {
            mMapControl = mapControl;
        }

        public void OnMouseMove( Point position ) {
            var region = mMapManager.FindRegionForPosition( position );

            mRegionsFacade.SetFocusedRegion( region );
        }

        private void OnCenterMap() {
            if( mMapControl != null ) {
                mMapControl.Map.Home( mMapControl.Navigator );
            }
        }
    }
}
