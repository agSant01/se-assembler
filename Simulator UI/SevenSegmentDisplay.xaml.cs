using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Simulator_UI
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool entry = (bool)value;
            SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#333333"));
            if (entry)
            {
                color = Brushes.Green;
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Convert Back has not been implemented.");
        }
    }

    public class SevenSegmentDisplayModel : INotifyPropertyChanged
    {
        private bool[] _firstNumber;
        private bool[] _secondNumber;
        public bool[] FirstNumber
        {
            get
            {
                return _firstNumber;
            }
        }
        public bool[] SecondNumber
        {
            get
            {
                return _secondNumber;
            }
        }

        public bool IsFirstDigitActive { get; set; }

        public bool IsSecondDigitActive { get; set; }

        public SevenSegmentDisplayModel()
        {
            IsFirstDigitActive = false;
            IsSecondDigitActive = false;

            _firstNumber = offState;
            _secondNumber = offState;
        }

        public void Reset()
        {
            IsFirstDigitActive = false;
            IsSecondDigitActive = false;

            _firstNumber = offState;
            _secondNumber = offState;

            OnPropertyChanged("FirstNumber"); //actualiza gui
            OnPropertyChanged("SecondNumber");
            OnPropertyChanged("IsFirstDigitActive");
            OnPropertyChanged("IsSecondDigitActive");
        }

        //2D array for how the 7-point segment should be displayed
        private readonly bool[] offState = new bool[] { false, false, false, false, false, false, false };

        public void ShowBinary(string binary)
        {
            var final = new bool[] { false, false, false, false, false, false, false };
            for (int i = 0; i < 7; i++)
            {
                final[i] = binary[i] == '1';
            }
            var showFirst = binary[7] == '0';
            if(showFirst)
            {
                _firstNumber = final;
            }
            else
            {
                _secondNumber = final;
            }
            IsFirstDigitActive = showFirst;
            IsSecondDigitActive = !showFirst;
            OnPropertyChanged("FirstNumber"); //actualiza gui
            OnPropertyChanged("SecondNumber");
            OnPropertyChanged("IsFirstDigitActive");
            OnPropertyChanged("IsSecondDigitActive");
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

        public void Reset()
        {
            Model.Reset();
        }

        public bool ShowDivider
        {
            get { return Divider.Visibility == Visibility.Visible; }
            set { Divider.Visibility = value ? Visibility.Visible : Visibility.Hidden; }
        }

        public void SetBinaryNumber(string binaryNumber)
        {
            if (string.IsNullOrEmpty(binaryNumber))
            {
                Reset();
            }
            else
            {
                Model.ShowBinary(binaryNumber);
            }
        }
    }
}