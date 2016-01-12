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
            Energy += 500;
            world.GetGameManager().summaryPrinter.NumberOfPreysEaten++;
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
            world.GetGameManager().summaryPrinter.NumberOfPlantsEaten++;
            world.GetGameManager().summaryPrinter.NumberOfPlantsEatenByHunter++;
            Energy += 30;
        }
        else if (typeCell == typeOfCell.trap)
        {
            world.GetGameManager().summaryPrinter.NumberOfDeathsByTrap++;
            world.GetGameManager().summaryPrinter. NumberOfHunterDeathsByTrap++;
            Death();
        }
    }

    public override void SaveResults(int sampleId)
    {
        System.IO.File.WriteAllText("../hunter" + sampleId + ".txt", ActorGenes.ToString());
    }

    public override void LoadResults(int sampleId)
    {
        ActorGenes = new GenesEncap();
        string[] lines = System.IO.File.ReadAllLines("../hunter" + sampleId + ".txt");
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
