using UnityEngine;
using System.Collections;

public class ProjectileHandler : MonoBehaviour {

	public Transform cloudParent;
	public GameObject cloud;

	public static Transform activeBaddy;

	private BaddyStateListener activeBaddyStateListener;
	private Transform[] baddyArray;
	private int numberOfBaddies;
	private int index;
	private int i;

	void Awake () {
		GameController.control.Load ();
		index = 0;
		i = 0;
		int test;
		GameController.control.highscores.TryGetValue ("level2", out test);
		Debug.Log ("Level2: " + test);

		numberOfBaddies = transform.childCount;
		Debug.Log ("Baddies: " + numberOfBaddies);
		baddyArray = new Transform[numberOfBaddies];

		foreach (Transform t in transform) {
			baddyArray[i++] = t;
		}
		activeBaddy = baddyArray[index];
		Resetting.projectile = activeBaddy.rigidbody2D;
		ProjectileFollow.projectile = activeBaddy;
		Resetting.baddies = gameObject;
		ProjectileDragging.clouds = gameObject;
	}

	public void AssignNextBaddy() {
		index++;
		if (index < numberOfBaddies) {
			activeBaddy = baddyArray [index];
			activeBaddyStateListener = activeBaddy.GetComponentInChildren<BaddyStateListener>();
			activeBaddyStateListener.onStateChange(BaddyController.baddyStates.preparing);
			activeBaddy.gameObject.SetActive (true);
			Resetting.projectile = activeBaddy.rigidbody2D;
		} else {
			Debug.Log ("No more baddies");	
		}
	}

	public void DrawNewCloud(Vector3 baddyPosition) {
		GameObject newCloud = Instantiate (cloud) as GameObject;
		newCloud.transform.position = baddyPosition;
		newCloud.transform.parent = cloudParent.transform;
	}

	public void DeleteClouds() {
		Debug.Log ("Delete the old clouds");
		foreach (Transform cloud in cloudParent.transform) {
			Destroy(cloud.gameObject);		
		}
	}
}
