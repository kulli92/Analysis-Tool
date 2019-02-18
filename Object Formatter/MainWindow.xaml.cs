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
        public static ObservableCollection<string> PTempList { get; set; } = new ObservableCollection<string>() { };

        //Fill the Window to the Right
        private void MoveElement(object sender, RoutedEventArgs e)
        {
            TempList.Clear();
            VM.ConfigurationList.Clear();
            VM.ParamConfigurationList.Clear();
            // 
            foreach (var item in FirstList.SelectedItems)
            {
                TempList.Add(item.ToString());
                VM.ConfigurationList.Add(item.ToString());
            }
            foreach (var item in AlterFirstList.SelectedItems)
            {
                TempList.Add(item.ToString());
                VM.ParamConfigurationList.Add(item.ToString());
            }
           
            SecondList.ItemsSource = TempList;
        }
        // Formate the string that is going to the device 
        private void Formate_Sent_String(object sender, RoutedEventArgs e)
        {
            //used in creating new columns header
            //ViewModel.StartUp_Report_FormatterVM.ConfigurationList = TempList;
            RequestString = VM.SentStringGenerator() + "" + VM.ConfigurationStringGenerator();
            Sent.Text = RequestString;
            Serial_Communication_Tunnel.FinalFormattedString = RequestString;
        }

        //Send the string and recieve the response
        private void Sent_To_Device(object sender, RoutedEventArgs e)
        {
            SerialPort.ToDevice(Sent.Text,1,1);
            Incomming.Text = Serial_Communication_Tunnel.StaticResponseString;
        }

        // Switch Between Parameters and Object View
        private void Change_View(object sender, RoutedEventArgs e)
        {
            if ( FirstList.Visibility == Visibility.Visible)
            {
                FirstList.Visibility = Visibility.Collapsed;
                AlterFirstList.Visibility = Visibility.Visible;
            }
            else
            {
                FirstList.Visibility = Visibility.Visible;
                AlterFirstList.Visibility = Visibility.Collapsed;
            }
        }
        private void AvailableCOM_DropDownOpened(object sender, EventArgs e)
        {
            ViewModel.ScanPorts();
        }

        private void OpenPort_Click(object sender, RoutedEventArgs e)
        {

            if ((string)OpenPort.Content != "Open Port")
            {
                OpenPort.Content = "Open Port";
                SerialPort.ClosePort();
                AvailableCOM.IsEnabled = true;
                
            }
            else
            {
                OpenPort.Content = "Close Port";
                SerialPort.OpenPort(AvailableCOM.SelectedValue.ToString());
                AvailableCOM.IsEnabled = false;
            }
        }
    }
}
