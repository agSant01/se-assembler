using Assembler;
using Assembler.Assembler;
using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO;
using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Parsing;
using Assembler.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Simulator_UI
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, Window> _ioDevicesWindows = new Dictionary<string, Window>();

        private readonly MicroProcessor microProcessor = new MicroProcessor();

        private string CurrentFileName;

        public MainWindow()
        {
            InitializeComponent();

            statusLabel.Content = "Status: First enter Stack Pointer Range Before Inserting File";

            micro_status_lbl.Background = Brushes.LightGray;

            micro_status_lbl.Content = "Micro Status: Disconnected";

            // Set instructions print mode to ASM Text
            IMCInstruction.AsmTextPrint = true;

            RefreshGUI();
        }

        private void ResetGUI()
        {
            //UI Elements
            instructionsHistoryBox.Items.Clear();

            memoryBox.Items.Clear();

            runAll.Content = "Run All";

            SetIOs();

            RefreshGUI();
        }

        private void RefreshGUI()
        {
            //UI Elements
            LoadMemory();

            UpdateRegisters();

            UpdateInstructionBox();


            if (microProcessor.IsOn())
            {
                micro_status_lbl.Background = Brushes.Green;
                micro_status_lbl.Content = "Micro Status: Connected";
            }
            else
            {
                micro_status_lbl.Background = Brushes.LightGray;
                micro_status_lbl.Content = "Micro Status: Disconnected";
            }

            runAll.Content = microProcessor.StopRun ? "Run All" : "Stop";
        }

        private void LoadMemory()
        {
            memoryBox.Items.Clear();

            if (!int.TryParse(memorySizeBox.Text, out int lines))
            {
                _ = MessageBox.Show("Invalid number of memory blocks to show. Setting default to 250.", "Invalid Input");
                memorySizeBox.Text = "250";
                lines = 50;
            }

            for (int i = 0; i < lines * 2; i += 2)
            {
                memoryBox.Items.Add($"{UnitConverter.IntToHex(i, defaultWidth: 3)} : " +
                    $"{microProcessor.VirtualMemory?.GetContentsInHex(i) ?? "NA"} " +
                    $"{microProcessor.VirtualMemory?.GetContentsInHex(i + 1) ?? "NA"}");
            }
        }

        private void UpdateRegisters()
        {
           if (!microProcessor.IsOn())
           {
                stackPointerStart.Text = "000";
           }

            stackPointerBox.Text = microProcessor.Micro == null ? "NA" : UnitConverter.U16BitToHex(microProcessor.Micro.StackPointer, defaultWidth: 3);

            programCounterBox.Text = microProcessor.Micro == null ? "NA" : UnitConverter.U16BitToHex(microProcessor.Micro.ProgramCounter, defaultWidth: 3);

            conditionalBitBox.Text = (microProcessor.Micro?.ConditionalBit ?? false) ? "1" : "0";

            registersBox.Items.Clear();

            registersBox.Items.Add($"R0: {(microProcessor.IsOn() ? "00" : "NA")}");

            for (int i = 1; i < 8; i++)
            {
                registersBox.Items.Add($"R{i}: {microProcessor.Micro?.MicroRegisters.GetRegisterValue((byte)i) ?? "NA"}");
            }
        }

        private void UpdateInstructionBox()
        {
            instructionsBox.Items.Clear();
            instructionsBox.Items.Add($"Previous Instruction:  {microProcessor.GetPerviousInstruction}");
            instructionsBox.Items.Add($"Current Instruction:   {microProcessor.GetCurrentInstruction}");
            instructionsBox.Items.Add($"Next Instruction:      {microProcessor.GetNextInstruction}");
        }

        private void RunAllBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!microProcessor.IsOn())
            {
                _ = MessageBox.Show("No target OBJ file found.\nLoad an OBJ file or an ASM and compile.", "Cannot run.");
                return;
            }

            microProcessor.StopRun = !microProcessor.StopRun;

            runAll.Content = microProcessor.StopRun ? "Run All" : "Stop";

            if (microProcessor.StopRun)
            {
                return;
            }

            try
            {
                string hexStackPointer = stackPointerStart.Text.Trim();

                microProcessor.Micro.StackPointer = UnitConverter.HexToU16Bit(hexStackPointer);
            }
            catch (Exception)
            {
                _ = MessageBox.Show($"Invalid Stack pointer: '0x{cbHexkeyboard}'.\nUsing Microprocessor default Stack Pointer Start: '0x0'", "Invalid Stack Pointer Start");
                microProcessor.Micro.StackPointer = 0;
                stackPointerStart.Text = microProcessor.Micro == null ? "000" : UnitConverter.U16BitToHex(microProcessor.Micro.StackPointer, defaultWidth: 3);
            }

            RefreshGUI();

            Task.Run(() =>
            {
                while (!microProcessor.StopRun)
                {
                    Thread.Sleep(100);
                    try
                    {
                        microProcessor.Micro.NextInstruction();
                        Dispatcher.Invoke(() =>
                        {
                            try
                            {
                                UpdateInstructionBox();
                                LoadMemory();
                                UpdateRegisters();
                                if (microProcessor.GetCurrentInstruction.Length > 0)
                                    instructionsHistoryBox.Items.Add(microProcessor.GetCurrentInstruction);
                            }
                            catch (Exception ex)
                            {
                                var message = ex.Message;
                                _ = MessageBox.Show($"{ex.Message}\nStopping instruction execution.", "Unexpected error");
                                microProcessor.StopRun = true;
                                Dispatcher.Invoke(() => { runAll.Content = "Run All"; });
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        _ = MessageBox.Show($"{ex.Message}\nStopping instruction execution.", "Unexpected error.");
                        microProcessor.StopRun = true;
                        Dispatcher.Invoke(() => { runAll.Content = "Run All"; });
                    }
                }
            });
        }

        private void RunNextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!microProcessor.IsOn())
            {
                _ = MessageBox.Show("No target OBJ file found.\nLoad an OBJ file or an ASM and compile.", "Microprocessor not connnected.");
                ResetGUI();
                return;
            }

            try
            {
                microProcessor.Micro.NextInstruction();
                if (microProcessor.GetCurrentInstruction.Length > 0)
                    instructionsHistoryBox.Items.Add(microProcessor.GetCurrentInstruction);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"{ex.Message}\nStopping instruction execution.", "Unexpected error");
            }
            
            RefreshGUI();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!microProcessor.IsOn())
            {
                ResetGUI();
                return;
            }

            Thread.Sleep(100);
            
            microProcessor.Reset();
           
            try { 
                string hexStackPointer = stackPointerStart.Text.Trim();

                microProcessor.Micro.StackPointer = UnitConverter.HexToU16Bit(hexStackPointer);
            }
            catch (Exception)
            {
                _ = MessageBox.Show($"Invalid Stack pointer: '0x{cbHexkeyboard}'.\nUsing Microprocessor default Stack Pointer Start: '0x0'", "Invalid Stack Pointer Start");
                microProcessor.Micro.StackPointer = 0;
                stackPointerStart.Text = microProcessor.Micro == null ? "000" : UnitConverter.U16BitToHex(microProcessor.Micro.StackPointer, defaultWidth: 3);
            }

            RefreshGUI();

            micro_status_lbl.Background = Brushes.Gray;

            micro_status_lbl.Content = "Micro Status: StandBy";
        }

        private void AssembleBtn_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(textEditorRB.Document.ContentStart, textEditorRB.Document.ContentEnd);
            
            if (textRange.Text.Trim().Length == 0)
            {
                _ = MessageBox.Show("No ASM code to assemble.", "Invalid State");

                microProcessor.Clear();

                ResetGUI();

                return;
            }

            string[] assemblyLines = textRange.Text.Split(Environment.NewLine);
            
            try
            {
                Thread.Sleep(100);

                microProcessor.Reset();

                microProcessor.InitializeMicroASM(assemblyLines, CurrentFileName);


                objFile.ItemsSource = microProcessor.OBJFileLines;

                statusLabel.Content = "Status: File Loaded";

                logLines.ItemsSource = microProcessor.AssemblyLogger.GetLines();

                if (microProcessor.AssemblyLogger.HasAssemblyError)
                {
                    _ = MessageBox.Show("Output code -1.\n\n" +
                        "The assembler exited with assembling errors.\n" +
                        "More info in the Assembly Log.", "Assembled with errors");
                }
                else if (microProcessor.AssemblyLogger.HasAssemblyWarning)
                {
                    _ = MessageBox.Show("The assembler exited with assembly warnings.\n" +
                        "More info in the Assembly Log.", "Assembled with warnings");
                }

                try
                {
                    string hexStackPointer = stackPointerStart.Text.Trim();

                    microProcessor.Micro.StackPointer = UnitConverter.HexToU16Bit(hexStackPointer);
                }
                catch (Exception)
                {
                    _ = MessageBox.Show($"Invalid Stack pointer: '0x{cbHexkeyboard}'.\nUsing Microprocessor default Stack Pointer Start: '0x0'", "Invalid Stack Pointer Start");
                    microProcessor.Micro.StackPointer = 0;
                    stackPointerStart.Text = microProcessor.Micro == null ? "000" : UnitConverter.U16BitToHex(microProcessor.Micro.StackPointer, defaultWidth: 3);
                }

                ResetGUI();
            }
            catch (Exception ex)
            {
                microProcessor.Clear();
             
                ResetGUI();

                _ = MessageBox.Show(ex.Message, "Error");

                statusLabel.Content = "Status: File Error";
            }
        }

        private void Btn_Click_ExportMemoryMap(object sender, RoutedEventArgs e)
        {
            if (!microProcessor.IsOn())
            {
                _ = MessageBox.Show("Cannot export Memory Map if no Object file is uploaded.", "Invalid IDE State");

                ResetGUI();

                return;
            }

            string[] virtualMemoryState = microProcessor.VirtualMemory.ExportVirtualMemory();

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"{CurrentFileName}_VM_MemoryMap.txt",
                DefaultExt = ".txt",
                Filter = "Text Document (.txt)|*.txt"
            };

            // Show save file dialog box
            bool? result = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // get full path for the document
                string fullPath = saveFileDialog.FileName;

                // Save document
                FileManager.Instance.ToWriteFile(fullPath, virtualMemoryState);

                _ = MessageBox.Show($"Exported Virtual Memory Map to: {saveFileDialog.FileName}.", "Exported successfuly");
            }
            else
            {
                _ = MessageBox.Show($"Folder to save file not selected.", "Memory Map not exported");
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
                    CurrentFileName = Path.GetFileNameWithoutExtension(ofd.FileName);

                    string[] objFileLines = File.ReadAllLines(ofd.FileName);

                    objFile.ItemsSource = objFileLines;

                    microProcessor.InitializeMicroOBJ(objFileLines);

                    statusLabel.Content = "Status: File Loaded";
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(ex.Message, "Unexpected error when loading object file.");
                    statusLabel.Content = "Status: Upload File Error";
                }

                RefreshGUI();
            }
            else
            {
                _ = MessageBox.Show("File not selected.", "File not saved");
            }
        }

        private void SaveObj_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"{CurrentFileName}.obj",
                DefaultExt = "*.obj;*.txt",
                Filter = "Files|*.obj;*.txt|Object Files|*.obj|Text Document|*.txt"
            };

            // Show save file dialog box
            bool? result = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // get full path for the document
                string fullPath = saveFileDialog.FileName;

                // Save document
                FileManager.Instance.ToWriteFile(fullPath, microProcessor.OBJFileLines);

                _ = MessageBox.Show($"Object File saved to: {saveFileDialog.FileName}.", "Exported successfuly");
            }
            else
            {
                _ = MessageBox.Show($"Folder to save file not selected.", "File not saved");
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
                    CurrentFileName = Path.GetFileNameWithoutExtension(ofd.FileName);
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
                    _ = MessageBox.Show(ex.Message, "Unexpected error when loading ASM file.");
                    statusLabel.Content = "Status: File Error";
                }
            } 
            else
            {
                _ = MessageBox.Show("File not selected.", "File not saved");
            }
        }

        private void SaveAsm_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(textEditorRB.Document.ContentStart, textEditorRB.Document.ContentEnd);
            string[] assemblyConent = textRange.Text.Split(Environment.NewLine);

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"{CurrentFileName}_Assembly.txt",
                DefaultExt = "*.asm;*.txt",
                Filter = "Files|*.asm;*.txt|Assembly Files|*.asm|Text Document|*.txt"
            };

            // Show save file dialog box
            bool? result = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // get full path for the document
                string fullPath = saveFileDialog.FileName;

                // Save document
                FileManager.Instance.ToWriteFile(fullPath, assemblyConent);

                _ = MessageBox.Show($"Assembly File saved to: {saveFileDialog.FileName}.", "Saved successfuly");
            }
            else
            {
                _ = MessageBox.Show($"Folder to save file not selected.", "File not saved");
            }
        }

        private void SaveLog_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"{CurrentFileName ?? ""}_log.txt",
                DefaultExt = "*.log;*.txt",
                Filter = "Files|*.log;*.txt|Log Files|*.log|Text Document|*.txt"
            };

            // Show save file dialog box
            bool? result = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // get full path for the document
                string fullPath = saveFileDialog.FileName;

                // Save document
                FileManager.Instance.ToWriteFile(fullPath, microProcessor.AssemblyLogger.GetLines() ?? (new string[] { "log is empty" }));

                _ = MessageBox.Show($"Log File saved to: {saveFileDialog.FileName}.", "Saved successfuly");
            }
            else
            {
                _ = MessageBox.Show($"Folder to save file not selected.", "File not saved");
            }
        }

        private void VerifyMicroStateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!microProcessor.IsOn())
            {
                _ = MessageBox.Show("Cannot turn ON I/O devices while the Microprocessor " +
                    "is Disconnected. Load ASM or OBJ file or Assemble a ASM script to connect the Microprocessor.", "Invalid State");
            }

            RefreshGUI();
        }

        private void ConnectIO(object sender, RoutedEventArgs e)
        {
            Type ioGUI;
            TextBox portTextBox;
            CheckBox ioCheckBox;
            string id;

            CheckBox cb = (CheckBox)sender;

            if (cb.Name.Equals("cbHexkeyboard"))
            {
                ioGUI = typeof(IOHexKeyboardUI);
                portTextBox = tbHexKeyBoard;
                ioCheckBox = cbHexkeyboard;
                id = IOHexKeyboardUI.DeviceID;
            }
            else if (cb.Name.Equals("cbAsciiDisplay"))
            {
                ioGUI = typeof(Display_GUI);
                portTextBox = tbAsciiDisp;
                ioCheckBox = cbAsciiDisplay;
                id = Display_GUI.DeviceID;
            }
            else if (cb.Name.Equals("cb7Segment"))
            {
                ioGUI = typeof(SevenSegmentWindow);
                portTextBox = tb7Seg;
                ioCheckBox = cb7Segment;
                id = SevenSegmentWindow.DeviceID;
            }
            else if (cb.Name.Equals("cbTrafficLight"))
            {
                ioGUI = typeof(IOBinSemaforoUI);
                portTextBox = tbTrafficLight;
                ioCheckBox = cbTrafficLight;
                id = IOBinSemaforoUI.DeviceID;
            }
            else
            {
                _ = MessageBox.Show("Invalid IO");
                return;
            }

            if (!ValidIDEState(ioCheckBox))
            {
                return;
            }

            if (_ioDevicesWindows.TryGetValue(id, out Window window))
            {
                window.Close();
                _ioDevicesWindows.Remove(id);
            }

            if (IsValidPort(portTextBox.Text, out ushort port))
            {
                try
                {
                    Window ioWindow = (Window)Activator
                            .CreateInstance(ioGUI, new object[] { microProcessor.IoManager, port });
                    
                    ioWindow.Activate();

                    ioWindow.Show();

                    _ioDevicesWindows.Add(id, ioWindow);

                    portTextBox.IsEnabled = false;
                }
                catch(TargetInvocationException errr)
                {
                    // error message
                    MessageBox.Show(errr.InnerException.Message, "Error assigning port.");
                    portTextBox.IsEnabled = true;
                    ioCheckBox.IsChecked = false;
                }
                catch (Exception err)
                {
                    // error message
                    MessageBox.Show(err.Message, "Error assigning port.");
                    portTextBox.IsEnabled = true;
                    ioCheckBox.IsChecked = false;

                } 

            }
            else if (portTextBox.Text.Length == 0)
            {
                // no port selected
                MessageBox.Show("Select a port before activating the I/O Device.", "Invalid Port");

                ioCheckBox.IsChecked = false;
            }
            else
            {
                // no port selected
                MessageBox.Show("Tried to connect I/O device to invalid port.", "Invalid Port");

                ioCheckBox.IsChecked = false;
            }
        }

        private void DisconnectIO(object sender, RoutedEventArgs e)
        {
            TextBox portTextBox;
            string id;

            CheckBox cb = (CheckBox)sender;

            if (cb.Name.Equals("cbHexkeyboard"))
            {
                portTextBox = tbHexKeyBoard;
                id = IOHexKeyboardUI.DeviceID;
            }
            else if (cb.Name.Equals("cbAsciiDisplay"))
            {
                portTextBox = tbAsciiDisp;
                id = Display_GUI.DeviceID;
            }
            else if (cb.Name.Equals("cb7Segment"))
            {
                portTextBox = tb7Seg;
                id = SevenSegmentWindow.DeviceID;
            }
            else if (cb.Name.Equals("cbTrafficLight"))
            {
                portTextBox = tbTrafficLight;
                id = IOBinSemaforoUI.DeviceID;
            }
            else
            {
                _ = MessageBox.Show("Invalid IO");
                return;
            }

            if (_ioDevicesWindows.TryGetValue(id, out Window window))
            {
                _ioDevicesWindows.Remove(id);
                window.Close();
            }

            portTextBox.IsEnabled = true;
        }

        private void SetToUpperCase(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int index = tb.CaretIndex;
            tb.Text = tb.Text.ToUpper();
            tb.CaretIndex = index;
        }

        private void SetIOs()
        {
            ConnectIO(cbHexkeyboard, null);
            ConnectIO(cbTrafficLight, null);
            ConnectIO(cbAsciiDisplay, null);
            ConnectIO(cb7Segment, null);
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

            if (!microProcessor.IsOn())
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

        private bool IsValidPort(string targetPort, out ushort port)
        {
            if (ushort.TryParse(targetPort, System.Globalization.NumberStyles.HexNumber, null, out port))
            {
                return !microProcessor.IoManager.IsUsedPort(port);
            }

            return false;
        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (Window w in _ioDevicesWindows.Values)
                w.Close();
            base.OnClosed(e);
        }

        private readonly RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.ECMAScript;

        private void textEditorRB_TextChange(object sender, TextChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
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
                            catch (Exception ex)
                            {
                                _ = MessageBox.Show(ex.ToString());
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
