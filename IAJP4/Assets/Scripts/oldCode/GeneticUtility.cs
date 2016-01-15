using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.oldCode
{
    static class GeneticUtility
    {

        public static GenesEncap Crossover(Actor actor1, Actor actor2)
        {
            GenesEncap newActor = new GenesEncap();
            // Loop through genes
            System.Random r = new System.Random();
            /*
            foreach (KeyValuePair<byte[], Dictionary<int, float>> gene in actor1.ActorGenes.Genes)
            {
                if (actor2.ActorGenes.ContainsKey(gene.Key))
                {
                    if ((float) r.NextDouble() <= 0.5f)
                    {
                        newActor.Add(gene.Key, actor1.ActorGenes.Genes[gene.Key]);
                    }
                    else
                    {
                        newActor.Add(gene.Key, actor2.ActorGenes.Genes[gene.Key]);
                    }
                }
                else
                {
                    newActor.Add(gene.Key, gene.Value);
                }
            }
            */

            /*
            foreach (KeyValuePair<byte[], Dictionary<int, float>> gene in actor2.ActorGenes.Genes)
            {
                if (!actor1.ActorGenes.ContainsKey(gene.Key))
                {
                    newActor.Add(gene.Key, gene.Value);
                }
            }
            */
            return newActor;
        }


        public static void mutate(GenesEncap genes)
        {
            System.Random r = new System.Random();
            GenesEncap modified = new GenesEncap();

            foreach (KeyValuePair<byte[], Dictionary<int, float>> gene in genes.Genes)
            {
                if ((float) r.NextDouble() <= 0.015f)
                {
                    Dictionary<int, float> newGene = new Dictionary<int, float>();
                    int choosenIndex = r.Next(gene.Value.Count);
                    for (int i = 0; i < gene.Value.Count; i++)
                    {
                        newGene.Add(i, (i == choosenIndex ? 0.9f : 0.1f/(gene.Value.Count - 1)));
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

}