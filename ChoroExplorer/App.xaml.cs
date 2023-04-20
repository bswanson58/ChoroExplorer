using ReusableBits.Platform.Interfaces;
using ReusableBits.Platform.Preferences;
using ReusableBits.Wpf.DialogService;
using ReusableBits.Wpf.EventAggregator;
using ReusableBits.Wpf.VersionSpinner;
using ReusableBits.Wpf.ViewModelLocator;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using ChoroExplorer.Models;
using ChoroExplorer.Platform;
using ChoroExplorer.Regions;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChoroExplorer {
    public partial class App {
        private IBasicLog ?     mLog;

        // designated startup method in App.xaml
        private async void OnStartup( object sender, StartupEventArgs e ) {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices( ( _, services ) => {
                    ConfigureServices( services );
                })
                .Build();

            await InitializeApp( builder.Services );
            StartApp( builder.Services );

            await builder.RunAsync();
        }

        private void StartApp( IServiceProvider serviceProvider ) {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            var lifetime = serviceProvider.GetService<IHostApplicationLifetime>();

            mLog?.LogMessage( "--- ChoroExplorer Launched ---" );

            if( mainWindow != null ) {
                mainWindow.Closing += ( _, _ ) => lifetime?.StopApplication();

                mainWindow.Show();
            }
        }

        private async Task InitializeApp( IServiceProvider serviceProvider ) {
            ViewModelLocationProvider.SetDefaultViewModelFactory( serviceProvider.GetService );

            mLog = serviceProvider.GetRequiredService<IBasicLog>();

            var store = serviceProvider.GetService<IStore>();

            if( store != null ) {
                await store.InitializeAsync();
            }

            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskException;
        }
    
        private static void ConfigureServices( IServiceCollection services ) {
            services.AddSingleton<IAppStartup, AppStartup>();
            services.AddSingleton<IMapManager, MapManager>();

            services.AddScoped<IRegionsFacade, RegionsFacade>();

            services.AddScoped<IBasicLog, SeriLogAdapter>();

            services.AddScoped<IApplicationConstants, ApplicationConstants>();
            services.AddScoped<IEnvironment, OperatingEnvironment>();
            services.AddScoped<IFileWriter, JsonObjectWriter>();
            services.AddScoped<IPreferences, PreferencesManager>();

            services.AddScoped<IDialogService, DialogService>();
            services.AddTransient<IDialogWindow, DialogWindow>();
            services.AddScoped<IDialogServiceContainer, DialogServiceResolver>();

            services.AddScoped<IEventAggregator, EventAggregator>();
            services.AddScoped<IVersionFormatter, VersionSpinnerViewModel>();

            services.AddFluxor( options => options.ScanAssemblies( typeof( App ).Assembly ));

            services.Scan( scan => 
                scan.FromAssembliesOf( typeof( App ), typeof( App ))
                    .AddClasses( c => c.Where( classType => classType.FullName?.EndsWith( "View" ) == true ||
                                                            classType.FullName?.EndsWith( "ViewModel" ) == true ||
                                                            classType.FullName?.EndsWith( "Window" ) == true ||
                                                            classType.FullName?.EndsWith( "Dialog" ) == true ))
                    .AsSelf()
                    .WithScopedLifetime());
        }

        private void CurrentDomainUnhandledException( object sender, UnhandledExceptionEventArgs e ) {
            if( e.ExceptionObject is Exception exception ) {
                mLog?.LogException( "Application Domain unhandled exception", exception );
            }

            Shutdown( -1 );
        }

        private void AppDispatcherUnhandledException( object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e ) {
            if( Debugger.IsAttached ) {
                Clipboard.SetText( e.Exception.ToString());
            }

            mLog?.LogException( "Application Dispatcher unhandled exception", e.Exception );

            e.Handled = true;
            Shutdown( -1 );
        }

        private void TaskSchedulerUnobservedTaskException( object ? sender, UnobservedTaskExceptionEventArgs e ) {
            mLog?.LogException( "Task Scheduler unobserved exception", e.Exception );

            e.SetObserved(); 
        }
    }
}
