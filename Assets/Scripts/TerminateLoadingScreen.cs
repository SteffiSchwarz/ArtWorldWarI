using UnityEngine;
using System.Collections;

public class TerminateLoadingScreen : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        //this is cheating and no loading screen.. i know :D
        Invoke("Load", 2);
	}
	
    void Load()
    {
        Application.LoadLevel("StartMenu");
    }
}
