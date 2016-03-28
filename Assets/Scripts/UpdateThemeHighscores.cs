using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class UpdateThemeHighscores : MonoBehaviour 
{
    public int[,] starCount = new int[5, 2]; // <------- 2 because of MediaNight - normally 15
    int[] summedUpStarCount = new int[5];
    public Sprite[] themeStars;

    public int amountOfStarsFor1Piece;
    public int amountOfStarsFor2Pieces;
    public int amountOfStarsFor3Pieces;
    public int amountOfStarsFor4Pieces;
    public int amountOfStarsFor5Pieces;


    public Dictionary<string, int> savedHighscores = new Dictionary<string, int>();
    public Text[] highscores = new Text[4];
    int summedUpHighscoreBasel = 0;
    int summedUpHighscoreNewYork = 0;
    int summedUpHighscoreVienna = 0;
    int summedUpHighscoreLondon = 0;

	// Use this for initialization
	void Start () 
    {
        GameController.control.Load();

        starCount = GameController.control.starCount;
        savedHighscores = GameController.control.highscores;

        GameObject.FindObjectOfType<ThemeLevelStorage>().starCount = starCount;

        CheckHighscores();
        CalculateThemeStarImages();
	}

    void CheckHighscores()
    {
        foreach (KeyValuePair<string, int> entry in savedHighscores)
        {
            if(entry.Key.StartsWith("Basel"))
                summedUpHighscoreBasel += entry.Value;
            else if(entry.Key.StartsWith("NewYork"))
                summedUpHighscoreNewYork += entry.Value;
            else if (entry.Key.StartsWith("Vienna"))
                summedUpHighscoreVienna += entry.Value;
            else if (entry.Key.StartsWith("London"))
                summedUpHighscoreLondon += entry.Value; 
        }

        highscores[0].text = summedUpHighscoreBasel.ToString();
        highscores[1].text = summedUpHighscoreNewYork.ToString();
        highscores[2].text = summedUpHighscoreVienna.ToString();
        highscores[3].text = summedUpHighscoreLondon.ToString();
    }

    void CalculateThemeStarImages()
    {
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Theme");
        Array.Sort(foundObjects, CompareObjectNames);

        for (int i = 0; i < starCount.GetLength(0); i++)
        {
            for (int j = 0; j < starCount.GetLength(1); j++)
            {
                summedUpStarCount[i] += starCount[i, j];
            }

			if (i + 1 < foundObjects.Length) //i+1 because i=0 is Pyramide --> all arrays have to be  adjusted because of BERLIN!! 
            {
                GameObject scoreStar = foundObjects[i + 1].transform.Find("Footer").transform.Find("ScoreStar").gameObject;

                if (summedUpStarCount[i] >= amountOfStarsFor5Pieces)
                    scoreStar.GetComponent<Image>().sprite = themeStars[5];
                else if (summedUpStarCount[i] >= amountOfStarsFor4Pieces)
                    scoreStar.GetComponent<Image>().sprite = themeStars[4];
                else if (summedUpStarCount[i] >= amountOfStarsFor3Pieces)
                    scoreStar.GetComponent<Image>().sprite = themeStars[3];
                else if (summedUpStarCount[i] >= amountOfStarsFor2Pieces)
                    scoreStar.GetComponent<Image>().sprite = themeStars[2];
                else if (summedUpStarCount[i] >= amountOfStarsFor1Piece)
                    scoreStar.GetComponent<Image>().sprite = themeStars[1];
                else
                    scoreStar.GetComponent<Image>().sprite = themeStars[0];
            }
        }
    }

    int CompareObjectNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }
}
