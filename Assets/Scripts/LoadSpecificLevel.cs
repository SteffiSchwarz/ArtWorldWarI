using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadSpecificLevel : MonoBehaviour {

    Button button;
    ButtonHandler bh;
    string themeName;
    string buttonName;
    string extractedLevelName;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => LoadLevel());

        bh = GetComponent<ButtonHandler>();
        themeName = SwitchSelectLevelTheme.themeName;
	}

    void LoadLevel()
    {
        //create the Level name regarding which in which Theme we are and which button is pressed
        buttonName = gameObject.transform.name;
        extractedLevelName = buttonName.Substring(buttonName.LastIndexOf('_'));

        bh.LoadScreen(themeName + extractedLevelName);
    }
}
