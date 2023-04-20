using Mapsui.UI.Wpf;
using Mapsui;
using System.Windows;

namespace ChoroExplorer.Controls {
    public static class BindableMap {
        public static readonly DependencyProperty BoundMap =
            DependencyProperty.RegisterAttached( "BoundMap", typeof( Map ), typeof( BindableMap ), 
                new PropertyMetadata( null, OnMapChanged ));

        public static void SetBoundMap( DependencyObject dp, Map value ) {
            dp.SetValue( BoundMap, value );
        }

        public static Map ? GetBoundMap( DependencyObject dp ) {
            return dp.GetValue( BoundMap ) as Map;
        }

        private static void OnMapChanged( DependencyObject dp, DependencyPropertyChangedEventArgs e ) {
            if(( dp is MapControl mapControl ) &&
               ( e.NewValue is Map map )) {
                mapControl.Map = map;
            }
        }
    }
}
