using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engineering_Calculator
{
    internal class CalculatorHistoryForm : Form
    {
        public CalculatorHistoryForm(List<Calculation> _calculations)
        {
            if (_calculations.Any())
            { 
                calculations = _calculations;
                InitializeComponents();
            }
                
        }
        private TextBox historyTextBox;
        private List<Calculation> calculations;
        private void InitializeComponents()
        {
            historyTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Dock = DockStyle.Fill

            };    
            Controls.Add(historyTextBox);
            FillHistoryTextBox();
        }
        private void FillHistoryTextBox()
        {
            string calculatedExprssion = String.Empty;
            var sb = new StringBuilder();
            foreach (Calculation calculation in calculations)
            {
                calculatedExprssion = calculation.Input + " = " + calculation.Result;
                sb.AppendLine(calculatedExprssion);
            }
            historyTextBox.Text = sb.ToString();    
        }
    }
}
