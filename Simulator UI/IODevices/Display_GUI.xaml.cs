
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Display_GUI : Window
    {
        private readonly TextBox[] boxes;

        private bool _active;
        
        private readonly IOManager _ioManager;

        public ASCII_Display display; //{ get; private set; }

        public readonly static string DeviceID = "leprechaunt";

        public Display_GUI(IOManager ioManager, ushort port)
        {
            InitializeComponent();

            _ioManager = ioManager;

            _active = true;

            // initialize IO Device
            display = new ASCII_Display(port);

            portNumber.Content = "0x" + UnitConverter.IntToHex(port, defaultWidth:3);

            display.GotHexData += UpdateAsciiDisplay;

            // try to add to IO Manager
            // exception wil be thrown if invalid port is selected
          
            _ioManager.AddIODevice(port, display);

            try
            {
                MouseDown += delegate { DragMove(); };
            }
            catch (Exception) { }

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

        protected override void OnClosing(CancelEventArgs e)
        {
            // remove IO from IO Manager
            if (display != null)
            {
                _ioManager?.RemoveIODevice(display.IOPort);
            }
            
            _active = false;

            base.OnClosing(e);
        }
    }
}
