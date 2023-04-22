using System;
using System.IO;
using ChoroExplorer.Platform;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;
using System.Threading.Tasks;

namespace ChoroExplorer.Facts.Effects {
    // ReSharper disable once UnusedType.Global
    internal class AddFactSetEffect : Effect<AddFactSetAction> {
        private readonly ILogger<AddFactSetEffect>  mLogger;
        private readonly IEnvironment               mEnvironment;

        public AddFactSetEffect( ILogger<AddFactSetEffect> logger, IEnvironment environment ) {
            mLogger = logger;
            mEnvironment = environment;
        }

        public override Task HandleAsync( AddFactSetAction action, IDispatcher dispatcher ) {
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
