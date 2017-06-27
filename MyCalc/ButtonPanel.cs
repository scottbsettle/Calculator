using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCalc
{
    class ButtonPanel : Panel
    {
       public Button[,] Button;
        ButtonPanel(int _row,int _columns)
        {
            Button = new Button[_row,_columns];
            for(int row = 0; row < _row; row++)
            {
                for(int column = 0; column < _columns; column++ )
                {
                    float width = 0; float hieght = 0;
                    
                }
            }
        }

    }
}
