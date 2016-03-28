using UnityEngine;
using System.Collections;

public class AndroidBackButtonBehaviour : MonoBehaviour {

    ScreenHandling screenHandler;
    ButtonHandler buttonHandler;
    int lastLevel;

	// Update is called once per frame
	void Update () 
    {
        HandleAndroidBackButton();
    }

    void HandleAndroidBackButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            buttonHandler = GameObject.FindObjectOfType<ButtonHandler>();
            buttonHandler.LoadLastScreen();
        }
    }

    void OnLevelWasLoaded()
    {
        screenHandler = GameObject.FindObjectOfType<ScreenHandling>();

        if (Application.loadedLevel == 0)
            lastLevel = 0;
        else
            lastLevel = screenHandler.GetLastScreenIndex();
    }
}
