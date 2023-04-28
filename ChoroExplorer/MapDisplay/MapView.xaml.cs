using System.Windows.Input;

namespace ChoroExplorer.MapDisplay {
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView {
        public MapView() {
            InitializeComponent();

            _mapControl.MouseMove += OnMouseMove;
            if( DataContext is MapViewModel viewModel ) {
                viewModel.SetMapControl( _mapControl );
            }
        }

        private void OnMouseMove( object sender, MouseEventArgs e ) {
            if( DataContext is MapViewModel viewModel ) {
                var screenPosition = e.GetPosition( _mapControl );
                var worldPosition = _mapControl.Viewport.ScreenToWorld( screenPosition.X, screenPosition.Y );

                viewModel.OnMouseMove( worldPosition );
            }
        }
    }
}
