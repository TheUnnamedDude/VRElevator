using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public bool IsRunning
    {
        get;
        set;
    }

    // Use this for initialization
    void Start () {
        IsRunning = true;
	}
	
	// Update is called once per frame
	void Update () {
            
    }

    public void OnTargetDestroy()
    {
        Debug.Log(GameObject.FindGameObjectsWithTag("Target").Length);
        
        if (GameObject.FindGameObjectsWithTag("Target").Length <= 1) //Off by 1 so default set to 1 so we can destroy the object after running this method
        {
            Debug.Log("You cleared this level!");
        }
    }

}
