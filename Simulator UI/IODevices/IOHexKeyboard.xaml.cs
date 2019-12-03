using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO.IODevices;
using Assembler.Utils;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Simulator_UI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class IOHexKeyboardUI : Window
    {
        private readonly IOManager _ioManager;

        public IOHexKeyboard Keyboard { get; private set; }

        public readonly static string DeviceID = "V8dv83";

        public IOHexKeyboardUI(IOManager ioManager, ushort port)
        {
            InitializeComponent();

            _ioManager = ioManager;

            // initialize IO Device
            Keyboard = new IOHexKeyboard(port);

            portNumber.Content = "0x" + UnitConverter.IntToHex(port, defaultWidth: 3);

            // try to add to IO Manager
            // exception wil be thrown if invalid port is selected
            _ioManager.AddIODevice(port, Keyboard);

            try
            {
                MouseDown += delegate { DragMove(); };
            }
            catch (Exception) { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            string hexChar = button.Content.ToString();

            Keyboard?.KeyPress(hexChar);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // remove IO from IO Manager
            if (Keyboard != null)
            {
                _ioManager?.RemoveIODevice(Keyboard.IOPort);
            }
            base.OnClosing(e);
        }
    }
}
