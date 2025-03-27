using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CalculatorApp
{
    public partial class MainWindow : Window
    {
        private double memoryValue = 0;
        private double currentValue = 0;
        private double result = 0;
        private string currentOperation = "";
        private List<double> memoryStack = new List<double>();


        public MainWindow()
        {
            InitializeComponent();
            Display.Text = "0";

        }
        
       

        #region Help
        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aplicatia implementata de:\nNumele: [Badin Miruna Cristina]\nGrupa: [10LF231]", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Coppy Paste Cut
        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Display.Text))
            {
                Clipboard.SetText(Display.Text);
            }
        }

        private void CutButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Display.Text))
            {
                Clipboard.SetText(Display.Text);
                Display.Text = "";
            }
        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                string clipboardText = Clipboard.GetText();

               
                string numericText = "";
                foreach (char c in clipboardText)
                {
                    if (char.IsDigit(c))
                    {
                        numericText += c;
                    }
                }

              
                if (!string.IsNullOrEmpty(numericText))
                {
                    Display.Text += numericText;
                }
            }
            else
            {
                MessageBox.Show("Clipboard-ul este gol!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string buttonContent = button.Content.ToString();

            if (char.IsDigit(buttonContent[0]))
            {
           
                if (Display.Text == "0")
                {
                    Display.Text = buttonContent;
                }
                else
                {
                    Display.Text += buttonContent;
                }
            }
            else if (buttonContent == ".")
            {
                string currentNumber = GetCurrentNumber(Display.Text);

                if (!currentNumber.Contains("."))
                {
                    Display.Text += buttonContent;
                }
            }
            else if ("+-*/".Contains(buttonContent))
            {
                if (!string.IsNullOrEmpty(Display.Text) && !"+-*/".Contains(Display.Text[^1]))
                {
                    Display.Text += buttonContent;
                }
            }


        }

        private string GetCurrentNumber(string text)
        {

            int operatorIndex = text.LastIndexOfAny(new char[] { '+', '-', '*', '/' });
            if (operatorIndex >= 0)
            {
                return text.Substring(operatorIndex + 1);
            }
            else
            {
                return text; 
            }
        }

        #region Backspace + Clear
        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (Display.Text.Length > 1)
            {
                Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
            }
            else
            {
                Display.Text = "0";
            }
        }
        public void ClearElem_Click(object sender, RoutedEventArgs e)
        {
            if (Display.Text.Length > 1)
            {
                int lastOpIndex = Display.Text.LastIndexOfAny(new char[] { '+', '-', '*', '/' });

                if (lastOpIndex != -1)
                {
                    Display.Text = Display.Text.Substring(0, lastOpIndex + 1);
                }
                else
                {
                    Display.Text = "0";
                }
            }
            else
            {
                Display.Text = "0";
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
            currentValue = 0;
            result = 0;
            currentOperation = "";
        }

        #endregion

        #region Ecuatii
        private void PercentageButton_Click(object sender, RoutedEventArgs e)
        {
            double value = double.Parse(Display.Text);
            Display.Text = (value / 100).ToString();
        }

        private void SquareRootButton_Click(object sender, RoutedEventArgs e)
        {
            double value = double.Parse(Display.Text);
            Display.Text = Math.Sqrt(value).ToString();
        }

        private void SquareButton_Click(object sender, RoutedEventArgs e)
        {
            double value = double.Parse(Display.Text);
            Display.Text = (value * value).ToString();
        }

        private void InverseButton_Click(object sender, RoutedEventArgs e)
        {
            double value = double.Parse(Display.Text);
            Display.Text = (1 / value).ToString();
        }

        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string expression = Display.Text;

                if (string.IsNullOrWhiteSpace(expression))
                {
                    MessageBox.Show("Te rog sa introduci o operatie.");
                    return;
                }

                var result = new DataTable().Compute(expression, null);

                double resultValue = Convert.ToDouble(result);
                FormatNumberWithGrouping(resultValue);
            }
            catch (EvaluateException)
            {
                MessageBox.Show("Expresia introdusa nu este valida!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                Display.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("A aparut o eroare: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                Display.Text = "0";
            }
        }

        private void PositiveNegative_Click(object sender, RoutedEventArgs e)
        { 
           
            if (Display.Text != "0")
            {
               
                if (Display.Text.StartsWith("-"))
                {
                    Display.Text = Display.Text.Substring(1); 
                }
                else
                {
                    Display.Text = "-" + Display.Text; 
                }
            }
        }

        #endregion

        private void FormatNumberWithGrouping(double number)
        {
            string formattedNumber = number.ToString("#,0.##", CultureInfo.CurrentCulture);
            Display.Text = formattedNumber; 
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                string buttonName = "Button" + (e.Key - Key.D0).ToString();
                Button_Click(this.FindName(buttonName) as Button, null);
            }
            else if (e.Key == Key.Decimal)
            {
                Button_Click(this.FindName("ButtonDot") as Button, null);
            }
            else if (e.Key == Key.Add)
            {
                Button_Click(this.FindName("ButtonPlus") as Button, null);
            }
            else if (e.Key == Key.Subtract)
            {
                Button_Click(this.FindName("ButtonMinus") as Button, null);
            }
            else if (e.Key == Key.Multiply)
            {
                Button_Click(this.FindName("ButtonMultiply") as Button, null);
            }
            else if (e.Key == Key.Divide)
            {
                Button_Click(this.FindName("ButtonDivide") as Button, null);
            }
            else if (e.Key == Key.Enter)
            {
                EqualButton_Click(null, null);
            }
            else if (e.Key == Key.Back)
            {
                BackspaceButton_Click(null, null);
            }
            else if (e.Key == Key.Escape)
            {
                ClearButton_Click(null, null);
            }

            if (e.Key == Key.OemMinus)
            {
                PositiveNegative_Click(null, null);  
                    }

            if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    switch (e.Key)
                    {
                        case Key.C:
                            CopyButton_Click(null, null);
                            break;

                        case Key.X:
                            CutButton_Click(null, null);
                            break;

                        case Key.V:
                            PasteButton_Click(null, null);
                            break;
                    }
                }
            }
        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string operation = button.Content.ToString();
            if (currentValue != 0)
            {
                EqualButton_Click(null, null);
                currentOperation = operation;
            }
            else
            {
                currentValue = double.Parse(Display.Text);
                currentOperation = operation;
                Display.Text = "0";
            }
        }

    

        #region Memory

        private void MemoryAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double value = double.Parse(Display.Text);
                memoryStack.Add(value);
            }
            catch (FormatException)
            {
                MessageBox.Show("Valoare invalida in Display!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MemoryStackView_Click(object sender, RoutedEventArgs e)
        {
            if (memoryStack.Count == 0)
            {
                MessageBox.Show("Stiva de memorie este goală!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Window selectWindow = new Window
            {
                Title = "Selectează o valoare",
                Width = 300,
                Height = 350,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            Grid mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });


            Button closeButton = new Button
            {

                HorizontalAlignment = HorizontalAlignment.Right,


            };
            closeButton.Click += (s, args) => selectWindow.Close();
            Grid.SetRow(closeButton, 0);

            ListBox listBox = new ListBox { Margin = new Thickness(10) };
            foreach (var item in memoryStack)
            {
                listBox.Items.Add(item.ToString(CultureInfo.CurrentCulture));
            }
            Grid.SetRow(listBox, 1);

            Button selectButton = new Button
            {
                Content = "Selectează",
                Margin = new Thickness(10),
                Padding = new Thickness(5)
            };

            Button deleteButton = new Button
            {
                Content = "Șterge",
                Margin = new Thickness(10),
                Padding = new Thickness(5)
            };

            selectButton.Click += (s, args) =>
            {
                if (listBox.SelectedItem != null)
                {
                    Display.Text = listBox.SelectedItem.ToString();
                    selectWindow.Close();
                }
                else
                {
                    MessageBox.Show("Te rog să selectezi o valoare!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            deleteButton.Click += (s, args) =>
            {
                if (listBox.SelectedItem != null)
                {
                    double selectedValue = double.Parse(listBox.SelectedItem.ToString(), CultureInfo.CurrentCulture);
                    memoryStack.Remove(selectedValue);
                    listBox.Items.Remove(listBox.SelectedItem);
                }
                else
                {
                    MessageBox.Show("Te rog să selectezi o valoare pentru ștergere!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            StackPanel buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            buttonPanel.Children.Add(selectButton);
            buttonPanel.Children.Add(deleteButton);
            Grid.SetRow(buttonPanel, 2);

            mainGrid.Children.Add(closeButton);
            mainGrid.Children.Add(listBox);
            mainGrid.Children.Add(buttonPanel);

            selectWindow.Content = mainGrid;
            selectWindow.ShowDialog();
        }

        private void MemorySave_Click(object sender, RoutedEventArgs e)
        {
            memoryValue = double.Parse(Display.Text);
        }

        private void MemorySubtract_Click(object sender, RoutedEventArgs e)
        {
            if (memoryStack.Count > 0)
            {
                memoryStack.RemoveAt(memoryStack.Count - 1);
            }
            else
            {
                MessageBox.Show("Stiva de memorie este goala!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MemoryClear_Click(object sender, RoutedEventArgs e)
        {
            memoryStack.Clear();
            memoryValue = 0;
            Display.Text = "0";
        }

        private void MemoryRecall_Click(object sender, RoutedEventArgs e)
        {
            if (memoryStack.Count > 0)
            {
                string memoryValues = string.Join(", ", memoryStack);
                MessageBox.Show("Stiva de memorie: " + memoryValues, "Valori stocate", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Stiva de memorie este goala!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UseMemoryForCalculation_Click(object sender, RoutedEventArgs e)
        {
            if (memoryStack.Count > 0)
            {
                double memoryValue = memoryStack[memoryStack.Count - 1];
                result += memoryValue;
                Display.Text = result.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                MessageBox.Show("Stiva de memorie este goala!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion


    }
}
