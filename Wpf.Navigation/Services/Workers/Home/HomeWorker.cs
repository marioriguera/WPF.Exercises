using Microsoft.Extensions.Logging;
using Wpf.Navigation.ViewsModels;

namespace Wpf.Navigation.Services.Workers.Home
{
    /// <summary>
    /// Represents a worker that performs background tasks related to the Home view.
    /// </summary>
    public class HomeWorker : WorkerBase
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly ILogger<HomeWorker> _logger;
        private int _count = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeWorker"/> class.
        /// </summary>
        /// <param name="logger">The logger used for logging messages.</param>
        /// <param name="homeViewModel">The home view model to update with messages.</param>
        public HomeWorker(ILogger<HomeWorker> logger, HomeViewModel homeViewModel)
            : base(logger)
        {
            _logger = logger;
            _homeViewModel = homeViewModel;
        }

        /// <inheritdoc/>
        protected override TimeSpan Interval => TimeSpan.FromSeconds(3);

        /// <inheritdoc/>
        protected override TimeSpan StartDelay => TimeSpan.FromSeconds(1);

        /// <inheritdoc/>
        protected override string Name => "Home Worker";

        /// <summary>
        /// Performs the work cycle of the home worker.
        /// </summary>
        protected override Task CycleWork()
        {
            try
            {
                string message = GetMessageToSend();
                _logger.LogInformation($"The message to send is {message}.");
                _homeViewModel.UpdateMessage(message);

                UpdateCounter();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Determines the message to be sent based on the current counter value.
        /// </summary>
        /// <returns>The message to send.</returns>
        private string GetMessageToSend()
        {
            return _count switch
            {
                0 => "Hola desde el servicio home worker",
                1 => "He cambiado el mensaje",
                2 => "El mensaje se reiniciará",
                _ => "Algo mal ha ocurrido",
            };
        }

        /// <summary>
        /// Updates the counter, resetting it if it reaches the limit.
        /// </summary>
        private void UpdateCounter()
        {
            if (_count >= 2)
            {
                _count = 0;
            }
            else
            {
                _count++;
            }
        }
    }
}
