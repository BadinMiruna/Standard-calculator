using System;
using System.ComponentModel;
using System.Windows.Input;

namespace CalculatorApp.ViewModel
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private string _display;
        private string _currentInput;
        private double _firstOperand;
        private string _operation;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Display
        {
            get => _display;
            set
            {
                _display = value;
                OnPropertyChanged(nameof(Display));
            }
        }

        public ICommand NumberCommand { get; }
        public ICommand OperationCommand { get; }
        public ICommand EqualsCommand { get; }
        public ICommand ClearCommand { get; }

        public CalculatorViewModel()
        {
            Display = "0";
            _currentInput = "";
            NumberCommand = new RelayCommand(param => AppendNumber(param.ToString()));
            OperationCommand = new RelayCommand(param => SetOperation(param.ToString()));
            EqualsCommand = new RelayCommand(_ => CalculateResult());
            ClearCommand = new RelayCommand(_ => Clear());
        }

        private void AppendNumber(string number)
        {
            _currentInput += number;
            Display = _currentInput;
        }

        private void SetOperation(string operation)
        {
            if (double.TryParse(_currentInput, out _firstOperand))
            {
                _operation = operation;
                _currentInput = "";
            }
        }

        private void CalculateResult()
        {
            if (double.TryParse(_currentInput, out double secondOperand))
            {
                double result = 0;
                switch (_operation)
                {
                    case "+":
                        result = _firstOperand + secondOperand;
                        break;
                    case "-":
                        result = _firstOperand - secondOperand;
                        break;
                    case "*":
                        result = _firstOperand * secondOperand;
                        break;
                    case "/":
                        result = secondOperand != 0 ? _firstOperand / secondOperand : 0;
                        break;
                }
                Display = result.ToString();
                _currentInput = "";
            }
        }

        private void Clear()
        {
            _currentInput = "";
            _firstOperand = 0;
            _operation = "";
            Display = "0";
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

   
}
