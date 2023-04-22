using System;
using System.IO;
using System.Threading.Tasks;
using ChoroExplorer.Platform;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Facts.Effects {
    // ReSharper disable once UnusedType.Global
    internal class UpdateFactSetEffect : Effect<UpdateFactSetAction> {
        private readonly ILogger<UpdateFactSetEffect>   mLogger;
        private readonly IEnvironment                   mEnvironment;

        public UpdateFactSetEffect( ILogger<UpdateFactSetEffect> logger, IEnvironment environment ) {
            mLogger = logger;
            mEnvironment = environment;
        }

        public override Task HandleAsync( UpdateFactSetAction action, IDispatcher dispatcher ) {
            try {
                var setPath = Path.ChangeExtension( 
                    Path.Combine( mEnvironment.FactsDirectory(), action.FactSet.SetId ), ChoroConstants.FactSetExtension );

                FactSetLoader.SaveFactSet( action.FactSet, setPath );
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
