using System;
using System.IO;
using System.Threading.Tasks;
using ChoroExplorer.Platform;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Facts.Effects {
    // ReSharper disable once UnusedType.Global
    internal class DeleteFactSetEffect : Effect<DeleteFactSetAction> {
        private readonly ILogger<DeleteFactSetEffect>   mLogger;
        private readonly IEnvironment                   mEnvironment;

        public DeleteFactSetEffect( ILogger<DeleteFactSetEffect> logger, IEnvironment environment ) {
            mLogger = logger;
            mEnvironment = environment;
        }

        public override Task HandleAsync( DeleteFactSetAction action, IDispatcher dispatcher ) {
            try {
                var setPath = Path.ChangeExtension( 
                    Path.Combine( mEnvironment.FactsDirectory(), action.FactSet.SetId ), ChoroConstants.FactSetExtension );

                File.Delete( setPath );
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
