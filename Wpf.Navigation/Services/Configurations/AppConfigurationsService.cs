using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Wpf.Navigation.Services.Configurations
{
    /// <summary>
    /// Manages service configuration values of the application.
    /// </summary>
    public sealed class AppConfigurationsService
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static readonly AppConfigurationsService _instance = new();

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="AppConfigurationsService"/> class.
        /// </summary>
        /// <remarks>
        /// Explicit static constructor to tell C# compiler not to mark type as before field initialization.
        /// </remarks>
        static AppConfigurationsService()
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="AppConfigurationsService"/> class from being created.
        /// </summary>
        private AppConfigurationsService()
        {
        }
        #endregion

        #region Core Properties

        /// <summary>
        /// Gets current service configuration.
        /// </summary>
        public static AppConfigurationsService Current => _instance;

        /// <summary>
        /// Gets a value indicating whether application is in design mode or not.
        /// </summary>
        public static bool IsInDesignMode { get => System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime; }

        #endregion

        #region LogProperties

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        public LogEventLevel LogLevel
        {
            get => LevelSwitch.MinimumLevel;
            set => LevelSwitch.MinimumLevel = value;
        }

        /// <summary>
        /// Gets the current Serilog LogLevel switcher.
        /// </summary>
        public LoggingLevelSwitch LevelSwitch { get; } = new LoggingLevelSwitch(LogEventLevel.Verbose);

        /// <summary>
        /// Gets or sets the log path to store log as file.
        /// </summary>
        public string LogPath { get; set; } = @"Logs/WpfNavigation.log";

        /// <summary>
        /// Gets or sets the log max file size before rolling.
        /// </summary>
        public long LogMaxFileSize { get; set; } = 1048576000;

        /// <summary>
        /// Gets or sets the log max file size before rolling.
        /// </summary>
        public int LogRetainedFiles { get; set; } = 7;
        #endregion

        #region ILogable Properties

        /// <summary>
        /// Gets a value to show cycle on logs.
        /// </summary>
        public int LogCycle { get; } = 0;

        /// <summary>
        /// Gets a value to show thread name on logs.
        /// </summary>
        public string LogThreadName { get; } = nameof(AppConfigurationsService);

        #endregion

        #region Functions

        /// <summary>
        /// Update the serilog logger.
        /// </summary>
        public static void UpdateLogger()
        {
            if (!string.IsNullOrWhiteSpace(Current.LogPath))
            {
                Log.Logger = new LoggerConfiguration().MinimumLevel.ControlledBy(Current.LevelSwitch)
                                                          .Enrich.FromLogContext()
                                                          .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.ffff} {Level:u3} {Cycle} {Threadname}] {Message}{NewLine}{Exception}")
                                                          .WriteTo.FileEx(
                                                                          outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.ffff} {Level:u3} {Cycle} {Threadname}] {Message}{NewLine}{Exception}",
                                                                          preserveLogFileName: true,
                                                                          path: Current.LogPath,
                                                                          fileSizeLimitBytes: Current.LogMaxFileSize,
                                                                          rollingInterval: Serilog.Sinks.FileEx.RollingInterval.Day,
                                                                          rollOnEachProcessRun: true,
                                                                          rollOnFileSizeLimit: true,
                                                                          useLastWriteAsTimestamp: true,
                                                                          retainedFileCountLimit: Current.LogRetainedFiles,
                                                                          shared: true,
                                                                          buffered: false)
                                                          .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration().MinimumLevel.ControlledBy(Current.LevelSwitch)
                                      .Enrich.FromLogContext()
                                      .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.ffff} {Level:u3} {Cycle} {Threadname}] {Message}{NewLine}{Exception}")
                                      .CreateLogger();
            }

            Log.Information($"Logger configuration updated");
        }
        #endregion
    }
}
