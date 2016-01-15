using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GenesEncap
{
    int[] genes;
    public int numActions;

    public int[] Genes
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

    public GenesEncap(int numActions)
    {
        genes = new int[12];
        this.numActions = numActions;
        randomizeGene();
    }

    public GenesEncap(int[] genes, int numActions)
    {
        this.genes = (int[])genes.Clone();
        this.numActions = numActions;
        randomizeGene();
    }

    private void randomizeGene()
    {
        System.Random r = new System.Random();
        for (int i = 0; i < genes.Length; i++)
        {
            genes[i] = r.Next(numActions);
        }
    }
    public override string ToString()
    {
        string ret = "";
        for (int i = 0; i < genes.Length; i++)
            ret += genes[i];
        return ret;
    }

    public static string ByteArrayToString(byte[] ba)
    {
        string hex = BitConverter.ToString(ba);
        return hex.Replace("-", "");
    }

    public List<int> getGeneFromState(List<int> state)
    {
        List<int> gene = new List<int>();
        for (int i = 0; i < state.Count; i++)
        {
            gene.Add(genes[state[i]]);
        }
        return gene;
    }
}

