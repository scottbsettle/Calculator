using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCalc
{
    public partial class StandardClac : Form
    {
        string string_holder = "";
        bool ResetToZero = false;
        private bool mouseIsDown = false;
        private Point firstPoint;
        Timer timer;
        
        //  bool ModePanel_OC = false;
        public StandardClac()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 1;
            timer.Tick += new EventHandler(timer_Tick);
           
        }
        #region Personal_Methods
        public void timer_Tick(object sender, EventArgs e)
        {
            if (ModePanel.Visible == false)
            {
                if (ModePanel.Location.X > -202)
                    ModePanel.Location = new Point(ModePanel.Location.X - 12, ModePanel.Location.Y);
                else
                    timer.Stop();
            }
            else
            {
                if (ModePanel.Location.X < 0)
                    ModePanel.Location = new Point(ModePanel.Location.X + 12, ModePanel.Location.Y);
                else
                    timer.Stop();
            }
        }
        public void timer_Tick2()
        {
           
            Font BufferFont = new Font(NumberText.Font.FontFamily, 30, NumberText.Font.Style);
            NumberText.Font = BufferFont;
            while (NumberText.Width >= Width)
            {
                BufferFont = new Font(NumberText.Font.FontFamily, NumberText.Font.Size - 0.5f, NumberText.Font.Style);
                NumberText.Font = BufferFont;
            }
        }
        private double String_to_Double(string _doublestring)
        {
            if (_doublestring.Length > 0 && _doublestring.Contains("."))
            {
                double Converted = 0.0;
                string Number = "", Decimal = "";
                int Buffer = 0;
                long j = 0;
                for (int loop = 0; loop < _doublestring.Length; loop++)
                {
                    if (_doublestring[loop] != '.')
                    {
                        Number += _doublestring[loop];
                    }
                    else
                    {
                        if (long.TryParse(Number, out j))
                        {

                            Converted += (double)j;
                            Buffer = loop + 1;
                            break;
                        }

                    }
                }
                for (int loop = Buffer; loop < _doublestring.Length; loop++)
                {
                    Decimal += _doublestring[loop];
                }
                Buffer = 1;
                for (int loop = 0; loop < Decimal.Length; loop++)
                    Buffer *= 10;
                if (long.TryParse(Decimal, out j))
                {
                    if (j > 0)
                        Converted += (double)j / (double)Buffer;
                    else
                        Converted -= (double)j / (double)Buffer;
                }
                return Converted;
            }
            return 0;
        }
        private string AddCommas(string _NumberText)
        {
            string _buffer = "";
            if (!_NumberText.Contains("."))
            {
                for(int loop = 0; loop < _NumberText.Length; loop++)
                {
                    _buffer += _NumberText[(_NumberText.Length - 1) - loop];
                   if(loop != 0 && loop + 1 % 3 == 0)
                    {
                        _buffer += ",";
                    } 
                }
                string _bufferreversed = "";
                for(int loop = 0; loop < _buffer.Length; loop++)
                {
                    _bufferreversed += _buffer[(_buffer.Length - 1) - loop];
                }
                _buffer = _bufferreversed;
            }
            return _buffer;
        }
        private void Operator_Function()
        {
            long j = 0;
            if (!NumberText.Text.Contains(".") && !HolderTExt.Text.Contains("."))
            {
                if (long.TryParse(NumberText.Text, out j))
                {
                    string Buffer = "";
                    long i = 0;
                    if (HolderTExt.Text.Length == 0)
                    {
                        if (long.TryParse(Buffer, out i))
                            NumberText.Text = (j).ToString();
                    }
                    else
                    {
                        for (int loop = 0; loop < HolderTExt.Text.Length; loop++)
                        {
                            if (HolderTExt.Text[loop + 1] == ' ')
                            {
                                Buffer += HolderTExt.Text[loop];
                                break;
                            }
                            Buffer += HolderTExt.Text[loop];
                        }
                        if (long.TryParse(Buffer, out i))
                        {
                            try
                            {
                                if (string_holder == "-")
                                    NumberText.Text = checked(i - j).ToString();
                                else if (string_holder == "+")
                                    NumberText.Text = checked(i + j).ToString();
                                else if (string_holder == "x")
                                    NumberText.Text = checked(i * j).ToString();
                                else if (string_holder == "÷")
                                {
                                    if (NumberText.Text != "0" && i % j == 0)
                                        NumberText.Text = checked(i / j).ToString();
                                    else if (NumberText.Text != "0" && i % j != 0)
                                    {
                                        if (!HolderTExt.Text.Contains("."))
                                            Buffer += ".0";
                                        if (!NumberText.Text.Contains("."))
                                            NumberText.Text += ".0";
                                        NumberText.Text = checked(String_to_Double(Buffer) / String_to_Double(NumberText.Text)).ToString();


                                    }
                                }
                               NumberText.Text = AddCommas(NumberText.Text);
                            }
                            catch
                            {
                                NumberText.Text = "OverFlow";
                                ResetToZero = true;
                            }
                        }
                    }
                }
            }
            else
            {
                string Buffer = "";
                if (HolderTExt.Text.Length == 0)
                {
                    Buffer = "0.0";
                    NumberText.Text = String_to_Double(NumberText.Text).ToString();
                }
                else
                {
                    for (int loop = 0; loop < HolderTExt.Text.Length; loop++)
                    {
                        if (HolderTExt.Text[loop + 1] == ' ')
                        {
                            Buffer += HolderTExt.Text[loop];
                            break;
                        }
                        Buffer += HolderTExt.Text[loop];
                    }
                    if (!HolderTExt.Text.Contains("."))
                        Buffer += ".0";
                    if (!NumberText.Text.Contains("."))
                        NumberText.Text += ".0";
                    try
                    {
                        if (string_holder == "-")
                            NumberText.Text = checked(String_to_Double(Buffer) - String_to_Double(NumberText.Text)).ToString();
                        else if (string_holder == "+")
                            NumberText.Text = checked(String_to_Double(Buffer) + String_to_Double(NumberText.Text)).ToString();
                        else if (string_holder == "x")
                            NumberText.Text = checked(String_to_Double(Buffer) * String_to_Double(NumberText.Text)).ToString();
                        else if (string_holder == "÷")
                        {
                            if (NumberText.Text != "0.0")
                                NumberText.Text = checked(String_to_Double(Buffer) / String_to_Double(NumberText.Text)).ToString();

                        }
                    }
                    catch
                    {
                        NumberText.Text = "OverFlow";
                        ResetToZero = true;
                    }

                }
            }
            timer_Tick2();
        }
        private void Operator_Action(string _symbol)
        {
            
                if (string_holder == "")
                {
                    string_holder = _symbol;
                    Operator_Function();
                }
                else
                {
                    Operator_Function();
                    string_holder = _symbol;
                }

                HolderTExt.Text = NumberText.Text + " " + _symbol;
                NumberText.Text = "0";
            
            timer_Tick2();

        }
        private void Number_Button_Pressed(string _Number)
        {
            if (NumberText.Text.Length < 16)
            {
                if (ResetToZero == true)
                {
                    ResetToZero = false;
                    NumberText.Text = "0";
                }
                if (NumberText.Text == "0")
                    NumberText.Text = _Number;
                else
                    NumberText.Text += _Number;
                timer_Tick2();
            }
           
        }
        #endregion
        #region Number_B
        private void OneB_Click(object sender, EventArgs e)
        {
            Number_Button_Pressed("1");
        }

        private void TwoB_Click(object sender, EventArgs e)
        {
            Number_Button_Pressed("2");
        }

        private void ThreeB_Click(object sender, EventArgs e)
        {
            Number_Button_Pressed("3");
        }

        private void FourB_Click(object sender, EventArgs e)
        {
            Number_Button_Pressed("4");
        }

        private void FiveB_Click(object sender, EventArgs e)
        {
            Number_Button_Pressed("5");
        }

        private void SixB_Click(object sender, EventArgs e)
        {
            Number_Button_Pressed("6");
        }

        private void SevenB_Click(object sender, EventArgs e)
        {
            Number_Button_Pressed("7");
        }

        private void EightB_Click(object sender, EventArgs e)
        {
            Number_Button_Pressed("8");
        }

        private void NineB_Click(object sender, EventArgs e)
        {
            Number_Button_Pressed("9");
        }

        private void ZeroB_Click(object sender, EventArgs e)
        {
            Number_Button_Pressed("0");
        }
        #endregion
        #region Clear_Delete_Buttons
        private void button8_Click(object sender, EventArgs e)
        {
            NumberText.Text = "0";
            HolderTExt.Text = "";
            timer_Tick2();

        }

        private void ClearB_Click(object sender, EventArgs e)
        {
            NumberText.Text = "0";
            HolderTExt.Text = "";
            timer_Tick2();

        }

        private void DeleteB_Click(object sender, EventArgs e)
        {
            if (NumberText.Text.Length > 1)
            {
                string RemoveOne = "";
                for (int loop = 0; loop < NumberText.Text.Length - 1; loop++)
                    RemoveOne += NumberText.Text[loop];
                NumberText.Text = RemoveOne;
            }
            else
                NumberText.Text = "0";

            timer_Tick2();
        }
        #endregion
        #region _Operators
        private void PlusB_Click(object sender, EventArgs e)
        {
            Operator_Action("+");
        }

        private void EqualB_Click(object sender, EventArgs e)
        {
            Operator_Function();
            //Font BufferFont = new Font(NumberText.Font.FontFamily, 30, NumberText.Font.Style);
            //NumberText.Font = BufferFont;
            //while (NumberText.Width > Width)
            //{
            //    BufferFont = new Font(NumberText.Font.FontFamily, NumberText.Font.Size - 0.5f, NumberText.Font.Style);
            //    NumberText.Font = BufferFont;
            //}
            timer_Tick2();
            HolderTExt.Text = "";
            string_holder = "";
            ResetToZero = true;
        }

        private void MinusB_Click(object sender, EventArgs e)
        {
            Operator_Action("-");
        }

        private void TimesB_Click(object sender, EventArgs e)
        {
            Operator_Action("x");
        }

        private void DevideB_Click(object sender, EventArgs e)
        {
            Operator_Action("÷");
        }

        private void Odd_EvenB_Click(object sender, EventArgs e)
        {
            if (NumberText.Text != "0")
            {
                if (NumberText.Text.Contains("-"))
                {
                    string makingpositive = "";
                    for (int loop = 1; loop < NumberText.Text.Length; loop++)
                        makingpositive += NumberText.Text[loop];
                    NumberText.Text = makingpositive;
                }
                else
                {
                    string makingpositive = "-";
                    for (int loop = 0; loop < NumberText.Text.Length; loop++)
                        makingpositive += NumberText.Text[loop];
                    NumberText.Text = makingpositive;
                }
            }
        }
        #endregion
        private void DecimalB_Click(object sender, EventArgs e)
        {
            ResetToZero = false;
            if (!NumberText.Text.Contains("."))
            {
                if (NumberText.Text != "0")
                    NumberText.Text += ".";
                else
                    NumberText.Text = "0.";
            }
        }

        /// Close Buttom
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        #region MoveForm_ExitPanel
        private void MainPanel_MouseHover(object sender, EventArgs e)
        {
            ExitPanel.Visible = true;
        }

        private void ExitPanel_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    this.Capture = false;
            //    Message msg = Message.Create(this.Handle, 0XA1, new IntPtr(2), IntPtr.Zero);
            //    this.WndProc(ref msg);
            //}
            firstPoint = e.Location;
            mouseIsDown = true;
        }

        private void ExitPanel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseIsDown = false;
        }

        private void ExitPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseIsDown)
            {
                // Get the difference between the two points
                int xDiff = firstPoint.X - e.Location.X;
                int yDiff = firstPoint.Y - e.Location.Y;

                // Set the new point
                int x = this.Location.X - xDiff;
                int y = this.Location.Y - yDiff;
                this.Location = new Point(x, y);
            }
        }
        #endregion

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ModePanel.Visible = false;
            timer.Start();
        }

        private void ModeButton_Click(object sender, EventArgs e)
        {
            ModePanel.Visible = true;
            timer.Start();
        }

        private void ModeScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            int offset = e.OldValue - e.NewValue;
            Point Loc = StandardB.Location;
            Loc.Y += offset;
            StandardB.Location = Loc;
        }

        private void SquareB_Click(object sender, EventArgs e)
        {
            long j;
            if (HolderTExt.Text != "")
                Operator_Function();
            try
            {
                if (!NumberText.Text.Contains("."))
                {

                    if (long.TryParse(NumberText.Text, out j))
                        NumberText.Text = checked(j * j).ToString();
                }
                else
                    NumberText.Text = checked(String_to_Double(NumberText.Text) * String_to_Double(NumberText.Text)).ToString();
            }
            catch
            {
                NumberText.Text = "Overflow";
              //  ResetToZero = true;
            }
            ResetToZero = true;
            timer_Tick2();
        }

        private void SquareRootB_Click(object sender, EventArgs e)
        {
            long j;
            if (HolderTExt.Text != "")
                Operator_Function();
            if (!NumberText.Text.Contains("."))
            {
                if (long.TryParse(NumberText.Text, out j))
                    NumberText.Text = Math.Sqrt(j).ToString();
            }
            else
                NumberText.Text = Math.Sqrt(String_to_Double(NumberText.Text)).ToString();
            ResetToZero = true;

            timer_Tick2();
        }

        private void Devide_X_B_Click(object sender, EventArgs e)
        {
            long j = 0;
            if (NumberText.Text != "0")
            {
                if (HolderTExt.Text != "")
                    Operator_Function();
                if (!NumberText.Text.Contains("."))
                    NumberText.Text += ".0";
                NumberText.Text = (1.0 / String_to_Double(NumberText.Text)).ToString();
            }
        }

        private void PercentB_Click(object sender, EventArgs e)
        {
            if (HolderTExt.Text != "")
            {
                int move_decimal = 1;
                double Make_int_Decimal = 0.0;
                long i;
                long j = 0;

                if (!NumberText.Text.Contains(".") && NumberText.Text != "0")
                {
                    if (long.TryParse(NumberText.Text, out i))
                    {
                       
                        string buffer = "";
                       for(int loop = 0; loop < HolderTExt.Text.Length; loop++)
                        {
                            if (HolderTExt.Text[loop] == ' ')
                                break;
                            buffer += HolderTExt.Text[loop];
                        }
                        Make_int_Decimal = i / 100.0;
                        if (!buffer.Contains(".")) 
                        if (long.TryParse(buffer, out j))
                        {
                                    NumberText.Text = (j * Make_int_Decimal).ToString();
                        }
                    }
                   
                }
                else
                {
                    
                    move_decimal = 1;
                    if (!HolderTExt.Text.Contains("."))
                        HolderTExt.Text += ".0";
                    if (!NumberText.Text.Contains("."))
                        NumberText.Text += ".0";
                    for (int loop = 0; loop < NumberText.Text.Length; loop++)
                    {
                        if (loop < NumberText.Text.Length)
                        {
                            if (NumberText.Text[loop] == '.')
                                break;
                            move_decimal *= 10;
                        }
                    }
                    Make_int_Decimal = (String_to_Double(NumberText.Text) / move_decimal);
                    NumberText.Text = (String_to_Double(NumberText.Text) * Make_int_Decimal).ToString();

                }
            }
            else
                NumberText.Text = "0";
            timer_Tick2();
        }

        private void StandardClac_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EqualB_Click(sender, e);
            }
            else if (e.KeyCode == Keys.NumPad1)
            {
                OneB_Click(sender, e);
            }
        }
    }
}

