using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using WorldDefinition;

namespace WorldDefinition
{
    public enum typeOfCell { obstacle, normal, trap, plant, hunter, prey };
}

public class World : MonoBehaviour
{

    [SerializeField]
    private int sizeX = 15;
    [SerializeField]
    private int sizeY = 15;
    [SerializeField]
    private float spacing = 4;
    [SerializeField]
    private bool debug = true;

    public const int SPRINT_LENGTH = 2;
    public int numberOfTraps = 5;
    public int numberOfPlants = 3;
    List<typeOfCell> world;
    Hunter hunter;
    Prey prey;
    int turn = 0;

    Text hunterEnergy;
    Text preyEnergy;
    float elapsedTime = 0f;

    // Use this for initialization
    void Start()
    {
        hunterEnergy = GameObject.Find("HunterEnergy").GetComponent<Text>();
        preyEnergy = GameObject.Find("PreyEnergy").GetComponent<Text>();
        world = new List<typeOfCell>();
        for (int i = 0; i < sizeX * sizeY; i++)
        {
            world.Add(typeOfCell.normal);
        }
        Debug.Log("World script is attached to plane, changing sizes");
        spacing = this.GetComponent<Renderer>().bounds.size.x / (float)sizeX;
        Debug.Log("Spacing is now: " + spacing);

        PopulateObstacles(1, 1);
        PopulateTraps(numberOfTraps);
        PopulatePlants(numberOfPlants);

        int hunterX = 0;
        int hunterY = 0;

        int fugitiveX = 0;
        int fugitiveY = 0;

        bool notPlaceable = (hunterX == fugitiveX && hunterY == fugitiveY) ||
            GetTypeOfCell(hunterX, hunterY) != typeOfCell.normal ||
            GetTypeOfCell(fugitiveX, fugitiveY) != typeOfCell.normal;
        while (notPlaceable)
        {
            hunterX = Random.Range(0, sizeX - 1);
            hunterY = Random.Range(0, sizeY - 1);
            fugitiveX = Random.Range(0, sizeX - 1);
            fugitiveY = Random.Range(0, sizeY - 1);
            notPlaceable = (hunterX == fugitiveX && hunterY == fugitiveY) ||
                GetTypeOfCell(hunterX, hunterY) != typeOfCell.normal ||
                GetTypeOfCell(fugitiveX, fugitiveY) != typeOfCell.normal;
        }
        hunter = new Hunter(hunterX, hunterY);
        SetTypeOfCell(hunterX, hunterY, typeOfCell.hunter);
        prey = new Prey(fugitiveX, fugitiveY);
        SetTypeOfCell(fugitiveX, fugitiveY, typeOfCell.prey);
    }

    public typeOfCell GetTypeOfCell(int i, int j)
    {
        if (i < 0 || j < 0 || i > sizeX - 1 || j > sizeY - 1)
            return typeOfCell.obstacle;
        return world[sizeX * i + j];
    }

    public typeOfCell GetTypeOfCell(int i)
    {
        return world[i];
    }

    void SetTypeOfCell(int i, int j, typeOfCell newType)
    {
        world[sizeX * i + j] = newType;
    }

    void SetTypeOfCell(int i, typeOfCell newType)
    {
        world[i] = newType;
    }

    Vector3 CellToWorld(int i, int j)
    {
        return new Vector3(i * spacing, j * spacing, -10) - new Vector3(sizeX / 2, sizeY / 2, 0);
    }

    Vector3 CellToWorld(int i)
    {
        return new Vector3(i / sizeX * spacing, i % sizeX * spacing, -10) - new Vector3(sizeX / 2, sizeY / 2, 0);
    }

    int WorldToCell(Vector3 location)
    {
        return 0; //not workerino
    }

