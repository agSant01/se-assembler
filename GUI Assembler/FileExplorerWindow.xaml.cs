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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI_Assembler
{
    /// <summary>
    /// Interaction logic for FileExplorerWindow.xaml
    /// </summary>
    public partial class FileExplorerWindow : Window
    {
        public FileExplorerWindow()
        {
            InitializeComponent();
           
        }

        public string ResponseString
        {
            get;
            set;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "Text Document (.txt)|*.txt";
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                string filename = ofd.FileName;
                pathText.Text = filename;
             

            }
            
           
        }

        private void pathText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
