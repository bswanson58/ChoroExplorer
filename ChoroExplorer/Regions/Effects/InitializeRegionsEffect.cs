using System;
using System.IO;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Regions.Effects {
    // ReSharper disable once UnusedType.Global
    internal class InitializeRegionsEffect : Effect<LoadRegions> {
        private readonly IEnvironment                       mEnvironment;
        private readonly ILogger<InitializeRegionsEffect>   mLogger;

        public InitializeRegionsEffect( ILogger<InitializeRegionsEffect> logger, IEnvironment environment ) {
            mLogger = logger;
            mEnvironment = environment;
        }

        public override Task HandleAsync( LoadRegions _, IDispatcher dispatcher ) {
            try {
                var regionPath = Path.Combine( mEnvironment.FactsDirectory(), "Regions.json" );
                var regions = RegionLoader.LoadRegions( regionPath );

                dispatcher.Dispatch( new InitializeRegions( regions ));
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
