using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoadFlow
{
    public class Network
    {
        private List<Node> nodes;
        public List<Branch> Branches { get; set; }
        public List<Node> Nodes { get => nodes; set => nodes = value; }
        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = Nodes.FindAll(i => i.Parent == parent.Name);
            if (children.Count > 0)
            {
                return children;
            }
            else
            {
                return null;
            }
        }
        public Node getParent(Node Child)
        {
            Node parent = Nodes.Single(i => i.Name == Child.Name);
            return parent;
        }
        public List<string> Names()
        {
            List<string> list = new List<string>();
            foreach (Node n in nodes)
            {
                list.Add(n.Name);
            }
            return list;
        }
        public string NamesString()
        {
            if (Names().Count > 0)
            {
                return "'" + string.Join("','", Names()) + "'";
            }
            else
            { return ""; }
        }

        public List<Node> GetItemsToSource(Node startNode)
        {
            List<Node> items = new List<Node>();
            Node Child = Nodes.Single(i => i.Parent == startNode.Name);
            if (Child != null)
            {
                items.Add(Child);
            }
            while (Child.Parent != null)
            {
                Child = Nodes.Single(i => i.Parent == Child.Name);
                items.Add(Child);
            }
            return items;
        }

    }
}