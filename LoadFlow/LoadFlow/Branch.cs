using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoadFlow
{
    public class Branch
    {
        private string id;
        private string name;
        private string startnode;
        private string endnode;
        private double r;
        private double x;
        private double length;
        private double a;
        private string material;
        private string geometry;
        private string izvodid;
        private string izvodnaziv;
        private double i_max;



        public Branch(string id, string name, string startnode, string endnode, double r, double x, double length, double a, string material, string geometry, string izvodid, string izvodnaziv, double i_max)
        {
            this.id = id;
            this.name = name;
            this.startnode = startnode;
            this.endnode = endnode;
            this.r = r;
            this.x = x;
            this.length = length;
            this.a = a;
            this.material = material;
            this.geometry = geometry;
            this.izvodid = izvodid;
            this.izvodnaziv = izvodnaziv;
            this.i_max = i_max;

        }
        public Branch()
        {

        }
        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }

        public double R { get => r; set => r = value; }
        public double X { get => x; set => x = value; }
        public double Length { get => length; set => length = value; }
        public double A { get => a; set => a = value; }
        public string Material { get => material; set => material = value; }
        public string Geometry { get => geometry; set => geometry = value; }
        public string Startnode { get => startnode; set => startnode = value; }
        public string Endnode { get => endnode; set => endnode = value; }
        public string Izvodid { get => izvodid; set => izvodid = value; }
        public string Izvodnaziv { get => izvodnaziv; set => izvodnaziv = value; }
        public double I_max { get => i_max; set => i_max = value; }
    }
}