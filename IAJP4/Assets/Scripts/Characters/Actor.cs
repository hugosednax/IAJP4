using UnityEngine;
using System.Collections;

public abstract class Actor : MonoBehaviour {

    [SerializeField]protected int energy = 100;

    public int Energy
    {
        get { return energy; }
        set { energy = value; }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Death()
    {
        energy = 0;
    }
}
