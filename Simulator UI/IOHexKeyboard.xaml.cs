using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO.IODevices;
using Assembler.Utils;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

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

        public IOHexKeyboardUI(IOManager ioManager)
        {
            InitializeComponent();

            _ioManager = ioManager;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button) sender;

            string hexChar = button.Content.ToString();

            MessageBox.Show(hexChar);

            Keyboard?.KeyPress(hexChar);
        }

        /// <summary>
        /// Activate device and register in IOManager
        /// </summary>
        /// <param name="sender">UI Object</param>
        /// <param name="e"></param>
        private void Toggle_Activate(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = (ToggleButton) sender;

            // verify if a port was selected
            if (int.TryParse(tbPort.Text, out int port))
            {
                // initialize IO Device
                Keyboard = new IOHexKeyboard((short) port);

                try
                {
                    // try to add to IO Manager
                    // exception wil be thrown if invalid port is selected
                    _ioManager.AddIODevice((short)port, Keyboard);

                    // change text of toggle text
                    toggle.Content = "Deactivate";
                }
                catch (Exception err)
                {
                    // error message
                    MessageBox.Show(err.Message, "Error assigning port.");
                    toggle.IsChecked = false;
                    Keyboard = null;
                }
            }
            else
            {
                // no port selected
                MessageBox.Show("Select a port before activating the IO Device");
            }

        }

        private void Toggle_Deactivate(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = (ToggleButton)sender;

            // change text of toggle text
            toggle.Content = "Activate";

            // remove IO from IO Manager
            if (Keyboard != null)
            {
                _ioManager?.RemoveIODevice(Keyboard.IOPort);
            }
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
