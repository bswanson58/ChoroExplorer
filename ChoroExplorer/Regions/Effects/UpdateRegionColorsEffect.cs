using System.Threading.Tasks;
using ChoroExplorer.Models;
using Fluxor;

namespace ChoroExplorer.Regions.Effects {
    // ReSharper disable once UnusedType.Global
    internal class UpdateRegionColorsEffect : Effect<UpdateRegionColorsAction> {
        private readonly IMapManager    mMapManager;

        public UpdateRegionColorsEffect( IMapManager mapManager ) {
            mMapManager = mapManager;
        }

        public override Task HandleAsync( UpdateRegionColorsAction action, IDispatcher dispatcher ) {
            mMapManager.UpdateRegionColors( action.RegionColors );

            return Task.CompletedTask;
        }
    }
}
