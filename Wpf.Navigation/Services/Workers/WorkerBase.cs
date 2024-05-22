using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using System.Runtime.CompilerServices;

namespace Wpf.Navigation.Services.Workers
{
    /// <summary>
    /// Base class for all workers.
    /// </summary>
    public abstract class WorkerBase : BackgroundService
    {
        /// <summary>
        /// <see cref="LogCycle"/> backfield.
        /// </summary>
        private int cycle;

        /// <summary>
        /// Initializes a new instance of the <see cref="CycleWorker"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public WorkerBase(ILogger<WorkerBase> logger)
        {
            Logger = logger;
            LogThreadName = $"{Name} ";
            LogCycle = 0;
        }

        /// <summary>
        /// Gets or sets a value to show cycle on logs. Values from 1 to 10.000.
        /// </summary>
        public int LogCycle
        {
            get => cycle;
            set
            {
                cycle = value <= 10000 ? value : 1;
            }
        }

        /// <summary>
        /// Gets or sets a value to show thread name on logs.
        /// </summary>
        public string LogThreadName { get; set; }

        /// <summary>
        /// Gets the sleep interval between cycles.
        /// </summary>
        protected abstract TimeSpan Interval { get; }

        /// <summary>
        /// Gets the sleep interval before start first work cycle.
        /// </summary>
        protected abstract TimeSpan StartDelay { get; }

        /// <summary>
        /// Gets the wroker name for reference.
        /// </summary>
        protected abstract string Name { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private ILogger<WorkerBase> Logger { get; }

        /// <summary>
        /// Triggerd when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task"/> that represents the lifetime of the start operation.</returns>
        public override sealed Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Triggerd when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns>A <see cref="Task"/> that represents the lifetime of the service stop operation.</returns>
        public override sealed Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// <para>This method is called when the <see cref="IHostedService"/> starts.</para>
        /// <para>The implementation should return a task that represents the lifetime of the service.</para>
        /// </summary>
        /// <param name="stoppingToken"> Triggered when <see cref="IHostedService.StopAsync(CancellationToken)"/> is called.</param>
        /// <returns>A <see cref="Task"/> that represents the lifetime of the service.</returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(ThreadCycle, stoppingToken, TaskCreationOptions.LongRunning);
            LogTrace($"{Name} service started.");

            return Task.CompletedTask;
        }

        /// <summary>
        /// /// Triggerd only once before main clycle starts.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the worker cycle initialization operation.</returns>
        protected virtual Task CycleInitialization() => Task.CompletedTask;

        /// <summary>
        /// /// Triggerd every cycle to do service work.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the worker cycle operation.</returns>
        protected abstract Task CycleWork();

        /// <summary>
        /// /// Triggerd only once after main clycle is cancelled.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the worker shutdown operation.</returns>
        protected virtual Task CycleShutdown() => Task.CompletedTask;

        #region Logable Interface implementation

        /// <summary>
        /// Formats and writes a critical log message and generates one critical message for every inner exception.
        /// </summary>
        /// /// <param name="ex">The exception to log.</param>
        /// <param name="message">
        /// <para>Format string of the log message in message template format. Example:</para>
        /// <para>"User {User} logged in from {Address}".</para>
        /// </param>
        /// <param name="memberName">The method or property name of the caller to the method.</param>
        protected void LogCriticalInner(Exception ex, string? message = null, [CallerMemberName] string memberName = "")
        {
            using (LogContext.PushProperty("Cycle", LogCycle))
            using (LogContext.PushProperty("Threadname", LogThreadName))
            using (LogContext.PushProperty("Callername", memberName))
            {
                if (ex != null)
                {
                    Log.Fatal(ex, string.IsNullOrWhiteSpace(message) ? ex.Message : message);
                }

                Exception? inner = ex?.InnerException;
                while (inner != null)
                {
                    Log.Fatal(inner, inner.Message);
                    inner = inner.InnerException;
                }
            }
        }

        /// <summary>
        /// Formats and writes a critical log message.
        /// </summary>
        /// <param name="message">
        /// <para>Format string of the log message in message template format. Example:</para>
        /// <para>"User {User} logged in from {Address}".</para>
        /// </param>
        /// <param name="memberName">The method or property name of the caller to the method.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        protected void LogCritical(string message, [CallerMemberName] string memberName = "", params object[] args)
        {
            using (LogContext.PushProperty("Cycle", LogCycle))
            using (LogContext.PushProperty("Threadname", LogThreadName))
            using (LogContext.PushProperty("Callername", memberName))
            {
                Logger.LogCritical(message, args);
            }
        }

