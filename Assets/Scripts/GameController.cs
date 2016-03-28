using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour {
	public static GameController control;

	public Dictionary<string, int> highscores = new Dictionary<string, int>();
    public int[,] starCount = new int[5,2];
	//public Dictionary<int, int> stars = new Dictionary<int, int>();
	public List<string> characters = new List<string>();
    public static bool turorialWasActive;

	void Awake () {
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy (gameObject);	
		}
	}

	public void InsertScore(string levelKey, int scoreValue) {
		// Insert 
		if(highscores.ContainsKey(levelKey)) {
			highscores[levelKey] = scoreValue;
		} else {
			highscores.Add (levelKey, scoreValue);
		}
		Save ();
	}

	public void SetCharacterDestroyed(string characterName) {
		if (!(characters.Contains (characterName))) {
			characters.Add (characterName);	
		}
		Save ();
	}

    public void SetStarsForLevel(int themeIndex, int levelKey, int starNumber)
    {
        starCount[themeIndex, levelKey] = starNumber;
		Save ();
	}

    //public void SetStarsForLevel(int levelKey, int starNumber)
    //{
    //    if (stars.ContainsKey(levelKey))
    //    {
    //        stars[levelKey] = starNumber;
    //    }
    //    else
    //    {
    //        stars.Add(levelKey, starNumber);
    //    }
    //    Save();
    //}

    public void SetTutorialShown(bool shown)
    {
        turorialWasActive = shown;
    }

	public void Save() {
		// not working on web! need to use webserver
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/levelInfo.dat");

		LevelData data = new LevelData ();
		data.highscores = highscores;
		data.characters = characters;
        data.starCount = starCount;
		//data.stars = stars;

		bf.Serialize (file, data);
		file.Close();
	}

	public void Load() {
		if (File.Exists (Application.persistentDataPath + "/levelInfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/levelInfo.dat", FileMode.Open);
			LevelData data = (LevelData)bf.Deserialize (file);
			file.Close ();

			highscores = data.highscores;
			characters = data.characters;
            starCount = data.starCount;
		}
	}
}

[Serializable]
class LevelData {
	public Dictionary<string, int> highscores;
	//public Dictionary<int, int> stars;
    public int[,] starCount;
	public List<string> characters;
}
