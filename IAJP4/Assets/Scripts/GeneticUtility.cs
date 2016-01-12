using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


static class GeneticUtility
{

    public static GenesEncap Crossover(Actor actor1, Actor actor2)
    {
        GenesEncap newActor = new GenesEncap();
        // Loop through genes
        System.Random r = new System.Random();
        foreach (KeyValuePair<byte[], Dictionary<int, float>> gene in actor1.ActorGenes.Genes)
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

        foreach (KeyValuePair<byte[], Dictionary<int, float>> gene in actor2.ActorGenes.Genes)
        {
            if (!actor1.ActorGenes.ContainsKey(gene.Key))
            {
                newActor.Add(gene.Key, gene.Value);
            }
        }

        return newActor;
    }


    public static void mutate(GenesEncap genes)
    {
        System.Random r = new System.Random();
        GenesEncap modified = new GenesEncap();

        foreach (KeyValuePair<byte[], Dictionary<int, float>> gene in genes.Genes)
        {
            if ((float)r.NextDouble() <= 0.015f)
            {
                Dictionary<int, float> newGene = new Dictionary<int, float>();
                float cumProb = 0;
                foreach (KeyValuePair<int, float> actionValuePair in gene.Value)
                {
                    float newProb = (float)r.NextDouble();
                    if (newProb <= 0.1)
                        newProb = (float)r.NextDouble() * 5.0f;
                    else newProb = (float)r.NextDouble();
                    cumProb += newProb;
                    newGene.Add(actionValuePair.Key, newProb);
                }
                foreach (KeyValuePair<int, float> actionValuePair in gene.Value)
                {
                    newGene[actionValuePair.Key] = newGene[actionValuePair.Key] / cumProb;
                }

                modified.Add(gene.Key, newGene);
            }
        }

        List<byte[]> keyOfModified = modified.Genes.Keys.ToList<byte[]>();
        for (int i = 0; i < keyOfModified.Count; i++)
        {
            genes.Genes[keyOfModified[i]] = modified.Genes[keyOfModified[i]];

        }
    }





}

