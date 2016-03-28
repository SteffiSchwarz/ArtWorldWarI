using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Linq;

public class SnapDisplay : MonoBehaviour {

    static GameObject[] themeObjects;
    public GameObject[] dots;
    public Sprite filledCircle;
    public Sprite emptyCircle;
    ScrollPanelHandler sph;

	// Use this for initialization
	void Start () 
    {
        sph = GameObject.Find("ScrollPanelHandler").GetComponent<ScrollPanelHandler>();

        themeObjects = GameObject.FindGameObjectsWithTag("ThemeButton").OrderBy( go => go.name ).ToArray();;

        dots = new GameObject[themeObjects.Length];
        GameObject snapDisplay = GameObject.Find("SnapDisplay");

        for (int i = 0; i < dots.Length; i++)
            dots[i] = snapDisplay.transform.GetChild(i).gameObject;
	}

    // Update is called once per frame
    void Update() 
    {
        UpdateCircleImages();
	}

    void UpdateCircleImages()
    {
        GameObject currentObject = sph.GetCurrentObject();
        int snapIndex = Array.IndexOf<GameObject>(themeObjects, currentObject);

        for (int i = 0; i < dots.Length; i++)
        {
            Sprite currentImage = dots[i].gameObject.GetComponent<Image>().sprite;

            if (i == snapIndex && currentImage != filledCircle)
            {
                dots[i].gameObject.GetComponent<Image>().sprite = filledCircle;
            }
            else if (i != snapIndex && currentImage != emptyCircle)
            {
                dots[i].gameObject.GetComponent<Image>().sprite = emptyCircle;
            }
        }
    }
}
