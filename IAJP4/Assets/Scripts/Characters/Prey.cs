using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using WorldDefinition;

public class Prey : Actor {

    public Prey(int posX, int posY, World world)
        : base(posX, posY, Actor.typeofActor.prey, world)
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
    public override void SaveResults(bool hasWon)
    {
        string toWrite = "";
        for (int i = 0; i < statesOfThisGame.Count; i++)
        {
            toWrite += System.Text.Encoding.UTF8.GetString(statesOfThisGame[i].First);
        }
        toWrite += "|" + (hasWon ? "w" : "l") + "\n";

        System.IO.File.AppendAllText("../prey.txt", toWrite);
    }
}
