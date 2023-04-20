using System;
using System.Threading.Tasks;
using ChoroExplorer.Models;
using Fluxor;
using Microsoft.Extensions.Logging;

namespace ChoroExplorer.Regions.Effects {
    // ReSharper disable once UnusedType.Global
    internal class ConfigureRegionShapesEffect : Effect<ConfigureRegionShapesAction> {
        private readonly IMapManager                            mMapManager;
        private readonly IState<RegionState>                    mRegionState;
        private readonly ILogger<ConfigureRegionShapesEffect>   mLogger;

        public ConfigureRegionShapesEffect( IMapManager mapManager, IState<RegionState> regionState,
                                            ILogger<ConfigureRegionShapesEffect> logger ) {
            mMapManager = mapManager;
            mRegionState = regionState;
            mLogger = logger;
        }

        public override Task HandleAsync( ConfigureRegionShapesAction action, IDispatcher dispatcher ) {
            try {
                mMapManager.ConfigureRegionLayers( action.RegionShapes );
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
