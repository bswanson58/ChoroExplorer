using System;
using ChoroExplorer.Platform;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;
using System.IO;
using System.Threading.Tasks;
using ChoroExplorer.Models;

namespace ChoroExplorer.Facts.Effects {
    // ReSharper disable once UnusedType.Global
    internal class LoadFactSetsEffect : Effect<LoadFactSetsAction> {
        private readonly ILogger<LoadFactSetsEffect>    mLogger;
        private readonly IEnvironment                   mEnvironment;

        public LoadFactSetsEffect( ILogger<LoadFactSetsEffect> logger, IEnvironment environment ) {
            mLogger = logger;
            mEnvironment = environment;
        }

        public override Task HandleAsync( LoadFactSetsAction action, IDispatcher dispatcher ) {
            try {
                var factFiles = Directory.EnumerateFiles( mEnvironment.FactsDirectory(), $"*{ChoroConstants.FactSetExtension}" );
                var haveFactSets = false;

                foreach( var factFile in factFiles ) {
                    var factSet = FactSetLoader.LoadFactSet( factFile );

                    if( factSet != null ) {
                        dispatcher.Dispatch( new PopulateFactSetAction( factSet ));

                        haveFactSets = true;
                    }
                }

                if(!haveFactSets ) {
                    dispatcher.Dispatch( new AddFactSetAction( FactSet.UnnamedSet ));
                }
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
