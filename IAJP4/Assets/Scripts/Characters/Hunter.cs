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
        Actions = new List<Action>();
        Actions.Add(new MoveDown(this));
        Actions.Add(new MoveUp(this));
        Actions.Add(new MoveLeft(this));
        Actions.Add(new MoveRight(this));
        Actions.Add(new Rest(this));
        Actions.Add(new SprintDown(this));
        Actions.Add(new SprintUp(this));
        Actions.Add(new SprintLeft(this));
        Actions.Add(new SprintRight(this));

    }

    public Hunter(int posX, int posY, World world, GenesEncap genesFromPappi)
        : base(posX, posY, Actor.typeofActor.hunter, world, genesFromPappi)
    {
        Actions = new List<Action>();
        Actions.Add(new MoveDown(this));
        Actions.Add(new MoveUp(this));
        Actions.Add(new MoveLeft(this));
        Actions.Add(new MoveRight(this));
        Actions.Add(new Rest(this));
        Actions.Add(new SprintDown(this));
        Actions.Add(new SprintUp(this));
        Actions.Add(new SprintLeft(this));
        Actions.Add(new SprintRight(this));

    }

    public override void HandleCollision(typeOfCell typeCell)
    {
        if (typeCell == typeOfCell.prey)
        {
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
            Energy += 1;
        }
        else if (typeCell == typeOfCell.trap)
        {
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
                    ActorGenes.Genes[state].Add(Action.GetAction(actionId, this), value);
                }
                else
                {
                    Dictionary<Action, float> actionValues = new Dictionary<Action, float>();
                    actionValues.Add(Action.GetAction(actionId, this), value);
                    ActorGenes.Add(state, actionValues);
                }
            }

        }
    }
}
