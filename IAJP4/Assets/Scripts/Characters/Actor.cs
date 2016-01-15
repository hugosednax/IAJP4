using UnityEngine;
using System.Collections.Generic;
using WorldDefinition;
using System;

public abstract class Actor {

    [SerializeField]protected int energy = 100;
    protected World world;
    public int Energy {
        get { return energy; }
        set { energy = value; }
    }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public enum typeofActor { hunter, prey };
    public typeofActor type { private set; get; }
    public GenesEncap ActorGenes { get; set; }
    public List<int> Actions { protected set; get; }

    public const int SPRINT_LENGTH = 2;

    /*
        enemyDown, enemyUp, enemyLeft, enemyRight,
        trapDown, trapUp, trapLeft, trapRight,
        plantDown, plantUp, plantLeft, plantRight
    */

    public Actor(int posX, int posY, typeofActor type, World world)
    {
        this.PosX = posX;
        this.PosY = posY;
        this.type = type;
        ActorGenes = new GenesEncap(this.type);
        this.world = world;
        //percisa de valores randomizados
    }

    public Actor(int posX, int posY, typeofActor type, World world, GenesEncap genesFromPappi)
    {
        this.PosX = posX;
        this.PosY = posY;
        this.type = type;
        ActorGenes = genesFromPappi;
        this.world = world;
    }

    public void Death()
    {
        if (world.isGeneticWorld)
        {
            if (type == typeofActor.hunter)
                ((GeneticWorld)world).manager.summaryPrinter.NumberOfWinsByPrey++;
            if (type == typeofActor.prey)
                ((GeneticWorld)world).manager.summaryPrinter.NumberOfWinsByHunter++;
        }
        Energy -= 999;
    }

    public abstract void HandleCollision(typeOfCell typeOfCell);
    public abstract void SaveResults(int sampleId);
    public abstract void LoadResults(int sampleId);

    public void Turn()
    {
        bool isValid = false;
        //Actor.state state = chooseState(actor);
        List<int> state = world.getState(this);

        float[] gene = ActorGenes.getGeneFromState(state);

        System.Random r = new System.Random();

        while (!isValid)
        {
            //Debug.Log("whilevalid");
            float diceRoll = (float)r.NextDouble();
            float cumulative = 0.0f;
            for (int i = 0; i < Actions.Count; i++)
            {
                cumulative += gene[Actions[i]];
                if (diceRoll < cumulative)
                {
                    isValid = ActionManager.CanExecute(Actions[i], this, world);
                    if (isValid)
                    {
                        //Debug.Log(action.ToString());
                        ActionManager.Execute(Actions[i], this, world);
                        break;
                    }
                }
            }
        }
    }

    public static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }
}

