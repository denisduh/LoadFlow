using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Numerics;

namespace LoadFlow
{
    public class ModelBlock : Node
    {
        private double level;
        private Complex sgen_pu;
        private Complex sload_pu;

        private Complex ivozpu;
        private Complex zpu;
        private Complex uvozpu;
        private Complex utemp;
        private Complex ivejpu;
        private Complex deltaUpu;
        private Complex ivej;
        private Complex zshc;



        public ModelBlock(double level, Node a) : base(a)
        {
            this.level = level;
            this.Branches = new List<ModelBranch>();
        }
        public double Level { get => level; set => level = value; }
        public Complex Sgen_pu { get => sgen_pu; set => sgen_pu = value; }

        public Complex Ivozpu { get => ivozpu; set => ivozpu = value; }
        public Complex Zpu { get => zpu; set => zpu = value; }
        public Complex Sload_pu { get => sload_pu; set => sload_pu = value; }
        public Complex Snode_pu { get => sload_pu - sgen_pu; }
        public Complex Uvozpu { get => uvozpu; set => uvozpu = value; }
        public Complex Utemp { get => utemp; set => utemp = value; }
        public Complex Ivejpu { get => ivejpu; set => ivejpu = value; }
        public Complex DeltaUpu { get => deltaUpu; set => deltaUpu = value; }
        public double DeltaUpAbs
        {
            get => Complex.Abs(deltaUpu);

        }
        public Complex Ivej { get => ivej; set => ivej = value; }
        public Complex Zshc { get => zshc; set => zshc = value; }
        public double Ik3
        {
            get
            {
                return 400 / (Complex.Abs(Zshc * Math.Sqrt(3)));
            }
        }
        public double Sk3
        {
            get { return Ik3 * 3 * 400 / Math.Sqrt(3); }
        }

        public List<ModelBranch> Branches
        {
            get; set;
        }
    }
}