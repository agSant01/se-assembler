
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
                        //Do updates here to the UI
                        string[] curr_data = display.ReadAllFromPort(display.IOPort);

                        for (int i = 0; i < curr_data.Length; i++)
                        {
                            boxes[i].Text = curr_data[i];
                        }
                        //{ box_a, box_b,box_c, box_d, box_e, box_f, box_g, box_h};

                    });
                }
            }).Start();
        }


        /*

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (activeButton.IsChecked == false)
            {
                MessageBox.Show("Activate IO Device before writing", "Inactive IO");
                return;
            }

           // Button button = (Button) sender;

            //string hexChar = button.Content.ToString();
            //string hexcontent = user_input.Text.ToString();//user_input.Content.ToString();
            //ADD ASCII DISPLAY HERE

            //if (hexcontent.ToCharArray().Length == 8)
            //    foreach (char c in hexcontent)
            //        display?.WriteInPort(port, c);
            else
            {
                MessageBox.Show("Sorry, the content must be of 8 chars long at maximum!", "Invalid Parameter");
                return;
            }
            //display = new ASCII_Display();
            //short port = port_number.Text
            //display?.ReadFromPort();

            //Keyboard?.KeyPress(hexChar);
        }*/

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
                _ioManager?.RemoveIODevice((short)(display.IOPort + 1));

                _ioManager?.RemoveIODevice((short)(display.IOPort + 2));

                _ioManager?.RemoveIODevice((short)(display.IOPort + 3));

                _ioManager?.RemoveIODevice((short)(display.IOPort + 4));

                _ioManager?.RemoveIODevice((short)(display.IOPort + 5));

                _ioManager?.RemoveIODevice((short)(display.IOPort + 6));

                _ioManager?.RemoveIODevice((short)(display.IOPort + 7));


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
