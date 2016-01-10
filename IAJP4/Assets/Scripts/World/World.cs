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
    [SerializeField]
    private int  generation = 0;
    [SerializeField]
    private float tickTimer = 0.5f;

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
    void Awake()
    {
        hunterEnergy = GameObject.Find("HunterEnergy").GetComponent<Text>();
        preyEnergy = GameObject.Find("PreyEnergy").GetComponent<Text>();
    }

    void Start()
    {
        ResetWorld();
    }

    public void ResetWorld()
    {
        Debug.Log("start reset");
        world = new List<typeOfCell>();
        for (int i = 0; i < sizeX * sizeY; i++)
        {
            world.Add(typeOfCell.normal);
        }
        spacing = this.GetComponent<Renderer>().bounds.size.x / (float)sizeX;

        PopulateObstacles(1, 1);
        PopulateTraps(numberOfTraps);
        PopulatePlants(numberOfPlants);

        int hunterX = 0;
        int hunterY = 0;

        int preyX = 0;
        int preyY = 0;

        bool notPlaceable = (hunterX == preyX && hunterY == preyY) ||
            GetTypeOfCell(hunterX, hunterY) != typeOfCell.normal ||
            GetTypeOfCell(preyX, preyY) != typeOfCell.normal;
        while (notPlaceable)
        {
            hunterX = Random.Range(0, sizeX - 1);
            hunterY = Random.Range(0, sizeY - 1);
            preyX = Random.Range(0, sizeX - 1);
            preyY = Random.Range(0, sizeY - 1);
            notPlaceable = (hunterX == preyX && hunterY == preyY) ||
                GetTypeOfCell(hunterX, hunterY) != typeOfCell.normal ||
                GetTypeOfCell(preyX, preyY) != typeOfCell.normal;
        }


        hunter = new Hunter(hunterX, hunterY, this);
        SetTypeOfCell(hunterX, hunterY, typeOfCell.hunter);


        prey = new Prey(preyX, preyY, this);
        SetTypeOfCell(preyX, preyY, typeOfCell.prey);
 
        generation++;
        Debug.Log("end reset");
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
        if(newType == typeOfCell.hunter)
            Debug.Log("Placed Hunter at " + i + ";" + j);
        else if (newType == typeOfCell.prey)
            Debug.Log("Placed Prey at " + i + ";" + j);

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

        if (Input.GetKeyUp(KeyCode.R))
            ResetWorld();

        //GAME CYCLE
        if (elapsedTime > tickTimer)
        {
            elapsedTime = 0f;
            if (hunter.Energy > 0 && prey.Energy > 0)
            {
                Debug.Log("turn");
                if (turn == 0)
                    prey.Turn();
                else hunter.Turn();
                turn = (turn + 1) % 2;
                hunterEnergy.text = "Hunter Energy: " + hunter.Energy;
                preyEnergy.text = "Prey Energy: " + prey.Energy;
            }
            else
            {
                string winner = "None";
                if (hunter.Energy > 0)
                {
                    hunter.SaveResults(true);
                    prey.SaveResults(false);
                    winner = "Hunter";
                }
                else
                {
                    prey.SaveResults(true);
                    hunter.SaveResults(false);
                    winner = "Prey;";
                }
                Debug.Log("reset by stamina");
                ResetWorld();
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
            //int y = Random.Range(0, sizeY - 1);

            for (int j = 1; j < sizeY - 1; j++)
            {
                SetTypeOfCell(x, j, typeOfCell.obstacle);
            }

        }

        //row
        for (int i = 0; i < nRows; i++)
        {
            //int x = Random.Range(0, sizeX - 1);
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

    byte detectCellNearby(int x, int y, int distance, typeOfCell typeToDetect)
    {
        byte cellNearby = 0x00;

        for(int i = 1; i < distance; i++)
        {
            if (GetTypeOfCell(x + i, y) == typeToDetect)
            {
                cellNearby |= 0x01;
                break;
            }
        }

        for (int i = 1; i < distance; i++)
        {
            if (GetTypeOfCell(x - i, y) == typeToDetect)
            {
                cellNearby |= 0x02;
                break;
            }
        }

        for (int i = 1; i < distance; i++)
        {
            if (GetTypeOfCell(x, y + i) == typeToDetect)
            {
                cellNearby |= 0x04;
                break;
            }
        }

        for (int i = 1; i < distance; i++)
        {
            if (GetTypeOfCell(x, y - i) == typeToDetect)
            {
                cellNearby |= 0x08;
                break;
            }
        }
        return cellNearby;
    }

    public byte[] getState(Actor actor)
    {
        byte[] states = new byte[3]; // 0 enemyD, 2 plant, 3 trap
        for (int i = 0; i < states.Length; i++)
        {
            states[i] = 0x00;
        }
        if (actor.type == Actor.typeofActor.hunter)
        {
            states[0] = detectCellNearby(actor.PosX, actor.PosY, 1, typeOfCell.prey);
        }
        else
        {
            states[0] = detectCellNearby(actor.PosX, actor.PosY, 1, typeOfCell.hunter);
        }
        
        states[1] = detectCellNearby(actor.PosX, actor.PosY, 1, typeOfCell.plant);
        states[2] = detectCellNearby(actor.PosX, actor.PosY, 1, typeOfCell.trap);

        return states;
    }

    public void MoveActor(Actor actor, int offsetX, int offsetY)
    {
        Debug.Log("start move Actor");
        int offset = offsetX == 0 ? 0 : (offsetX > 0 ? 1 : -1);

        for (int i = 0; i < Mathf.Abs(offsetX); i++)
        {
            actor.Energy--;
            HandleCollision(actor, actor.PosX + offset, actor.PosY);
            if (actor.Energy < 0)
                return;
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
            if (actor.Energy < 0)
                return;
            SetTypeOfCell(actor.PosX, actor.PosY, typeOfCell.normal);

            typeOfCell actorType;
            if (actor.type == Actor.typeofActor.hunter)
                actorType = typeOfCell.hunter;
            else
                actorType = typeOfCell.prey;

            SetTypeOfCell(actor.PosX, actor.PosY + offset, actorType);
            actor.PosY += offset;
        }
        Debug.Log("end move Actor");
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

}