using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO.IODevices;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Simulator_UI
{

    /// <summary>
    /// Interaction logic for IOBinSemaforo.xaml
    /// </summary>
    public partial class IOBinSemaforoUI : Window
    {
        private readonly IOManager _ioManager;

        public IOBinSemaforo semaforo { get; private set; }

        public readonly static string DeviceID = "D4an23";

        private bool _active;
        private bool blinkState = true;

        private bool IsPortHex = true;

        private readonly Brush Red;
        private readonly Brush Yello;
        private readonly Brush Green;
        private readonly Brush Black;

        public IOBinSemaforoUI(IOManager ioManager)
        {
            InitializeComponent();

            //initialize colors
            Red = IR.Fill;
            Yello = IA.Fill;
            Green = IV.Fill;
            Black = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            _active = false;

            _ioManager = ioManager;
            try
            {
                MouseDown += delegate { DragMove(); };
            }
            catch (Exception) { }

            UpdateSemaforo();
        }

        private void Toggle_Activate(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = (ToggleButton)sender;

            // verify if a port was selected
            if (int.TryParse(tbPort.Text, IsPortHex ? System.Globalization.NumberStyles.HexNumber :
                System.Globalization.NumberStyles.Integer, null, out int port))
            {
                if (_ioManager.IsUsedPort((short)port))
                {
                    MessageBox.Show("Port is already in use", "Invalid Port");
                    toggle.IsChecked = false;
                    return;
                }
                // initialize IO Device
                semaforo = new IOBinSemaforo((short)port);
                //add function to delegate
                semaforo.GotBinContent += UpdateSemaforo;
                try
                {
                    // try to add to IO Manager
                    // exception wil be thrown if invalid port is selected
                    _ioManager.AddIODevice((short)port, semaforo);

                    // change text of toggle text
                    toggle.Content = "Active";
                    _active = true;
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
                    semaforo = null;
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
            ToggleButton toggle = activeToggle;

            // change text of toggle text
            toggle.Content = "Inactive";
            _active = false;
            toggle.Background = Brushes.Red;

            // remove IO from IO Manager
            if (semaforo != null)
            {
                _ioManager?.RemoveIODevice(semaforo.IOPort);
                LightsOff();
            }

            rbDec.IsEnabled = true;
            rbHex.IsEnabled = true;
            tbPort.IsEnabled = true;
        }

        private void UpdateSemaforo()
        {
            char[] bitContent;

            if (semaforo != null)
            {
                bitContent = semaforo.BitContent;
            }
            else
            {
                bitContent = new char[] { '0', '0', '0', '0', '0', '0', '0', '0' };
                LightsOff();
                Dispatcher.Invoke(() =>
                {
                    CurrentBinLbl.Content = $"Current Bin Value: {string.Join(' ', bitContent)}";
                });
                return;
            }

            bool[] bits = new bool[8];

            // if no traffic light IO is initialize set the UI LIGHTS to OFF
            for (int i = 0; i < bitContent.Length; i++)
            {
                bits[i] = bitContent[i] == '1';
            }

            if (!_active)
            {
                return;
            }


            new Thread(() =>
            {
                if (!semaforo.HasData)
                {
                    Thread.Sleep(100);
                    Dispatcher.Invoke(() =>
                    {
                        CurrentBinLbl.Content = $"Current Bin Value: {string.Join(' ', bitContent)}";
                        LightsOff();
                    });
                }

                while (_active && semaforo.HasData)
                {
                    Thread.Sleep(100);
                    try
                    {
                        Dispatcher.Invoke(() =>
                        {
                            CurrentBinLbl.Content = $"Current Bin Value: {string.Join(' ', bitContent)}";

                            if (bits[6] && bits[7])
                                BlinkLights(bits);
                            else LightsOnValue(bits);
                        });
                    }
                    catch (Exception ex) { MessageBox.Show("Thread Ended", ex.Message); }

                }
            }).Start();

        }
        private void BlinkLights(bool[] binVal)
        {
            blinkState = !blinkState;
            if (blinkState)
                LightsOnValue(binVal);
            else LightsOff();

        }
        private void LightsOnValue(bool[] binValues)
        {
            IR.Fill = binValues[0] ? Red : Black;
            IA.Fill = binValues[1] ? Yello : Black;
            IV.Fill = binValues[2] ? Green : Black;
            DR.Fill = binValues[3] ? Red : Black;
            DA.Fill = binValues[4] ? Yello : Black;
            DV.Fill = binValues[5] ? Green : Black;
        }
        private void LightsOff()
        {
            //turn off all lights
            IR.Fill = IA.Fill = IV.Fill = DR.Fill = DA.Fill = DV.Fill = Black;
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

        protected override void OnClosing(CancelEventArgs e)
        {
            // remove IO from IO Manager
            if (semaforo != null)
            {
                _ioManager?.RemoveIODevice(semaforo.IOPort);
            }
            base.OnClosing(e);
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
