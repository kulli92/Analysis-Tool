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

        public static Dictionary<string, ParameterObject> RowDictionary { get; set; }
        public List<string> NamesList { get; set; } = new List<string> { };
        public static ObservableCollection<string> ConfigurationList { get; set; } = new ObservableCollection<string>() { };

        public VM()
        {
            
            RowDictionary = DictonaryImporter.RowObjectDictionaryProvider();
            foreach (var item in RowDictionary)
            {
             // Names list is what is showing on the window 
                NamesList.Add(item.Value.Name);
            }

        }
        public static string SentStringGenerator()
        {
            string ObjectString = "";
         
            foreach (var item in ConfigurationList)
            {
                var MyValue = RowDictionary.First(x => x.Value.Name == item).Value.ContainedParams;
                ObjectString += MyValue.ToString();
                if (ConfigurationList.IndexOf(item) == ConfigurationList.Count - 1)
                {
                    ObjectString += ";";
                }
                else
                    ObjectString += "|";
            };
            return  ObjectString;
        }
    }
}
