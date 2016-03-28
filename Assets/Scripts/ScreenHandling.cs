using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenHandling : MonoBehaviour 
{
    public static ScreenHandling handling;

    static Dictionary<int, int> levelIndixes = new Dictionary<int, int>();

    int currentScreenIndex;
    public static int lastScreenIndex;
    string lastScreenName;
    string currentScreenName;

    void Awake()
    {
        if (handling == null)
        {
            DontDestroyOnLoad(gameObject);
            handling = this;
        }
        else if (handling != this)
        {
            Destroy(gameObject);
        }

        if (levelIndixes.Count == 0)
        {
            // first is the levelindex in list and second is levelindex in Unity
            levelIndixes.Add(5, 1); // 5 is the first level (look UnityIndex)
            for (int i = 1; i < Application.levelCount - 5; i++)
            {
                levelIndixes.Add(5 + i, 1 + i);
            }
        }
    }

    void OnLevelWasLoaded()
    {
        currentScreenIndex = Application.loadedLevel;
        currentScreenName = Application.loadedLevelName;
    }

    public void SetLastScreenIndex(int index)
    {
        lastScreenIndex = index;
    }

    public void SetLastScreenName(string name)
    {
        lastScreenName = name;
    }


    public int GetLastScreenIndex()
    {
        return lastScreenIndex;
    }

    public string GetLastScreenName()
    {
        return lastScreenName;
    }

    public int GetCurrentScreenIndex()
    {
        return currentScreenIndex;
    }

    public string GetCurrentScreenName()
    {
        return currentScreenName;
    }
}
