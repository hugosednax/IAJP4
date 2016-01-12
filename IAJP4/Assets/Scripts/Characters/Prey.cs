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
            world.Hunter.Energy += 500;
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
            world.GetGameManager().summaryPrinter.NumberOfPlantsEaten++;
            world.GetGameManager().summaryPrinter.NumberOfPlantsEatenByPrey++;
            Energy += 60;
        }
        else if (typeCell == typeOfCell.trap)
        {
            world.GetGameManager().summaryPrinter.NumberOfDeathsByTrap++;
            world.GetGameManager().summaryPrinter.NumberOfPreyDeathsByTrap++;
            Death();
        }
    }

    public override void SaveResults(int sampleId)
    {
       System.IO.File.WriteAllText("../prey" + sampleId + ".txt", ActorGenes.ToString());
    }

    public override void LoadResults(int sampleId)
    {
        ActorGenes = new GenesEncap();
        string[] lines = System.IO.File.ReadAllLines("../prey" + sampleId + ".txt");
        for (int i = 0; i < lines.Length; i++)
        {
            string[] stateParts = lines[i].Split(':');
            byte[] state = GetBytes(stateParts[0]);

            string[] actionEvaluationParts = stateParts[1].Split(';');
            for (int j = 0; j < actionEvaluationParts.Length; j++)
            {
                string[] evaluation = actionEvaluationParts[j].Split(',');
                int actionId = Int32.Parse(evaluation[0]);
                float value = float.Parse(evaluation[1]);
                if (ActorGenes.Genes.ContainsKey(state))
                {
                    ActorGenes.Genes[state].Add(actionId, value);
                }
                else
                {
                    Dictionary<int, float> actionValues = new Dictionary<int, float>();
                    actionValues.Add(actionId, value);
                    ActorGenes.Add(state, actionValues);
                }
            }

        }
        
    }



}
