using UnityEngine;
using System.Collections;
using System;

public class ButtonHandler : MonoBehaviour
{
    ScreenHandling screenHandler;
    GameController gameController;

    public int[,] starCount;

    string currentLevelName;

    void Start()
    {
        screenHandler = GameObject.FindObjectOfType<ScreenHandling>();
        gameController = GameObject.FindObjectOfType<GameController>();

        if(ThemeLevelStorage.tls)
            starCount = ThemeLevelStorage.tls.starCount;
    }

    public void LoadScreen(string levelName)
    {
        screenHandler.SetLastScreenIndex(Application.loadedLevel);
        screenHandler.SetLastScreenName(Application.loadedLevelName);

        LoadingScreen46.show();
        if (Application.loadedLevelName == "SelectLevel")
            gameController.SetTutorialShown(false);

        Application.LoadLevel(levelName);
    }

    public void LoadLastScreen()
    {
        int currentLevel = Application.loadedLevel;
        currentLevelName = Application.loadedLevelName;

        LoadingScreen46.show();

        //TODO: change if()
        if (currentLevelName == "Basel_level1" || currentLevelName == "Basel_level2" || currentLevelName == "Basel_level3")
            Application.LoadLevel("SelectLevel_Basel");

        else if (currentLevelName == "EnemyGallery")
            Application.LoadLevel(screenHandler.GetLastScreenIndex());

        else if (currentLevel >= 2)
            Application.LoadLevel(currentLevel - 1);

        else
            Application.Quit();
    }

    public void LoadNextLevel()
    {
        int currentLevelIndex = Application.loadedLevel;
        currentLevelName = Application.loadedLevelName;

        if (currentLevelIndex + 1 < Application.levelCount)
        {
            screenHandler.SetLastScreenIndex(currentLevelIndex);
            screenHandler.SetLastScreenName(currentLevelName);

            int themeIndex = Array.IndexOf(SwitchSelectLevelTheme.themeNames, currentLevelName.Split('_')[0]);

            //int x = Convert.ToInt32(currentLevelName.Length);
            string a = currentLevelName.Substring(Math.Max(0, currentLevelName.Length - 1));
            int b = Int32.Parse(a);
            //Debug.Log("x: " + x);
            //Debug.Log("a: " + a);
            //Debug.Log("b: " + b);
            //Debug.Log("starCount.GetLength(1): " + starCount.GetLength(1));

            if (3 > starCount.GetLength(1) && 3 <= (b + 1))
            {
                SwitchSelectLevelTheme.themeName = SwitchSelectLevelTheme.themeNames[themeIndex + 1];
                CutSceneHandler.showNextCutscene = true;
                Application.LoadLevel("SelectLevel");
            }
            else
                Application.LoadLevel(currentLevelIndex + 1);
        }
    }

    public void RestartLevel()
    {
        screenHandler.SetLastScreenIndex(Application.loadedLevel);
        gameController.SetTutorialShown(true);
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ContinueLevelAndCloseCurrentScreen(GameObject currentScreen)
    {
        currentScreen.SetActive(false);
        gameController.SetTutorialShown(true);
    }
}
