using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class GeneticUtility
{

    private static Dictionary<byte[], Dictionary<Action, float>> Crossover(Actor actor1, Actor actor2)
    {
        Dictionary<byte[], Dictionary<Action, float>> newActor = new Dictionary<byte[], Dictionary<Action, float>>();
        // Loop through genes
        System.Random r = new System.Random();
        foreach (KeyValuePair<byte[], Dictionary<Action, float>> gene in actor1.Genes)
        {
            if (actor2.Genes.ContainsKey(gene.Key))
            {
                if ((float)r.NextDouble() <= 0.5f)
                {
                    newActor.Add(gene.Key, actor1.Genes[gene.Key]);
                }
                else
                {
                    newActor.Add(gene.Key, actor2.Genes[gene.Key]);
                }
            } else
            {
                newActor.Add(gene.Key, gene.Value);
            }
        }

        foreach (KeyValuePair<byte[], Dictionary<Action, float>> gene in actor2.Genes)
        {
            if (!actor2.Genes.ContainsKey(gene.Key))
            {
                newActor.Add(gene.Key, gene.Value);
            }
        }

        return newActor;
    }

    private static void mutate(Dictionary<byte[], Dictionary<Action, float>> genes)
    {
        System.Random r = new System.Random();
        foreach (KeyValuePair<byte[], Dictionary<Action, float>> gene in genes)
        {
            if ((float)r.NextDouble() <= 0.015f)
            {
                    Dictionary<Action, float> newGene = new Dictionary<Action, float>();
                    foreach (KeyValuePair<Action, float> actionValuePair in gene.Value)
                    {
                        genes[gene.Key][actionValuePair.Key] = (float)r.NextDouble();
                    }
            }
        }
    }

}

