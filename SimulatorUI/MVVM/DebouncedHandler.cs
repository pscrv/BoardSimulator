using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SimulatorUI
{
    internal class DebouncedHandler
    {
        private DispatcherTimer _timer;

        public void Handle(Task task, int interval = 100)
        {
            _timer?.Stop();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(interval);
            _timer.Tick +=
                (s, e) =>
                {
                    _timer.Stop();
                    task.Start();
                };
            _timer.Start();
        }
    }

    
}