using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO.IODevices;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Simulator_UI
{

    /// <summary>
    /// Interaction logic for IOBinSemaforo.xaml
    /// </summary>
    public partial class IOBinSemaforoUI : Window
    {
        private readonly IOManager _ioManager;
        //file:///C:/Users/oremo/Downloads/Sprint%203%20DispositivosIO.pdf

        public IOBinSemaforo semaforo { get; private set; }

        public readonly static string DeviceID = "D4an23";

        private bool _active;
        private bool blinkState = true;

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
            if (int.TryParse(tbPort.Text, out int port))
            {
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

                }
                catch (Exception err)
                {
                    // error message
                    MessageBox.Show(err.Message, "Error assigning port.");
                    toggle.IsChecked = false;
                    semaforo = null;
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
            _active = false;
            toggle.Background = Brushes.Red;

            // remove IO from IO Manager
            if (semaforo != null)
            {
                _ioManager?.RemoveIODevice(semaforo.IOPort);
            }
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
                Dispatcher.Invoke(() =>
                {
                    CurrentBinLbl.Content = $"Current Bin Value: {string.Join(' ', bitContent)}";
                });

                while (_active)
                {
                    Thread.Sleep(100);

                    Dispatcher.Invoke(() =>
                    {
                        CurrentBinLbl.Content = $"Current Bin Value: {string.Join(' ', bitContent)}";

                        if (bits[6] && bits[7])
                            BlinkLights(bits);
                        else LightsOnValue(bits);
                    });
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
        protected override void OnClosing(CancelEventArgs e)
        {
            // remove IO from IO Manager
            if (semaforo != null)
            {
                _ioManager?.RemoveIODevice(semaforo.IOPort);
            }
            base.OnClosing(e);
        }


    }
}
