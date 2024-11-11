using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ModelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\em3923\\source\\repos\\AnalizaNNOmrezja\\AnalizaNNO\\AnalizaNNO\\bin\\Debug\\LFNapake\\LFB4000018.json";
            string s = File.ReadAllText(path);
            LoadFlow.Network NNO = Newtonsoft.Json.JsonConvert.DeserializeObject<LoadFlow.Network>(s);
            LoadFlow.Model M = new LoadFlow.Model(NNO);
            LoadFlow.ModelBlock mb = M.ModelBlocks.Find(m => m.Name == "4744813");
        }
    }
}
