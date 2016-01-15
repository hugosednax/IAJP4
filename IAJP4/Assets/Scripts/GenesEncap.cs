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
        /*string gene = "Gene: ";
        for (int i = 0; i < genes.Length; i++)
        {
            gene += genes[i];
        }
        Debug.Log(gene);*/
    }

    public GenesEncap(int[] genes, int numActions)
    {
        this.genes = (int[])genes.Clone();
        this.numActions = numActions;
        //randomizeGene();
    }

    private void randomizeGene()
    {
        for (int i = 0; i < genes.Length; i++)
        {
            genes[i] = UnityEngine.Random.Range(0, numActions);
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

