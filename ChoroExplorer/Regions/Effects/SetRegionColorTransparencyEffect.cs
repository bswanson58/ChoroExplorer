using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChoroExplorer.ColorMapping;
using ChoroExplorer.Models;
using Fluxor;
using Microsoft.Extensions.Logging;
using ReusableBits.Platform.Preferences;

namespace ChoroExplorer.Regions.Effects {
    // ReSharper disable once UnusedType.Global
    internal class SetRegionColorTransparencyEffect : Effect<SetRegionColorTransparencyAction> {
        private readonly ILogger<SetRegionColorTransparencyEffect>  mLogger;
        private readonly IState<RegionState>                        mRegionState;
        private readonly IColorMapper                               mColorMapper;
        private readonly IPreferences                               mPreferences;

        public SetRegionColorTransparencyEffect( IState<RegionState> regionState, IColorMapper colorMapper,
            IPreferences preferences, ILogger<SetRegionColorTransparencyEffect> logger ) {
            mRegionState = regionState;
            mColorMapper = colorMapper;
            mPreferences = preferences;
            mLogger = logger;
        }

        public override Task HandleAsync( SetRegionColorTransparencyAction action, IDispatcher dispatcher ) {
            try {
                var regionColors = new List<RegionColor>();

                foreach( var region in mRegionState.Value.RegionScores ) {
                    regionColors.Add( 
                        new RegionColor( region.RegionName, 
                            mColorMapper.MapColor( region.RegionScore, (byte)action.ColorTransparency )));
                }

                dispatcher.Dispatch( new UpdateRegionColorsAction( regionColors ));

                var preferences = mPreferences.Load<ChoroPreferences>();

                preferences.RegionColorTransparency = action.ColorTransparency;

                mPreferences.Save( preferences );
            }
            catch( Exception ex ) {
                mLogger.LogError( ex, String.Empty );
            }

            return Task.CompletedTask;
        }
    }
}
