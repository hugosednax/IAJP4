using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using WorldDefinition;

public class GeneticWorld : World
{
    void Awake(){
        isGeneticWorld = true;
    }
    public GameManager manager { get; set; }

    void Start()
    {
        base.Start();
    }

    public void EndGame()
    {
        manager.EndedWorld(id);
    }

    void Update()
    {
        //DebugDoubles(false);
        elapsedTime += Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.E))
        {
            toEnd = true;
        }
        //GAME CYCLE
        if (elapsedTime > tickTimer && !saved)
        {
            elapsedTime = 0f;
            if (hunter.Energy > 0 && prey.Energy > 0)
            {
                if (turn == 0)
                    prey.Turn();
                else hunter.Turn();
                turn = (turn + 1) % 2;
            }
            else
            {
                if (toEnd)
                {
                    hunter.SaveResults(id);
                    prey.SaveResults(id);
                    saved = true;
                }
                EndGame();
            }
        }
    }

    public override byte detectCellNearby(int x, int y, int distance, typeOfCell typeToDetect)
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

                    if (manager.logSummaryInfo)
                    {
                        if (typeToDetect == typeOfCell.plant)
                        {
                            manager.summaryPrinter.NumberOfPlantsDetected++;
                            if (GetTypeOfCell(x, y) == typeOfCell.hunter)
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
}