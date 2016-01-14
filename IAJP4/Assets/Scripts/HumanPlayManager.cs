using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public enum typeOfPlayer { hunter, prey };

public class HumanPlayManager : MonoBehaviour
{
    public GameObject worldPrefab;

    private PlayableWorld world;
    private GameObject worldPhysical;
    public typeOfPlayer playerType;
    public GameObject preSelectionCanvas;
    public GameObject postSelectionCanvas;
    // Use this for initialization

    private Text[] roleTexts;
    private Text[] energyTexts;
    private typeOfPlayer enemyType;

    void Awake()
    {
        roleTexts = new Text[2];
        energyTexts = new Text[2];

        roleTexts[0] = postSelectionCanvas.transform.FindChild("PlayerRole").GetComponent<Text>();
        roleTexts[1] = postSelectionCanvas.transform.FindChild("EnemyRole").GetComponent<Text>();
        energyTexts[0] = postSelectionCanvas.transform.FindChild("PlayerEnergy").GetComponent<Text>();
        energyTexts[1] = postSelectionCanvas.transform.FindChild("EnemyEnergy").GetComponent<Text>();
    }

    void Start()
    {
        setPlayerAsHunter();
    }


    public void Initialize()
    {
        worldPhysical = (GameObject)Instantiate(worldPrefab, worldPrefab.transform.position, worldPrefab.transform.rotation);
        worldPhysical.transform.parent = this.transform;
        worldPhysical.transform.name = "Playable World";
        world = worldPhysical.GetComponent<PlayableWorld>();
        world.setGameManager(this);
        preSelectionCanvas.SetActive(false);
        postSelectionCanvas.SetActive(true);
        SetRoleText();
    }

    private void SetRoleText()
    {
        roleTexts[0].text = "Player role: " + playerType;
        roleTexts[1].text = "Enemy role: " + enemyType;
    }

    private void SetEnergyText()
    {
        if (world.player != null && world.enemy != null)
        {
            energyTexts[0].text = "Player energy: " + world.player.Energy;
            energyTexts[1].text = "Enemy energy: " + world.enemy.Energy;
        }
    }

    public void setPlayerAsHunter()
    {
        playerType = typeOfPlayer.hunter;
        enemyType = typeOfPlayer.prey;
    }

    public void setPlayerAsPrey()
    {
        playerType = typeOfPlayer.prey;
        enemyType = typeOfPlayer.hunter;
    }

    public void EndedWorld()
    {
        Debug.Log("Game Over: ");    
    }

    // Update is called once per frame
    void Update() //having energy being updated is suboptimal, but changing it to "only update when energy is updated" requires the actor to know what type of world he is playing in
    { 
        if(postSelectionCanvas.activeSelf)
            SetEnergyText();

        if (Input.GetKey(KeyCode.T))
        {
            Application.LoadLevel("PlayableScene");
        }
    }

}

