using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarsFadeIn : MonoBehaviour {

    GameObject[] stars = new GameObject[3];

	// Use this for initialization
	void Start () 
    {
        stars[0] = gameObject.transform.FindChild("Star01").gameObject;
        stars[0].SetActive(false);
        stars[1] = gameObject.transform.FindChild("Star02").gameObject;
        stars[1].SetActive(false);
        stars[2] = gameObject.transform.FindChild("Star03").gameObject;
        stars[2].SetActive(false);

        DisplayStars();
	}

    void DisplayStars()
    {
        for (int i = 0; i < Resetting.starsToPass; i++)
            stars[i].SetActive(true);
    }
}
