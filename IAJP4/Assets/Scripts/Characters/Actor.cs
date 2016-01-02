﻿using UnityEngine;
using System.Collections.Generic;
using WorldDefinition;

public abstract class Actor {

    [SerializeField]protected int energy = 100;

    public int Energy {
        get { return energy; }
        set { energy = value; }
    }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public enum typeofActor { hunter, prey };
    public typeofActor type { private set; get; }
    public List<Action> Actions { protected set; get; }

    public Actor(int posX, int posY, typeofActor type )
    {
        this.PosX = posX;
        this.PosY = posY;
        this.type = type;
    }

    public void Death()
    {
        Energy = -999;
    }

    public abstract void MoveActor(int offsetX, int offsetY);

    public abstract void HandleCollision(typeOfCell typeOfCell);
}

