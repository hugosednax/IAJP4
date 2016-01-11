using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

    public GameObject worldPrefab;
    public int numberOfWorlds;

    private List<World> worlds;
    private bool[] finishedSamples;

	// Use this for initialization
	void Start () {
        worlds = new List<World>();
        GameObject worldPhysical;
        World worldInstance;
        finishedSamples = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            finishedSamples[i] = false;
        }
        for (int i = 0; i < numberOfWorlds; i++)
        {
            worldPhysical = (GameObject)Instantiate(worldPrefab, worldPrefab.transform.position + new Vector3(i*300, 0,0)
                , worldPrefab.transform.rotation);
            worldPhysical.transform.parent = this.transform;
            worldPhysical.transform.name = "WorldNumber_"+i;
            worldInstance = worldPhysical.GetComponent<World>();
            worldInstance.setGameManager(this);
            worldInstance.setId(i);
            worlds.Add(worldInstance);
        }
	}

    public void EndedWorld(int id)
    {
        //Debug.Log(name + " finished");
        finishedSamples[id] = true;
        bool finished = true;
        for (int i = 0; i < 4; i++)
        {
            finished |= finishedSamples[i];
        }

        if (finished)
        {
            //Debug.Log("Resetting all");
            Pair<List<GenesEncap>, List<GenesEncap>> pairOfResults = StartNewGen();
            for (int i = 0; i < numberOfWorlds; i++)
            {
                worlds[i].ResetWorld(pairOfResults.First[i], pairOfResults.Second[i]);
            }
            for (int i = 0; i < 4; i++)
            {
                finishedSamples[i] = false;
            }
        }
        
    }

    private Pair<List<GenesEncap>, List<GenesEncap>> StartNewGen()
    {
        
        List<Hunter> hunters = new List<Hunter>();
        List<Prey> preyz = new List<Prey>();
        for (int i = 0; i < numberOfWorlds; i++)
        {
            hunters.Add(worlds[i].Hunter);
            preyz.Add(worlds[i].Prey);
        }

        List<Hunter> SortedHunterList = hunters.OrderByDescending(o => o.Energy).ToList(); //can be changed to be better optimized
        List<Prey> SortedPreyList = preyz.OrderByDescending(o => o.Energy).ToList();

        //creating the best hunters
        List<GenesEncap> bestHunters = new List<GenesEncap>();
        List<GenesEncap> bestPreys = new List<GenesEncap>();

        for (int i = 0, samples = 0; i < SortedHunterList.Count && samples < numberOfWorlds; i++) 
        {
            for (int j = i; j < SortedHunterList.Count && samples < numberOfWorlds; j++, samples++)
            {
                GenesEncap newGenes = GeneticUtility.Crossover(SortedHunterList[i], SortedHunterList[j]);
                GeneticUtility.mutate(newGenes);
                bestHunters.Add(newGenes);
            }
        }

        //creating the best preys
        for (int i = 0, samples = 0; i < SortedPreyList.Count && samples < numberOfWorlds; i++)
        {
            for (int j = i; j < SortedPreyList.Count && samples < numberOfWorlds; j++, samples++)
            {
                GenesEncap newGenes = GeneticUtility.Crossover(SortedPreyList[i], SortedPreyList[j]);
                GeneticUtility.mutate(newGenes);
                bestPreys.Add(newGenes);
            }
        }
       
        return new Pair<List<GenesEncap>, List<GenesEncap>>(bestHunters, bestPreys);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
