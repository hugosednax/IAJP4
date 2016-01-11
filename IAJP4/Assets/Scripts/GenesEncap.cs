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
        genes = new Dictionary<byte[], Dictionary<Action, float>>();
    }

    public GenesEncap(Dictionary<byte[], Dictionary<Action, float>> genes)
    {
        this.genes = genes;
    }

    public void Add(byte[] byteState, Dictionary<Action, float> dic) { genes.Add(byteState, dic); }

    public void Remove(byte[] byteState) { genes.Remove(byteState); }
}

