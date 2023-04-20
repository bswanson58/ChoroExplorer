using ReusableBits.Platform.Interfaces;
using ReusableBits.Platform.Preferences;
using ReusableBits.Wpf.Commands;
using ReusableBits.Wpf.DialogService;
using ReusableBits.Wpf.EventAggregator;
using ReusableBits.Wpf.ViewModelSupport;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows;
using System;
using ChoroExplorer.Platform;

namespace ChoroExplorer {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class MainWindowViewModel : PropertyChangeBase, IHandle<Events.DisplayExplorerRequest> {
        private readonly IAppStartup    mAppStartup;
        private readonly IDialogService mDialogService;
        private readonly IPreferences   mPreferences;
        private readonly IBasicLog      mLog;
        private Window ?                mShell;

        public  ICommand                Configuration { get; }

        public MainWindowViewModel( IDialogService dialogService, IEventAggregator eventAggregator, 
                                    IPreferences preferences, IBasicLog log, IAppStartup appStartup ) {
            mAppStartup = appStartup;
            mDialogService = dialogService;
            mPreferences = preferences;
            mLog = log;

            Configuration = new DelegateCommand( OnConfiguration );

            eventAggregator.Subscribe( this );
        }

        public void Handle( Events.DisplayExplorerRequest eventArgs ) {
            try {
                var startInfo = new ProcessStartInfo {
                    Arguments = eventArgs.Target,
                    FileName = "explorer.exe"
                };

                Process.Start( startInfo );    
            }
            catch( Exception ex ) {
                mLog.LogException( "OnLaunchRequest:", ex );
            }
        }

        private void OnConfiguration() {
//            mDialogService.ShowDialog<ConfigurationView>();
        }

        public void ShellLoaded( Window shell ) {
            mShell = shell;

            mShell.Closing += OnShellClosing;

            mAppStartup.StartApplication();
        }

        private void OnShellClosing( object ? sender, System.ComponentModel.CancelEventArgs e ) {
            if( mShell != null ) {
                mShell.Closing -= OnShellClosing;
            }

            mAppStartup.StopApplication();
        }
        /*
        private void ActivateShell() {
            if( mShell != null ) {
                mShell.Show();
                mShell.Activate();
            }
        }
        */
    }
}
