using System;
using System.IO;
using System.Threading.Tasks;
using ChoroExplorer.Platform;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Facts.Effects {
    // ReSharper disable once UnusedType.Global
    internal class DeleteFactEffect : Effect<DeleteFactAction> {
        private readonly ILogger<DeleteFactEffect>  mLogger;
        private readonly IEnvironment               mEnvironment;

        public DeleteFactEffect( ILogger<DeleteFactEffect> logger, IEnvironment environment ) {
            mLogger = logger;
            mEnvironment = environment;
        }

        public override Task HandleAsync( DeleteFactAction action, IDispatcher dispatcher ) {
            try {
                var factPath = Path.ChangeExtension( 
                    Path.Combine( mEnvironment.FactsDirectory(), action.Fact.FactId ), ChoroConstants.FactExtension );

                File.Delete( factPath );
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
