using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour 
{
    LevelPause lp;

    void Start()
    {
        lp = GameObject.FindObjectOfType<LevelPause>();

        Debug.Log(GameController.turorialWasActive);

        if (GameController.turorialWasActive)
        {
            gameObject.SetActive(false);
            lp.UnPauseLevel();
        }
        else
            gameObject.SetActive(true);
    }
}
