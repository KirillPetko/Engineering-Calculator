using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Engineering_Calculator
{
    public partial class CalculatorForm : Form
    {
        public CalculatorForm()
        {
            InitializeComponent();
            //enable double-buffering for faster rendering of form
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
        }
       
        //fields
        Graphics g;
        CalculatorCore core;

        //methods

        //allocating resources for form usage
        private void CalculatorForm_Load(object sender, EventArgs e)
        {
            g = CreateGraphics();
            core = new CalculatorCore(this.Width, this.Height);
        }

        //creating visual representation of bitmap on paint event
        private void CalculatorForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(core.Buffer, 0, 0);
        }
        private void CalculatorForm_KeyDown(object sender, KeyEventArgs e)
        {
            core.InvokeKeyHandler(e, g);
        }
        private void CalculatorForm_MouseClick(object sender, MouseEventArgs e)
        {
            core.InvokeClickHandler(e, g);
        }
        private void CalculatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            g.Dispose();
            core.Buffer.Dispose();
        }
    }
}
