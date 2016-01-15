using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using WorldDefinition;
using System.IO;
using System.Text;
using System.Linq;

public class Hunter : Actor
{
    public Hunter(int posX, int posY, World world)
        : base(posX, posY, Actor.typeofActor.hunter, world)
    {
        Actions = new List<int>();
        Actions.Add(0);
        Actions.Add(1);
        Actions.Add(2);
        Actions.Add(3);
        Actions.Add(4);
        Actions.Add(5);
        Actions.Add(6);
        Actions.Add(7);
        Actions.Add(8);
    }

    public Hunter(int posX, int posY, World world, GenesEncap genesFromPappi)
        : base(posX, posY, Actor.typeofActor.hunter, world, genesFromPappi)
    {
        Actions = new List<int>();
        Actions.Add(0);
        Actions.Add(1);
        Actions.Add(2);
        Actions.Add(3);
        Actions.Add(4);
        Actions.Add(5);
        Actions.Add(6);
        Actions.Add(7);
        Actions.Add(8);
    }

    public override void HandleCollision(typeOfCell typeCell)
    {
        if (typeCell == typeOfCell.prey)
        {
            Energy += 1000;
            if(world.isGeneticWorld)
                ((GeneticWorld)world).manager.summaryPrinter.NumberOfPreysEaten++;
            world.killPrey();
        }
        else if (typeCell == typeOfCell.hunter)
        {
            Debug.Log("[BUG]Two hunters ingame!!");
        }
        else if (typeCell == typeOfCell.normal)
        {
            //Debug.Log("Hunter in normal");
        }
        else if (typeCell == typeOfCell.obstacle)
        {
            Debug.Log("[BUG]Hunter Moving through an obstacle!!");
        }
        else if (typeCell == typeOfCell.plant)
        {
            if (world.isGeneticWorld) {
                ((GeneticWorld)world).manager.summaryPrinter.NumberOfPlantsEaten++;
                ((GeneticWorld)world).manager.summaryPrinter.NumberOfPlantsEatenByHunter++;
            }
            Energy += 50;
        }
        else if (typeCell == typeOfCell.trap)
        {
            if (world.isGeneticWorld)
            {
                ((GeneticWorld)world).manager.summaryPrinter.NumberOfDeathsByTrap++;
                ((GeneticWorld)world).manager.summaryPrinter.NumberOfHunterDeathsByTrap++;
            }
            Death();
        }
    }

    public override void SaveResults(int sampleId)
    {
        System.IO.File.WriteAllText("../hunter" + sampleId + ".txt", ActorGenes.ToString());
    }

    public override void LoadResults(int sampleId)
    {
        
        string input = System.IO.File.ReadAllText("../hunter" + sampleId + ".txt");
        int[] genes = new int[12];
        for (int i = 0; i < genes.Length - 1; i++)
        {
            genes[i] = int.Parse(""+input[i]);
        }
        ActorGenes = new GenesEncap(genes, Actions.Count);
    }
}
