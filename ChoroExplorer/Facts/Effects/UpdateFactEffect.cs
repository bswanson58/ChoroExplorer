﻿using System;
using System.IO;
using ChoroExplorer.Platform;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;
using System.Threading.Tasks;

namespace ChoroExplorer.Facts.Effects {
    // ReSharper disable once UnusedType.Global
    internal class UpdateFactEffect : Effect<UpdateFactAction> {
        private readonly ILogger<UpdateFactEffect>  mLogger;
        private readonly IEnvironment               mEnvironment;

        public UpdateFactEffect( ILogger<UpdateFactEffect> logger, IEnvironment environment ) {
            mLogger = logger;
            mEnvironment = environment;
        }

        public override Task HandleAsync( UpdateFactAction action, IDispatcher dispatcher ) {
            try {
                var setPath = Path.ChangeExtension( 
                    Path.Combine( mEnvironment.FactsDirectory(), action.Fact.FactId ), ChoroConstants.FactExtension );

                FactLoader.SaveFact( action.Fact, setPath );
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}