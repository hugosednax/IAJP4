using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using WorldDefinition;
using UnityEngine.SceneManagement;

public class PlayableWorld : MonoBehaviour, IWorld
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
    private float tickTimer = 0.01f;

    public bool isGeneticWorld { get; set; }
    public int numberOfTraps = 5;
    public int numberOfPlants = 3;
    List<typeOfCell> world;
    Hunter hunter;
    Prey prey;
    int turn = 0;
    int id = 0;
    bool finished = false;
    public Actor player;
    public Actor enemy;

    //Text hunterEnergy;
    //Text preyEnergy;
    float elapsedTime = 0f;

    HumanPlayManager manager;

    public Hunter Hunter
    {
        get
        {
            return player.type == Actor.typeofActor.hunter? (Hunter)player : (Hunter)enemy;
        }

        set
        {
            if(player.type == Actor.typeofActor.hunter)
            {
                player = value;
            }
            else
            {
                enemy = value;
            }
        }
    }

    public Prey Prey
    {
        get
        {
            return player.type == Actor.typeofActor.prey ? (Prey)player : (Prey)enemy;
        }

        set
        {
            if (player.type == Actor.typeofActor.prey)
            {
                player = value;
            }
            else
            {
                enemy = value;
            }
        }
    }

    void Start()
    {
        isGeneticWorld = false;
        ResetWorld();
    }

    void ResetWorld()
    {
        world = new List<typeOfCell>();
        for (int i = 0; i < sizeX * sizeY; i++)
        {
            world.Add(typeOfCell.normal);
        }
        spacing = this.GetComponent<Renderer>().bounds.size.x / (float)sizeX;

        PopulateTraps(numberOfTraps);
        PopulatePlants(numberOfPlants);

        int hunterX = 0;
        int hunterY = 0;

        int preyX = 0;
        int preyY = 0;

        bool notPlaceable = true;
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

        if (manager.playerType == typeOfPlayer.hunter)
        {
            player = new Hunter(hunterX, hunterY, this);
            enemy = new Prey(preyX, preyY, this);
        }
        else
        {
            enemy = new Hunter(hunterX, hunterY, this);
            player = new Prey(preyX, preyY, this);
        }
        enemy.LoadResults(0);
        SetTypeOfCell(hunterX, hunterY, typeOfCell.hunter);
        SetTypeOfCell(preyX, preyY, typeOfCell.prey);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            ResetWorld();
        }

        elapsedTime += Time.deltaTime;
        
        //GAME CYCLE
        if (elapsedTime > tickTimer)
        {
            elapsedTime = 0f;
            if (player.Energy > 0 && enemy.Energy > 0)
            {
                if (turn == 0)
                {
                    #region ActionKeys
                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        if (ActionManager.CanExecute(0, player, this))
                        {
                            ActionManager.Execute(0, player, this);
                            turn = (turn + 1) % 2;
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.A))
                    {
                        if (ActionManager.CanExecute(1, player, this))
                        {
                            ActionManager.Execute(1, player, this);
                            turn = (turn + 1) % 2;
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.S))
                    {
                        if (ActionManager.CanExecute(3, player, this))
                        {
                            ActionManager.Execute(3, player, this);
                            turn = (turn + 1) % 2;
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.D))
                    {
                        if (ActionManager.CanExecute(2, player, this))
                        {
                            ActionManager.Execute(2, player, this);
                            turn = (turn + 1) % 2;
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.X))
                    {
                        if (ActionManager.CanExecute(4, player, this))
                        {
                            ActionManager.Execute(4, player, this);
                            turn = (turn + 1) % 2;
                        }
                    }
                    else if (player.type == Actor.typeofActor.hunter)
                    {
                        if (Input.GetKeyUp(KeyCode.UpArrow))
                        {
                            if (ActionManager.CanExecute(5, player, this))
                            {
                                ActionManager.Execute(5, player, this);
                                turn = (turn + 1) % 2;
                            }
                        }
                        else if (Input.GetKeyUp(KeyCode.LeftArrow))
                        {
                            if (ActionManager.CanExecute(6, player, this))
                            {
                                ActionManager.Execute(6, player, this);
                                turn = (turn + 1) % 2;
                            }
                        }
                        else if (Input.GetKeyUp(KeyCode.DownArrow))
                        {
                            if (ActionManager.CanExecute(8, player, this))
                            {
                                ActionManager.Execute(8, player, this);
                                turn = (turn + 1) % 2;
                            }
                        }
                        else if (Input.GetKeyUp(KeyCode.RightArrow))
                        {
                            if (ActionManager.CanExecute(7, player, this))
                            {
                                ActionManager.Execute(7, player, this);
                                turn = (turn + 1) % 2;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    enemy.Turn();
                    turn = (turn + 1) % 2;
                }
            }
            else
            {
                Debug.Log("Game Over, Winner: " + (player.Energy > 0 ? "Human Player" : "Bot"));
                SceneManager.LoadScene("PlayableScene");
            }
        }
    }

    public void setGameManager(HumanPlayManager gm) { manager = gm; }

    public HumanPlayManager GetGameManager() { return manager; }

    #region cellLogic
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
    #endregion

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
            Gizmos.DrawCube(transform.position + new Vector3(i / sizeX * spacing, i % sizeX * spacing, 0) - new Vector3(spacing * sizeX / 2, spacing * sizeY / 2, 0)
                + new Vector3(spacing / 2, spacing / 2, 0)
                , new Vector3(spacing, spacing, 1));
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
                //Debug.Log("worldwhilenotplace2");
            }
            //Debug.Log(selectedLocation);
            SetTypeOfCell(selectedLocation, typeOfCell.trap);
        }
    }

    void PopulatePlants(int nPlants)
    {
        bool notPlaceable = true;
        int selectedLocation = 0;
        while (notPlaceable)
        {
            selectedLocation = Random.Range(0, sizeX * sizeY - 1);
            notPlaceable = GetTypeOfCell(selectedLocation) != typeOfCell.normal;
        }
        for (int i = 0; i < nPlants; i++)
        {
            int selectedLocationX = 0;
            int selectedLocationY = 0;
            notPlaceable = true;
            while (notPlaceable)
            {
                selectedLocationX = (selectedLocation % sizeX) - Random.Range(0, 3) + Random.Range(0, 3);
                selectedLocationY = (selectedLocation / sizeX) - Random.Range(0, 3) + Random.Range(0, 3);
                notPlaceable = GetTypeOfCell(selectedLocationX, selectedLocationY) != typeOfCell.normal;
            }
            SetTypeOfCell(selectedLocationX, selectedLocationY, typeOfCell.plant);
        }
    }

    public void killPrey()
    {
        Prey.Death();
    }

    byte detectCellNearby(int x, int y, int distance, typeOfCell typeToDetect)
    {
        byte cellNearby = 0x00;

        for (int i = -distance; i < distance; i++)
        {
            for (int j = -distance; j < distance; j++)
            {
                if (GetTypeOfCell(x + i, y + j) == typeToDetect)
                {
                    if (i > 0)
                        cellNearby |= 0x01;
                    if (i < 0)
                        cellNearby |= 0x02;
                    if (j > 0)
                        cellNearby |= 0x04;
                    if (j < 0)
                        cellNearby |= 0x08;
                }
            }
        }
        return cellNearby;
    }

    public byte[] getState(Actor actor)
    {
        byte[] states = new byte[3]; // 0 enemy, 2 plant, 3 trap
        for (int i = 0; i < states.Length; i++)
        {
            states[i] = 0x00;
        }
        if (actor.type == Actor.typeofActor.hunter)
        {
            states[0] = detectCellNearby(actor.PosX, actor.PosY, 10, typeOfCell.prey);
        }
        else
        {
            states[0] = detectCellNearby(actor.PosX, actor.PosY, 10, typeOfCell.hunter);
        }

        states[1] = detectCellNearby(actor.PosX, actor.PosY, 15, typeOfCell.plant);
        states[2] = detectCellNearby(actor.PosX, actor.PosY, 1, typeOfCell.trap);

        return states;
    }

    public void MoveActor(Actor actor, int offsetX, int offsetY)
    {
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
    }

    private void HandleCollision(Actor actor, int posX, int posY)
    {
        typeOfCell typeCell = GetTypeOfCell(posX, posY);
        actor.HandleCollision(typeCell);
    }
}
