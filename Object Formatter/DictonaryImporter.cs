using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OfficeOpenXml;


namespace Object_Formatter
{
    public class DictonaryImporter
    {
        public static Dictionary<string, Parameter> ParamDic = new Dictionary<string, Parameter> { };
        public static Dictionary<string, Parameter> ParamObjectRelatedDic = new Dictionary<string, Parameter> { };
        static Dictionary<string, ParameterObject> ObjectDic = new Dictionary<string, ParameterObject> { };
        // readonly string data;
        static public bool DictionaryHasBeenInitilized { get; set; } = false;
        //public static SerialCommunicationTunnel Tunnel { get; set; } = new SerialCommunicationTunnel();
        //------------------------------------------
        public static void ParameterDicInitilizer()
        {
            DictionaryHasBeenInitilized = true;
            var package = new ExcelPackage(new FileInfo(@"C:\Users\Aklli\source\repos\Object Formatter\Object Formatter\bin\Debug\Parameters Dictionary Reordered.xlsx"));
            ExcelWorksheet sheet = package.Workbook.Worksheets[1];
            for (int i = 1; i < 419; i++)
            {
                ParamDic.Add(sheet.Cells["A" + i].Value.ToString(),
                new Parameter()
                {
                    ParamName = sheet.Cells["B" + i].Value.ToString(),
                    Index = Convert.ToInt16(sheet.Cells["G" + i].Value.ToString()),
                    MemoryAddress = sheet.Cells["F" + i].Value.ToString(),
                    Value = ""
                });
              /*  ParamObjectRelatedDic.Add(sheet.Cells["A" + i].Value.ToString(),
                new Parameter()
                {
                    ParamName = sheet.Cells["B" + i].Value.ToString(),
                    Index = Convert.ToInt16(sheet.Cells["G" + i].Value.ToString()),
                    MemoryAddress = sheet.Cells["F" + i].Value.ToString(),
                    MinValue = sheet.Cells["C" + i].Value.ToString(),
                    MaxValue = sheet.Cells["D" + i].Value.ToString(),
                });*/
            }
            var package2 = new ExcelPackage(new FileInfo(@"C:\Users\Aklli\source\repos\Object Formatter\Object Formatter\bin\Debug\Object Dictionary.xlsx"));
            ExcelWorksheet sheet2 = package2.Workbook.Worksheets[1];
            for (int i = 2; i < 33; i++)
            {
                ObjectDic.Add(sheet2.Cells["A" + i].Value.ToString(),
                new ParameterObject()
                {
                    Name = sheet2.Cells["B" + i].Value.ToString(),
                    MemoryAddress = sheet2.Cells["C" + i].Value.ToString(),
                    ContainedParams = sheet2.Cells["H" + i].Value.ToString()
                });
            }
        }
        //------------------------------------------
        public static Dictionary<string, Parameter> RowDictionaryProvider()
        {
            if (DictionaryHasBeenInitilized)
            {
                return ParamDic;
            }
            else
            {
                ParameterDicInitilizer();
                return ParamDic;
            }
        }
        //----------------------
        public static Dictionary<string, ParameterObject> RowObjectDictionaryProvider()
        {
            if (DictionaryHasBeenInitilized)
            {
                return ObjectDic;
            }
            else
            {
                ParameterDicInitilizer();
                return ObjectDic;
            }
        }
        //------------------------------------------
        private static ObservableCollection<Parameter> FinalList = new ObservableCollection<Parameter> { };
        public static ObservableCollection<ParameterObject> FinalListOfObjects = new ObservableCollection<ParameterObject> { };
        public static ObservableCollection<Parameter> FinalContainedParameter = new ObservableCollection<Parameter> { };
        public static async Task<ObservableCollection<Parameter>> ParameterList(string ConfigurationString)
        {
            string TempString = "";
            //string DeviceResponse = ""; 
            FinalList.Clear();
            FinalListOfObjects?.Clear();
            List<string> ProccessedList = new List<string> { };
            if (DictionaryHasBeenInitilized == false)
            {
                ParameterDicInitilizer();
            }
            //  ProccessedList = StringSplitter(await Tunnel.SelectedParameterValueGetter(ConfigurationString));
            ProccessedList = StringSplitter("AA[32423]|Ad34345|BJ'ThisisEspartea'|AC<Ab43Bw33>|Bc3994345|Cg[8234]|AI21|AZ99;");
            foreach (var item in ParamDic)
            {
                item.Value.Value = "";
            }
            ParameterDefiner(ProccessedList);
            foreach (var ParameterKey in ParamDic)
            {
                if (ParameterKey.Value.Value != "")
                {
                    TempString = ParameterKey.Value.Value;
                    TempString = new string((from c in TempString
                                             where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c) || c == '/' || c == '.' || c == ':' || c == ','
                                             select c
                                              ).ToArray());
                    ParameterKey.Value.Value = TempString;
                    FinalList.Add(ParameterKey.Value);
                }
            }
            foreach (var ParameterKey in ParamObjectRelatedDic)
            {
                if (ParameterKey.Value.Value != "")
                {
                    TempString = ParameterKey.Value.Value;
                    TempString = new string((from c in TempString
                                             where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c) || c == '/' || c == '.' || c == ':' || c == ','
                                             select c
                                              ).ToArray());
                    ParameterKey.Value.Value = TempString;
                    FinalContainedParameter.Add(ParameterKey.Value);
                }
            }
            return FinalList;
        }
        //----------------------------------- String Splitter_________

        #region 
        private static List<string> StringSplitter(string str)
        {

            /*  Based on the old Logic  AB234AR32
            if (str == "")
            {
                throw new NotImplementedException();
            }

            List<string> ProccessedList = new List<string> { };
            ProccessedList.Add(str[0] + "" + str[1]);
            var SinglPartString = ProccessedList[0];

            int counter = 3;
            int i = 0;

            //59 = ;
            while (counter < str.Length && str[counter - 1] != 59)
            {
                //as long as it is a decimal number increase the counter  .=46  ,=44
                while (str[counter - 1] > 47 && str[counter - 1] < 58 || str[counter - 1] == 46 || str[counter - 1] == 44)
                {
                    SinglPartString = ProccessedList[i];
                    SinglPartString += str[counter - 1];
                    ProccessedList[i] = SinglPartString;
                    if (str.Length - counter == 0)
                    {
                        break;
                    }
                    counter++;
                }
                //handling "[]" and "<>" and "'"
                #region
                // If the character is "["--------
                if (str[counter - 1] == 91)
                    //Keep in while until u meen "]"
                    while (str[counter - 1] != 93)
                    {
                        SinglPartString = ProccessedList[i];
                        SinglPartString += str[counter - 1];
                        ProccessedList[i] = SinglPartString;
                        if (str.Length - counter == 0)
                        {
                            break;
                        }
                        counter++;
                    }
                if (str[counter - 1] == 93)
                {
                    SinglPartString = ProccessedList[i];
                    SinglPartString += str[counter - 1];
                    ProccessedList[i] = SinglPartString;
                    counter++;
                }
                // If the character is "<"-------
                if (str[counter - 1] == 60)
                    //Keep in while until u meen ">"
                    while (str[counter - 1] != 62)
                    {
                        SinglPartString = ProccessedList[i];
                        SinglPartString += str[counter - 1];
                        ProccessedList[i] = SinglPartString;
                        if (str.Length - counter == 0)
                        {
                            break;
                        }
                        counter++;
                    }
                if (str[counter - 1] == 62)
                {
                    SinglPartString = ProccessedList[i];
                    SinglPartString += str[counter - 1];
                    ProccessedList[i] = SinglPartString;
                    counter++;
                }
                // If the character is "'"-------
                if (str[counter - 1] == 39)
                    //Keep in while until u meet "the second '"
                    do
                    {
                        SinglPartString = ProccessedList[i];
                        SinglPartString += str[counter - 1];
                        ProccessedList[i] = SinglPartString;
                        if (str.Length - counter == 0)
                        {
                            break;
                        }
                        counter++;
                    }
                    while (str[counter - 1] != 39);
                if (str[counter - 1] == 39)
                {
                    SinglPartString = ProccessedList[i];
                    SinglPartString += str[counter - 1];
                    ProccessedList[i] = SinglPartString;
                    counter++;
                }

                #endregion
                i++;
                if (str[counter - 1] == 59)
                    break;
                if (str.Length - counter != 0) //count is standing on a letter take it 
                    //ProccessedList[i] += str[counter-1] + "" + str[counter]; Delete this
                    ProccessedList.Add(str[counter - 1] + "" + str[counter]);
                if (str.Length - counter == 0)
                    break;
                if (str.Length - counter > 2)
                    counter += 2;
            }
            return ProccessedList;
            */


            //Based on the new logic  AB324|AR34
            List<string> ProcessedList = new List<string> { };
            string SinglePartString = "";
            foreach (var character in str)
            {
                if (character == 59)
                {
                    ProcessedList.Add(SinglePartString);
                    break;
                }
                else if (character == 124)
                {
                    ProcessedList.Add(SinglePartString);
                    SinglePartString = "";
                }
                else
                {
                    SinglePartString += character;
                }
            }
            return ProcessedList;

        }
        #endregion

        //------------------------------------------ Assign Value for each key__________________
        private static void ParameterDefiner(List<string> ProccessedList)
        {
            List<string> TempObjectContainer = new List<string> { };
            List<string> TempParameterContainer = new List<string> { };
            List<char> temp = new List<char> { };

            foreach (var item in ProccessedList)
            {
                if (item[2] == 60)  //Seperate Objects segments that starts with a <
                    TempObjectContainer.Add(item);
                else
                    TempParameterContainer.Add(item);
            }
            foreach (var item in TempParameterContainer)
            {
                temp = item.ToList();
                ParamDic[temp[0] + "" + temp[1]].Value = "";
                for (int i = 2; i < temp.Count; i++)
                {
                    //Assign The value in the stinrg to value in dictionary 
                    ParamDic[temp[0] + "" + temp[1]].Value += temp[i];
                }
            }
            foreach (var item in TempObjectContainer)
            {
                List<string> TempList = new List<string>() { };
                //here we have AA<Gg345Gi834>

                //first send all inside <> to split

                TempList = StringSplitter(item.Substring(3, (item.Count() - 4)));
                foreach (var InnerItem in TempList)
                {
                    temp = InnerItem.ToList();
                    ParamObjectRelatedDic[temp[0] + "" + temp[1]].Value = "";
                    for (int i = 2; i < temp.Count; i++)
                    {
                        //Assign The value in the stinrg to value in dictionary 
                        ParamObjectRelatedDic[temp[0] + "" + temp[1]].Value += temp[i];
                    }
                }
                var TempListForContainedParameter = new ObservableCollection<Parameter>() { };
                foreach (var ContainedParam in ParamObjectRelatedDic)
                {
                    if (ContainedParam.Value.Value != "")
                    {
                        TempListForContainedParameter.Add(ContainedParam.Value);
                    }
                }

                ObjectDic[item[0] + "" + item[1]].ContainedParams = "emptu";
                FinalListOfObjects.Add(ObjectDic[item[0] + "" + item[1]]);

            }
        }
        //------------------------------------------
    }
}
