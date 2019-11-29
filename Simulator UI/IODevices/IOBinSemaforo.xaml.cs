using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO.IODevices;
using Assembler.Utils;
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

        private readonly Brush Red;
        private readonly Brush Yello;
        private readonly Brush Green;
        private readonly Brush Black;

        public IOBinSemaforoUI(IOManager ioManager, ushort port)
        {
            InitializeComponent();

            //initialize colors
            Red = IR.Fill;
            Yello = IA.Fill;
            Green = IV.Fill;
            Black = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            _active = true;

            _ioManager = ioManager;

            LightsOff();

            portNumber.Content = "0x" + UnitConverter.IntToHex(port, defaultWidth: 3);

            // initialize IO Device
            semaforo = new IOBinSemaforo(port);

            //add function to delegate
            semaforo.GotBinContent += UpdateSemaforo;

            // try to add to IO Manager
            // exception wil be thrown if invalid port is selected
            _ioManager.AddIODevice(port, semaforo);

            try
            {
                MouseDown += delegate { DragMove(); };
            }
            catch (Exception) { }
        }

        private void UpdateSemaforo()
        {
             new Thread(() =>
            {
                while (_active)
                {
                    bool[] bits = new bool[8];

                    // if no traffic light IO is initialize set the UI LIGHTS to OFF
                    for (int i = 0; i < semaforo.BitContent.Length; i++)
                    {
                        bits[i] = semaforo.BitContent[i] == '1';
                    }

                    Thread.Sleep(100);
                   
                    try
                    {
                        Dispatcher.Invoke(() =>
                        {
                            CurrentBinLbl.Content = $"Current Bin Value: {string.Join(' ', semaforo.BitContent)}";

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

        protected override void OnClosing(CancelEventArgs e)
        {
            // remove IO from IO Manager
            if (semaforo != null)
            {
                _ioManager?.RemoveIODevice(semaforo.IOPort);
            }

            _active = false;

            base.OnClosing(e);
        }
    }
}
