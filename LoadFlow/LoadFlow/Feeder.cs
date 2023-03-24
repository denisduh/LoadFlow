using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadFlow
{
    public class Feeder
    {
        public string FeederID {get;set;}
        public string FeederName { get; set; }
        public double TechLoss { get; set; }
        public double FeederSumLenghth { get; set; }
        public double FeederRelativeTechLoss { get { return TechLoss / FeederSumLenghth; } }
        public Feeder()
        {

        }

        public Feeder(string feederID, string feederName)
        {
            FeederID = feederID;
            FeederName = feederName;
            TechLoss = 0;
        }
    }
}
