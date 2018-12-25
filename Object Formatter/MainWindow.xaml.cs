using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Object_Formatter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
        public VM ViewModel { get; set; } = new VM();
        public Serial_Communication_Tunnel SerialPort { get; set; } = new Serial_Communication_Tunnel();
        string RequestString = "";
        public static ObservableCollection<string> TempList { get; set; } = new ObservableCollection<string>() { };
        private void MoveElement(object sender, RoutedEventArgs e)
         
        {
            TempList.Clear();

            foreach (var item in FirstList.SelectedItems)
            {
                TempList.Add(item.ToString());
            }
            VM.ConfigurationList = TempList;
            SecondList.ItemsSource = TempList;
        }

        private void Formate_Sent_String(object sender, RoutedEventArgs e)
        {
            
            
            //used in creating new columns header
            //ViewModel.StartUp_Report_FormatterVM.ConfigurationList = TempList;
            RequestString = VM.SentStringGenerator();
            Sent.Text = RequestString;
        }

        private void Sent_To_Device(object sender, RoutedEventArgs e)
        {
           Incomming.Text = SerialPort.ToDevice();
        }
    }
}
