using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class BaddyStateListener : MonoBehaviour
{         
	public float playerWalkSpeed = 0.5f;
	public Transform leftBoundary;
	public Transform rightBoundary;
	public Transform jumpPosition;
	public Transform slingPosition;
	public Sprite rollingBaddy;
	public bool firstBaddy;

	private Animator baddyAnimator = null;
	private BaddyController.baddyStates previousState;
	private BaddyController.baddyStates currentState;
	private bool walkingRight;
	private float verticalPosition;

	void OnEnable()
	{
		BaddyController.onStateChange += onStateChange;
	}
	
	void OnDisable()
	{
		BaddyController.onStateChange -= onStateChange;
	}
	
	void Start()
	{
		walkingRight = true;
		previousState = BaddyController.baddyStates.right;
		currentState = BaddyController.baddyStates.right;

		baddyAnimator = GetComponent<Animator> ();
		if (firstBaddy) {
			previousState = BaddyController.baddyStates.readyToFly;
			currentState = BaddyController.baddyStates.readyToFly;
			baddyAnimator.CrossFade("BaddyFlying", 0f);
		}
	}
	
	void LateUpdate()
	{
		onStateCycle();
	}

	// Every cycle of the engine, process the current state.
	void onStateCycle()
	{
		// Grab the current localScale of the object so we have 
		// access to it in the following code
		Vector3 localScale = transform.localScale;
		
		transform.localEulerAngles = Vector3.zero;
		
		switch(currentState)
		{

		case BaddyController.baddyStates.right:
			transform.parent.Translate(new Vector3(playerWalkSpeed * Time.deltaTime, 0.0f, 0.0f));
			
			if(transform.parent.position.x > rightBoundary.position.x && walkingRight)
			{
				onStateChange(BaddyController.baddyStates.left);
				localScale.x *= -1.0f;
				transform.localScale = localScale;              
			}
			
			randomAnimation ();
			
			break;

			
		case BaddyController.baddyStates.left:
			transform.parent.Translate(new Vector3((playerWalkSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f));
			
			if(transform.parent.position.x < leftBoundary.position.x && !walkingRight)
			{
				onStateChange(BaddyController.baddyStates.right);
				localScale.x *= -1.0f;
				transform.localScale  = localScale;
			}

			randomAnimation ();
			
			break;

		case BaddyController.baddyStates.preparing:
			StartCoroutine("waitForPreparation");
			break;

		case BaddyController.baddyStates.jump:
			// change position of baddy to gameobject lerp
			transform.parent.position = Vector3.Lerp (transform.parent.position, jumpPosition.position, Time.deltaTime * 3.0f);
			if (transform.parent.position.x >= jumpPosition.position.x - 1.0f && transform.parent.position.y >= jumpPosition.position.y - 1.0f) {
				onStateChange(BaddyController.baddyStates.dropping);
			}
			break;
			
		case BaddyController.baddyStates.dropping:
			// fall down from position to starting point
			transform.parent.position = Vector3.Lerp (transform.parent.position, slingPosition.position, Time.deltaTime * 2.0f);
			if (transform.parent.position.x >= slingPosition.position.x - 0.25f && transform.parent.position.y <= slingPosition.position.y + 0.25f) {
				transform.parent.position = slingPosition.position;
				onStateChange(BaddyController.baddyStates.cucumbering);
			}
			break;
			
		case BaddyController.baddyStates.cucumbering:
			StartCoroutine("waitForCucumber");
			break;              
			
		case BaddyController.baddyStates.readyToFly:
			break;         
			
		case BaddyController.baddyStates.flying:
			if (transform.parent.position.y < verticalPosition) {
				baddyAnimator.SetBool("Flying", false);
				baddyAnimator.SetBool("Falling", true);
			}
			verticalPosition = transform.parent.position.y;
			break;
			
		case BaddyController.baddyStates.falling:
			// wait for baddy to collide or win height and show upper skirt animation
			if (transform.parent.position.y > verticalPosition) {
				baddyAnimator.SetBool("Falling", false);
				baddyAnimator.SetBool("Flying", true);
			}
			verticalPosition = transform.parent.position.y;
			break;

		case BaddyController.baddyStates.rolling:
			break;
		
		case BaddyController.baddyStates.dying:
			break;
		}
	}
	
	// onStateChange is called whenever we make a change to Baddy's state 
	// from anywhere within the game's code.
	public void onStateChange(BaddyController.baddyStates newState)
	{
		// If the current state and the new state are the same, abort - no need 
		// to change to the state we're already in.
		if(newState == currentState)
			return;
		
		// Check if the current state is allowed to transition into this state. If it's not, abort.
		if(!checkForValidStatePair(newState))
			return;
		
		// Having reached here, we now know that this state change is allowed. 
		// So let's perform the necessary actions depending on what the new state is.
		switch(newState)
		{
		case BaddyController.baddyStates.right:
			walkingRight = true;
			break;
		case BaddyController.baddyStates.left:
			walkingRight = false;
			break;
		case BaddyController.baddyStates.preparing:
			Vector3 localScale = transform.localScale;
			if(transform.localScale.x < 0) {
				localScale.x *= -1.0f;
				transform.localScale = localScale;
			}
			baddyAnimator.SetBool("Preparing", true);
			break;
		case BaddyController.baddyStates.jump:
			break;
		case BaddyController.baddyStates.dropping:
			break;
		case BaddyController.baddyStates.cucumbering:
			baddyAnimator.SetBool("Cucumbering", true);
			break;
		case BaddyController.baddyStates.readyToFly:
			transform.parent.GetComponent<CircleCollider2D> ().enabled = true;
			transform.parent.GetComponent<ProjectileDragging>().enabled = true;
			baddyAnimator.SetBool("Flying", true);
			break;
		case BaddyController.baddyStates.flying:
			verticalPosition = transform.parent.position.y;
			break;
		case BaddyController.baddyStates.falling:
			verticalPosition = transform.parent.position.y;
			break;
		case BaddyController.baddyStates.rolling:
			baddyAnimator.SetBool ("Flying", false);
			baddyAnimator.SetBool ("Falling", false);
			baddyAnimator.SetBool ("Rolling", true);
			baddyAnimator.enabled = false;
			GetComponent<SpriteRenderer>().sprite = rollingBaddy;
			break;
		case BaddyController.baddyStates.dying:
			// Run dying animation
			Resetting.numberOfBaddies--;
			if (Resetting.numberOfBaddies == 0 && Resetting.numberOfEnemies != 0) {
				Resetting.loosing = true;
			}
			break;
		}
		
		// Store the current state as the previous state
		previousState = currentState;
		
		// And finally, assign the new state to the player object
		currentState = newState;
	}    
	
	// Compare the desired new state against the current, and see if we are 
	// allowed to change to the new state. This is a powerful system that ensures 
	// we only allow the actions to occur that we want to occur.
	bool checkForValidStatePair(BaddyController.baddyStates newState)
	{
		bool returnVal = false;
		
		// Compare the current against the new desired state.
		switch (currentState) {
				case BaddyController.baddyStates.right:
				// The only states that can take over from right are left and preparing
						if (
								newState == BaddyController.baddyStates.left
								|| newState == BaddyController.baddyStates.preparing
								|| newState == BaddyController.baddyStates.readyToFly
						)
								returnVal = true;
						else
								returnVal = false;
						break;
			
				case BaddyController.baddyStates.left:
				// The only states that can take over from left are right and preparing
						if (
								newState == BaddyController.baddyStates.right
								|| newState == BaddyController.baddyStates.preparing
								|| newState == BaddyController.baddyStates.readyToFly
						)
								returnVal = true;
						else
								returnVal = false;
						break;
			
				case BaddyController.baddyStates.preparing:
				// The only state that can take over from preparing is jump
						if (newState == BaddyController.baddyStates.jump)
								returnVal = true;
						else 
								returnVal = false;
						break;
			
				case BaddyController.baddyStates.jump:
				// The only state that can take over from jump is dropping
						if (newState == BaddyController.baddyStates.dropping)
								returnVal = true;
						else
								returnVal = false;
						break;

				case BaddyController.baddyStates.dropping:
				// The only state that can take over from dropping is cucumbering
						if (newState == BaddyController.baddyStates.cucumbering)
								returnVal = true;
						else
								returnVal = false;
						break;
		
				case BaddyController.baddyStates.cucumbering:
				// The only state that can take over from cucumbering is readyToFly
						if (newState == BaddyController.baddyStates.readyToFly)
								returnVal = true;
						else
								returnVal = false;
						break;

				case BaddyController.baddyStates.readyToFly:
				// The only state that can take over from readyToFly is flying
						if (newState == BaddyController.baddyStates.flying)
								returnVal = true;
						else
								returnVal = false;
						break;
		
				case BaddyController.baddyStates.flying:
				// The only state that can take over from flying are falling and dying and rolling
						if (newState == BaddyController.baddyStates.falling 
			    				|| newState == BaddyController.baddyStates.rolling
			    				|| newState == BaddyController.baddyStates.dying)
								returnVal = true;
						else
								returnVal = false;
						break;

				case BaddyController.baddyStates.falling:
				// The only state that can take over from falling are flying, rolling and dying
						if (newState == BaddyController.baddyStates.rolling
								|| newState == BaddyController.baddyStates.flying
			    				|| newState == BaddyController.baddyStates.dying)
								returnVal = true;
						else
								returnVal = false;
						break;

				case BaddyController.baddyStates.rolling:
				// The only state that can take over from rolling is dying
						if (newState == BaddyController.baddyStates.dying)
								returnVal = true;
						else
								returnVal = false;
						break;

				case BaddyController.baddyStates.dying:
				// No new state allowed
						break;
		
		}
		return returnVal;
	}

	void randomAnimation () {
		int random = Random.Range (0, 3000);
		if (random <= 10) {
			baddyAnimator.SetTrigger ("Blinking");		
		} else if (random >= 10 && random <= 20) {
			baddyAnimator.SetTrigger ("Adjusting");	
		}
	}

	private IEnumerator waitForPreparation() {
		yield return new WaitForSeconds (1.1f);
		baddyAnimator.SetBool ("Preparing", false);
		onStateChange (BaddyController.baddyStates.jump);
	}

	private IEnumerator waitForCucumber() {
		yield return new WaitForSeconds (1.0f);
		baddyAnimator.SetBool ("Cucumbering", false);
		onStateChange (BaddyController.baddyStates.readyToFly);
	}
}
