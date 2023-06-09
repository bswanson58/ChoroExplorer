﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReusableBits.Platform.Interfaces;
using ReusableBits.Platform.Preferences;
using Serilog;
using Serilog.Core;
using Serilog.Events;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace ChoroExplorer.Platform {
    internal class LoggingSink {
        public  ILogEventSink   Sink { get; }
        public  LogEventLevel   EventLevel { get; }

        public LoggingSink( ILogEventSink sink, LogEventLevel forMinimumEventLevel ) {
            Sink = sink;
            EventLevel = forMinimumEventLevel;
        }
    }

    internal class SeriLogAdapter : IBasicLog {
        private readonly IEnvironment           mEnvironment;
        private readonly List<LoggingSink>      mAdditionalSinks;
        private ILogger ?	                    mLog;

        public SeriLogAdapter( IEnvironment environment ) {
            mEnvironment = environment;

            mAdditionalSinks = new List<LoggingSink>();
        }

        public void AddLoggingSink( ILogEventSink sink, LogEventLevel forMinimumEventLevel = LogEventLevel.Information ) {
            mAdditionalSinks.Add( new LoggingSink( sink, forMinimumEventLevel ));
        }

        private void CreateLog() {
            var logFile = Path.Combine( mEnvironment.LogFileDirectory(), mEnvironment.ApplicationName() + " log - {Date}.log" );

            var configuration = new LoggerConfiguration()
                .WriteTo.File( logFile, 
                    outputTemplate:"{Timestamp:MM-dd-yy HH:mm:ss.ffff} [{ProcessId}] [{Level}] {Message}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes:8192 * 1024,	
                    retainedFileCountLimit:10 );
#if DEBUG
            configuration.WriteTo.Console();
#endif

            if( mAdditionalSinks.Any()) {
                mAdditionalSinks.ForEach( sink => configuration.WriteTo.Sink( sink.Sink, sink.EventLevel ));
            }
            
            mLog = configuration.CreateLogger();
        }

        private ILogger Log {
            get {
                if( mLog == null ) {
                    CreateLog();
                }

                return mLog!;
            }
        }
        public void LogException( string message, Exception ex ) {
            Log.Error( ex, message );
        }

        public void LogMessage( string message ) {
            Log.Information( message );
        }
    }
}
