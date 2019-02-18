using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Object_Formatter
{
    public class VM
    {
        public PVM PViewModel { get; set; } = new PVM();
        public static Dictionary<string, ParameterObject> RowDictionary { get; set; }
        public static Dictionary<string, Parameter> RowParamDictionary { get; set; }
        public List<string> NamesList { get; set; } = new List<string> { };
        public List<string> ParamNamesList { get; set; } = new List<string> { };
        public static ObservableCollection<string> ConfigurationList { get; set; } = new ObservableCollection<string>() { };
        public static ObservableCollection<string> ParamConfigurationList { get; set; } = new ObservableCollection<string>() { };
        public ObservableCollection<string> AvailableCOM { get; set; } = new ObservableCollection<string> { };

        public VM()
        {
            RowDictionary = DictonaryImporter.RowObjectDictionaryProvider();
            foreach (var item in RowDictionary)
            {
                // Names list is what is showing on the window 
                NamesList.Add(item.Value.Name);
            }
            RowParamDictionary = DictonaryImporter.RowDictionaryProvider();
            foreach (var item in RowParamDictionary)
            {
                ParamNamesList.Add(item.Value.ParamName);
            }
        }
        public  void ScanPorts()
        {
            AvailableCOM.Clear();
            foreach (var item in System.IO.Ports.SerialPort.GetPortNames())
            {
                AvailableCOM.Add(item);
            }
        }
        //----------------------------------
        public static string SentStringGenerator()
        {
            string ObjectString = "";
         
            foreach (var item in ConfigurationList)
            {
                var MyValue = RowDictionary.First(x => x.Value.Name == item).Value.Value;
                ObjectString += MyValue.ToString();
                if (ConfigurationList.IndexOf(item) == ConfigurationList.Count - 1 && VM.ParamConfigurationList.Count ==0)
                {
                    ObjectString += ";";
                }
                else
                    ObjectString += "|";
            };
            return  ObjectString;
        }
        //------------------------------------------
        public static string ConfigurationStringGenerator()
        {
            string KiesString = "";
            foreach (var item in ParamConfigurationList)
            {
                var MyKey = RowParamDictionary.First(x => x.Value.ParamName == item).Key;
                KiesString += MyKey;
                if (ParamConfigurationList.IndexOf(item) == ParamConfigurationList.Count - 1)
                {
                    KiesString += ";";
                }
                else
                    KiesString += "|";
            };
            return KiesString;
        }
        //-------------------------

    }
}
