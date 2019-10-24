using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        private MicroSimulator micro;
        private VirtualMemory vm;
        private bool stopRun;

        //stack pointer 
        ushort spMax, spMin;

        private string[] lines;
        public MainWindow()
        {
            InitializeComponent();
            statusLabel.Content = "Status: First enter Stack Pointer Range Before Inserting File";
        }


        private void fileLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = fileLines.SelectedIndex;
            var lineString = fileLines.SelectedItem as string;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "Text Document (.txt)|*.txt";
            Nullable<bool> result = ofd.ShowDialog();
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
                    MessageBox.Show(ex.Message, "There was a problem reading the file.");
                    statusLabel.Content = "Status: File not found or open somewhere else";
                }
                Init();
            }
        }

        private void RunNextBtn_Click(object sender, RoutedEventArgs e)
        {
            micro.NextInstruction();
            UpdateInstructionBox(micro.currentInstruction?.ToString() ?? "", micro.previousInstruction?.ToString() ?? "");
            LoadMemory();
            UpdateRegisters();
            instructionsHistoryBox.Items.Add(micro.currentInstruction);
        }

        private void UpdateRegisters()
        {
            if (micro.StackPointer >= spMin && micro.StackPointer <= spMax)
            {
                stackPointerBox.Text = micro.StackPointer.ToString();
            }
            else
            {
                MessageBox.Show("Stack out of range or stack overflow");
            }
            programCounterBox.Text = micro.ProgramCounter.ToString();
            conditionalBitBox.Text = micro.ConditionalBit ? "1" : "0";

            registersBox.Items.Clear();
            registersBox.Items.Add("R0: 00");
            for (int i = 1; i < 8; i++)
            {
                registersBox.Items.Add($"R{i}: {micro.MicroRegisters.GetRegisterValue((byte)i)}");
            }
        }

        private void RunAllBtn_Click(object sender, RoutedEventArgs e)
        {
            stopRun = !stopRun;
            runAllBtn.Header = stopRun ? "Run All" : "Stop";
            for(int i = 0; i<100 && !stopRun; i++)
            {
                micro.NextInstruction();
                UpdateInstructionBox(micro.currentInstruction?.ToString() ?? "", micro.previousInstruction?.ToString() ?? "");
                LoadMemory();
                UpdateRegisters();
                instructionsHistoryBox.Items.Add(micro.currentInstruction);
            }
            

        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            stackPointerBox.Clear();
            stackPointerRangeBox.Clear();
            programCounterBox.Clear();
            conditionalBitBox.Clear();
            instructionsHistoryBox.Items.Clear();
            memoryBox.Items.Clear();

            UpdateInstructionBox("", "");
            Init();
        }

        private void Init()
        {
            //UI Elements

            stopRun = true;
            //Micro simulator setup
            vm = new VirtualMemory(lines);
            micro = new MicroSimulator(vm);

            try
            {
                string[] spRange = stackPointerRangeBox.Text.ToString().Split('-');
                if (!ushort.TryParse(spRange[0], out spMin) || !ushort.TryParse(spRange[1], out spMax))
                {
                    MessageBox.Show("Bad format on Stack Pointer Range");
                }
                else micro.StackPointer = spMax;
                statusLabel.Content = "Status: Ready";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "format error on Stack Pointer Range");
            }


        }

        private void LoadMemory()
        {
            memoryBox.Items.Clear();
            string a = "";
            int size = 50;
            Int32.TryParse(memorySizeBox.Text,out size);
            for (int i = 0; i < size; i++)
            {
                if (i % 2 == 0)
                    a += $"{micro.MicroVirtualMemory.GetContentsInHex(i)??"00"} ";
                else
                {
                    a += $"{micro.MicroVirtualMemory.GetContentsInHex(i)??"00"} ";
                    memoryBox.Items.Add(a);
                    a = "";
                }
            }
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateInstructionBox(string current, string previous)
        {
            instructionsBox.Items.Clear();
            instructionsBox.Items.Add($"Curr: {current}");
            instructionsBox.Items.Add($"Prev: {previous}");
        }

        private String getPretyInst(IMCInstruction i)
        {
            return "";
        }
    }
}
