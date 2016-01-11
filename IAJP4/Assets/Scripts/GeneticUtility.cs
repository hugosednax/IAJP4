using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class GeneticUtility
{

    public static GenesEncap Crossover(Actor actor1, Actor actor2)
    {
        GenesEncap newActor = new GenesEncap();
        // Loop through genes
        System.Random r = new System.Random();
        foreach (KeyValuePair<byte[], Dictionary<Action, float>> gene in actor1.ActorGenes.Genes)
        {
            if (actor2.ActorGenes.ContainsKey(gene.Key))
            {
                if ((float)r.NextDouble() <= 0.5f)
                {
                    newActor.Add(gene.Key, actor1.ActorGenes.Genes[gene.Key]);
                }
                else
                {
                    newActor.Add(gene.Key, actor2.ActorGenes.Genes[gene.Key]);
                }
            } else
            {
                newActor.Add(gene.Key, gene.Value);
            }
        }

        foreach (KeyValuePair<byte[], Dictionary<Action, float>> gene in actor2.ActorGenes.Genes)
        {
            if (!actor2.ActorGenes.ContainsKey(gene.Key))
            {
                newActor.Add(gene.Key, gene.Value);
            }
        }

        return newActor;
    }

    public static void mutate(GenesEncap genes)
    {
        System.Random r = new System.Random();
        foreach (KeyValuePair<byte[], Dictionary<Action, float>> gene in genes.Genes)
        {
            if ((float)r.NextDouble() <= 0.015f)
            {
                    Dictionary<Action, float> newGene = new Dictionary<Action, float>();
                    foreach (KeyValuePair<Action, float> actionValuePair in gene.Value)
                    {
                        genes.Genes[gene.Key][actionValuePair.Key] = (float)r.NextDouble();
                    }
            }
        }
    }

}

