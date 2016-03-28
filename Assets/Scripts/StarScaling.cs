using UnityEngine;
using System.Collections;

public class StarScaling : MonoBehaviour {

	void Update () {
		if (transform.localScale.x < 1.0f) {
			transform.localScale = new Vector3 (transform.localScale.x * 1.10f, transform.localScale.y * 1.10f, 1.0f);
		}
	}
}
