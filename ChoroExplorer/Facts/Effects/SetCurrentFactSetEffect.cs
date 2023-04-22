using System;
using System.Threading.Tasks;
using ChoroExplorer.Models;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Facts.Effects {
    // ReSharper disable once UnusedType.Global
    internal class SetCurrentFactSetEffect : Effect<SetCurrentFactSetAction> {
        private readonly IPreferences                       mPreferences;
        private readonly ILogger<SetCurrentFactSetEffect>   mLogger;

        public SetCurrentFactSetEffect( IPreferences preferences, ILogger<SetCurrentFactSetEffect> logger ) {
            mPreferences = preferences;
            mLogger = logger;
        }

        public override Task HandleAsync( SetCurrentFactSetAction action, IDispatcher dispatcher ) {
            try {
                var preferences = mPreferences.Load<ChoroPreferences>();

                preferences.CurrentFactSet = action.FactSetId;

                mPreferences.Save( preferences );
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
