using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Resetting : MonoBehaviour {

	public int levelNumber;
	public int pointsTwoStar = 100;
	public int pointsThreeStar = 200;
	public GameObject enemies;

	public static int numberOfEnemies;
	public static int numberOfBaddies;
	public static GameObject baddies;
	public static Rigidbody2D projectile;
	public static int starsToPass;
	public static bool loosing;
    public GameObject levelClearedUI;
    public GameObject levelFailedUI;
    public GameObject pauseButton;

	private SpringJoint2D spring;
	private int gameIndex;
	private ProjectileHandler projectileHandler;
	private bool gameOver;
	private bool changing;

    private int themeIndex;
	
	void Start () {
		loosing = false;
		gameOver = false;
		changing = false;

		spring = projectile.GetComponent <SpringJoint2D>();
		projectileHandler = baddies.GetComponent<ProjectileHandler>();

		numberOfEnemies = enemies.transform.childCount;
		numberOfBaddies = baddies.transform.childCount;

        themeIndex = Array.IndexOf(SwitchSelectLevelTheme.themeNames, SwitchSelectLevelTheme.themeName);  
	}
	
	void Update () {
		CheckWin ();		
		if (Input.GetKeyDown (KeyCode.R)) {
			Reset ();
		}

		if (spring == null && !changing) {
			changing = true;
			StartCoroutine("waitForNextBaddy");	
		}
	}
	
	void OnTriggerExit2D (Collider2D other) {;
		if (other.tag == "Baddy") {
			Resetting.numberOfBaddies--;
			other.gameObject.SetActive (false);
			ProjectileFollow.resetCamera = true;
			ProjectileFollow.fly = false;		
		} else if (other.tag == "Target") {
			Resetting.numberOfEnemies--;
			Destroy (other.gameObject);
			print ("Kill enemy out of view");
		} else {
			Destroy (other.gameObject);	
		}
	}
	
	public void Reset () {
		Application.LoadLevel (Application.loadedLevel);
	}
	
	void NextLevel () {
		gameIndex = Application.loadedLevel;
		gameIndex++;
		if (gameIndex < Application.levelCount) {
			Application.LoadLevel (gameIndex);
		} else {
			Debug.Log ("Game finished");
		}
	}

	public void CheckWin () {
		if (numberOfEnemies == 0 && !gameOver) {
			gameOver = true;
			StartCoroutine("waitShortTime");		
		}
	}

	void winningLevel () {
		for (int i = 0; i < numberOfBaddies; i++) {
			ScoreManager.score += 10000;
		}
		int countStars = 0;

        countStars = GameController.control.starCount[themeIndex, levelNumber];
		//GameController.control.stars.TryGetValue (levelNumber, out countStars);

		if (ScoreManager.score >= pointsThreeStar) {
			starsToPass = 3;
			Debug.Log ("3 Stars");
		}
		else
			if (ScoreManager.score >= pointsTwoStar) {
				starsToPass = 2;
				Debug.Log ("2 Stars");
			}
			else {
				starsToPass = 1;
				Debug.Log ("1 Star");
			}

        if (starsToPass > countStars)
        {
            GameController.control.SetStarsForLevel(themeIndex, levelNumber, starsToPass);
            int nextLevelIndex = levelNumber + 1;

            //if (!(GameController.control.stars.ContainsKey(nextLevelIndex)))
            //{
            //    GameController.control.SetStarsForLevel(themeIndex, nextLevelIndex, 0);
            //}
        }

        //if (starsToPass > countStars) {
        //    GameController.control.SetStarsForLevel (levelNumber, starsToPass);
        //    int nextLevelIndex = levelNumber + 1;
        //    if (!(GameController.control.stars.ContainsKey (nextLevelIndex))) {
        //        GameController.control.SetStarsForLevel (nextLevelIndex, 0);
        //    }
        //}
		LevelFinished ();
		LoadLevelClearedUI ();
	}

	public void NextBaddyOrLose() {
		if (numberOfEnemies >= 0 && numberOfBaddies == 0 && !gameOver) {
			gameOver = true;
			LevelFinished ();
			LoadLevelFailedUI ();
		} else {
			if (projectile != null) {
				Debug.Log ("Neue Baddy");
				projectileHandler.AssignNextBaddy();
				spring = projectile.GetComponent <SpringJoint2D>();
			}	
		}
	}
	
	public void LevelFinished () {
		int test;
		GameController.control.highscores.TryGetValue(Application.loadedLevelName, out test);
		if (ScoreManager.score > test) {
			GameController.control.InsertScore (Application.loadedLevelName, ScoreManager.score);
		}
		
	}

    public void LoadLevelClearedUI()
    {
        if (!levelClearedUI.activeInHierarchy)
        {
            levelClearedUI.SetActive(true);
            pauseButton.GetComponent<Button>().interactable = false;
			levelClearedUI.transform.Find ("Panel").Find ("FinalNumber").GetComponent<FinalScoring>().enabled = true;
        }
    }

    public void LoadLevelFailedUI()
    {
        if (!levelFailedUI.activeInHierarchy)
        {
            levelFailedUI.SetActive(true);
            pauseButton.GetComponent<Button>().interactable = false;
        }
    }

	IEnumerator waitForNextBaddy () {
		yield return new WaitForSeconds (1);
		NextBaddyOrLose ();
		changing = false;
	}

	IEnumerator waitShortTime () {
		yield return new WaitForSeconds (3);
		winningLevel ();
	}
}
