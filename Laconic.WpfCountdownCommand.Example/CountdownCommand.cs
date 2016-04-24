using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace Laconic.WpfCountdownCommand.Example
{
    public class CountdownCommand : ICommand
    {
        private readonly int _secondsToGo;
        private readonly Action<int> _tickCallback;
        private readonly Action _executeAction;
            
        private readonly DispatcherTimer _timer;
        private int _tickCounter;

        private bool _canExecuteFlag;
        private bool CanExecuteFlag
        {
            get { return _canExecuteFlag; }
            set
            {
                if (_canExecuteFlag == value) return;
                _canExecuteFlag = value;
                OnCanExecuteChanged();
            }
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public CountdownCommand(int secondsToGo, Action<int> tickCallback, Action executeAction)
        {
            _secondsToGo = secondsToGo;
            _tickCallback = tickCallback;
            _executeAction = executeAction;

            _canExecuteFlag = true;
            _timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 1)};
            _timer.Tick += DispatcherTimer_Tick;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            _tickCounter--;
            _tickCallback(_tickCounter);

            if (_tickCounter != 0) return;

            _timer.Stop();
            _executeAction();

            CanExecuteFlag = true;
        }

        public void Execute(object parameter)
        {
            CanExecuteFlag = false;

            _tickCounter = _secondsToGo;
            _tickCallback(_tickCounter);
            _timer.Start();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFlag;
        }

        public void Interrupt()
        {
            if (_timer.IsEnabled == false) return;

            _timer.Stop();
            _tickCallback(0);

            CanExecuteFlag = true;
        }
    }
}