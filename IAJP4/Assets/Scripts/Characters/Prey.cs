using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using WorldDefinition;

public class Prey : Actor {

    public Prey(int posX, int posY)
        : base(posX, posY, Actor.typeofActor.prey)
    {
        Actions = new List<Action>();
        Actions.Add(new MoveDown(this));
        Actions.Add(new MoveUp(this));
        Actions.Add(new MoveLeft(this));
        Actions.Add(new MoveRight(this));
        Actions.Add(new Rest(this));

        Dictionary<Action, float> startGeneVal = new Dictionary<Action, float>();
        foreach (Action action in Actions)
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
                Debug.Log("[BUG]Two preys ingame!!");
            }
            else if (typeCell == typeOfCell.hunter)
            {
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
                Energy += 3;
            }
            else if (typeCell == typeOfCell.trap)
            {
                Death();
            }
    }

}
