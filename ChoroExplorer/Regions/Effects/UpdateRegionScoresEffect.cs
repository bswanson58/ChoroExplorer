using System.Collections.Generic;
using System.Threading.Tasks;
using ChoroExplorer.ColorMapping;
using ChoroExplorer.Models;
using Fluxor;

namespace ChoroExplorer.Regions.Effects {
    // ReSharper disable once UnusedType.Global
    internal class UpdateRegionScoresEffect : Effect<UpdateRegionScoresAction> {
        private readonly IColorMapper           mColorMapper;
        private readonly IRegionFilter          mRegionFilter;
        private readonly IState<RegionState>    mRegionState;

        public UpdateRegionScoresEffect( IColorMapper colorMapper, IState<RegionState> regionState, IRegionFilter regionFilter ) {
            mColorMapper = colorMapper;
            mRegionState = regionState;
            mRegionFilter = regionFilter;
        }

        public override Task HandleAsync( UpdateRegionScoresAction action, IDispatcher dispatcher ) {
            var regionColors = new List<RegionColor>();

            foreach( var region in mRegionFilter.FilterRegions( action.Scores, mRegionState.Value.RegionFilter )) {
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
