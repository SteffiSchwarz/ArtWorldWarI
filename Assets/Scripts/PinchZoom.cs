using UnityEngine;
using System.Collections;

public class PinchZoom : MonoBehaviour {
		
	public float orthoZoomSpeed = 0.1f;

	private Vector3 newPosition;

	void Update() {
		if (Input.touchCount == 2) {
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			float deltaMagnitudediff = prevTouchDeltaMag - touchDeltaMag;

			camera.orthographicSize += deltaMagnitudediff * orthoZoomSpeed;
			camera.orthographicSize = Mathf.Clamp (camera.orthographicSize, 4, 11);
		}
	}
}
