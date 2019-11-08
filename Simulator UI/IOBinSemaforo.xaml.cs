using Assembler.Core.Microprocessor;
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

    /// <summary>
    /// Interaction logic for IOBinSemaforo.xaml
    /// </summary>
    public partial class IOBinSemaforo : Window
    {
        private readonly IOManager _ioManager;
        //file:///C:/Users/oremo/Downloads/Sprint%203%20DispositivosIO.pdf
        public IOBinSemaforo(IOManager ioManager)
        {
            InitializeComponent();
            _ioManager = ioManager;
        }
    }
}
