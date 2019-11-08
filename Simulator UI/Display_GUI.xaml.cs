
using Assembler.Core.Microprocessor.IO.IODevices;
using Assembler.Core.Microprocessor.IO;
using Assembler.Core.Microprocessor;
using Assembler.Utils;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Simulator_UI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Display_GUI: Window
    {
        private readonly IOManager _ioManager;

        public ASCII_Display display; //{ get; private set; }

        public readonly static string DeviceID = "leprechaunt";

        public Display_GUI(IOManager ioManager)
        {
            InitializeComponent();

            _ioManager = ioManager;

            MouseDown += delegate { DragMove(); };
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (activeButton.IsChecked == false)
            {
                MessageBox.Show("Activate IO Device before writing", "Inactive IO");
                return;
            }

            Button button = (Button) sender;

            //string hexChar = button.Content.ToString();
            string hexcontent = user_input.Text.ToString();//user_input.Content.ToString();
            //ADD ASCII DISPLAY HERE
            
            //display = new ASCII_Display();
            //short port = port_number.Text
            //display?.ReadFromPort();

            //Keyboard?.KeyPress(hexChar);
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
            if (int.TryParse(port_number.Text, out int port))
            {
                // initialize IO Device
                display = new ASCII_Display((short) port);

                try
                {
                    // try to add to IO Manager
                    // exception wil be thrown if invalid port is selected
                    _ioManager.AddIODevice((short)port, display);

                    // change text of toggle text
                    toggle.Content = "Active";

                    toggle.Background = Brushes.Green;

                }
                catch (Exception err)
                {
                    // error message
                    MessageBox.Show(err.Message, "Error assigning port.");
                    toggle.IsChecked = false;
                    display = null;
                }
            }
            else
            {
                // no port selected
                MessageBox.Show("Select a port before activating the IO Device");

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
            if (display != null)
            {
                _ioManager?.RemoveIODevice(display.IOPort);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // remove IO from IO Manager
            if (display != null)
            {
                _ioManager?.RemoveIODevice(display.IOPort);
            }
            base.OnClosing(e);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button send = (Button)sender;
            //if(int.TryParse(port_number.Text, out int port))
            //display?.WriteInPort( ,user_input.Text.ToString());

            // verify if a port was selected
            if (int.TryParse(port_number.Text, out int port))
            {
                // initialize IO Device
                display = new ASCII_Display((short)port);

                try
                {
                    // try to add to IO Manager
                    // exception wil be thrown if invalid port is selected
                    _ioManager.AddIODevice((short)port, display);

                    // change text of toggle text
                    //toggle.Content = "Active";

                    send.Background = Brushes.Green;

                }
                catch (Exception err)
                {
                    // error message
                    MessageBox.Show(err.Message, "Error assigning port.");
                    //toggle.IsChecked = false;
                    user_input.Text = "";
                    display = null;
                }
            }
            else
            {
                // no port selected
                MessageBox.Show("Select a port before activating the IO Device");

                //send.IsEnabled(false);
            }
        }
    }
}
