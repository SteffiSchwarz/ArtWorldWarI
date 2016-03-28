using UnityEngine;
using System.Collections;

public class ParallaxController : MonoBehaviour {

	public GameObject[] nearObjects;
	public GameObject[] farObjects;

	public float nearLayerSpeedModifier;
	public float farLayerSpeedModifier;

	public Camera myCamera;

	private Vector3 lastCamPos;

	void Start () {
		lastCamPos = myCamera.transform.position;
	}

	void Update () {
		Vector3 currCamPos = myCamera.transform.position;
		float xPosDiff = lastCamPos.x - currCamPos.x;

		adjustParallaxPositionsForArray (nearObjects, nearLayerSpeedModifier, xPosDiff);
		adjustParallaxPositionsForArray (farObjects, farLayerSpeedModifier, xPosDiff);

		lastCamPos = myCamera.transform.position;
	}

	void adjustParallaxPositionsForArray(GameObject[] layerArray, float layerSpeedModifier, float xPosDiff) {
		for (int i = 0; i < layerArray.Length; i++) {
			Vector3 objPos = layerArray[i].transform.position;
			objPos.x += xPosDiff * layerSpeedModifier;
			layerArray[i].transform.position = objPos;
		}
	}
}
