using System;
using System.IO;
using System.Threading.Tasks;
using ChoroExplorer.Platform;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Filters.Effects {
    // ReSharper disable once UnusedType.Global
    internal class DeleteFilterEffect : Effect<DeleteFilterAction> {
        private readonly ILogger<DeleteFilterEffect>    mLogger;
        private readonly IEnvironment                   mEnvironment;

        public DeleteFilterEffect( ILogger<DeleteFilterEffect> logger, IEnvironment environment ) {
            mLogger = logger;
            mEnvironment = environment;
        }

        public override Task HandleAsync( DeleteFilterAction action, IDispatcher dispatcher ) {
            try {
                var filterPath = Path.ChangeExtension( 
                    Path.Combine( mEnvironment.FactsDirectory(), action.Filter.FilterId ), ChoroConstants.FilterExtension );

                File.Delete( filterPath );
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
