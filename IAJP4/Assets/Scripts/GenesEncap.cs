using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class GenesEncap
{
    float[] genes;
    public Actor.typeofActor typeOfActor;

    public float[] Genes
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

    public GenesEncap(Actor.typeofActor type)
    {
        if(type == Actor.typeofActor.hunter)
            genes = new float[108];
        else genes = new float[60];
        this.typeOfActor = type;
        randomizeGene();
    }

    public GenesEncap(float[] genes)
    {
        this.genes = (float[])genes.Clone();
        this.typeOfActor = (genes.Length == 108 ? Actor.typeofActor.hunter : Actor.typeofActor.prey);
        randomizeGene();
    }

    private void randomizeGene()
    {
        System.Random r = new System.Random();
        int sizeOfGene = (typeOfActor == Actor.typeofActor.hunter ? 9 : 5);
        for (int i = 0; i < genes.Length / sizeOfGene; i++)
        {
            int choosenIndex = r.Next(sizeOfGene);
            for (int j = 0; j < sizeOfGene; j++)
            {
                genes[i * sizeOfGene + j] = (j == choosenIndex ? 0.9f : 0.1f / (sizeOfGene - 1));
            }
        }
    }
    public override string ToString()
    {
        string ret = "";
        for (int i = 0; i < genes.Length; i++)
            ret += genes[i] + ";";
        return ret;
    }

    public static string ByteArrayToString(byte[] ba)
    {
        string hex = BitConverter.ToString(ba);
        return hex.Replace("-", "");
    }

    public float[] getGeneFromState(List<int> state)
    {
        float[] probSum;
        int sizeOfGene = (typeOfActor == Actor.typeofActor.hunter ? 9 : 5);
        probSum = new float[sizeOfGene];
        for (int i = 0; i < probSum.Length; i++)
        {
            probSum[i] = 0.0f;
        }

        for (int i = 0; i < state.Count; i++)
        {
            float[] stateGene = new float[sizeOfGene];
            Array.Copy(genes, state[i] * sizeOfGene, stateGene, 0, sizeOfGene);
            probSum = probSum.Select((x, index) => x + stateGene[index]).ToArray();
        }
        probSum = probSum.Select(d => d / (float)state.Count).ToArray();
        return probSum;
    }
}

