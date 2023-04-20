using System.Collections.Generic;
using System.Diagnostics;
using Mapsui.Geometries;

namespace ChoroExplorer.Models {
    [DebuggerDisplay("Region:{Name}")]
    internal class RegionShape {
        public  string          Name;
        public  List<Point>     ShapePoints { get; }

        public RegionShape( string stateName, IEnumerable<Point> shapePoints ) {
            Name = stateName;
            ShapePoints = new List<Point>( shapePoints );
        }
    }
}
