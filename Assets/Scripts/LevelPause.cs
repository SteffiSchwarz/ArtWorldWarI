using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelPause : MonoBehaviour {

    public GameObject pausedLevelUI;
    public Button pauseButton;
    GameObject tutorialUI;

	// Use this for initialization
	void Start () 
    {
        pauseButton = GameObject.Find("LevelUI").GetComponentInChildren<Button>();
        tutorialUI = GameObject.Find("TutorialUI");

        if (tutorialUI)
            PauseLevel();
	}

    void Update()
    {
        if (GameObject.Find("LevelClearedUI") || GameObject.Find("LevelFailedUI"))
            PauseLevel();
    }

    public void ActivatePauseScreen()
    {
        if (!pausedLevelUI.activeInHierarchy)
        {
            PauseLevel();
            pausedLevelUI.SetActive(true);
        }
    }

    public void DeactivatePauseScreen()
    {
        if (pausedLevelUI.activeInHierarchy)
        {
            UnPauseLevel();
            pausedLevelUI.SetActive(false);
        }
    }

    public void PauseLevel()
    {
        GameObject.Find("Main Camera").GetComponent<ProjectileFollow>().enabled = false;
        GameObject.Find("Baddies").transform.GetChild(0).GetComponent<ProjectileDragging>().enabled = false;
        pauseButton.interactable = false;
    }

    public void UnPauseLevel()
    {
        GameObject.Find("Main Camera").GetComponent<ProjectileFollow>().enabled = true;
        GameObject.Find("Baddies").transform.GetChild(0).GetComponent<ProjectileDragging>().enabled = true;
        pauseButton.interactable = true;
    }
}
