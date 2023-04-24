using System.Collections.Generic;
using System.Threading.Tasks;
using ChoroExplorer.ColorMapping;
using ChoroExplorer.Models;
using Fluxor;

namespace ChoroExplorer.Regions.Effects {
    // ReSharper disable once UnusedType.Global
    internal class UpdateRegionScoresEffect : Effect<UpdateRegionScoresAction> {
        private readonly IColorMapper           mColorMapper;
        private readonly IState<RegionState>    mRegionState;

        public UpdateRegionScoresEffect( IColorMapper colorMapper, IState<RegionState> regionState ) {
            mColorMapper = colorMapper;
            mRegionState = regionState;
        }

        public override Task HandleAsync( UpdateRegionScoresAction action, IDispatcher dispatcher ) {
            var regionColors = new List<RegionColor>();

            foreach( var region in action.Scores ) {
                regionColors.Add( 
                    new RegionColor( region.RegionName,
                        region.Enabled ?
                            mColorMapper.MapColor( region.RegionScore, (byte)mRegionState.Value.RegionColorTransparency ) :
                            mColorMapper.DisabledRegionColor((byte)mRegionState.Value.RegionColorTransparency )));
            }

            dispatcher.Dispatch( new UpdateRegionColorsAction( regionColors ));

            return Task.CompletedTask;
        }
    }
}
