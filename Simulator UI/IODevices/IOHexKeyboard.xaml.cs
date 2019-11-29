using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO.IODevices;
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

        private bool IsPortHex = true;

        public IOHexKeyboardUI(IOManager ioManager)
        {
            InitializeComponent();

            _ioManager = ioManager;
            try
            {
                MouseDown += delegate { DragMove(); };
            }
            catch (Exception) { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (activeToggle.IsChecked == false)
            {
                MessageBox.Show("Activate IO Device before writing in the Keyboard", "Inactive IO");
                return;
            }

            Button button = (Button)sender;

            string hexChar = button.Content.ToString();

            Keyboard?.KeyPress(hexChar);
        }

        /// <summary>
        /// Activate device and register in IOManager
        /// </summary>
        /// <param name="sender">UI Object</param>
        /// <param name="e"></param>
        private void Toggle_Activate(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = (ToggleButton) activeToggle;

            // verify if a port was selected
            if (int.TryParse(tbPort.Text, IsPortHex ? System.Globalization.NumberStyles.HexNumber : System.Globalization.NumberStyles.Integer, null, out int port))
            {
                if (_ioManager.IsUsedPort((short)port))
                {
                    MessageBox.Show("Port is already in use", "Invalid Port");
                    toggle.IsChecked = false;
                    return;
                }

                // initialize IO Device
                Keyboard = new IOHexKeyboard((short)port);

                try
                {
                    // try to add to IO Manager
                    // exception wil be thrown if invalid port is selected
                    _ioManager.AddIODevice((short)port, Keyboard);

                    // change text of toggle text
                    toggle.Content = "Active";

                    toggle.Background = Brushes.Green;

                    rbDec.IsEnabled = false;
                    rbHex.IsEnabled = false;
                    tbPort.IsEnabled = false;
                }
                catch (Exception err)
                {
                    // error message
                    MessageBox.Show(err.Message, "Error assigning port.");
                    toggle.IsChecked = false;
                    Keyboard = null;
                }
            }
            else if (tbPort.Text.Length == 0)
            {
                // no port selected
                MessageBox.Show("Select a port before activating the I/O Device.", "Invalid Port");

                toggle.IsChecked = false;
            }
            else
            {
                // no port selected
                MessageBox.Show("Tried to connect I/O device to invalid port.", "Invalid Port");

                toggle.IsChecked = false;
            }
        }

        private void Toggle_Deactivate(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = (ToggleButton)sender;

            // change text of toggle text
            toggle.Content = "Inactive";

            toggle.Background = Brushes.Red;

            // remove IO from IO Manager
            if (Keyboard != null)
            {
                _ioManager?.RemoveIODevice(Keyboard.IOPort);
            }

            rbDec.IsEnabled = true;
            rbHex.IsEnabled = true;
            tbPort.IsEnabled = true;
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton b = (RadioButton)sender;

            if (b.Content.ToString() == "Hexadecimal")
            {
                IsPortHex = true;
            }
            else if (b.Content.ToString() == "Decimal")
            {
                IsPortHex = false;
            }
            else
            {
                IsPortHex = true;

                MessageBox.Show("Invalid Port Format. Choose between Hexadecimal and Decimal. Using default.", "Invalid Format.");
            }
        }

        private void tbPort_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                activeToggle.IsChecked = true;
            }
        }
    }
}
