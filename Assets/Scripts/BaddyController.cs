using UnityEngine;
using System.Collections;

public class BaddyController : MonoBehaviour {
	public enum baddyStates {
		right = 0,
		left,
		preparing,
		jump,
		dropping,
		cucumbering,
		readyToFly,
		flying,
		falling,
		rolling,
		dying,
		_stateCount
	}

	public delegate void baddyStateHandler(BaddyController.baddyStates newState);
	public static event baddyStateHandler onStateChange;
}
