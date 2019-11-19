
using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO.IODevices;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Simulator_UI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Display_GUI : Window
    {
        private readonly TextBox[] boxes;
        private bool _active;
        private readonly IOManager _ioManager;

        public ASCII_Display display; //{ get; private set; }

        public readonly static string DeviceID = "leprechaunt";

        public Display_GUI(IOManager ioManager)
        {
            InitializeComponent();

            _ioManager = ioManager;
            _active = false;
            MouseDown += delegate { DragMove(); };

            boxes = new TextBox[8];
            boxes[0] = box_a;
            boxes[1] = box_b;
            boxes[2] = box_c;
            boxes[3] = box_d;
            boxes[4] = box_e;
            boxes[5] = box_f;
            boxes[6] = box_g;
            boxes[7] = box_h;
        }


        private void UpdateAsciiDisplay()
        {
            //CurrentBinLbl.Content = $"Current Bin Value: {String.Join(' ', semaforo.BitContent)}";
            //parse boolean representacion of char bit array
            //bool[] bits = new bool[8];
            //for (int i = 0; i < semaforo.BitContent.Length; i++)
            //    bits[i] = semaforo.BitContent[i] == '1';
            //MessageBox.Show(String.Join(',', bits));

            //thread control
            if (!_active)
            {
                MessageBox.Show("Load an Object file before trying to execute instructions.");
                return;
            }

            if (!_active)
            {
                return;
            }


            new Thread(() =>
            {
                while (_active)
                {
                    Thread.Sleep(100);

                    //micro.NextInstruction();
                    Dispatcher.Invoke(() =>
                    {
                        for (int i = 0; i < display.DisplaySlots.Length; i++)
                        {
                            boxes[i].Text = display.DisplaySlots[i];
                        }
                    });
                }
            }).Start();
        }

        /// <summary>
        /// Activate device and register in IOManager
        /// </summary>
        /// <param name="sender">UI Object</param>
        /// <param name="e"></param>
        private void Toggle_Activate(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = (ToggleButton)sender;

            // verify if a port was selected
            if (short.TryParse(port_number.Text, out short port))
            {
                if (_ioManager.IsUsedPort((short)port))
                {
                    MessageBox.Show("Port is already in use", "Invalid Port");
                    toggle.IsChecked = false;
                    return;
                }
                // initialize IO Device
                display = new ASCII_Display(port);
                display.GotHexData += UpdateAsciiDisplay;
                try
                {
                    // try to add to IO Manager
                    // exception wil be thrown if invalid port is selected
                    _ioManager.AddIODevice(port, display);

                    // change text of toggle text
                    toggle.Content = "Active";

                    toggle.Background = Brushes.Green;
                    _active = true;

                }
                catch (Exception err)
                {
                    // error message
                    MessageBox.Show(err.Message, "Error assigning port.");
                    toggle.IsChecked = false;
                    display = null;
                    _active = false;
                }
            }
            else
            {
                // no port selected
                MessageBox.Show("Select a port before activating the IO Device");

                toggle.IsChecked = false;
                _active = false;
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

            _active = false;
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

    }
}
