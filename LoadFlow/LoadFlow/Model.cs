﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Web;

namespace LoadFlow
{
    public class Model
    {
        public List<ModelBlock> ModelBlocks;
        public List<ModelBranch> ModelBranches;
        public List< Feeder> Feeders;
        public double GridTechLosses { get { return Feeders.Sum(f => f.TechLoss); } }
        public double GridTechLossesPrice { get { return GridTechLosses * (236.76 / 1000000); } }
        private Network network;
        public Model(Network _network)
        {
            network = _network;
            ModelBlocks = new List<ModelBlock>();
            CreateBlockHierarchy(0, network.Nodes.Single(n => n.Type == "External Network"), new Complex(0, 0));
            CreateBranches();
            LoadFlowBFS();
            CalculateCurrents();
            CalculateLosses();
        }

        public void CreateBlockHierarchy(int level, Node parent, Complex Zshcparent)
        {
            if (level > 1000)
            {
                throw new Exception("Level to big on node: " + parent.Name);

            }
            else
            {
                if (ModelBlocks.Any(p => p.Name == parent.Name))
                {
                    throw new Exception("Zanka na: " + parent.Name);

                }
                else
                {
                    ModelBlock MB = new ModelBlock(level, parent);
                    MB.Zshc = Zshcparent + new Complex(parent.R, parent.X);
                    ModelBlocks.Add(MB);

                    level++;
                    List<Node> chidren = network.Nodes.FindAll(n => n.Parent == parent.Name);
                    foreach (Node node in chidren)
                    {
                        CreateBlockHierarchy(level, node, MB.Zshc);
                    }
                }
            }

        }
        public void CalculateCurrents()
        {
            foreach (ModelBranch mb in ModelBranches)
            {
                mb.Ivej = mb.childBlock.Ivej;
            }

        }
        public void CreateBranches()
        {
            ModelBranches = new List<ModelBranch>();
            Feeders = new List< Feeder>();
            foreach (Branch br in network.Branches)
            {
                List<ModelBlock> Blocks = ModelBlocks.FindAll(i => (i.Name == br.Endnode) && (i.Parent == br.Startnode));
                if (Blocks.Count > 0)
                {
                    if (Blocks.Count == 1)
                    {
                        ModelBranch mbr = new ModelBranch(br);
                        mbr.childBlock = Blocks[0];
                        Blocks[0].Branches.Add(mbr);
                        mbr.parentBlock = Blocks.First(i => i.Name == br.Endnode);
                        ModelBranches.Add(mbr);


                    }
                }
                else if (Blocks.Count == 0)
                {
                    Blocks = ModelBlocks.FindAll(i => (i.Name == br.Startnode) && (i.Parent == br.Endnode));
                    if (Blocks.Count > 0)
                    {
                        if (Blocks.Count == 1)
                        {
                            ModelBranch mbr = new ModelBranch(br);
                            mbr.childBlock = Blocks[0];
                            Blocks[0].Branches.Add(mbr);
                            mbr.parentBlock = Blocks.First(i => i.Name == br.Startnode);
                            ModelBranches.Add(mbr);


                        }
                    }
                    // throw new Exception("test");
                }

                if (Feeders.FindAll(i=>i.FeederID==br.Izvodid).Count==0)
                {
                    Feeders.Add( new Feeder(br.Izvodid, br.Izvodnaziv));
                }
            }

        }
        private void CalculateLosses()
        {
            foreach(Feeder f in Feeders)
            {
                List<ModelBranch> mb=ModelBranches.FindAll(i => i.Izvodid == f.FeederID);
                if(mb.Count>0)
                {
                    f.TechLoss = mb.Sum(s=>s.TechLoss15min);
                    f.Pb = mb.Sum(s => s.childBlock.Pb);
                    f.Pg = mb.Sum(s => s.childBlock.Pg);
                    f.MaxRelOverCurrent = mb.Max(s => s.RelOverCurrent);
                    f.FeederSumLenghth = mb.Sum(s => s.Length);
                }
            }
            
        }
        public void LoadFlowBFS()

        {

            ///PB and PG are in KW so SBAZ is 1000 for normalisation
            double Ubil = 20.0e3 / Math.Sqrt(3);
            double UbazSN = 20000 / Math.Sqrt(3);
            double UbazNN = 400 / Math.Sqrt(3);
            double Sbaz = 1000;
            double SbazMaxPower = ModelBlocks.Max(m => m.Pb) ;
            if(SbazMaxPower==0)
            {
                SbazMaxPower = 1;
            }
            double ZbazSN = Math.Pow(UbazSN, 2) / Sbaz;
            double ZbazNN = Math.Pow(UbazNN, 2) / Sbaz;
            double IbazSN = Sbaz* SbazMaxPower / UbazSN;
            double IbazNN = Sbaz* SbazMaxPower / UbazNN;
            int maxlevel = Convert.ToInt32(ModelBlocks.Max(n => n.Level));
            foreach (ModelBlock mb in ModelBlocks)
            {

                //  mb.Sgen_pu = new Complex((mb.Pbg / 3) / Sbaz, 0) ;
                mb.Sgen_pu = new Complex(mb.Pg / 3/ SbazMaxPower, mb.Qg / 3);
                mb.Sload_pu = new Complex(mb.Pb / 3/SbazMaxPower, mb.Qb / 3);




                if (mb.Type == "External Network")
                {

                    mb.Zpu = new System.Numerics.Complex(mb.R / ZbazSN, mb.X / ZbazSN);
                    mb.Uvozpu = Ubil / UbazSN;
                }
                else
                {
                    mb.Zpu = new System.Numerics.Complex(mb.R / ZbazNN, mb.X / ZbazNN);
                    mb.Uvozpu = UbazNN / UbazNN;
                }
            }
            int k = 0;
            double eps = 1e-6;
            double delta = 1;

            while ((delta > eps) )
            {
                
                k = k + 1;
                if(k>100)
                {
                    throw new Exception("Ne konvergira");
                }
                //Injicirani tokovi
                foreach (ModelBlock mb in ModelBlocks)
                {
                    mb.Utemp = mb.Uvozpu;
                    mb.Ivozpu = Complex.Conjugate(mb.Snode_pu / mb.Utemp);
                    mb.Ivejpu = 0;
                }

                ///
                for (int l = maxlevel; l > -1; l--)
                {
                    foreach (ModelBlock mb in ModelBlocks.FindAll(lev => lev.Level == l))
                    {
                        mb.Ivejpu += mb.Ivozpu;
                        if (l > 0)
                        {
                            ModelBlocks.Find(par => par.Name == mb.Parent).Ivejpu += mb.Ivejpu;
                        }


                    }
                }
                for (int l = 0; l <= maxlevel; l++)
                {
                    foreach (ModelBlock mb in ModelBlocks.FindAll(lev => lev.Level == l))
                    {
                        if (l == 0)
                        {
                            mb.Uvozpu = Ubil / UbazSN - mb.Zpu * mb.Ivejpu;

                        }
                        else
                        {
                            mb.Uvozpu = ModelBlocks.Find(par => par.Name == mb.Parent).Uvozpu - mb.Zpu * mb.Ivejpu;

                        }


                        mb.DeltaUpu = mb.Uvozpu - mb.Utemp;
                    }




                }
                delta = ModelBlocks.Max(del => del.DeltaUpAbs);




            }
            foreach (ModelBlock mb in ModelBlocks)
            {
                mb.Ivej = mb.Ivejpu * IbazNN;
            }
        }



    }
}