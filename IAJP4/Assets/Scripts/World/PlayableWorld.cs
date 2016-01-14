using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using WorldDefinition;
//using UnityEngine.SceneManagement;

public class PlayableWorld : World
{
    void Awake()
    {
        isGeneticWorld = false;
    }

    public HumanPlayManager manager { get; set; }
    public Actor player {get; set;}
    public Actor enemy { get; set; }

    void Start()
    {
        base.Start();
        if (manager.playerIsHunter)
        {
            player = hunter;
            enemy = prey;
        }
        else
        {
            player = prey;
            enemy = hunter;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            ResetWorld();
        }

        elapsedTime += Time.deltaTime;
        
        //GAME CYCLE
        if (elapsedTime > tickTimer)
        {
            elapsedTime = 0f;
            if (hunter.Energy > 0 && prey.Energy > 0)
            {
                if (turn == 0)
                {
                    #region ActionKeys
                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        if (ActionManager.CanExecute(0, player, this))
                        {
                            ActionManager.Execute(0, player, this);
                            turn = (turn + 1) % 2;
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.A))
                    {
                        if (ActionManager.CanExecute(1, player, this))
                        {
                            ActionManager.Execute(1, player, this);
                            turn = (turn + 1) % 2;
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.S))
                    {
                        if (ActionManager.CanExecute(3, player, this))
                        {
                            ActionManager.Execute(3, player, this);
                            turn = (turn + 1) % 2;
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.D))
                    {
                        if (ActionManager.CanExecute(2, player, this))
                        {
                            ActionManager.Execute(2, player, this);
                            turn = (turn + 1) % 2;
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.X))
                    {
                        if (ActionManager.CanExecute(4, player, this))
                        {
                            ActionManager.Execute(4, player, this);
                            turn = (turn + 1) % 2;
                        }
                    }
                    else if (player.type == Actor.typeofActor.hunter)
                    {
                        if (Input.GetKeyUp(KeyCode.UpArrow))
                        {
                            if (ActionManager.CanExecute(5, player, this))
                            {
                                ActionManager.Execute(5, player, this);
                                turn = (turn + 1) % 2;
                            }
                        }
                        else if (Input.GetKeyUp(KeyCode.LeftArrow))
                        {
                            if (ActionManager.CanExecute(6, player, this))
                            {
                                ActionManager.Execute(6, player, this);
                                turn = (turn + 1) % 2;
                            }
                        }
                        else if (Input.GetKeyUp(KeyCode.DownArrow))
                        {
                            if (ActionManager.CanExecute(8, player, this))
                            {
                                ActionManager.Execute(8, player, this);
                                turn = (turn + 1) % 2;
                            }
                        }
                        else if (Input.GetKeyUp(KeyCode.RightArrow))
                        {
                            if (ActionManager.CanExecute(7, player, this))
                            {
                                ActionManager.Execute(7, player, this);
                                turn = (turn + 1) % 2;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    enemy.Turn();
                    turn = (turn + 1) % 2;
                }
            }
            else
            {
                Debug.Log("Game Over, Winner: " + (player.Energy > 0 ? "Human Player" : "Bot"));
                //SceneManager.LoadScene("PlayableScene");
                Application.LoadLevel("PlayableScene");
            }
        }
    }

}
