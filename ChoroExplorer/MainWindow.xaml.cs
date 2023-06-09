﻿using ChoroExplorer.Resources;
using System;
using System.ComponentModel;
using System.Windows;
using ReusableBits.Wpf.Platform;

namespace ChoroExplorer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();

            Loaded += OnLoaded;
            Closing += OnWindowClosing;
        }

        private void OnWindowClosing( object ? sender, CancelEventArgs args ) {
            Settings.Default.WindowPlacement = this.GetPlacement();
            Settings.Default.Save();
        }

        protected override void OnSourceInitialized( EventArgs e ) {
            base.OnSourceInitialized( e );

            this.SetPlacement( Settings.Default.WindowPlacement );
        }

        private void OnLoaded( object sender, RoutedEventArgs args ) {
            if( DataContext is MainWindowViewModel vm ) {
                vm.ShellLoaded( this );
            }

            Loaded -= OnLoaded;
        }
    }
}
