
using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO.IODevices;
using Assembler.Microprocessor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Simulator_UI
{

    
   // Button a, b, c, d, e, f, g, h;
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ASCII_DisplayGUI : Window
    {
        private readonly IOManager _ioManager;
        public Assembler.Core.Microprocessor.IO.IODevices.ASCII_Display display;

        public ASCII_DisplayGUI(IOManager ioManager, VirtualMemory mem,short port)
        {
            _ioManager = ioManager;
            //display = new ASCII_Display(mem);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
           // throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Does nothing, all we wanna do is let them listen for the changes to the byte array
        }

        private void Update_ASCII_Display(ASCII_Display display)
        {
            /*TextBox[] ascii_display = { a, b, c, d, e, f, g, h };
            int[] actives = display.ActiveCharactersIndexes();
            int[] inactives = display.InactiveCharactersIndexes();

            foreach (int i in actives)
            {
                ascii_display[i].Background = Brushes.White;
            }
            foreach (int i in inactives)
            {
                ascii_display[i].Background = Brushes.Black;
            }
            */
        }

        private void ResetASCII_Display()
        {
            /*TextBox[] ascii_display = { a, b, c, d, e, f, g, h };

            foreach (TextBox i in ascii_display)
            {
                i.Background = Brushes.White;
            }*/

        }
    }
}
