using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using WorldDefinition;
using System.IO;
using System.Text;

public class Prey : Actor {

    public Prey(int posX, int posY, World world)
        : base(posX, posY, Actor.typeofActor.prey, world)
    {
        Actions = new List<int>();
        Actions.Add(0);
        Actions.Add(1);
        Actions.Add(2);
        Actions.Add(3);
        Actions.Add(4);
    }

    public Prey(int posX, int posY, World world, GenesEncap genesFromPappi)
        : base(posX, posY, Actor.typeofActor.prey, world, genesFromPappi)
    {
        Actions = new List<int>();
        Actions.Add(0);
        Actions.Add(1);
        Actions.Add(2);
        Actions.Add(3);
        Actions.Add(4);
    }

    public override void HandleCollision(typeOfCell typeCell)
    {
        if (typeCell == typeOfCell.prey)
        {
            Debug.Log("[BUG]Two preys ingame!!");
        }
        else if (typeCell == typeOfCell.hunter)
        {
            world.Hunter.Energy += 1000;
            Death();
        }
        else if (typeCell == typeOfCell.normal)
        {
        }
        else if (typeCell == typeOfCell.obstacle)
        {
            Debug.Log("[BUG]Prey Moving through an obstacle!!");
        }
        else if (typeCell == typeOfCell.plant)
        {
            if (world.isGeneticWorld)
            {
                ((GeneticWorld)world).manager.summaryPrinter.NumberOfPlantsEaten++;
                ((GeneticWorld)world).manager.summaryPrinter.NumberOfPlantsEatenByPrey++;
            }
            Energy += 80;
        }
        else if (typeCell == typeOfCell.trap)
        {
            if(world.isGeneticWorld) {
                ((GeneticWorld)world).manager.summaryPrinter.NumberOfDeathsByTrap++;
                ((GeneticWorld)world).manager.summaryPrinter.NumberOfPreyDeathsByTrap++;
            }
            Death();
        }
    }

    public override void SaveResults(int sampleId)
    {
       System.IO.File.WriteAllText("../prey" + sampleId + ".txt", ActorGenes.ToString());
    }

    public override void LoadResults(int sampleId)
    {
        string input = System.IO.File.ReadAllText("../prey" + sampleId + ".txt");
        int[] genes = new int[12];
        for (int i = 0; i < genes.Length - 1; i++)
        {
            genes[i] = int.Parse("" + input[i]);
        }
        ActorGenes = new GenesEncap(genes, Actions.Count);
    }
}