        /// <summary>
        /// Formats and writes a critical log message.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="message">
        /// <para>Format string of the log message in message template format. Example:</para>
        /// <para>"User {User} logged in from {Address}".</para>
        /// </param>
        /// <param name="memberName">The method or property name of the caller to the method.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        protected void LogCritical(Exception ex, string message, [CallerMemberName] string memberName = "", params object[] args)
        {
            using (LogContext.PushProperty("Cycle", LogCycle))
            using (LogContext.PushProperty("Threadname", LogThreadName))
            using (LogContext.PushProperty("Callername", memberName))
            {
                Logger.LogCritical(ex, message, args);
            }
        }

        /// <summary>
        /// Formats and writes an error log message.
        /// </summary>
        /// <param name="message">
        /// <para>Format string of the log message in message template format. Example:</para>
        /// <para>"User {User} logged in from {Address}".</para>
        /// </param>
        /// <param name="memberName">The method or property name of the caller to the method.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        protected void LogError(string message, [CallerMemberName] string memberName = "", params object[] args)
        {
            using (LogContext.PushProperty("Cycle", LogCycle))
            using (LogContext.PushProperty("Threadname", LogThreadName))
            using (LogContext.PushProperty("Callername", memberName))
            {
                Logger.LogError(message, args);
            }
        }

        /// <summary>
        /// Formats and writes a error log message.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="message">
        /// <para>Format string of the log message in message template format. Example:</para>
        /// <para>"User {User} logged in from {Address}".</para>
        /// </param>
        /// <param name="memberName">The method or property name of the caller to the method.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        protected void LogError(Exception ex, string message, [CallerMemberName] string memberName = "", params object[] args)
        {
            using (LogContext.PushProperty("Cycle", LogCycle))
            using (LogContext.PushProperty("Threadname", LogThreadName))
            using (LogContext.PushProperty("Callername", memberName))
            {
                Logger.LogError(ex, message, args);
            }
        }

        /// <summary>
        /// Formats and writes a warning log message.
        /// </summary>
        /// <param name="message">
        /// <para>Format string of the log message in message template format. Example:</para>
        /// <para>"User {User} logged in from {Address}".</para>
        /// </param>
        /// <param name="memberName">The method or property name of the caller to the method.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        protected void LogWarning(string message, [CallerMemberName] string memberName = "", params object[] args)
        {
            using (LogContext.PushProperty("Cycle", LogCycle))
            using (LogContext.PushProperty("Threadname", LogThreadName))
            using (LogContext.PushProperty("Callername", memberName))
            {
                Logger.LogWarning(message, args);
            }
        }

        /// <summary>
        /// Formats and writes a warning log message.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="message">
        /// <para>Format string of the log message in message template format. Example:</para>
        /// <para>"User {User} logged in from {Address}".</para>
        /// </param>
        /// <param name="memberName">The method or property name of the caller to the method.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        protected void LogWarning(Exception ex, string message, [CallerMemberName] string memberName = "", params object[] args)
        {
            using (LogContext.PushProperty("Cycle", LogCycle))
            using (LogContext.PushProperty("Threadname", LogThreadName))
            using (LogContext.PushProperty("Callername", memberName))
            {
                Logger.LogWarning(ex, message, args);
            }
        }

        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        /// <param name="message">
        /// <para>Format string of the log message in message template format. Example:</para>
        /// <para>"User {User} logged in from {Address}".</para>
        /// </param>
        /// <param name="memberName">The method or property name of the caller to the method.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        protected void LogInformation(string message, [CallerMemberName] string memberName = "", params object[] args)
        {
            using (LogContext.PushProperty("Cycle", LogCycle))
            using (LogContext.PushProperty("Threadname", LogThreadName))
            using (LogContext.PushProperty("Callername", memberName))
            {
                Logger.LogInformation(message, args);
            }
        }

        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="message">
        /// <para>Format string of the log message in message template format. Example:</para>
        /// <para>"User {User} logged in from {Address}".</para>
        /// </param>
        /// <param name="memberName">The method or property name of the caller to the method.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        protected void LogInformation(Exception ex, string message, [CallerMemberName] string memberName = "", params object[] args)
        {
            using (LogContext.PushProperty("Cycle", LogCycle))
            using (LogContext.PushProperty("Threadname", LogThreadName))
            using (LogContext.PushProperty("Callername", memberName))
            {
                Logger.LogInformation(ex, message, args);
            }
        }

