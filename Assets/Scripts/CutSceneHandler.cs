using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class CutSceneHandler : MonoBehaviour 
{
    public static CutSceneHandler csh;

    GameObject[] cutScenesArray;
    string currentCutScene;
	static Dictionary<string, bool> cutScenes;
    public static bool showNextCutscene;

	// Use this for initialization
	void Awake () 
    {
        if (csh == null)
        {
            DontDestroyOnLoad(gameObject);
            csh = this;

			cutScenes = new Dictionary<string, bool>();
			
			cutScenesArray = new GameObject[5];
			cutScenesArray = GameObject.FindGameObjectsWithTag("CutScene");
			
			for (int i=0; i<cutScenesArray.Length; i++)
			{
                string temp = GameObject.Find(cutScenesArray[i].name).ToString();
                cutScenes.Add(temp, false);
			}
        }
        else if (csh != this)
        {
            Destroy(gameObject);
        }

		SelectCutScene();
	}

    void SelectCutScene()
    {
        bool viewed = false;

        currentCutScene = GameObject.Find(SwitchSelectLevelTheme.themeName + "_CutScene").ToString();
        viewed = cutScenes[currentCutScene];

        HandleCutScene(viewed);
    }

    void HandleCutScene(bool viewed)
    {
        int lastScreen = ScreenHandling.lastScreenIndex;
        GameObject currentCutSceneObject = GameObject.Find(SwitchSelectLevelTheme.themeName + "_CutScene");
        
        // entering from the Menu and Cutscene hasn't been displayed -> display correct Cutscene and disable the others
        if (showNextCutscene == true || lastScreen == 2) // && !viewed) <----------------------------------------------------für MediaNight rausgemacht
        {
            currentCutSceneObject.SetActive(true); //activate it
            currentCutSceneObject.transform.FindChild("Canvas").GetComponent<Animator>().gameObject.SetActive(true);

            cutScenes[currentCutScene] = true; //set viewed to true
            GameObject.Find("SelectLevelUI").SetActive(false);

            foreach (string key in cutScenes.Keys)
            {
                string cutSceneName = GetGameObjectSubstring(key);

                if (GameObject.Find(cutSceneName) && GameObject.Find(cutSceneName) != currentCutSceneObject)
                {
                    GameObject cutScene = GameObject.Find(cutSceneName);
                    cutScene.SetActive(false);
                    cutScene.transform.FindChild("Canvas").GetComponent<Canvas>().sortingOrder = -1;
                    cutScene.transform.FindChild("Canvas").GetComponent<Animator>().gameObject.SetActive(false);
                }
            }
            showNextCutscene = false;
        }
        // disable all Cutscenes
        else
        {
            foreach (string key in cutScenes.Keys)
            {
                string cutSceneName = GetGameObjectSubstring(key);

                if (GameObject.Find(cutSceneName))
                {
                    GameObject cutScene = GameObject.Find(cutSceneName);

                    if (cutScene.activeInHierarchy)
                    {
                        cutScene.SetActive(false);
                        cutScene.transform.FindChild("Canvas").GetComponent<Canvas>().sortingOrder = -1;
                    }
                }
            }
        }
    }

    private string GetGameObjectSubstring(string keyString)
    {
        int index = keyString.IndexOf(" ");
        string newKey = "";

        if (index > 0)
        {
            newKey = keyString.Substring(0, index);
        }
        return newKey;
    }
}
