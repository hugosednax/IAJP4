using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class World : MonoBehaviour {

    [SerializeField]private int sizeX = 10;
    [SerializeField]private int sizeY = 10;
    [SerializeField]private float spacing = 4;
    [SerializeField]private bool debug = true;

    public const int SPRINT_LENGTH = 2;
    public enum typeOfCell { obstacle, normal, trap, plant, hunter, prey };
    List<typeOfCell> world;
    Hunter hunter;
    Prey prey;
    int turn = 0;

    Text hunterEnergy;
    Text preyEnergy;
    float elapsedTime = 0f;

	// Use this for initialization
	void Start () {
        hunterEnergy = GameObject.Find("HunterEnergy").GetComponent<Text>();
        preyEnergy = GameObject.Find("PreyEnergy").GetComponent<Text>();
        world = new List<typeOfCell>();
        for(int i = 0; i < sizeX * sizeY; i++)
        {
            world.Add(typeOfCell.normal);
        }
        if (this.GetComponent<Renderer>() != null)
        {
            Debug.Log("World script is attached to plane, changing sizes");
            spacing = this.GetComponent<Renderer>().bounds.size.x / (float)sizeX;
            Debug.Log("Spacing is now: "+ spacing);
        }
        else
        {
            Debug.Log("World script is attached to plane, generating world");
            GameObject quad = (GameObject)Resources.Load("Quad");
            Vector3 pos;
            for (int i = 0; i < sizeX * sizeY; i++)
            {
                pos = new Vector3(i / sizeX * spacing, i % sizeX * spacing, 0) - new Vector3(spacing * sizeX / 2, spacing * sizeY / 2, 0) + new Vector3(spacing / 2, spacing / 2, 0);
                GameObject instancedQuad = (GameObject) Instantiate(quad, pos, quad.transform.rotation);
                instancedQuad.transform.localScale = new Vector3(spacing,spacing,1);
                instancedQuad.transform.parent = transform;
            }
            Debug.Log("World is now generated");
        }


        int hunterX = 0;
        int hunterY = 0;

        int fugitiveX = 0;
        int fugitiveY = 0;

        while (hunterX == fugitiveX && hunterY == fugitiveY)
        {
            hunterX = Random.Range(0, sizeX - 1);
            hunterY = Random.Range(0, sizeY - 1);
            fugitiveX = Random.Range(0, sizeX - 1);
            fugitiveY = Random.Range(0, sizeY - 1);
        }
        hunter = new Hunter(hunterX, hunterY);
        SetTypeOfCell(hunterX, hunterY, typeOfCell.hunter);
        prey = new Prey(fugitiveX, fugitiveY);
        SetTypeOfCell(fugitiveX, fugitiveY, typeOfCell.prey);
    }
	
    public typeOfCell GetTypeOfCell (int i, int j)
    {
        if (i < 0 || j < 0 || i > sizeX - 1 || j > sizeY - 1)
            return typeOfCell.obstacle;
        return world[sizeX*i + j];
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
        return 0;
    }

    Vector2 WorldToCellMatrixIndex(Vector3 location)
    {
        return new Vector2(0,0);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        for (int i = 0; i < sizeX * sizeY; i++)
        {
            if(GetTypeOfCell(i) == typeOfCell.normal)
            {
                Gizmos.color = Color.blue;
            }
            else if(GetTypeOfCell(i) == typeOfCell.plant)
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
                Gizmos.color = Color.black;
            }
            else if (GetTypeOfCell(i) == typeOfCell.trap)
            {
                Gizmos.color = Color.cyan;
            }
            else
            {
                Gizmos.color = Color.gray;
            }
            Gizmos.DrawCube(new Vector3(i / sizeX * spacing, i % sizeX * spacing, 0) - new Vector3(spacing * sizeX / 2, spacing * sizeY / 2,0) 
                + new Vector3(spacing / 2, spacing / 2, 0)
                , new Vector3(spacing, spacing, 1));
        }
    }

    // location = i % 50 * spacing

    // Update is called once per frame
    void Update () {
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

    private void Turn(Actor actor)
    {
        List<Action> actions = actor.Actions;
        bool isValid = false;

        while(!isValid){
            int randIndex = Random.Range(0, actions.Count - 1);
            isValid = actions[randIndex].CanExecute(this);
            if (isValid)
                actions[randIndex].Execute(this);
        }
    }

    public void MoveActor(Actor actor, int offsetX, int offsetY)
    {
        int offset = offsetX == 0 ? 0 : (offsetX > 0 ? 1 : -1);

        for (int i = 0; i < Mathf.Abs(offsetX); i++)
        {
            actor.Energy--;
            if (actor.type == Actor.typeofActor.hunter)
            {
                HandleCollision(hunter, hunter.PosX + offset, hunter.PosY);
                SetTypeOfCell(hunter.PosX, hunter.PosY, typeOfCell.normal);
                SetTypeOfCell(hunter.PosX + offset, hunter.PosY, typeOfCell.hunter);
                hunter.PosX += offset;
            }
            else if (actor.type == Actor.typeofActor.prey)
            {
                HandleCollision(prey, prey.PosX + offset, prey.PosY);
                SetTypeOfCell(prey.PosX, prey.PosY, typeOfCell.normal);
                SetTypeOfCell(prey.PosX + offset, prey.PosY, typeOfCell.prey);
                prey.PosX += offset;
            }
            else Debug.Log("Move Unknown Actor: " + actor.type);
        }

        offset = offsetY == 0 ? 0 : (offsetY > 0 ? 1 : -1);

        for (int i = 0; i < Mathf.Abs(offsetY); i++)
        {
            actor.Energy--;
            if (actor.type == Actor.typeofActor.hunter)
            {
                HandleCollision(hunter, hunter.PosX, hunter.PosY + offset);
                SetTypeOfCell(hunter.PosX, hunter.PosY, typeOfCell.normal);
                SetTypeOfCell(hunter.PosX, hunter.PosY + offset, typeOfCell.hunter);
                hunter.PosY += offset;
            }
            else if (actor.type == Actor.typeofActor.prey)
            {
                HandleCollision(prey, prey.PosX, prey.PosY + offset);
                SetTypeOfCell(prey.PosX, prey.PosY, typeOfCell.normal);
                SetTypeOfCell(prey.PosX, prey.PosY + offset, typeOfCell.prey);
                prey.PosY += offset;
            }
            else Debug.Log("Move Unknown Actor: " + actor.type);
        }
    }

    private void HandleCollision(Actor actor, int posX, int posY)
    {
        typeOfCell typeCell = GetTypeOfCell(posX, posY);

        if (actor.type == Actor.typeofActor.hunter)
        {
            if (typeCell == typeOfCell.prey)
            {
                prey.Energy = -999;
                //Debug.Log("Hunter in Prey");
            }
            else if(typeCell == typeOfCell.hunter)
            {
                Debug.Log("[BUG]Two hunters ingame!!");
            }
            else if (typeCell == typeOfCell.normal)
            {
                //Debug.Log("Hunter in normal");
            }
            else if (typeCell == typeOfCell.obstacle)
            {
                Debug.Log("[BUG]Hunter Moving through an obstacle!!");
            }
            else if (typeCell == typeOfCell.plant)
            {
                hunter.Energy += 1;
                //Debug.Log("Hunter in plant");
            }
            else if (typeCell == typeOfCell.trap)
            {
                hunter.Energy = -999;
                //Debug.Log("Hunter in trap");
            }
        }
        else if(actor.type == Actor.typeofActor.prey)
        {
            if (typeCell == typeOfCell.prey)
            {
                Debug.Log("[BUG]Two preys ingame!!");
            }
            else if (typeCell == typeOfCell.hunter)
            {
                prey.Energy = -999;
                //Debug.Log("Prey in Hunter");
            }
            else if (typeCell == typeOfCell.normal)
            {
                //Debug.Log("Prey in normal");
            }
            else if (typeCell == typeOfCell.obstacle)
            {
                Debug.Log("[BUG]Prey Moving through an obstacle!!");
            }
            else if (typeCell == typeOfCell.plant)
            {
                prey.Energy += 3;
                //Debug.Log("Prey in plant");
            }
            else if (typeCell == typeOfCell.trap)
            {
                prey.Energy = -999;
                //Debug.Log("Prey in trap");
            }
        }
    }
}