        /// <summary>
        /// Formats and writes a debug log message.
        /// </summary>
        /// <param name="message">
        /// <para>Format string of the log message in message template format. Example:</para>
        /// <para>"User {User} logged in from {Address}".</para>
        /// </param>
        /// <param name="memberName">The method or property name of the caller to the method.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        protected void LogDebug(string message, [CallerMemberName] string memberName = "", params object[] args)
        {
            using (LogContext.PushProperty("Cycle", LogCycle))
            using (LogContext.PushProperty("Threadname", LogThreadName))
            using (LogContext.PushProperty("Callername", memberName))
            {
                Logger.LogDebug(message, args);
            }
        }

        /// <summary>
        /// Formats and writes a debug log message.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="message">
        /// <para>Format string of the log message in message template format. Example:</para>
        /// <para>"User {User} logged in from {Address}".</para>
        /// </param>
        /// <param name="memberName">The method or property name of the caller to the method.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        protected void LogDebug(Exception ex, string message, [CallerMemberName] string memberName = "", params object[] args)
        {
            using (LogContext.PushProperty("Cycle", LogCycle))
            using (LogContext.PushProperty("Threadname", LogThreadName))
            using (LogContext.PushProperty("Callername", memberName))
            {
                Logger.LogDebug(ex, message, args);
            }
        }

        /// <summary>
        /// Formats and writes a trace log message.
        /// </summary>
        /// <param name="message">
        /// <para>Format string of the log message in message template format. Example:</para>
        /// <para>"User {User} logged in from {Address}".</para>
        /// </param>
        /// <param name="memberName">The method or property name of the caller to the method.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        protected void LogTrace(string message, [CallerMemberName] string memberName = "", params object[] args)
        {
            using (LogContext.PushProperty("Cycle", LogCycle))
            using (LogContext.PushProperty("Threadname", LogThreadName))
            using (LogContext.PushProperty("Callername", memberName))
            {
                Logger.LogTrace(message, args);
            }
        }

        /// <summary>
        /// Formats and writes a trace log message.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="message">
        /// <para>Format string of the log message in message template format. Example:</para>
        /// <para>"User {User} logged in from {Address}".</para>
        /// </param>
        /// <param name="memberName">The method or property name of the caller to the method.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        protected void LogTrace(Exception ex, string message, [CallerMemberName] string memberName = "", params object[] args)
        {
            using (LogContext.PushProperty("Cycle", LogCycle))
            using (LogContext.PushProperty("Threadname", LogThreadName))
            using (LogContext.PushProperty("Callername", memberName))
            {
                Logger.LogTrace(ex, message, args);
            }
        }
        #endregion

        /// <summary>
        /// Service thread cycle.
        /// </summary>
        /// <param name="cts">Indicates that the shutdown process should no longer be graceful.</param>
        private async void ThreadCycle(object? cts)
        {
            CancellationToken token = (CancellationToken)cts!;

            token.WaitHandle.WaitOne(StartDelay);

            LogTrace($"Starts initialization.");

            await CycleInitialization();

            LogTrace($"Initialization completed.");

            // WorkerReport wr = new WorkerReport(Name, Interval);
            while (!token.IsCancellationRequested)
            {
                // To statistics worker implementation.
                // DateTime start = DateTime.UtcNow;
                // DateTime end = DateTime.UtcNow;
                try
                {
                    LogCycle++;

                    LogInformation($"Cycle start.");

                    await CycleWork();

                    LogInformation($"Cycle end.");

                    LogTrace($"Sleep {Interval}");

                    // Add statistics
                    // end = DateTime.UtcNow;
                    // wr.AddCycleStatistics(start, end);
                    token.WaitHandle.WaitOne(Interval);
                }
                catch (Exception ex)
                {
                    LogCriticalInner(ex, $"Unexpected exception inside worker cycle: {ex.Message}");

                    // Add statistics
                    // end = DateTime.UtcNow;
                    // wr.AddCycleStatistics(start, end, ex);
                }
            }

            LogTrace($"Starts shutdown.");

            await CycleShutdown();

            LogTrace($"Shutdown completed.");
        }
    }
}
