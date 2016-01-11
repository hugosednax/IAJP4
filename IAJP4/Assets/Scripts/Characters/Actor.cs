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
    public typeofActor type { private set; get; }
    public GenesEncap ActorGenes { get; set; }
    public List<Action> Actions { protected set; get; }

    protected List<Pair<byte[], int>> statesOfThisGame;
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
        statesOfThisGame = new List<Pair<byte[], int>>();
        //percisa de valores randomizados
    }

    public Actor(int posX, int posY, typeofActor type, World world, GenesEncap genesFromPappi)
    {
        this.PosX = posX;
        this.PosY = posY;
        this.type = type;
        ActorGenes = genesFromPappi;
        this.world = world;
        statesOfThisGame = new List<Pair<byte[], int>>();
    }

    public Dictionary<Action, float> getGeneFromState(byte[] state)
    {
        Dictionary<Action, float> gene;

        if (ActorGenes.Genes.ContainsKey(state))
        {
            gene = ActorGenes.Genes[state];
        }
        else {
            gene = new Dictionary<Action, float>();
            System.Random r = new System.Random();
            for (int i = 0; i < Actions.Count; i++)
            {
                gene.Add(Actions[i], (float)r.NextDouble());
            }
            ActorGenes.Add(state, gene);
        }
        return gene;
    }

    public void Death()
    {
        Energy = -999;
        world.EndGame();
    }

    public abstract void HandleCollision(typeOfCell typeOfCell);
    public abstract void SaveResults(bool hasWon);

    public void Turn()
    {
        bool isValid = false;
        //Actor.state state = chooseState(actor);
        byte[] state = world.getState(this);

        Dictionary<Action, float> gene = getGeneFromState(state);

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

