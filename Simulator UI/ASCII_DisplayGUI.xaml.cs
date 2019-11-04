
using Assembler.Core.Microprocessor;
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
        public ASCII_Display display;

        public ASCII_DisplayGUI(IOManager ioManager, VirtualMemory mem,short port)
        {
            _ioManager = ioManager;
            display = new ASCII_Display(mem);
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
    }
}
