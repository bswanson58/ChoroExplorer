using System;
using System.IO;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Regions.Effects {
    // ReSharper disable once UnusedType.Global
    internal class LoadRegionsEffect : Effect<LoadRegionsAction> {
        private readonly IEnvironment                   mEnvironment;
        private readonly ILogger<LoadRegionsEffect>     mLogger;

        public LoadRegionsEffect( ILogger<LoadRegionsEffect> logger, IEnvironment environment ) {
            mLogger = logger;
            mEnvironment = environment;
        }

        public override Task HandleAsync( LoadRegionsAction _, IDispatcher dispatcher ) {
            try {
                var regionPath = Path.Combine( mEnvironment.FactsDirectory(), "Regions.json" );
                var regions = RegionLoader.LoadRegions( regionPath );

                dispatcher.Dispatch( new InitializeRegionsAction( regions ));
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
