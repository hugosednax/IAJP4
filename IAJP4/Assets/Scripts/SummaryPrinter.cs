using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


public class SummaryPrinter
{
    public int SampleNumber { get; set; }

    public int NumberOfDeathsByTrap { get; set; }
    public int NumberOfPreyDeathsByTrap { get; set; }
    public int NumberOfHunterDeathsByTrap { get; set; }
    public int NumberOfTrapsDetected { get; set; }
    public int NumberOfTrapsDetectedByPrey { get; set; }
    public int NumberOfTrapsDetectedByHunter { get; set; }
    public int TrapNumber { get; set; }

    public int NumberOfPlantsEaten { get; set; }
    public int NumberOfPlantsEatenByHunter { get; set; }
    public int NumberOfPlantsEatenByPrey { get; set; }
    public int NumberOfPlantsDetectedByPrey { get; set; }
    public int NumberOfPlantsDetectedByHunter { get; set; }
    public int NumberOfPlantsDetected { get; set; }
    public int PlantNumber { get; set; }

    public int NumberOfWinsByHunter { get; set; }
    public int NumberOfWinsByPrey { get; set; }
    public int NumberOfPreysEaten { get; set; }

    public string SummarizeTraps()
    {
        return "There were " + NumberOfDeathsByTrap + " deaths by traps, out of these " + NumberOfHunterDeathsByTrap + " were hunter deaths and " +
              NumberOfPreyDeathsByTrap + " were prey deaths.\n There were " + NumberOfTrapsDetected + " traps that were detected, out of these " + NumberOfTrapsDetectedByHunter
              + " were detected by a hunter and " + NumberOfTrapsDetectedByPrey + " were detected by a prey. There were " + TrapNumber + " traps per world, making it total " + (TrapNumber * SampleNumber) + " traps ";
    }

    public string SummarizePlants()
    {
        return "There were " + NumberOfPlantsEaten + " plants eaten, out of these " + NumberOfPlantsEatenByHunter + " were eaten by hunters and " +
               NumberOfPlantsEatenByPrey + " were eaten by preys.\n There were " + NumberOfPlantsDetected + " plants that were detected, out of these " + NumberOfPlantsDetectedByHunter
               + " were detected by a hunter and " + NumberOfPlantsDetectedByPrey + " were detected by a prey. There were "+ PlantNumber + " plants per world, making it total "+(PlantNumber * SampleNumber) +" plants ";
    }

    public string SummarizeResults()
    {
        return "Out of " + SampleNumber + " samples, " + NumberOfWinsByHunter + " were won by a hunter and " +
               NumberOfWinsByPrey + "were won by a prey."+NumberOfPreysEaten +" is the number of times hunters ate the prey";
    }

    public void ResetVariables()
    {

        NumberOfDeathsByTrap = 0;
        NumberOfPreyDeathsByTrap = 0;
        NumberOfHunterDeathsByTrap = 0;
        NumberOfTrapsDetected = 0;
        NumberOfTrapsDetectedByPrey = 0;
        NumberOfTrapsDetectedByHunter = 0;

        NumberOfPlantsEaten = 0;
        NumberOfPlantsEatenByHunter = 0;
        NumberOfPlantsEatenByPrey = 0;
        NumberOfPlantsDetectedByPrey = 0;
        NumberOfPlantsDetectedByHunter = 0;
        NumberOfPlantsDetected = 0;

        NumberOfWinsByHunter = 0;
        NumberOfWinsByPrey = 0;
        NumberOfPreysEaten = 0;

    }

    public void SummarizeGeneration(string fileName)
    {
        WriteSummary(fileName, SummarizeTraps()+SummarizePlants()+ SummarizeResults());
    }

    public void WriteSummary(string fileName, string content)
    {
        File.WriteAllText("../"+ fileName + ".txt",content);
    }
}

