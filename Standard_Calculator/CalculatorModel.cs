/*using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace CalculatorApp
{
    public class CalculatorModel
    {
        private double memoryValue = 0;
        private double currentValue = 0;
        private double result = 0;
        private string currentOperation = "";
        private List<double> memoryStack = new List<double>();

        public string Display { get; private set; } = "0";

        public void AddToMemory(double value)
        {
            memoryStack.Add(value);
        }

        public void SubtractFromMemory(double value)
        {
            memoryValue -= value;
        }

        public void SaveToMemory(double value)
        {
            memoryValue = value;
        }

        public string RecallMemory()
        {
            if (memoryStack.Count > 0)
            {
                return string.Join(", ", memoryStack);
            }
            else
            {
                return "Memoria este goala!";
            }
        }

        public void UseMemoryForCalculation()
        {
            if (memoryStack.Count > 0)
            {
                result += memoryStack[memoryStack.Count - 1];
                Display = FormatNumberWithGrouping(result);
            }
            else
            {
                Display = "Stiva de memorie este goala!";
            }
        }

        public void Clear()
        {
            Display = "0";
            currentValue = 0;
            result = 0;
            currentOperation = "";
        }

        public void ClearEntry()
        {
            Display = "0";
        }

        public void Backspace()
        {
            if (Display.Length > 1)
            {
                Display = Display.Substring(0, Display.Length - 1);
            }
            else
            {
                Display = "0";
            }
        }

        public void PerformOperation(string operation)
        {
            if (currentValue != 0)
            {
                EqualOperation();
                currentOperation = operation;
            }
            else
            {
                currentValue = double.Parse(Display);
                currentOperation = operation;
                Display = "0";
            }
        }

        public void EqualOperation()
        {
            try
            {
                string expression = Display;

                if (string.IsNullOrWhiteSpace(expression))
                {
                    Display = "Te rog sa introduci o operatie.";
                    return;
                }

                if (expression.Contains("/") && expression.Split('/')[1] == "0")
                {
                    Display = "Impartirea la 0 nu este permisa!";
                    return;
                }

                var result = new DataTable().Compute(expression, null);
                result = Convert.ToDouble(result);
                Display = FormatNumberWithGrouping((double)result);
            }
            catch (Exception)
            {
                Display = "Eroare!";
            }
        }

       

        private string FormatNumberWithGrouping(double number)
        {
            return number.ToString("#,0.##", CultureInfo.CurrentCulture);
        }
    }
}
*/