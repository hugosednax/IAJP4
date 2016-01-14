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
}