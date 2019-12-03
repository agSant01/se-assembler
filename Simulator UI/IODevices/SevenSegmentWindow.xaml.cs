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
    /// Interaction logic for SevenSegmentWindow.xaml
    /// </summary>
    public partial class SevenSegmentWindow : Window
    {
        private readonly IOManager _ioManager;

        public IOSevenSegmentDisplay SegmentDisplay { get; private set; }

        public readonly static string DeviceID = "S8ds83";

        public SevenSegmentWindow(IOManager ioManager, ushort port)
        {
            InitializeComponent();

            _ioManager = ioManager;

            portNumber.Content = "0x" + UnitConverter.IntToHex(port, defaultWidth: 3);

            // initialize IO Device
            SegmentDisplay = new IOSevenSegmentDisplay(port);

            SegmentDisplay.UpdateGui += UpdateDisplay;

            // try to add to IO Manager
            // exception wil be thrown if invalid port is selected
            _ioManager.AddIODevice(port, SegmentDisplay);

            try
            {
                MouseDown += delegate { DragMove(); };
            }
            catch (Exception) { }
        }

        private void UpdateDisplay()
        {
            Dispatcher.Invoke(() =>
            {
                if (SegmentDisplay != null)
                {
                    Display.SetBinaryNumber(SegmentDisplay.Data);
                }
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // remove IO from IO Manager
            if (SegmentDisplay != null)
            {
                _ioManager?.RemoveIODevice(SegmentDisplay.IOPort);
                SegmentDisplay = null;
            }
            base.OnClosing(e);
        }
    }
}