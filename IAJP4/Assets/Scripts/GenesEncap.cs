using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class GenesEncap
{
    Dictionary<byte[], Dictionary<Action, float>> genes;

    public Dictionary<byte[], Dictionary<Action, float>> Genes
    {
        get
        {
            return genes;
        }

        set
        {
            genes = value;
        }
    }

    public bool ContainsKey(byte[] key) { return genes.ContainsKey(key); }

    public GenesEncap()
    {
        genes = new Dictionary<byte[], Dictionary<Action, float>>(new BaComp());
    }

    public GenesEncap(Dictionary<byte[], Dictionary<Action, float>> genes)
    {
        this.genes = genes;
    }

    public void Add(byte[] byteState, Dictionary<Action, float> dic) {
        if (genes.ContainsKey(byteState)) genes[byteState] = dic;
        else genes.Add(byteState, dic);
    }

    public void Remove(byte[] byteState) { genes.Remove(byteState); }

    public override string ToString()
    {
        string ret = "";
        List<byte[]> keys = new List<byte[]>(Genes.Keys);
        List<KeyValuePair<Action, float>> actions;
        for (int i = 0; i < keys.Count; i++)
        {
            byte[] geneState = keys[i];
            ret += ByteArrayToString(geneState) + ":";
            actions = Genes[geneState].ToList();
            for (int j = 0; j < actions.Count; j++)
            {
                ret += "" + actions[j].Key.Id + "," + actions[j].Value + ";";
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

