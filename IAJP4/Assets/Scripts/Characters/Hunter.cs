﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using WorldDefinition;

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

        Dictionary<Action, float> startGeneVal = new Dictionary<Action, float>();
        foreach(Action action in Actions)
        {
            startGeneVal.Add(action, 1.0f / Actions.Count);
        }
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

    public override void SaveResults(bool hasWon)
    {
        string toWrite = "";
        for (int i = 0; i < statesOfThisGame.Count; i++)
        {
            toWrite += System.Text.Encoding.UTF8.GetString(statesOfThisGame[i].First);
        }
        toWrite += "|" + (hasWon ? "w" : "l") + "\n";

        System.IO.File.AppendAllText("../hunter.txt", toWrite);
    }
}
