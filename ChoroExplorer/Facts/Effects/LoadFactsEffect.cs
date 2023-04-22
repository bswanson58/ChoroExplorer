using System;
using System.IO;
using System.Threading.Tasks;
using ChoroExplorer.Platform;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Facts.Effects {
    // ReSharper disable once UnusedType.Global
    internal class LoadFactsEffect : Effect<LoadFactsAction> {
        private readonly ILogger<LoadFactsEffect>   mLogger;
        private readonly IEnvironment               mEnvironment;

        public LoadFactsEffect( ILogger<LoadFactsEffect> logger, IEnvironment environment ) {
            mLogger = logger;
            mEnvironment = environment;
        }

        public override Task HandleAsync( LoadFactsAction action, IDispatcher dispatcher ) {
            try {
                var factFiles = Directory.EnumerateFiles( mEnvironment.FactsDirectory(), $"*{ChoroConstants.FactExtension}" );

                foreach( var factFile in factFiles ) {
                    var factData = FactLoader.LoadFact( factFile );

                    dispatcher.Dispatch( new PopulateFactAction( factData ));
                }
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
