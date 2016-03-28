using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwitchSelectLevelTheme : MonoBehaviour {

    public static SwitchSelectLevelTheme sslt;

    public Sprite[] themeImages;
    public Sprite[] themeBanners;

    public static string themeName;
    public static string[] themeNames = new string[5];

	// Use this for initialization
	void Awake () 
    {
        if (sslt == null)
        {
            DontDestroyOnLoad(gameObject);
            themeName = "";

            InitializeThemeNames();

            sslt = this;
        }
        else if (sslt != this)
        {
            Destroy(gameObject);
        }
	}

    void OnLevelWasLoaded()
    {
        if (Application.loadedLevelName == "SelectLevel")
            SetCurrentThemePreferences();
    }

    private void InitializeThemeNames()
    {
        themeNames[0] = "Basel";
        themeNames[1] = "NewYork";
        themeNames[2] = "Vienna";
        themeNames[3] = "London";
        themeNames[4] = "Berlin";
    }

    public void SetThemeNameForSelectLevel(string theme)
    {
        themeName = theme;
    }

    void SetCurrentThemePreferences()
    {
        Debug.Log("themeName: " + themeName);
        switch (themeName)
        {
            case "Basel":
                SetThemObjectImage(0);
                SetThemeObjectBanners(0);
                break;
            case "NewYork":
                SetThemObjectImage(1);
                SetThemeObjectBanners(1);
                break;
            case "Vienna":
                SetThemObjectImage(2);
                SetThemeObjectBanners(2);
                break;
            case "London":
                SetThemObjectImage(3);
                SetThemeObjectBanners(3);
                break;
            case "Berlin":
                SetThemObjectImage(4);
                SetThemeObjectBanners(4);
                break;
        }
    }

    void SetThemObjectImage(int i)
    {
        if(GameObject.Find("Img_ThemeObject"))
            GameObject.Find("Img_ThemeObject").GetComponent<Image>().sprite = themeImages[i];
    }

    void SetThemeObjectBanners(int i)
    {
        if(GameObject.Find("Img_banner"))
            GameObject.Find("Img_banner").GetComponent<Image>().sprite = themeBanners[i];
    }
}
