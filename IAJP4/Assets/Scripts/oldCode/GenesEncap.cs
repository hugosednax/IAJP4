using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.oldCode
{
    public class GenesEncap
    {
        Dictionary<byte[], Dictionary<int, float>> genes;

        public Dictionary<byte[], Dictionary<int, float>> Genes
        {
            get { return genes; }

            set { genes = value; }
        }

        public bool ContainsKey(byte[] key)
        {
            return genes.ContainsKey(key);
        }

        public GenesEncap()
        {
            genes = new Dictionary<byte[], Dictionary<int, float>>(new BaComp());
        }

        public GenesEncap(Dictionary<byte[], Dictionary<int, float>> genes)
        {
            this.genes = genes;
        }

        public void Add(byte[] byteState, Dictionary<int, float> dic)
        {
            if (genes.ContainsKey(byteState)) genes[byteState] = dic;
            else genes.Add(byteState, dic);
        }

        public void Remove(byte[] byteState)
        {
            genes.Remove(byteState);
        }

        public override string ToString()
        {
            string ret = "";
            List<byte[]> keys = new List<byte[]>(Genes.Keys);
            List<KeyValuePair<int, float>> actions;
            for (int i = 0; i < keys.Count; i++)
            {
                byte[] geneState = keys[i];
                ret += ByteArrayToString(geneState) + ":";
                actions = Genes[geneState].ToList();
                for (int j = 0; j < actions.Count; j++)
                {
                    ret += "" + actions[j].Key + "," + actions[j].Value + ";";
                }
                ret += "\n";
            }
            return ret;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
    }

}