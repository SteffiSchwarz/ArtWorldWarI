using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighscoreManager : MonoBehaviour {
	
	public static int highscore;

	private string levelName;
	Text hightext;                     

	void Start () {
		hightext = GetComponent <Text> ();
		levelName = Application.loadedLevelName; //level1, level2, etc.
		
		if(GameController.control.highscores.ContainsKey(levelName)) {
			//if there is a highscore -> use it
			GameController.control.highscores.TryGetValue(levelName, out highscore);
			Debug.Log ("Highscore initial: " + highscore);
		} else {
			//no highscore set
			highscore = 0;
		}

		hightext.text = "" + highscore;
	}

	void Update () {
		if (ScoreManager.score > highscore) {
			highscore = ScoreManager.score;
			hightext.text = "" + highscore;
		}
	}
}
