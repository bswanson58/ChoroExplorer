using System;
using ChoroExplorer.Regions;
using Fluxor;
using ReusableBits.Wpf.ViewModelSupport;

namespace ChoroExplorer.MapDisplay {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class MapControlViewModel : PropertyChangeBase {
        private readonly IState<RegionState>    mRegionState;
        private readonly IRegionsFacade         mRegionsFacade;

        public MapControlViewModel( IState<RegionState> regionState, IRegionsFacade regionsFacade ) {
            mRegionState = regionState;
            mRegionsFacade = regionsFacade;

            mRegionState.StateChanged += OnRegionStateChanged;
        }

        private void OnRegionStateChanged( object ? sender, EventArgs e ) {
            RaisePropertyChanged( () => RegionColorTransparency );
        }

        public int RegionColorTransparency {
            get => mRegionState.Value.RegionColorTransparency;
            set => mRegionsFacade.SetRegionColorTransparency( value );
        }
    }
}
