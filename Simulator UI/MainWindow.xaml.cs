using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using System;
using System.Collections;
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
using Assembler.Core.IO_Devices;

namespace Simulator_UI
{
    public partial class MainWindow : Window
    {
        private MicroSimulator micro;
        private VirtualMemory vm;
        private bool stopRun;

        private ASCII_Display display;
        //stack pointer 
        ushort spMax, spMin;


        private string[] lines;

        public MainWindow()
        {
            InitializeComponent();
            statusLabel.Content = "Status: First enter Stack Pointer Range Before Inserting File";
        }

        private void Init()
        {
            //UI Elements
            stopRun = true;

            //Micro simulator setup
            vm = new VirtualMemory(lines);

            micro = new MicroSimulator(vm);

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

            }
        }

        private void RunNextBtn_Click(object sender, RoutedEventArgs e)
        {
            micro.NextInstruction();
            UpdateInstructionBox(micro.CurrentInstruction?.ToString() ?? "", micro.PreviousInstruction?.ToString() ?? "");
            LoadMemory();
            UpdateRegisters();
            instructionsHistoryBox.Items.Add(micro.CurrentInstruction);
            UpdateASCIIButtons();//TODO Maybe this doesnt work

                stackPointerStart.Text = micro.StackPointer.ToString();

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
                memoryBox.Items.Add($"{i}) {vm.GetContentsInHex(i)} {vm.GetContentsInHex(i + 1)}");
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
                registersBox.Items.Add($"R{i}: {micro.MicroRegisters.GetRegisterValue((byte)i)}");
            }

            //DONT KNOW IF I SHOULD PUT THESE HERE
            //UpdateASCIIButtons();//TODO Maybe remove this crap
        }

        private void UpdateInstructionBox()
        {
            instructionsBox.Items.Clear();
            instructionsBox.Items.Add($"Previous Instruction: {GetPrettyInstruction(micro.PreviousInstruction)}");
            instructionsBox.Items.Add($"Current Instruction:   {GetPrettyInstruction(micro.CurrentInstruction)}");
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

        private void UpdateASCIIButtons()
        {
            TextBox[] ASCII_Display  = { a, b,c ,d,e,f,g,h};
            int[] reserved = display.ReservedAddresses();

            ArrayList active = display.ActiveCharactersIndexes(vm);
            foreach(int i in active)
            {
                //ASCII_Display[i].IsEnabled= true;
                if (!ASCII_Display[i].IsVisible)
                    ASCII_Display[i].Background = Brushes.White;
                

                else
                {

                    //Do nothing;
                }
            }

            ArrayList inactive = display.InactiveCharactersIndexes(vm);
            foreach (int i in inactive)
            {
                ASCII_Display[i].Background = Brushes.Transparent;
            }
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

            UpdateInstructionBox();

            Init();
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text Document (.txt)|*.txt"
            };

            bool? result = ofd.ShowDialog();
            try
            {
                display = new ASCII_Display(vm);
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message, "No Contiguous Memory Found For ASCII Display");
            }


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
        //private void RunNextBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (micro == null)
        //    {
        //        MessageBox.Show("Load an Object file before trying to execute instructions.");
        //        return;
        //    }

        //    micro.NextInstruction();

        //    UpdateInstructionBox();

        //    LoadMemory();
            
        //    UpdateRegisters();

        //    instructionsHistoryBox.Items.Add(micro.CurrentInstruction);
        //}

        private void c_TextChanged(object sender, TextChangedEventArgs e)
        {

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
    }
}
