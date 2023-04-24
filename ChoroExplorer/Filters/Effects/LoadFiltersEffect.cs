using System;
using ChoroExplorer.Platform;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;
using System.IO;
using System.Threading.Tasks;

namespace ChoroExplorer.Filters.Effects {
    // ReSharper disable once UnusedType.Global
    internal class LoadFiltersEffect : Effect<LoadFiltersAction> {
        private readonly ILogger<LoadFiltersEffect> mLogger;
        private readonly IEnvironment               mEnvironment;

        public LoadFiltersEffect( ILogger<LoadFiltersEffect> logger, IEnvironment environment ) {
            mLogger = logger;
            mEnvironment = environment;
        }

        public override Task HandleAsync( LoadFiltersAction action, IDispatcher dispatcher ) {
            try {
                var filterFiles = Directory.EnumerateFiles( mEnvironment.FactsDirectory(), $"*{ChoroConstants.FilterExtension}" );

                foreach( var filterFile in filterFiles ) {
                    var filterData = FilterLoader.LoadFilter( filterFile );

                    dispatcher.Dispatch( new PopulateFilterAction( filterData ));
                }
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
