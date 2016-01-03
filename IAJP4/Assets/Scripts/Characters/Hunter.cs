using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using WorldDefinition;

public class Hunter : Actor
{
    World world;
    public Hunter(int posX, int posY)
        : base(posX, posY, Actor.typeofActor.hunter)
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

        Dictionary<Action, float> startGeneVal = new Dictionary<Action, float>();
        foreach(Action action in Actions)
        {
            startGeneVal.Add(action, 1.0f / Actions.Count);
        }

        Genes = new Dictionary<state, Dictionary<Action, float>>();
        Genes.Add(state.nextToTrap, startGeneVal);
        Genes.Add(state.nextToPlant, startGeneVal);
        Genes.Add(state.nextToActor, startGeneVal);
        Genes.Add(state.nextToObstacle, startGeneVal);
        Genes.Add(state.emptySpace, startGeneVal);
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

}
