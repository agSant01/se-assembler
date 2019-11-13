using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO.IODevices;
using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simulator_UI
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, Window> _ioDevicesWindows = new Dictionary<string, Window>();

        private MicroSimulator micro;
        private IOManager ioManager;
        private VirtualMemory vm;

        private bool stopRun;

        private string[] lines;


        public MainWindow()
        {
            InitializeComponent();

            statusLabel.Content = "Status: First enter Stack Pointer Range Before Inserting File";

            Init();

            UpdateInstructionBox();
        }

        private void Init()
        {
            //UI Elements
            stopRun = true;

            if (lines == null) return;

            //Micro simulator setup
            vm = new VirtualMemory(lines);

            // state the last port for the micro
            ioManager = new IOManager(vm.VirtualMemorySize - 1);

            micro = new MicroSimulator(vm, ioManager);

            SetIOs();

            // Set instructions print mode to ASM Text
            IMCInstruction.AsmTextPrint = true;

            try
            {
                string stackPointer = stackPointerStart.Text.Trim();

                micro.StackPointer = Convert.ToUInt16(stackPointer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid Stack Pointer. Using 0 as default.");

                stackPointerStart.Text = micro.StackPointer.ToString();
            }

        }

        private void LoadMemory()
        {
            memoryBox.Items.Clear();

            memoryBox.Items.Add($"Address\t| EvenColumn\t| OddColumn");

            if (!int.TryParse(memorySizeBox.Text, out int lines))
            {
                MessageBox.Show("Invalid number of memory blocks to show. Setting default to 50.", "Invalid Input");
                memorySizeBox.Text = "50";
                lines = 50;
            }

            for (int i = 0; i < lines; i += 2)
            {
                memoryBox.Items.Add($"{i}\t : \t{vm.GetContentsInHex(i)}\t| \t{vm.GetContentsInHex(i + 1)}");
            }
        }

        private void UpdateRegisters()
        {
            stackPointerBox.Text = micro.StackPointer.ToString();

            programCounterBox.Text = micro.ProgramCounter.ToString();

            conditionalBitBox.Text = micro.ConditionalBit ? "1" : "0";

            registersBox.Items.Clear();

            registersBox.Items.Add("R0: 00");

            for (int i = 1; i < 8; i++)
            {
                registersBox.Items.Add($"R{i}: {micro?.MicroRegisters.GetRegisterValue((byte)i)}");
            }
        }

        private void UpdateInstructionBox()
        {
            instructionsBox.Items.Clear();
            instructionsBox.Items.Add($"Previous Instruction: {GetPrettyInstruction(micro?.PreviousInstruction)}");
            instructionsBox.Items.Add($"Current Instruction:   {GetPrettyInstruction(micro?.CurrentInstruction)}");
            instructionsBox.Items.Add($"Next Instruction: {GetPrettyInstruction(micro?.PeekNextInstruction())}");
        }

        private void RunAllBtn_Click(object sender, RoutedEventArgs e)
        {
            if (micro == null)
            {
                MessageBox.Show("Load an Object file before trying to execute instructions.");
                return;
            }

            stopRun = !stopRun;

            runAllBtn.Header = stopRun ? "Run All" : "Stop";

            if (stopRun)
            {
                return;
            }

            new Thread(() =>
            {
                while (!stopRun)
                {
                    Thread.Sleep(100);

                    micro.NextInstruction();

                    Dispatcher.Invoke(() =>
                    {
                        UpdateInstructionBox();

                        LoadMemory();

                        UpdateRegisters();

                        instructionsHistoryBox.Items.Add(micro.CurrentInstruction);
                    });
                }
            }).Start();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            stopRun = true;

            runAllBtn.Header = "Run All";

            stackPointerBox.Clear();
            stackPointerStart.Text = "0";
            programCounterBox.Clear();
            conditionalBitBox.Clear();
            instructionsHistoryBox.Items.Clear();
            memoryBox.Items.Clear();

            Init();

            instructionsBox.Items.Clear();
            UpdateInstructionBox();
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text Document (.txt)|*.txt"
            };

            bool? result = ofd.ShowDialog();

            if (result == true)
            {
                try
                {
                    fileLines.ItemsSource = lines = File.ReadAllLines(ofd.FileName);
                    statusLabel.Content = "Status: File Loaded";
                }
                catch (Exception ex)
                {
                    //TODO: Create log with error
                    MessageBox.Show(ex.Message, "Unexpected error when loading object file.");
                    statusLabel.Content = "Status: File Error";
                }

                Init();
            }
        }

        private void RunNextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (micro == null)
            {
                MessageBox.Show("Load an Object file before trying to execute instructions.");
                return;

            }

            MessageBox.Show(ioManager.ToString());

            micro.NextInstruction();

            UpdateInstructionBox();

            LoadMemory();

            UpdateRegisters();

            instructionsHistoryBox.Items.Add(micro.CurrentInstruction);
        }

        private string GetPrettyInstruction(IMCInstruction instruction)
        {
            if (instruction == null)
                return string.Empty;

            string addressHex = UnitConverter.IntToHex(instruction.InstructionAddressDecimal, defaultWidth: 3);

            string contentHex = vm.GetContentsInHex(instruction.InstructionAddressDecimal) +
                vm.GetContentsInHex(instruction.InstructionAddressDecimal + 1);

            return $"{addressHex}: {contentHex}: {instruction.ToString()}";
        }

        private void Checked_IOASCIIDisplay(object sender, RoutedEventArgs e)
        {
            if (ioManager == null)
            {
                cbAsciiDisplay.IsChecked = false;

                MessageBox.Show("Load an obj file before activating an IO Device.");

                return;
            }

            if (cbAsciiDisplay.IsChecked == false)
            {
                return;
            }

            if (_ioDevicesWindows.TryGetValue(Display_GUI.DeviceID, out Window window))
            {
                window.Close();
                _ioDevicesWindows.Remove(Display_GUI.DeviceID);
            }

            Display_GUI display_GUI = new Display_GUI(ioManager);

            display_GUI.Activate();

            display_GUI.Show();

            _ioDevicesWindows.Add(Display_GUI.DeviceID, display_GUI);
        }

        private void Unchecked_IOAsciiDisplay(object sender, RoutedEventArgs e)
        {
            if (_ioDevicesWindows.TryGetValue(Display_GUI.DeviceID, out Window window))
            {
                _ioDevicesWindows.Remove(Display_GUI.DeviceID);
                window.Close();
                MessageBox.Show("Closed");
            }
        }

        private void Checked_IOHexaKeyBoard(object sender, RoutedEventArgs e)
        {
            if(!ValidIDEState((CheckBox) sender))
            {
                return;
            }

            if (_ioDevicesWindows.TryGetValue(IOHexKeyboardUI.DeviceID, out Window window))
            {
                window.Close();
                _ioDevicesWindows.Remove(IOHexKeyboardUI.DeviceID);
            }

            IOHexKeyboardUI hexKeyboardUI = new IOHexKeyboardUI(ioManager);

            hexKeyboardUI.Activate();

            hexKeyboardUI.Show();

            _ioDevicesWindows.Add(IOHexKeyboardUI.DeviceID, hexKeyboardUI);
        }

        private void Unchecked_IOHexaKeyBoard(object sender, RoutedEventArgs e)
        {
            if (_ioDevicesWindows.TryGetValue(IOHexKeyboardUI.DeviceID, out Window window))
            {
                _ioDevicesWindows.Remove(IOHexKeyboardUI.DeviceID);
                window.Close();
                MessageBox.Show("Closed");
            }
        }
        private void Checked_IO7SegmentDisplay(object sender, RoutedEventArgs e)
        {
            if (!ValidIDEState((CheckBox)sender))
            {
                return;
            }

            if (_ioDevicesWindows.TryGetValue(SevenSegmentDisplay.DeviceID, out Window window))
            {
                window.Close();
                _ioDevicesWindows.Remove(SevenSegmentDisplay.DeviceID);
            }

            SevenSegmentWindow sevenSegmentDisplay = new SevenSegmentWindow(ioManager);

            sevenSegmentDisplay.Activate();

            sevenSegmentDisplay.Show();

            _ioDevicesWindows.Add(SevenSegmentDisplay.DeviceID, sevenSegmentDisplay);
        }

        private void Unchecked_IO7SegmentDisplay(object sender, RoutedEventArgs e)
        {
            if (_ioDevicesWindows.TryGetValue(SevenSegmentDisplay.DeviceID, out Window window))
            {
                _ioDevicesWindows.Remove(SevenSegmentDisplay.DeviceID);
                window.Close();
                MessageBox.Show("Closed");
            }
        }
        private void SetIOs()
        {
            Checked_IOHexaKeyBoard(null, null);
            cbTrafficLight_Checked(null, null);
            Checked_IOASCIIDisplay(null, null);
            Checked_IO7SegmentDisplay(null, null);
        }

        ///// <summary>
        ///// Helper method for verifying state of the IDE microprocessor and IOManager
        ///// </summary>
        ///// <param name="cb">Checkbox instance</param>
        ///// <returns>True if state is valid</returns>
        //private bool ValidIDEState(CheckBox cb)
        //{
        //    if (cb == null)
        //    {
        //        return false;
        //    }

        //    if (ioManager == null)
        //    {
        //        cb.IsChecked = false;

        //        MessageBox.Show("Load an obj file before activating an IO Device.");

        //        return false;
        //    }

        //    if (cb.IsChecked == false)
        //    {
        //        return false;
        //    }

        //    return true;
        //    cbTrafficLight_Checked(null,null);
        //}
        private void cbTrafficLight_Checked(object sender, RoutedEventArgs e)
        {
            if (ioManager == null)
            {
                cbTrafficLight.IsChecked = false;

                MessageBox.Show("Load an obj file before activating an IO Device.");

                return;
            }

            if (cbTrafficLight.IsChecked == false)
            {
                return;
            }

            if (_ioDevicesWindows.TryGetValue(IOBinSemaforoUI.DeviceID, out Window window))
            {
                window.Close();
                _ioDevicesWindows.Remove(IOBinSemaforoUI.DeviceID);
            }

            IOBinSemaforoUI semaforo = new IOBinSemaforoUI(ioManager);

            semaforo.Activate();

            semaforo.Show();

            _ioDevicesWindows.Add(IOBinSemaforoUI.DeviceID, semaforo);
        }
        private void cbTrafficLight_Unhecked(object sender, RoutedEventArgs e)
        {
            if (_ioDevicesWindows.TryGetValue(IOBinSemaforoUI.DeviceID, out Window window))
            {
                _ioDevicesWindows.Remove(IOBinSemaforoUI.DeviceID);
                window.Close();
                MessageBox.Show("Closed");
            }
        }

        /// <summary>
        /// Helper method for verifying state of the IDE microprocessor and IOManager
        /// </summary>
        /// <param name="cb">Checkbox instance</param>
        /// <returns>True if state is valid</returns>
        private bool ValidIDEState(CheckBox cb)
        {
            if (cb == null)
            {
                return false;
            }

            if (ioManager == null)
            {
                cb.IsChecked = false;

                MessageBox.Show("Load an obj file before activating an IO Device.");

                return false;
            }

            if (cb.IsChecked == false)
            {
                return false;
            }

            return true;
        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (Window w in _ioDevicesWindows.Values)
                w.Close();
            base.OnClosed(e);
        }
    }
}
