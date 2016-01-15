using UnityEngine;
using System.Collections.Generic;
using WorldDefinition;
using System;
using System.Linq;

public abstract class Actor {

    [SerializeField]protected int energy = 300;
    public World world;
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
    public int plantsCollected = 0;
    public int collisionWithEnemy = 0;

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
        ActorGenes = new GenesEncap((type == typeofActor.hunter ? 9 : 5));
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
        List<int> geneSeed = ActorGenes.getGeneFromState(state);
        float[] gene = generateProbabilities(geneSeed);
        /*string geneStr = "";
        for (int i = 0; i < gene.Length; i++)
        {
            geneStr += gene[i] + " ; ";
        }
        Debug.Log(geneStr);*/
        int counter = 0;
        while (!isValid && counter < 4)
        {
            counter++;
            float diceRoll = UnityEngine.Random.Range(0.0f, 1.0f);
            float cumulative = 0.0f;
            for (int i = 0; i < Actions.Count; i++)
            {
                cumulative += gene[Actions[i]];
                if (diceRoll < cumulative)
                {
                    isValid = ActionManager.CanExecute(Actions[i], this, world);
                    if (isValid)
                    {
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

    public float[] generateProbabilities(List<int> gene)
    {
        float[] finalProbabilities = new float[Actions.Count];
        Array.Clear(finalProbabilities, 0, finalProbabilities.Length);
        if (gene.Count == 0)
        {
            for (int i = 0; i < finalProbabilities.Length; i++)
            {
                finalProbabilities[i] = 1.0f / (float)finalProbabilities.Length;
            }
            return finalProbabilities;
        }
        for (int i = 0; i < gene.Count; i++)
        {
            float[] actionsEvaluation = new float[Actions.Count];
            actionsEvaluation = generateProbabilities(gene[i]);
            finalProbabilities = finalProbabilities.Select((x, index) => x + actionsEvaluation[index]).ToArray();
        }
        finalProbabilities = finalProbabilities.Select(d => d / (float)gene.Count).ToArray();
        return finalProbabilities;
    }

    public float[] generateProbabilities(int action)
    {
        float[] probs = new float[Actions.Count];
        for (int i = 0; i < probs.Length; i++)
        {
            probs[i] = (i == action ? 0.9f : 0.1f / (Actions.Count - 1));
        }
        return probs;
    }
}

