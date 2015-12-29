using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

    [SerializeField]private int sizeX = 10;
    [SerializeField]private int sizeY = 10;
    [SerializeField]private float spacing = 4;
    [SerializeField]private bool debug = true;


    public enum typeOfCell { obstacle, normal, trap, plant, hunter, fugitive };
    List<typeOfCell> world;
    Hunter hunter;
    Prey prey;

	// Use this for initialization
	void Start () {
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
        SetTypeOfCell(fugitiveX, fugitiveY, typeOfCell.fugitive);
    }
	
    public typeOfCell GetTypeOfCell (int i, int j)
    {
        if (i < 0 || j < 0 || i > sizeX || j > sizeY)
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
            else //IT'S A TRAP!!
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawCube(new Vector3(i / sizeX * spacing, i % sizeX * spacing, 0) - new Vector3(spacing * sizeX / 2, spacing * sizeY / 2,0) 
                + new Vector3(spacing / 2, spacing / 2, 0)
                , new Vector3(spacing, spacing, 1));
        }
    }

    // location = i % 50 * spacing

    // Update is called once per frame
    void Update () {
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
    }

    public void MoveActor(Actor actor, int offsetX, int offsetY)
    {
        if (actor.type == Actor.typeofActor.hunter)
        {
            SetTypeOfCell(hunter.PosX, hunter.PosY, typeOfCell.normal);
            SetTypeOfCell(hunter.PosX + offsetX, hunter.PosY + offsetY, typeOfCell.hunter);
        }
        else if (actor.type == Actor.typeofActor.fugitive)
        {
            SetTypeOfCell(prey.PosX, prey.PosY, typeOfCell.normal);
            SetTypeOfCell(prey.PosX + offsetX, prey.PosY + offsetY, typeOfCell.fugitive);
        }
        else Debug.Log("Move Unknown Actor: " + actor.type);
    }
}
