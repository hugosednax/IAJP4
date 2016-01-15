using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using WorldDefinition;
using System;

namespace WorldDefinition
{
    public enum typeOfCell { obstacle, normal, trap, plant, hunter, prey };
}

public abstract class World : MonoBehaviour {
    [SerializeField]
    protected int sizeX = 15;
    [SerializeField]
    protected int sizeY = 15;
    [SerializeField]
    protected float spacing = 4;
    [SerializeField]
    protected bool debug = true;
    [SerializeField]
    protected float tickTimer = 0.001f;
    public bool isGeneticWorld = false;
    
    public int numberOfTraps = 5;
    public int numberOfPlants = 3;
    protected List<typeOfCell> world;
    protected Hunter hunter;
    protected Prey prey;
    protected int turn = 0;
    protected int id = 0;
    protected bool finished = false;
    protected bool toEnd = false;
    protected bool saved = false;
    protected float elapsedTime = 0f;

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

    public void Start()
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

        PopulateTraps(numberOfTraps);
        PopulatePlants(numberOfPlants);

        int hunterX = 0;
        int hunterY = 0;

        int preyX = 0;
        int preyY = 0;

        bool notPlaceable = true;
        while (notPlaceable)
        {
            hunterX = UnityEngine.Random.Range(0, sizeX - 1);
            hunterY = UnityEngine.Random.Range(0, sizeY - 1);
            preyX = UnityEngine.Random.Range(0, sizeX - 1);
            preyY = UnityEngine.Random.Range(0, sizeY - 1);
            notPlaceable = (hunterX == preyX && hunterY == preyY) ||
                GetTypeOfCell(hunterX, hunterY) != typeOfCell.normal ||
                GetTypeOfCell(preyX, preyY) != typeOfCell.normal;
        }

        if (hunterGenesFromPappi == null)
        {
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
        SetTypeOfCell(preyX, preyY, typeOfCell.prey);
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

    #region populateEnvironment

    void PopulateTraps(int nTraps)
    {
        for (int i = 0; i < nTraps; i++)
        {
            int selectedLocation = UnityEngine.Random.Range(0, sizeX * sizeY - 1);
            bool notPlaceable = GetTypeOfCell(selectedLocation) != typeOfCell.normal;
            while (notPlaceable)
            {
                selectedLocation = UnityEngine.Random.Range(0, sizeX * sizeY - 1);
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
            selectedLocation = UnityEngine.Random.Range(0, sizeX * sizeY - 1);
            notPlaceable = GetTypeOfCell(selectedLocation) != typeOfCell.normal;
        }
        for (int i = 0; i < nPlants; i++)
        {
            int selectedLocationX = 0;
            int selectedLocationY = 0;
            notPlaceable = true;
            while (notPlaceable)
            {
                selectedLocationX = (selectedLocation % sizeX) - UnityEngine.Random.Range(0, 3) + UnityEngine.Random.Range(0, 3);
                selectedLocationY = (selectedLocation / sizeX) - UnityEngine.Random.Range(0, 3) + UnityEngine.Random.Range(0, 3);
                notPlaceable = GetTypeOfCell(selectedLocationX, selectedLocationY) != typeOfCell.normal;
            }
            SetTypeOfCell(selectedLocationX, selectedLocationY, typeOfCell.plant);
        }
    }
    #endregion

    public virtual byte detectCellNearby(int x, int y, int distance, typeOfCell typeToDetect)
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
                    
                }
            }
        }
        return cellNearby;
    }

    public List<int> getState(Actor actor)
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
        List<int> ret = new List<int>();
        int j = 0;
        if ((states[0] & 0x08) != 0) ret.Add(j); j++; //EnemyDown
        if ((states[0] & 0x04) != 0) ret.Add(j); j++; //EnemyUp
        if ((states[0] & 0x02) != 0) ret.Add(j); j++; //EnemyLeft
        if ((states[0] & 0x01) != 0) ret.Add(j); j++; //EnemyRight

        if ((states[1] & 0x08) != 0) ret.Add(j); j++; //PlantDown
        if ((states[1] & 0x04) != 0) ret.Add(j); j++; //PlantUp
        if ((states[1] & 0x02) != 0) ret.Add(j); j++; //PlantLeft
        if ((states[1] & 0x01) != 0) ret.Add(j); j++; //PlantRight

        if ((states[2] & 0x08) != 0) ret.Add(j); j++; //TrapDown
        if ((states[2] & 0x04) != 0) ret.Add(j); j++; //TrapUp
        if ((states[2] & 0x02) != 0) ret.Add(j); j++; //TrapLeft
        if ((states[2] & 0x01) != 0) ret.Add(j); j++; //TrapRight
        return ret;
    }

    public void MoveActor(Actor actor, int offsetX, int offsetY)
    {
        int offset = offsetX == 0 ? 0 : (offsetX > 0 ? 1 : -1);

        for (int i = 0; i < Mathf.Abs(offsetX); i++)
        {
            actor.Energy-=3;
            HandleCollision(actor, actor.PosX + offset, actor.PosY);
            SetTypeOfCell(actor.PosX, actor.PosY, typeOfCell.normal);

            typeOfCell actorType;
            if (actor.type == Actor.typeofActor.hunter)
                actorType = typeOfCell.hunter;
            else
                actorType = typeOfCell.prey;

            SetTypeOfCell(actor.PosX + offset, actor.PosY, actorType);
            actor.PosX += offset;
            if (actor.Energy <= 0) return;
        }

        offset = offsetY == 0 ? 0 : (offsetY > 0 ? 1 : -1);

        for (int i = 0; i < Mathf.Abs(offsetY); i++)
        {
            actor.Energy-=3;
            HandleCollision(actor, actor.PosX, actor.PosY + offset);
            SetTypeOfCell(actor.PosX, actor.PosY, typeOfCell.normal);

            typeOfCell actorType;
            if (actor.type == Actor.typeofActor.hunter)
                actorType = typeOfCell.hunter;
            else
                actorType = typeOfCell.prey;

            SetTypeOfCell(actor.PosX, actor.PosY + offset, actorType);
            actor.PosY += offset;
            if (actor.Energy <= 0) return;
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