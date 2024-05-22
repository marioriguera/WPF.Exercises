using Wpf.Navigation.ViewsModels;

namespace Wpf.Navigation.Services.Workers.Home
{
    public class HomeWorker : WorkerBase
    {
        private readonly HomeViewModel _homeViewModel;
        private int _count = 0;

        public HomeWorker(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        protected override TimeSpan Interval => TimeSpan.FromSeconds(3);

        protected override TimeSpan StartDelay => TimeSpan.FromSeconds(1);

        protected override string Name => "Home Worker";

        protected override async Task CycleWork()
        {
            _homeViewModel.UpdateMessage(GetMessageToSend());

            UpdateConunter();
        }

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

        private void UpdateConunter()
        {
            if (_count >= 2)
            {
                _count = 0;
                return;
            }

            _count++;
        }
    }
}
