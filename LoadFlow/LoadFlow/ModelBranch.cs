using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LoadFlow
{
   public class ModelBranch :Branch
    {
        public System.Numerics.Complex Ivej { get; set; }
        public ModelBlock parentBlock { get; set; }
        public ModelBlock childBlock { get; set; }
        
        public ModelBranch (Branch b)
        {
            this.A = b.A;
            this.Endnode = b.Endnode;
            this.Geometry = b.Geometry;
            this.Id = b.Id;
            this.Izvodid = b.Izvodid;
            this.Izvodnaziv = b.Izvodnaziv;
            this.I_max = b.I_max;
            this.Length = b.Length;
            this.Material = b.Material;
            this.Name = b.Name;
            this.R = b.R;
            this.Startnode = b.Startnode;
            this.X = b.X;
            
               

        }
        
       public ModelBranch()
        {
            
        }
        public Complex Z
        { get { return new Complex(Length* R/1000,Length* X/1000); } }
        public double TechLoss15min
        {
            get { return 3*(Complex.Pow(Ivej,2)*Z).Real*15*60; }
        }
        public double RelOverCurrent
        {
            get
            {
                if(Complex.Abs(Ivej)<I_max)
                {
                    return 0;
                 }
                else
                {
                    return ((Complex.Abs(Ivej) - I_max) / I_max);
                }
            }
        }
    }
}
