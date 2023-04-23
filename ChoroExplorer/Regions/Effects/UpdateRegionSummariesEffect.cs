using System.Threading.Tasks;
using ChoroExplorer.Models;
using Fluxor;

namespace ChoroExplorer.Regions.Effects {
    // ReSharper disable once UnusedType.Global
    internal class UpdateRegionSummariesEffect : Effect<UpdateRegionScoresAction> {
        private readonly IMapManager    mMapManager;

        public UpdateRegionSummariesEffect( IMapManager mapManager ) {
            mMapManager = mapManager;
        }

        public override Task HandleAsync( UpdateRegionScoresAction action, IDispatcher dispatcher ) {
            mMapManager.UpdateRegions( action.Scores );

            return Task.CompletedTask;
        }
    }
}
