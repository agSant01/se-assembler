using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO.IODevices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Simulator_UI
{
    /// <summary>
    /// Interaction logic for SevenSegmentWindow.xaml
    /// </summary>
    public partial class SevenSegmentWindow : Window
    {
        private readonly IOManager _ioManager;
        public IOSevenSegmentDisplay SegmentDisplay { get; private set; }

        public SevenSegmentWindow(IOManager ioManager)
        {
            InitializeComponent();
            _ioManager = ioManager;
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

        private void ResetBtn_Clicked(object sender, RoutedEventArgs e)
        {
            Display.Reset();
        }

        private void Toggle_Activate(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = (ToggleButton)sender;

            // verify if a port was selected
            if (int.TryParse(tbPort.Text, out int port))
            {
                if (_ioManager.IsUsedPort((short)port))
                {
                    MessageBox.Show("Port is already in use", "Invalid Port");
                    toggle.IsChecked = false;
                    return;
                }
                // initialize IO Device
                SegmentDisplay = new IOSevenSegmentDisplay((short)port);
                SegmentDisplay.UpdateGui += UpdateDisplay;
                try
                {
                    // try to add to IO Manager
                    // exception wil be thrown if invalid port is selected
                    _ioManager.AddIODevice((short)port, SegmentDisplay);

                    // change text of toggle text
                    toggle.Content = "Active";

                    toggle.Background = Brushes.Green;

                }
                catch (Exception err)
                {
                    // error message
                    MessageBox.Show(err.Message, "Error assigning port.");
                    toggle.IsChecked = false;
                    SegmentDisplay = null;
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
            if (SegmentDisplay != null)
            {
                _ioManager?.RemoveIODevice(SegmentDisplay.IOPort);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // remove IO from IO Manager
            if (SegmentDisplay != null)
            {
                _ioManager?.RemoveIODevice(SegmentDisplay.IOPort);
            }
            base.OnClosing(e);
        }
    }
}