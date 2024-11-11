using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoadFlow
{
    public class Node : Network
    {
        private string name;
        private double r;
        private double x;
        private string type;
        private string parent;
        private double pg;
        private double qg;
        private double pb;
        private double qb;
        private string geometry;
        private string id;
        private string feederId;
        private string feederName;

        public Node(Node copy)
        {
            this.name = copy.name;
            this.r = copy.r;
            this.x = copy.x;
            this.type = copy.type;
            this.parent = copy.parent;
            this.pg = copy.pg;
            this.qg = copy.qg;
            this.pb = copy.pb;
            this.qb = copy.qb;
            this.geometry = copy.geometry;
            this.id = copy.id;
            this.feederId = copy.feederId;
            this.feederName = copy.feederName;
        }
        public Node()
        {


        }
        public Node(string name, double r, double x, string type, string parent)
        {
            this.name = name;
            this.r = r;
            this.x = x;
            this.type = type;
            this.parent = parent;
            this.pg = 0;
            this.qg = 0;
            this.pb = 0;
            this.qb = 0;
            this.geometry = "";
        }
        public Node(string name, double r, double x, string type, string parent, string id, string feederName, string feederId)
        {
            this.name = name;
            this.r = r;
            this.x = x;
            this.type = type;
            this.parent = parent;
            this.pg = 0;
            this.qg = 0;
            this.pb = 0;
            this.qb = 0;
            this.geometry = "";
            this.feederName = feederName;
            this.feederId = feederId;
            this.id = id;
        }
        public string Name { get => name; set => name = value; }
        public double R { get => r; set => r = value; }
        public double X { get => x; set => x = value; }
        public double ZAbs { get => Math.Sqrt(Math.Pow(r, 2) + Math.Pow(x, 2)); }
        public string Type { get => type; set => type = value; }
        public string Parent { get => parent; set => parent = value; }
        public double Pg { get => pg; set => pg = value; }
        public double Qg { get => qg; set => qg = value; }
        public double Pb { get => pb; set => pb = value; }
        public double Qb { get => qb; set => qb = value; }
        public string Geometry { get => geometry; set => geometry = value; }
        public string Id { get => id; set => id = value; }
        public string FeederID { get => feederId; set => feederId = value; }
        public string FeederName { get => feederName; set => feederName = value; }
    }
}