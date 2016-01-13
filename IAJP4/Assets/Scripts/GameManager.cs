using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject worldPrefab;
    public int numberOfWorlds;
    public bool logSummaryInfo = true;

    [SerializeField]
    private int generations;

    private List<GeneticWorld> worlds;
    private bool[] finishedSamples;
    public SummaryPrinter summaryPrinter;

    // Use this for initialization
    void Start()
    {
        generations = 0;
        worlds = new List<GeneticWorld>();
        GameObject worldPhysical;
        GeneticWorld worldInstance;
        finishedSamples = new bool[4];
        for (int i = 0; i < numberOfWorlds; i++)
        {
            finishedSamples[i] = false;
        }
        for (int i = 0; i < numberOfWorlds; i++)
        {
            worldPhysical = (GameObject)Instantiate(worldPrefab, worldPrefab.transform.position + new Vector3((i / (numberOfWorlds/2)) * 230, (i % (numberOfWorlds / 2)) * 230, 0)
                , worldPrefab.transform.rotation);
            worldPhysical.transform.parent = this.transform;
            worldPhysical.transform.name = "WorldNumber_" + i;
            worldInstance = worldPhysical.GetComponent<GeneticWorld>();
            worldInstance.setGameManager(this);
            worldInstance.setId(i);
            worlds.Add(worldInstance);
        }

        if (logSummaryInfo)
        {
            summaryPrinter = new SummaryPrinter();
            summaryPrinter.SampleNumber = numberOfWorlds;
            summaryPrinter.TrapNumber = worlds[0].numberOfTraps;
            summaryPrinter.PlantNumber = worlds[0].numberOfPlants;
        }
    }

    public void EndedWorld(int id)
    {
        //Debug.Log(name + " finished");
        finishedSamples[id] = true;
        bool finished = true;
        for (int i = 0; i < numberOfWorlds; i++)
        {
            finished &= finishedSamples[i];
        }
        if (finished)
        {
            //Debug.Log("Resetting all");
            Pair<List<GenesEncap>, List<GenesEncap>> pairOfResults = StartNewGen();
            for (int i = 0; i < numberOfWorlds; i++)
            {
                worlds[i].ResetWorld(pairOfResults.First[i], pairOfResults.Second[i]);
            }
            for (int i = 0; i < numberOfWorlds; i++)
            {
                finishedSamples[i] = false;
            }

            if (generations % 10 == 0)
                summaryPrinter.SummarizeGeneration("generation" + generations);
            summaryPrinter.ResetVariables();

            generations++;
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

        for (int samples = 0; samples < numberOfWorlds; samples++)
        {
            GenesEncap newGenes = GeneticUtility.Crossover(SortedHunterList[0], SortedHunterList[1]);
            GeneticUtility.mutate(newGenes);
            bestHunters.Add(newGenes);
        }

        for (int samples = 0; samples < numberOfWorlds; samples++)
        {
            GenesEncap newGenes = GeneticUtility.Crossover(SortedPreyList[0], SortedPreyList[1]);
            GeneticUtility.mutate(newGenes);
            bestPreys.Add(newGenes);
        }

        return new Pair<List<GenesEncap>, List<GenesEncap>>(bestHunters, bestPreys);
    }

    // Update is called once per frame
    void Update()
    {

    }
}