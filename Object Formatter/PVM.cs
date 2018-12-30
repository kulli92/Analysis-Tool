using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Object_Formatter
{
   public class PVM
    {
        public static Dictionary<string, Parameter> RowDictionary { get; set; }
        public List<string> NamesList { get; set; } = new List<string> { };
        public static ObservableCollection<string> ConfigurationList { get; set; } = new ObservableCollection<string>() { };

        public PVM()
        {
            RowDictionary = DictonaryImporter.RowDictionaryProvider();
            foreach (var item in RowDictionary)
            {
                NamesList.Add(item.Value.ParamName);
            }
        }
        public static string ConfigurationStringGenerator()
        {
            string KiesString = "";
            foreach (var item in ConfigurationList)
            {
                var MyKey = RowDictionary.First(x => x.Value.ParamName == item).Key;
                KiesString += MyKey;
            };
            return KiesString;

        }
    }
}
