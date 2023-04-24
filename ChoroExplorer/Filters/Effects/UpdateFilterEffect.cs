using System;
using System.IO;
using ChoroExplorer.Platform;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;
using System.Threading.Tasks;

namespace ChoroExplorer.Filters.Effects {
    // ReSharper disable once UnusedType.Global
    internal class UpdateFilterEffect : Effect<UpdateFilterAction> {
        private readonly ILogger<UpdateFilterEffect>    mLogger;
        private readonly IEnvironment                   mEnvironment;

        public UpdateFilterEffect( ILogger<UpdateFilterEffect> logger, IEnvironment environment ) {
            mLogger = logger;
            mEnvironment = environment;
        }

        public override Task HandleAsync( UpdateFilterAction action, IDispatcher dispatcher ) {
            try {
                var setPath = Path.ChangeExtension( 
                    Path.Combine( mEnvironment.FactsDirectory(), action.Filter.FilterId ), ChoroConstants.FilterExtension );

                FilterLoader.SaveFilter( action.Filter, setPath );
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
