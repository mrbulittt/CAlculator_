using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CAlculator_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _firstNumber = 0;
        private double _secondNumber = 0;
        private string _operation = "";
        private bool _isNewNumber = true;
        private bool _isOperationPerformed = false;
        private bool _hasError = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnNumberClick(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                ClearError();
                DisplayText.Text = "0";
            }

            var button = (Button)sender;
            string number = button.Content.ToString();

            if (DisplayText.Text == "0" || _isNewNumber)
            {
                DisplayText.Text = number;
                _isNewNumber = false;
            }
            else
            {
                DisplayText.Text += number;
            }

            _isOperationPerformed = false;
        }

        private void OnDecimalClick(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                ClearError();
                DisplayText.Text = "0";
            }

            if (_isNewNumber)
            {
                DisplayText.Text = "0,";
                _isNewNumber = false;
            }
            else if (!DisplayText.Text.Contains(","))
            {
                DisplayText.Text += ",";
            }

            _isOperationPerformed = false;
        }

        private void OnOperatorClick(object sender, RoutedEventArgs e)
        {
            if (_hasError) return;

            var button = (Button)sender;

            if (!_isOperationPerformed)
            {
                if (!string.IsNullOrEmpty(_operation))
                {
                    if (!double.TryParse(DisplayText.Text, out _secondNumber))
                    {
                        ShowError("Некорректное число");
                        return;
                    }

                    Calculate();
                    _firstNumber = double.Parse(DisplayText.Text);
                }
                else
                {
                    if (!double.TryParse(DisplayText.Text, out _firstNumber))
                    {
                        ShowError("Некорректное число");
                        return;
                    }
                }
            }

            _operation = button.Content.ToString();
            HistoryText.Text = $"{_firstNumber} {_operation}";
            _isNewNumber = true;
            _isOperationPerformed = true;
        }

        private void OnEqualsClick(object sender, RoutedEventArgs e)
        {
            if (_hasError) return;

            if (!string.IsNullOrEmpty(_operation)
                && !_isOperationPerformed
                && !_isNewNumber)
            {
                if (!double.TryParse(DisplayText.Text, out _secondNumber))
                {
                    ShowError("Некорректное число");
                    return;
                }

                HistoryText.Text = $"{_firstNumber} {_operation} {_secondNumber} =";
                Calculate();
                _operation = "";
                _isNewNumber = true;
            }
        }

        private void Calculate()
        {
            double result = 0;

            try
            {
                switch (_operation)
                {
                    case "+":
                        result = _firstNumber + _secondNumber;
                        break;
                    case "-":
                        result = _firstNumber - _secondNumber;
                        break;
                    case "×":
                        result = _firstNumber * _secondNumber;
                        break;
                    case "÷":
                        if (_secondNumber == 0)
                        {
                            ShowError("Деление на ноль");
                            return;
                        }
                        result = _firstNumber / _secondNumber;
                        break;
                }

                DisplayText.Text = result.ToString();
                _firstNumber = result;
                _isOperationPerformed = true;
            }
            catch (Exception ex)
            {
                ShowError("Ошибка вычисления");
            }
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            ClearError();
            DisplayText.Text = "0";
            HistoryText.Text = "";
            _firstNumber = 0;
            _secondNumber = 0;
            _operation = "";
            _isNewNumber = true;
            _isOperationPerformed = false;
        }

        private void OnBackspaceClick(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                ClearError();
                DisplayText.Text = "0";
                return;
            }

            if (!_isNewNumber && DisplayText.Text.Length > 0)
            {
                if (DisplayText.Text.Length == 1 ||
                    (DisplayText.Text.Length == 2 && DisplayText.Text.StartsWith("-")))
                {
                    DisplayText.Text = "0";
                    _isNewNumber = true;
                }
                else
                {
                    DisplayText.Text = DisplayText.Text.Substring(0, DisplayText.Text.Length - 1);
                }
            }
        }

        private void OnPlusMinusClick(object sender, RoutedEventArgs e)
        {
            if (_hasError)
            {
                ClearError();
                DisplayText.Text = "0";
                return;
            }

            if (DisplayText.Text != "0")
            {
                if (DisplayText.Text.StartsWith("-"))
                    DisplayText.Text = DisplayText.Text.Substring(1);
                else
                    DisplayText.Text = "-" + DisplayText.Text;
            }
        }

        private void ShowError(string message)
        {
            DisplayText.Style = (Style)FindResource("ErrorTextStyle");
            DisplayText.Text = message;
            _hasError = true;
        }

        private void ClearError()
        {
            DisplayText.Style = (Style)FindResource(typeof(TextBlock));
            _hasError = false;
        }
    }
}
