using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace Laconic.WpfCountdownCommand.Example
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public string SecondsCounterText
        {
            get { return _secondsCounterText; }
            set
            {
                if (_secondsCounterText == value) return;
                _secondsCounterText = value;
                OnPropertyChanged("SecondsCounterText");
            }
        }

        private string _resultText;
        private string _secondsCounterText;

        public string ResultText
        {
            get { return _resultText; }
            set
            {
                if (_resultText == value) return;
                _resultText = value;
                OnPropertyChanged("ResultText");
            }
        }

        public ICommand CountdownCommand { get; set; }

        public MainWindowViewModel()
        {
            ResultText = "Waiting...";

            CountdownCommand = new CountdownCommand(3, TickCallback, ExecuteAction);
        }

        private void TickCallback(int secondsCounter)
        {
            SecondsCounterText = secondsCounter > 0 ? secondsCounter.ToString(CultureInfo.InvariantCulture) : "";
        }

        private void ExecuteAction()
        {
            ResultText = "Done!";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}