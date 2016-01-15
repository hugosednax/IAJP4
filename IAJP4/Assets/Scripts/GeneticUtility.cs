using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


static class GeneticUtility
{

    public static GenesEncap Crossover(Actor actor1, Actor actor2)
    {
        
        // Loop through genes
        System.Random r = new System.Random();
        int[] newGenes = new int[actor1.ActorGenes.Genes.Length];
        Array.Clear(newGenes, 0, newGenes.Length);
        for (int i = 0; i < actor1.ActorGenes.Genes.Length; i++)
        {
            if ((float)r.NextDouble() <= 0.5f)
            {
                newGenes[i] = actor1.ActorGenes.Genes[i];
            }
            else
            {
                newGenes[i] = actor2.ActorGenes.Genes[i];
            }
        }
        GenesEncap newActor = new GenesEncap(newGenes, actor1.Actions.Count);
        Debug.Log("Crossover: " + geneToStr(actor1.ActorGenes) + " with " + geneToStr(actor2.ActorGenes) + " = " + geneToStr(newActor));
        return newActor;
    }

    public static string geneToStr(GenesEncap genes)
    {
        string genestr = "";
        for (int i = 0; i < genes.Genes.Length; i++)
        {
            genestr += genes.Genes[i] + " ; ";
        }
        return genestr;
    }

    public static void mutate(GenesEncap genes)
    {
        System.Random r = new System.Random();
        for (int i = 0; i < genes.Genes.Length; i++)
        {
            if ((float)r.NextDouble() <= 0.1f)
            {
                genes.Genes[i] = r.Next(genes.numActions);
            }
        }
    }
}

