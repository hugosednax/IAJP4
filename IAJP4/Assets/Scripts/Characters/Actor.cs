using UnityEngine;
using System.Collections.Generic;
using WorldDefinition;

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
    public enum state { enemyDown, enemyUp, enemyLeft, enemyRight,
                        trapDown, trapUp, trapLeft, trapRight,
                        plantDown, plantUp, plantLeft, plantRight
    };
    public typeofActor type { private set; get; }
    public Dictionary<byte[], Dictionary<Action, float>> Genes { get; set; }
    public List<Action> Actions { protected set; get; }

    protected List<Pair<byte[], int>> statesOfThisGame;

    public Actor(int posX, int posY, typeofActor type, World world)
    {
        this.PosX = posX;
        this.PosY = posY;
        this.type = type;
        Genes = new Dictionary<byte[], Dictionary<Action, float>>(new BaComp());
        this.world = world;
        statesOfThisGame = new List<Pair<byte[], int>>();
        //percisa de valores randomizados
    }

    public Actor(int posX, int posY, typeofActor type, Dictionary<byte[], Dictionary<Action, float>> genesFromPappi)
    {
        this.PosX = posX;
        this.PosY = posY;
        this.type = type;
        Genes = genesFromPappi;
    }

    public void Death()
    {
        Energy = -999;
    }

    public abstract void HandleCollision(typeOfCell typeOfCell);
    public abstract void SaveResults(bool hasWon);

    public void Turn()
    {
        bool isValid = false;
        //Actor.state state = chooseState(actor);
        byte[] state = world.getState(this);

        if (!Genes.ContainsKey(state))
        {
            Genes.Add(state, new Dictionary<Action, float>());
        }
        Dictionary<Action, float> gene = Genes[state];

        System.Random r = new System.Random();

        while (!isValid)
        {
            float diceRoll = (float)r.NextDouble();
            float cumulative = 0.0f;
            foreach (Action action in Actions)
            {
                if(gene.ContainsKey(action))
                    cumulative += gene[action];
                if (diceRoll < cumulative)
                {
                    isValid = action.CanExecute(world);
                    if (isValid)
                    {
                        statesOfThisGame.Add(new Pair<byte[], int>(state, action.Id));
                        action.Execute(world); 
                        break;
                    }
                }
            }
        }
    }
}

