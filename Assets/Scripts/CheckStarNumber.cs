using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class CheckStarNumber : MonoBehaviour 
{
    public Sprite playedLevelButton;
	public List<GameObject> levels;
	Dictionary<int, int> starsPerLevel = new Dictionary<int,int>();
	GameObject levelButton;
    GameObject[] stars = new GameObject[3];

	void Start()
	{
		GameController.control.Load ();

        int themeIndex = Array.IndexOf(SwitchSelectLevelTheme.themeNames, SwitchSelectLevelTheme.themeName);

        for (int j = 0; j < GameController.control.starCount.GetLength(1); j++)
        {
            starsPerLevel.Add(j, GameController.control.starCount[themeIndex, j]);
            Debug.Log("StarsPerLevel: " + starsPerLevel[j]);
        }

        //starsPerLevel = GameController.control.stars;
		SetStarsPerLevel();
	}
	
	void SetStarsPerLevel()
	{
        for (int i = 0; i < levels.Count; i++)
        {
            stars[0] = levels[i].transform.FindChild("LevelStars").FindChild("star01").gameObject;
            stars[0].SetActive(false);
            stars[1] = levels[i].transform.FindChild("LevelStars").FindChild("star02").gameObject;
            stars[1].SetActive(false);
            stars[2] = levels[i].transform.FindChild("LevelStars").FindChild("star03").gameObject;
            stars[2].SetActive(false);

            int starsCount;
            starsPerLevel.TryGetValue(i + 1, out starsCount);

            for (int j = 0; j < starsCount; j++)
            {
                stars[j].SetActive(true);

                levels[i].GetComponent<Image>().sprite = playedLevelButton;
            }
            UnlockNextLevel(i, starsCount);
        }
    }

    void UnlockNextLevel(int i, int starsCount)
    {
        if (i + 1 < levels.Count)
        {
            string levelName = levels[i + 1].name;

            Debug.Log("levelName: " + levelName);
            Debug.Log("starCount: " + starsCount);

            if (starsCount >= 1)
            {
                levelButton = levels.Find(item => item.name == levelName);
                levelButton.GetComponent<Button>().interactable = true;
                levelButton.tag = "Button_unlocked";
                levelButton.transform.Find("Img_scull01").gameObject.SetActive(false);
                levelButton.transform.Find("Img_scull02").gameObject.SetActive(false);
                levelButton.transform.Find("Img_lock").gameObject.SetActive(false);
            }
        }
    }
}
