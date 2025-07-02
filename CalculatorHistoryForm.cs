using System;
using System.Collections.Generic;
using System.Drawing;
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
                calculations = _calculations;
                InitializeComponents();      
        }
        private TextBox historyTextBox;
        private List<Calculation> calculations;


        private void InitializeComponents()
        {
            int height = calculations.Count * 30 + 55;
            if(height > 600)
                height = 600;
            Size = new Size(500, height);
            Text = "History";
            ShowIcon = false;
            if (calculations.Any())
            {
                historyTextBox = new TextBox
                {
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Both,
                    WordWrap = false,
                    Dock = DockStyle.Fill,
                    Font = new Font("Consolas", 15),
                    TabStop = false
                };
                Controls.Add(historyTextBox);
                FillHistoryTextBox();
            }
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
