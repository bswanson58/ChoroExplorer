using System;
using System.IO;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Regions.Effects {
    // ReSharper disable once UnusedType.Global
    internal class InitializeRegionsEffect : Effect<InitializeRegionsAction> {
        private readonly IEnvironment                       mEnvironment;
        private readonly ILogger<InitializeRegionsEffect>   mLogger;

        public InitializeRegionsEffect( ILogger<InitializeRegionsEffect> logger, IEnvironment environment ) {
            mLogger = logger;
            mEnvironment = environment;
        }

        public override Task HandleAsync( InitializeRegionsAction _, IDispatcher dispatcher ) {
            try {
                var regionPath = Path.Combine( mEnvironment.FactsDirectory(), "StateShapes.xml" );
                var stateShapes = ShapeLoader.LoadShapes( regionPath );

                dispatcher.Dispatch( new ConfigureRegionShapesAction( stateShapes ));
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
