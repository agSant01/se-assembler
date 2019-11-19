using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Simulator_UI
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool entry = (bool)value;
            Visibility tmpVisibilty = Visibility.Collapsed;
            if (entry)
            {
                tmpVisibilty = Visibility.Visible;
            }
            return tmpVisibilty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Convert Back has not been implemented.");
        }
    }

    public class SevenSegmentDisplayModel : INotifyPropertyChanged
    {
        private bool[] _number;
        public bool[] Number
        {
            get
            {
                return _number;
            }
        }

        public SolidColorBrush SegmentColor { get; set; }

        public SevenSegmentDisplayModel()
        {
            ShowDigit("10");
            SegmentColor = Brushes.Green;
        }

        //2D array for how the 7-point segment should be displayed
        private readonly bool[][] _digits =
        {
            new bool[]{true, true, true, true, true, true, false}, // zero
            new bool[]{false, true, true, false, false, false, false}, // one
            new bool[]{true, true, false, true, true, false, true}, // two
            new bool[]{true, true, true, true, false, false, true}, // three
            new bool[]{false, true, true, false, false, true, true}, // four
            new bool[]{true, false, true, true, false, true, true}, // five
            new bool[]{true, false, true, true, true, true, true}, // six
            new bool[]{true, true, true, false, false, false, false}, // seven
            new bool[]{true, true, true, true, true, true, true}, // eight
            new bool[]{true, true, true, false, false, true, true}, // nine
            new bool[]{ true, true, true, true, true, true, true}, // off
        };

        public void ShowDigit(object number)
        {
            int num = 10;
            int.TryParse((string)number, out num);

            if (num < 0 || num > 9)
                _number = _digits[10];
            else
                _number = _digits[num];

            OnPropertyChanged("Number");
        }

        public void ShowBinary(string binary)
        {
            var final = new bool[] { false, false, false, false, false, false, false };
            for (int i = 0; i < 7; i++)
            {
                final[i] = binary[i] == '1';
            }
            _number = final;
            OnPropertyChanged("Number");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
    public partial class SevenSegmentDisplay : UserControl
    {
        public readonly static string DeviceID = "S8ds83";
        public SevenSegmentDisplayModel Model;
        public SevenSegmentDisplay()
        {
            InitializeComponent();
            Model = new SevenSegmentDisplayModel();
            DataContext = Model;
        }

        public bool ShowDivider
        {
            get { return Divider.Visibility == Visibility.Visible; }
            set { Divider.Visibility = value ? Visibility.Visible : Visibility.Hidden; }
        }

        public bool ShowFirstDigit
        {
            get { return DigitOne.Visibility == Visibility.Visible; }
            set { DigitOne.Visibility = value ? Visibility.Visible : Visibility.Hidden; }
        }

        public bool ShowSecondDigit
        {
            get { return DigitTwo.Visibility == Visibility.Visible; }
            set { DigitTwo.Visibility = value ? Visibility.Visible : Visibility.Hidden; }
        }

        public void SetBinaryNumber(string binaryNumber)
        {
            Model.ShowBinary(binaryNumber.Substring(0, 7));
            var showFirst = binaryNumber[7] == '0';
            ShowFirstDigit = showFirst;
            ShowSecondDigit = !showFirst;
        }

        public void SetNumber(int number, bool showFirst)
        {
            ShowFirstDigit = showFirst;
            ShowSecondDigit = !showFirst;
            Model.ShowDigit(number.ToString());
        }
    }

}
