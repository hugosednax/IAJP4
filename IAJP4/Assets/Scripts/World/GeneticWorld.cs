﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using WorldDefinition;

namespace WorldDefinition
{
    public enum typeOfCell { obstacle, normal, trap, plant, hunter, prey };
}

public class GeneticWorld : MonoBehaviour, IWorld
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

    
    public int numberOfTraps = 5;
    public int numberOfPlants = 3;
    List<typeOfCell> world;
    Hunter hunter;
    Prey prey;
    int turn = 0;
    int id = 0;
    bool finished = false;

    //Text hunterEnergy;
    //Text preyEnergy;
    float elapsedTime = 0f;

    GameManager manager;

    public Hunter Hunter
    {
        get
        {
            return hunter;
        }

        set
        {
            hunter = value;
        }
    }

    public Prey Prey
    {
        get
        {
            return prey;
        }

        set
        {
            prey = value;
        }
    }

    // Use this for initialization
    void Awake()
    {
        //hunterEnergy = GameObject.Find("HunterEnergy").GetComponent<Text>();
        //preyEnergy = GameObject.Find("PreyEnergy").GetComponent<Text>();
    }

    void Start()
    {
        ResetWorld();
    }

    public void ResetWorld(GenesEncap hunterGenesFromPappi = null, GenesEncap preyGenesFromPappi = null)
    {
        finished = false;
        world = new List<typeOfCell>();
        for (int i = 0; i < sizeX * sizeY; i++)
        {
            world.Add(typeOfCell.normal);
        }
        spacing = this.GetComponent<Renderer>().bounds.size.x / (float)sizeX;

        //PopulateObstacles(1, 1);
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
            //Debug.Log("worldwhilenotplace");
        }

        //Debug.Log("reset "+transform.name);
        if (hunterGenesFromPappi == null){
            hunter = new Hunter(hunterX, hunterY, this);
        }
        else
        {
            hunter = new Hunter(hunterX, hunterY, this, hunterGenesFromPappi);
        }
        
        SetTypeOfCell(hunterX, hunterY, typeOfCell.hunter);

        if (preyGenesFromPappi == null)
        {
            prey = new Prey(preyX, preyY, this);
        }
        else
        {
            prey = new Prey(preyX, preyY, this, preyGenesFromPappi);
        }
        
        
        //Debug.Log("Finihsed!!!");
        SetTypeOfCell(preyX, preyY, typeOfCell.prey);
       // DebugDoubles(true);
    }

    public void EndGame()
    {
        //Debug.Log("End");
        //Debug.Log("World + " + id + " trying to end game");
        /*if (!finished)
        {*/
            manager.EndedWorld(id);
           // finished = true;
        //}
    }

    public void DebugDoubles(bool start)
    {
        if (this.hunter == null || this.prey == null)
            Debug.Log("MISSING INSTANCES");
        int hunter = 0;
        int prey = 0;
        for (int i = 0; i < world.Count; i++)
        {
            if (GetTypeOfCell(i) == typeOfCell.hunter)
                hunter++;
            if (GetTypeOfCell(i) == typeOfCell.prey)
                prey++;
        }
        if (start && (hunter < 1 || prey < 1))
            Debug.Log("MISSING");
        if (hunter > 1 || prey > 1)
            Debug.Log("DOUBLES");
    }

    public void setGameManager(GameManager gm) { manager = gm; }

    public GameManager GetGameManager() { return manager;}

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

    // Update is called once per frame
    void Update()
    {
        //DebugDoubles(false);
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

        //GAME CYCLE
        if (elapsedTime > tickTimer)
        {
            elapsedTime = 0f;
            if (hunter.Energy > 0 && prey.Energy > 0)
            {
                if (turn == 0)
                    prey.Turn();
                else hunter.Turn();
                turn = (turn + 1) % 2;
                //hunterEnergy.text = "Hunter Energy: " + hunter.Energy;
                //preyEnergy.text = "Prey Energy: " + prey.Energy;
            }
            else
            {
                /*string winner = "None";
                if (hunter.Energy > 0)
                {
                    winner = "Hunter";
                }
                else
                {
                    winner = "Prey;";
                }*/
                //Debug.Log("reset by stamina");
                hunter.SaveResults(id);
                prey.SaveResults(id);
                EndGame();
                //Debug.Log("Game Over. Winner: " + winner);
            }
        }
    }

    #region populateEnvironment

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
    #endregion

    byte detectCellNearby(int x, int y, int distance, typeOfCell typeToDetect)
    {
        byte cellNearby = 0x00;

        for (int i = -distance; i < distance; i++)
        {
            for (int j = -distance; j < distance; j++)
            {
                if (GetTypeOfCell(x + i, y + j) == typeToDetect)
                {
                    if( i > 0)
                        cellNearby |= 0x01;
                    if ( i < 0)
                        cellNearby |= 0x02;
                    if (j > 0)
                        cellNearby |= 0x04;
                    if (j < 0)
                        cellNearby |= 0x08;

                    if (manager.logSummaryInfo)
                    {
                        if (typeToDetect == typeOfCell.trap)
                        {
                            manager.summaryPrinter.NumberOfPlantsDetected++;
                            if (GetTypeOfCell(x,y) == typeOfCell.hunter)
                            {
                                manager.summaryPrinter.NumberOfPlantsDetectedByHunter++;
                            }
                            else if (GetTypeOfCell(x, y) == typeOfCell.prey)
                            {
                                manager.summaryPrinter.NumberOfPlantsDetectedByPrey++;

                            }
                        }
                        if (typeToDetect == typeOfCell.trap)
                        {
                            manager.summaryPrinter.NumberOfTrapsDetected++;
                            if (GetTypeOfCell(x, y) == typeOfCell.hunter)
                            {
                                manager.summaryPrinter.NumberOfTrapsDetectedByHunter++;
                            }
                            else if (GetTypeOfCell(x, y) == typeOfCell.prey)
                            {
                                manager.summaryPrinter.NumberOfTrapsDetectedByPrey++;
                            }
                        }
                    }
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

    public void killPrey()
    {
        prey.Death();
    }

    public void setId(int id)
    {
        this.id = id;
    }

}