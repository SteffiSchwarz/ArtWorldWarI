using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnlockEnemies : MonoBehaviour {

    List<GameObject> enemies = new List<GameObject>();
	List<string> unlockedEnemies;

	// Use this for initialization
	void Awake () 
    {
        enemies.AddRange(GameObject.FindGameObjectsWithTag("EnemyUnlocked"));

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetActive(false);
        }
	}

    void OnLevelWasLoaded()
    {
		GameController.control.Load ();
		unlockedEnemies = GameController.control.characters;
        FindKilledEnemies();
    }

    //find all killed enemies and display them
    void FindKilledEnemies()
    {
        for (int i = 0; i < unlockedEnemies.Count; i++)
        {
            string enemyName = unlockedEnemies[i];
            if (unlockedEnemies.Contains(enemyName))
            {
                enemies.Find(item => item.name == enemyName).SetActive(true);
            }
        }
    }
}
