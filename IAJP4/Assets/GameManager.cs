using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public GameObject worldPrefab;
    public int numberOfWorlds;

    private List<World> worlds;
    private int finishedSamples = 0;

	// Use this for initialization
	void Start () {
        worlds = new List<World>();
        GameObject worldPhysical;
        World worldInstance;
        for (int i = 0; i < numberOfWorlds; i++)
        {
            worldPhysical = (GameObject)Instantiate(worldPrefab, worldPrefab.transform.position + new Vector3(i*300, 0,0)
                , worldPrefab.transform.rotation);
            worldPhysical.transform.parent = this.transform;
            worldInstance = worldPhysical.GetComponent<World>();
            worldInstance.setGameManager(this);
            worlds.Add(worldInstance);
        }
	}

    public void EndedWorld()
    {
        finishedSamples++;
        if (finishedSamples == worlds.Count)
        {
            for (int i = 0; i < numberOfWorlds; i++)
            {
                worlds[i].ResetWorld();
            }
            finishedSamples = 0;
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