    Vector2 WorldToCellMatrixIndex(Vector3 location)
    {
        return new Vector2(0, 0);//not workerino
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        for (int i = 0; i < sizeX * sizeY; i++)
        {
            if (GetTypeOfCell(i) == typeOfCell.normal)
            {
                Gizmos.color = Color.gray;
            }
            else if (GetTypeOfCell(i) == typeOfCell.plant)
            {
                Gizmos.color = Color.green;
            }
            else if (GetTypeOfCell(i) == typeOfCell.hunter)
            {
                Gizmos.color = Color.yellow;
            }
            else if (GetTypeOfCell(i) == typeOfCell.prey)
            {
                Gizmos.color = Color.white;
            }
            else if (GetTypeOfCell(i) == typeOfCell.obstacle)
            {
                Gizmos.color = Color.blue;
            }
            else if (GetTypeOfCell(i) == typeOfCell.trap)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.black; //wtf is this
            }
            Gizmos.DrawCube(new Vector3(i / sizeX * spacing, i % sizeX * spacing, 0) - new Vector3(spacing * sizeX / 2, spacing * sizeY / 2, 0)
                + new Vector3(spacing / 2, spacing / 2, 0)
                , new Vector3(spacing, spacing, 1));
        }
    }

    // location = i % 50 * spacing

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && debug)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.point);
                //Debug.Log(WorldToCellMatrixIndex(hit.point));
            }
        }
        if (Input.GetKeyUp(KeyCode.A))
            MoveActor(hunter, -1, 0);

        if (Input.GetKeyUp(KeyCode.L))
            SaveResults(hunter);

        //GAME CYCLE
        if (elapsedTime > 0.5f)
        {
            elapsedTime = 0f;
            if (hunter.Energy > 0 && prey.Energy > 0)
            {
                if (turn == 0)
                    Turn(prey);
                else Turn(hunter);
                turn = (turn + 1) % 2;
                hunterEnergy.text = "Hunter Energy: " + hunter.Energy;
                preyEnergy.text = "Prey Energy: " + prey.Energy;
            }
            else
            {
                string winner = "None";
                if (hunter.Energy > 0)
                    winner = "Hunter";
                else winner = "Prey;";
                Debug.Log("Game Over. Winner: " + winner);
            }
        }
    }

    void PopulateObstacles(int nColumns, int nRows)
    {

        //column
        for (int i = 0; i < nColumns; i++)
        {
            int x = Random.Range(1, sizeX - 2);
            int y = Random.Range(0, sizeY - 1);

            for (int j = 1; j < sizeY - 1; j++)
            {
                SetTypeOfCell(x, j, typeOfCell.obstacle);
            }

        }

        //row
        for (int i = 0; i < nRows; i++)
        {
            int x = Random.Range(0, sizeX - 1);
            int y = Random.Range(1, sizeY - 2);

            for (int j = 1; j < sizeX - 1; j++)
            {
                SetTypeOfCell(j, y, typeOfCell.obstacle);
            }
        }
    }

    void PopulateTraps(int nTraps)
    {
        for (int i = 0; i < nTraps; i++)
        {
            int selectedLocation = Random.Range(0, sizeX * sizeY - 1);
            bool notPlaceable = GetTypeOfCell(selectedLocation) != typeOfCell.normal;
            while (notPlaceable)
            {
                selectedLocation = Random.Range(0, sizeX * sizeY - 1);
                notPlaceable = GetTypeOfCell(selectedLocation) != typeOfCell.normal;
            }
            //Debug.Log(selectedLocation);
            SetTypeOfCell(selectedLocation, typeOfCell.trap);
        }
    }

    void PopulatePlants(int nPlants)
    {
        for (int i = 0; i < nPlants; i++)
        {
            int selectedLocation = Random.Range(0, sizeX * sizeY - 1);
            bool notPlaceable = GetTypeOfCell(selectedLocation) != typeOfCell.normal;
            while (notPlaceable)
            {
                selectedLocation = Random.Range(0, sizeX * sizeY - 1);
                notPlaceable = GetTypeOfCell(selectedLocation) != typeOfCell.normal;
            }
            SetTypeOfCell(selectedLocation, typeOfCell.plant);
        }
    }

    Vector2 detectCellNearby(int x, int y, int distance, typeOfCell typeToDetect)
    {
        Vector2 cellNearby = new Vector2(0,0);

        for(int i = x; i < x+distance; i++)
        {
            if (GetTypeOfCell(i, y) == typeToDetect)
                cellNearby.x = 1;
        }

        for (int i = x; i < x - distance; i--)
        {
            if (GetTypeOfCell(i, y) == typeToDetect)
                cellNearby.x = -1;
        }

        for (int i = y; i < y + distance; i++)
        {
            if (GetTypeOfCell(x, i) == typeToDetect)
                cellNearby.y = 1;
        }

        for (int i = y; i < y - distance; i--)
        {
            if (GetTypeOfCell(x, i) == typeToDetect)
                cellNearby.y = -1;
        }

        return cellNearby;
    }

    bool hasCellNearby(int x, int y, int distance, typeOfCell typeToDetect)
    {
        bool cellNearby = false;

        for (int i = x; i < x + distance; i++)
        {
            if (GetTypeOfCell(i, y) == typeToDetect)
                cellNearby = true;
        }

        for (int i = x; i < x - distance; i--)
        {
            if (GetTypeOfCell(i, y) == typeToDetect)
                cellNearby = true;
        }

        for (int i = y; i < y + distance; i++)
        {
            if (GetTypeOfCell(x, i) == typeToDetect)
                cellNearby = true;
        }

        for (int i = y; i < y - distance; i--)
        {
            if (GetTypeOfCell(x, i) == typeToDetect)
                cellNearby = true;
        }

        return cellNearby;
    }

    private void Turn(Actor actor)
    {
        /*List<Action> actions = actor.Actions;
        bool isValid = false;

        while (!isValid)
        {
            int randIndex = Random.Range(0, actions.Count - 1);
            isValid = actions[randIndex].CanExecute(this);
            if (isValid)
                actions[randIndex].Execute(this);
        }*/

        bool isValid = false;
        Actor.state state = chooseState(actor);
        //Dictionary<Action, float> gene = actor.Genes[state];

        System.Random r = new System.Random();

        /*while (!isValid)
        {
            float diceRoll = (float)r.NextDouble();
            float cumulative = 0.0f;
            foreach (Action action in actor.Actions)
            {
                cumulative += gene[action];
                if(diceRoll < cumulative)
                {
                    isValid = action.CanExecute(this);
                    if (isValid)
                        action.Execute(this);
                    break;
                }
            }
        }*/
        

    }

    public Actor.state chooseState(Actor actor)
    {

        /*if (hasCellNearby(actor.PosX, actor.PosY, 1, typeOfCell.hunter))
        {
            return Actor.state.nextToActor;
        }
        if (hasCellNearby(actor.PosX, actor.PosY, 1, typeOfCell.trap))
        {
            return Actor.state.nextToTrap;
        }
        if (hasCellNearby(actor.PosX, actor.PosY, 1, typeOfCell.prey))
        {
            return Actor.state.nextToActor;
        }
        if (hasCellNearby(actor.PosX, actor.PosY, 1, typeOfCell.plant))
        {
            return Actor.state.nextToPlant;
        }
        if (hasCellNearby(actor.PosX, actor.PosY, 1, typeOfCell.obstacle))
        {
            return Actor.state.nextToObstacle;
        }
        else
        {
            return Actor.state.emptySpace;
        }*/
        return Actor.state.enemyDown;
    }

    public void MoveActor(Actor actor, int offsetX, int offsetY)
    {
        int offset = offsetX == 0 ? 0 : (offsetX > 0 ? 1 : -1);

        for (int i = 0; i < Mathf.Abs(offsetX); i++)
        {
            actor.Energy--;
            HandleCollision(actor, actor.PosX + offset, actor.PosY);
            SetTypeOfCell(actor.PosX, actor.PosY, typeOfCell.normal);

            typeOfCell actorType;
            if (actor.type == Actor.typeofActor.hunter)
                actorType = typeOfCell.hunter;
            else
                actorType = typeOfCell.prey;

            SetTypeOfCell(actor.PosX + offset, actor.PosY, actorType);
            actor.PosX += offset;
        }

        offset = offsetY == 0 ? 0 : (offsetY > 0 ? 1 : -1);

        for (int i = 0; i < Mathf.Abs(offsetY); i++)
        {
            actor.Energy--;
            HandleCollision(actor, actor.PosX, actor.PosY + offset);
            SetTypeOfCell(actor.PosX, actor.PosY, typeOfCell.normal);

            typeOfCell actorType;
            if (actor.type == Actor.typeofActor.hunter)
                actorType = typeOfCell.hunter;
            else
                actorType = typeOfCell.prey;

            SetTypeOfCell(actor.PosX, actor.PosY + offset, actorType);
            actor.PosY += offset;
        }
    }

    private void HandleCollision(Actor actor, int posX, int posY)
    {
        typeOfCell typeCell = GetTypeOfCell(posX, posY);
        actor.HandleCollision(typeCell);
    }

    public void killPrey()
    {
        prey.Death();
    }

    public void SaveResults(Actor actor)
    {
        System.IO.File.AppendAllText("../yourtextfile.txt", "This is text that goes into the text file");
    }
}