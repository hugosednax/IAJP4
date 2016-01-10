using UnityEngine;
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
    public enum state { enemyDown, enemyUp, enemyLeft, enemyRight,
                        trapDown, trapUp, trapLeft, trapRight,
                        plantDown, plantUp, plantLeft, plantRight
    };
    public typeofActor type { private set; get; }
    public Dictionary<byte[], Dictionary<Action, float>> Genes { get; set; }
    public List<Action> Actions { protected set; get; }

    private byte[] currentState; // enemyD, enemyU, enemyL, enemyR, trapD, trapU, trapL, trapR, plantD, plantU, plantL, plantR

    public Actor(int posX, int posY, typeofActor type )
    {
        this.PosX = posX;
        this.PosY = posY;
        this.type = type;
        Genes = new Dictionary<byte[], Dictionary<Action, float>>(new BaComp());
        //percisa de valores randomizados
        currentState = new byte[12];
    }

    public Actor(int posX, int posY, typeofActor type, Dictionary<byte[], Dictionary<Action, float>> genesFromPappi)
    {
        this.PosX = posX;
        this.PosY = posY;
        this.type = type;
        Genes = genesFromPappi;
        currentState = new byte[12];
    }

    public void Death()
    {
        Energy = -999;
    }

    public abstract void HandleCollision(typeOfCell typeOfCell);
}

