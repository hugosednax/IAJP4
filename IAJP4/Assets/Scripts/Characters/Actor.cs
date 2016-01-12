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
        ActorGenes = new GenesEncap();
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

    public Dictionary<int, float> getGeneFromState(byte[] state)
    {
        Dictionary<int, float> gene;

        if (ActorGenes.Genes.ContainsKey(state))
        {
            gene = ActorGenes.Genes[state];
        }
        else
        {
            gene = new Dictionary<int, float>();
            System.Random r = new System.Random();
            float cumProb = 0.0f;
            for (int i = 0; i < Actions.Count; i++)
            {

                float newProb = (float)r.NextDouble();
                if (newProb <= 0.1)
                    newProb = (float)r.NextDouble() * 5.0f;
                else newProb = (float)r.NextDouble();
                cumProb += newProb;
                gene.Add(Actions[i], newProb);
            }
            
            for (int i = 0; i < Actions.Count; i++)
            {
                gene[Actions[i]] = gene[Actions[i]] / cumProb;
            }
            
            ActorGenes.Add(state, gene);
        }
        return gene;
    }

    public void Death()
    {
        if (type == typeofActor.hunter)
            world.GetGameManager().summaryPrinter.NumberOfWinsByPrey++;
        if (type == typeofActor.prey)
            world.GetGameManager().summaryPrinter.NumberOfWinsByHunter++;
        Energy = -999;
        world.EndGame();
    }

    public abstract void HandleCollision(typeOfCell typeOfCell);
    public abstract void SaveResults(int sampleId);
    public abstract void LoadResults(int sampleId);

    public void Turn()
    {
        bool isValid = false;
        //Actor.state state = chooseState(actor);
        byte[] state = world.getState(this);

        Dictionary<int, float> gene = getGeneFromState(state);

        System.Random r = new System.Random();

        while (!isValid)
        {
            //Debug.Log("whilevalid");
            float diceRoll = (float)r.NextDouble();
            float cumulative = 0.0f;
            for (int i = 0; i < Actions.Count; i++)
            {
                if (gene.ContainsKey(Actions[i]))
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

