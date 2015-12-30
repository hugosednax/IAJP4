using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Actor {

    [SerializeField]protected int energy = 100;

    public int Energy { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public enum typeofActor { hunter, fugitive };
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
        energy = 0;
    }

}

