using Assembler.Assembler;
using Assembler.Core;
using Assembler.Core.Microprocessor;
using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Parsing;
using Assembler.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Simulator_UI
{
    public partial class MainWindow : Window 
    {
        private readonly Dictionary<string, Window> _ioDevicesWindows = new Dictionary<string, Window>();
        
        private MicroSimulator micro;
        private IOManager ioManager;
        private VirtualMemory vm;

        //assembler
        public Compiler compiler;

        private bool stopRun;

        private string[] lines;

        private string currFileName;
        
        public MainWindow()
        {
            InitializeComponent();

            statusLabel.Content = "Status: First enter Stack Pointer Range Before Inserting File";

            Init();
        }

        private void Init()
        {
            //UI Elements
            stopRun = true;

            instructionsHistoryBox.Items.Clear();
            memoryBox.Items.Clear();

            LoadMemory();

            UpdateRegisters();

            UpdateInstructionBox();

            SetIOs();

            // Set instructions print mode to ASM Text
            IMCInstruction.AsmTextPrint = true;
        }

        private void LoadMemory()
        {
            memoryBox.Items.Clear();

            if (!int.TryParse(memorySizeBox.Text, out int lines))
            {
                MessageBox.Show("Invalid number of memory blocks to show. Setting default to 50.", "Invalid Input");
                memorySizeBox.Text = "50";
                lines = 50;
            }

            for (int i = 0; i < lines; i += 2)
            {
                memoryBox.Items.Add($"{UnitConverter.IntToHex(i, defaultWidth: 3)}\t: {vm?.GetContentsInHex(i) ?? "NA"} {vm?.GetContentsInHex(i + 1) ?? "NA"}");
            }
        }

        private void UpdateRegisters()
        {
            bool IsMicroNull = micro == null;

            stackPointerBox.IsEnabled = !IsMicroNull;

            programCounterBox.IsEnabled = !IsMicroNull;

            conditionalBitBox.IsEnabled = !IsMicroNull;

            if (IsMicroNull)
            {
                stackPointerStart.Text = "0";
            }

            stackPointerBox.Text = micro?.StackPointer.ToString() ?? "NA"; 

            programCounterBox.Text = micro?.ProgramCounter.ToString() ?? "NA";

            conditionalBitBox.Text = (micro?.ConditionalBit ?? false) ? "1" : "0";

            registersBox.Items.Clear();

            registersBox.Items.Add($"R0: {(IsMicroNull ? "NA" : "00")}");
            
            for (int i = 1; i < 8; i++)
            {
                registersBox.Items.Add($"R{i}: {micro?.MicroRegisters.GetRegisterValue((byte)i) ?? "NA"}");
            }
        }

        private void UpdateInstructionBox()
        {
            instructionsBox.Items.Clear();
            instructionsBox.Items.Add($"Previous Instruction: {GetPrettyInstruction(micro?.PreviousInstruction)}");
            instructionsBox.Items.Add($"Current Instruction:  {GetPrettyInstruction(micro?.CurrentInstruction)}");
            instructionsBox.Items.Add($"Next Instruction:     {GetPrettyInstruction(micro?.PeekNextInstruction())}");
        }

        private void RunAllBtn_Click(object sender, RoutedEventArgs e)
        {
            if (lines == null || lines.Length == 0)
            {
                MessageBox.Show("NOt target OBJ file found.\nLoad an OBJ file or an ASM and compile.", "No OBJ file found.");
                return;
            }

            if (micro == null)
            {
                MessageBox.Show("Cannot execute OBJ instructions file if Micro is turned OFF", "Microprocessor not connnected.");
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
                while (!stopRun && micro != null)
                {
                    Thread.Sleep(100);

                    micro.NextInstruction();

                    Dispatcher.Invoke(() =>
                    {
                        UpdateInstructionBox();

                        LoadMemory();

                        UpdateRegisters();

                        instructionsHistoryBox.Items.Add(micro?.CurrentInstruction);
                    });
                }
            }).Start();
        }

        private void RunNextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (lines == null || lines.Length == 0)
            {
                MessageBox.Show("NOt target OBJ file found.\nLoad an OBJ file or an ASM and compile.", "No OBJ file found.");
                return;
            }

            if (micro == null)
            {
                MessageBox.Show("Cannot execute OBJ instructions file if Micro is turned OFF", "Microprocessor not connnected.");
                return;
            }

            MessageBox.Show(ioManager.ToString());

            micro.NextInstruction();

            UpdateInstructionBox();

            LoadMemory();

            UpdateRegisters();

            instructionsHistoryBox.Items.Add(micro.CurrentInstruction);
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (micro == null)
            {
                MessageBox.Show("No microprocessor to reset.", "Microprocessor not connnected.");
                return;
            }

            stopRun = true;

            Thread.Sleep(100);

            if (lines != null || ioManager != null)
            {
                //Micro simulator setup
                vm = new VirtualMemory(lines);

                ioManager?.ResetIOs();

                micro = new MicroSimulator(vm, ioManager);

                try
                {
                    string stackPointer = stackPointerStart.Text.Trim();

                    micro.StackPointer = Convert.ToUInt16(stackPointer);
                }
                catch(Exception)
                {
                    MessageBox.Show("Using Microprocessor default Stack Pointer Start: 0", "Invalid Stack Pointer Start");
                    stackPointerStart.Text = micro.StackPointer.ToString();
                }
            }

            LoadMemory();

            UpdateRegisters();
            
            UpdateInstructionBox();

            runAllBtn.Header = "Run All";

            instructionsHistoryBox.Items.Clear();
            memoryBox.Items.Clear();
        }

        private void TurnOnBtn_Click(object sender, RoutedEventArgs e)
        {
            stopRun = true;

            if (lines != null)
            {
                //Micro simulator setup
                vm = new VirtualMemory(lines);

                // state the last port for the micro
                ioManager = new IOManager(vm.VirtualMemorySize - 1);

                micro = new MicroSimulator(vm, ioManager);

                try
                {
                    string stackPointer = stackPointerStart.Text.Trim();

                    micro.StackPointer = Convert.ToUInt16(stackPointer);
                }
                catch (Exception)
                {
                    stackPointerStart.Text = micro.StackPointer.ToString();
                }

                SetIOs();

                LoadMemory();

                UpdateRegisters();

                UpdateInstructionBox();
            } else
            {
                MessageBox.Show("There is no OBJ or ASM file to initialize the Microprocessor with.", "Invalid State");
            }
        }

        private void TurnOffBtn_Click(object sender, RoutedEventArgs e)
        {

            stopRun = true;

            Thread.Sleep(100);

            if(micro == null)
            {
                MessageBox.Show("Microprocessor was not detected to be in ON state.", "Invalid State");
                return;
            }

            ioManager.ResetIOs();
            
            foreach(Window window in _ioDevicesWindows.Values)
            {
                window.Close();
            }

            micro = null;
            ioManager = null;
            vm = null;

            SetIOs();

            LoadMemory();

            UpdateRegisters();

            UpdateInstructionBox();

            runAllBtn.Header = "Run All";

            instructionsHistoryBox.Items.Clear();
            memoryBox.Items.Clear();

            MessageBox.Show("Micro Turned OFF");
        }

        private string GetPrettyInstruction(IMCInstruction instruction)
        {
            if (instruction == null)
                return "NA";

            string addressHex = UnitConverter.IntToHex(instruction.InstructionAddressDecimal, defaultWidth: 3);

            string contentHex = vm.GetContentsInHex(instruction.InstructionAddressDecimal) +
                vm.GetContentsInHex(instruction.InstructionAddressDecimal + 1);

            return $"{addressHex}: {contentHex}: {instruction.ToString()}";
        }

        private void VerifyMicroStateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (micro == null)
            {
                MessageBox.Show("Cannot turn ON I/O devices while the Microprocessor is OFF.", "Invalid State");
            }
        }

        private void Checked_IOASCIIDisplay(object sender, RoutedEventArgs e)
        {
            if (ioManager == null)
            {
                cbAsciiDisplay.IsChecked = false;

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
            }
        }

        private void Checked_IOHexaKeyBoard(object sender, RoutedEventArgs e)
        {
            if (!ValidIDEState(cbHexkeyboard))
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
            }
        }
        private void Checked_IO7SegmentDisplay(object sender, RoutedEventArgs e)
        {
            if (!ValidIDEState(cb7Segment))
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
            }
        }

        private void cbTrafficLight_Checked(object sender, RoutedEventArgs e)
        {
            if (ioManager == null)
            {
                cbTrafficLight.IsChecked = false;
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
            }
        }

        private void SetIOs()
        {
            // set enabled or disabled depending on if micro processor is ON or OFF
            //cb7Segment.IsEnabled = micro != null;
            //cbHexkeyboard.IsEnabled = micro != null;
            //cbTrafficLight.IsEnabled = micro != null;
            //cbAsciiDisplay.IsEnabled = micro != null;

            Checked_IOHexaKeyBoard(null, null);
            cbTrafficLight_Checked(null, null);
            Checked_IOASCIIDisplay(null, null);
            Checked_IO7SegmentDisplay(null, null);
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

        private void AssembleBtn_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(textEditorRB.Document.ContentStart, textEditorRB.Document.ContentEnd);
            string[] rbText = textRange.Text.Split(Environment.NewLine);
            
            try
            {
                fileLines.ItemsSource = lines = Assemble(rbText);
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

        private string[] Assemble(string[] input)
        {
            Lexer lexer = new Lexer(input);
            Parser parser = new Parser(lexer);
            this.compiler = new Compiler(parser);
            this.compiler.Compile();
            return compiler.GetOutput();
        }
        private void Btn_Click_ExportMemoryMap(object sender, RoutedEventArgs e)
        {
            if (vm == null)
            {
                MessageBox.Show("Cannot export Memory Map if no Object file is uploaded.", "Invalid IDE State");

                return;
            }

            string[] virtualMemoryState = vm.ExportVirtualMemory();

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"{currFileName}_VM_MemoryMap.txt",
                DefaultExt = ".txt",
                Filter = "Text Document (.txt)|*.txt"
            };

            // Show save file dialog box
            Nullable<bool> result = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // get full path for the document
                string fullPath = saveFileDialog.FileName;

                // Save document
                FileManager.Instance.ToWriteFile(fullPath, virtualMemoryState);

                MessageBox.Show($"Exported Virtual Memory Map to: {saveFileDialog.FileName}.", "Exported successfuly");
            }
            else
            {
                MessageBox.Show($"Folder to save file not selected.", "Memory Map not exported");
            }
        }


        private void OpenOBJ_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = "*.obj;*.txt",
                Filter = "Files|*.obj;*.txt|Object Files|*.obj|Text Document|*.txt"
            };

            bool? result = ofd.ShowDialog();

            if (result == true)
            {
                try
                {
                    currFileName = Path.GetFileNameWithoutExtension(ofd.FileName);

                    fileLines.ItemsSource = lines = File.ReadAllLines(ofd.FileName);
                    statusLabel.Content = "Status: File Loaded";
                }
                catch (Exception ex)
                {
                    //TODO: Create log with error
                    MessageBox.Show(ex.Message, "Unexpected error when loading object file.");
                    statusLabel.Content = "Status: File Error";
                }

                ResetBtn_Click(sender,e);
                Init();
            }
        }

        private void SaveObj_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"{currFileName}.obj",
                DefaultExt = "*.obj;*.txt",
                Filter = "Files|*.obj;*.txt|Object Files|*.obj|Text Document|*.txt"
            };

            // Show save file dialog box
            Nullable<bool> result = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // get full path for the document
                string fullPath = saveFileDialog.FileName;

                // Save document
                FileManager.Instance.ToWriteFile(fullPath, lines);

                MessageBox.Show($"Object File saved to: {saveFileDialog.FileName}.", "Exported successfuly");
            }
            else
            {
                MessageBox.Show($"Folder to save file not selected.", "File not saved");
            }
        }

        private void OpenAsm_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = "*.asm;*.txt",
                Filter = "Files|*.asm;*.txt|Assembly Files|*.asm|Text Document|*.txt"
            };

            bool? result = ofd.ShowDialog();

            if (result == true)
            {
                try
                {
                    currFileName = Path.GetFileNameWithoutExtension(ofd.FileName);
                    TextRange textRange = new TextRange(textEditorRB.Document.ContentStart, textEditorRB.Document.ContentEnd)
                    {
                        Text = string.Join(Environment.NewLine, File.ReadAllLines(ofd.FileName))
                    };

                    statusLabel.Content = "Status: Assembly File Loaded";

                    AssembleBtn_Click(null, null);
                }
                catch (Exception ex)
                {
                    //TODO: Create log with error
                    MessageBox.Show(ex.Message, "Unexpected error when loading object file.");
                    statusLabel.Content = "Status: File Error";
                }
            }
        }

        private void SaveAsm_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(textEditorRB.Document.ContentStart, textEditorRB.Document.ContentEnd);
            string[] assemblyConent = textRange.Text.Split(Environment.NewLine);

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"{currFileName}_Assembly.txt",
                DefaultExt = "*.asm;*.txt",
                Filter = "Files|*.asm;*.txt|Assembly Files|*.asm|Text Document|*.txt"
            };

            // Show save file dialog box
            Nullable<bool> result = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // get full path for the document
                string fullPath = saveFileDialog.FileName;

                // Save document
                FileManager.Instance.ToWriteFile(fullPath, assemblyConent);

                MessageBox.Show($"Assembly File saved to: {saveFileDialog.FileName}.", "Saved successfuly");
            }
            else
            {
                MessageBox.Show($"Folder to save file not selected.", "File not saved");
            }
        }

        private RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.ECMAScript; 

        private void textEditorRB_TextChange(object sender, TextChangedEventArgs e)
        {
            //new Thread(() =>

            //{
           
            Dispatcher.Invoke(() => {
                if (textEditorRB.Document == null)
                    return;

                //first clear all the formats
                TextRange documentRange = new TextRange(textEditorRB.Document.ContentStart, textEditorRB.Document.ContentEnd);
                documentRange.ClearAllProperties();

                textEditorRB.TextChanged -= this.textEditorRB_TextChange;

                string pattern = @"([^\W_]+[^\s,.]*)";
                TextPointer pointer = textEditorRB.Document.ContentStart;

                    while (pointer.CompareTo(textEditorRB.Document.ContentEnd) < 0)
                    {
                        if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                        {

                            string textRun = pointer.GetTextInRun(LogicalDirection.Forward);
                            
                            MatchCollection matches = Regex.Matches(textRun, pattern, options);

                            foreach (Match match in matches)
                            {
                                try
                                {
                                    TextPointer start = pointer.GetPositionAtOffset(match.Index);

                                    TextRange wordRange = new TextRange(start, start.GetPositionAtOffset(match.Length));
                                    if (KeyWordDetector.IsKeyword(wordRange.Text, out SolidColorBrush color))
                                    {
                                        wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, color);
                                        wordRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                                    }
                                    else
                                    {
                                        wordRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
                                        wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.White);
                                    }
                                }
                                catch (Exception ex) {
                                    MessageBox.Show(ex.ToString());
                                }
                            }

                        }

                        pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
                    }
                textEditorRB.TextChanged += this.textEditorRB_TextChange;


            });
            //}).Start();
        }
    }
}
