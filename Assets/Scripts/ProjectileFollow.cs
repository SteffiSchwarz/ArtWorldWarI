using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileFollow : MonoBehaviour {
	
	public static bool fly;
	public static bool resetCamera;
	public static Transform projectile;
	
	public float speed = 0.75f;
	public float minSwipeDistX;

	public Transform farLeft;
	public Transform farRight;
	public Transform center;

	private Vector2 touchStartPosition;
	private Vector3 startPosition;
	private Vector3 newPosition;

	void Awake () {
		newPosition = transform.position;
	}

	void Start () {
		fly = false;
		resetCamera = true;
		startPosition = projectile.position;
		newPosition.x = startPosition.x;
		StartCoroutine ("waitSomeTime");
	}
	
	void Update () {
		if (camera.orthographicSize == 11) {
			transform.position = center.position;	
		} else {
			PositionChanging (); 		
		}
	}

	float calculateRatio ()
	{
		return (11 * ((camera.orthographicSize * camera.orthographicSize * camera.orthographicSize) / (11 * 11 * 11)));
	}

	void PositionChanging () {
		if (Input.touchCount > 0 && Input.touchCount != 2) {
			resetCamera = false;
			Touch touch = Input.touches[0];
			
			switch (touch.phase) {
				case TouchPhase.Began:
					touchStartPosition = touch.position;
					break;
				
				case TouchPhase.Ended:
					float swipeDistHorizontal = (new Vector3(touch.position.x,0, 0) - new Vector3(touchStartPosition.x, 0, 0)).magnitude;
					if (swipeDistHorizontal > minSwipeDistX) {
						float swipeValue = Mathf.Sign(touch.position.x - touchStartPosition.x);
						if (swipeValue > 0) {
							//left swipe
							if(camera.orthographicSize < 6) {
								newPosition.x = startPosition.x;
							} else {
								newPosition.x = center.position.x - 11 + calculateRatio ();
								newPosition.x = Mathf.Clamp(newPosition.x, startPosition.x, center.position.x);
							}
						} else if (swipeValue < 0) {
							//right swipe
							newPosition.x = center.position.x + 11 - calculateRatio ();
							newPosition.x = Mathf.Clamp(newPosition.x, center.position.x, farRight.position.x);
						}
					}
					break;
			}
		}

		if (fly && camera.orthographicSize < 11) {
			newPosition.x = projectile.position.x; //We only want the camera move to the left
			newPosition.x = Mathf.Clamp (newPosition.x, farLeft.position.x, farRight.position.x);
			transform.position = newPosition;
		} else if (resetCamera && camera.orthographicSize < 11) {
			if (transform.position.x >= startPosition.x + 1) {
				resetCamera = false;
			}
			newPosition.x = startPosition.x;
			transform.position = Vector3.Lerp (transform.position, newPosition, Time.deltaTime);
		} else {
			transform.position = Vector3.Lerp (transform.position, newPosition, Time.deltaTime);	
		}
	}

	IEnumerator waitSomeTime () {
		yield return new WaitForSeconds (3);
		transform.position = Vector3.Lerp (transform.position, newPosition, Time.deltaTime);
	}
}

