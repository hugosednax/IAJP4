﻿using System;
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
        float[] newGenes = new float[actor1.ActorGenes.Genes.Length];
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
        GenesEncap newActor = new GenesEncap(newGenes);
        return newActor;
    }


    public static void mutate(GenesEncap genes)
    {
        System.Random r = new System.Random();
        int sizeOfGene = genes.typeOfActor == Actor.typeofActor.hunter ? 9 : 5;
        for (int i = 0; i < genes.Genes.Length / sizeOfGene; i++)
        {
            if ((float)r.NextDouble() <= 0.015f)
            {
                int choosenIndex = r.Next(sizeOfGene);
                for (int j = 0; j < sizeOfGene; j++)
                {
                    genes.Genes[i * sizeOfGene + j] = (j == choosenIndex ? 0.9f : 0.1f / (sizeOfGene - 1));
                }
            }
        }
    }
}

