using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum typeOfPlayer { hunter, prey };

public class HumanPlayManager : MonoBehaviour
{
    public GameObject worldPrefab;

    private PlayableWorld world;
    public typeOfPlayer playerType;

    // Use this for initialization
    void Start()
    {
        GameObject worldPhysical;
            
        worldPhysical = (GameObject)Instantiate(worldPrefab, worldPrefab.transform.position, worldPrefab.transform.rotation);
        worldPhysical.transform.parent = this.transform;
        worldPhysical.transform.name = "Playable World";
        world = worldPhysical.GetComponent<PlayableWorld>();
        world.setGameManager(this);
        playerType = typeOfPlayer.hunter;
    }


    public void EndedWorld()
    {
        Debug.Log("Game Over: ");    
    }

    // Update is called once per frame
    void Update()
    {
            
    }

}

