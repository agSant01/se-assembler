using Assembler.Core;
using Assembler.Microprocessor;
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

namespace GUI_Assembler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MicroSimulator micro;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "Text Document (.txt)|*.txt";
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                try
                {
                    var path = ofd.FileName;
                    fileLines.ItemsSource = File.ReadAllLines(ofd.FileName);
                  
                    var shell = new Shell(path);
                    shell.ExportFiles();
                    var lines = shell.compiler.GetOutput();

                    VirtualMemory vm = new VirtualMemory(lines);

                    micro = new MicroSimulator(vm);
                    memoryString.Content = micro.MicroVirtualMemory.ToString();
                    registersString.Content = micro.MicroRegisters.ToString();
                }
                catch(Exception ex)
                {
                    //TODO: Create log with error
                    MessageBox.Show("There was a problem reading the file.");
                }
            }
        }

        private void fileLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = fileLines.SelectedIndex;
            var lineString = fileLines.SelectedItem as string;
        }
    }
}
