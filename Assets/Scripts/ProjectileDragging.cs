using UnityEngine;
using System.Collections;

public class ProjectileDragging : MonoBehaviour {

	public float maxStretch = 2.0f;
	public float killSpeed = 0.25f;
	public float cloudDistance = 1.0f;
	public LineRenderer slingLineFront;
	public LineRenderer slingLineBack;
	public AudioClip dragSlingSound;
	public AudioClip releaseSlingSound;

	public static GameObject clouds;

	private bool groundHit = false;
	private SpringJoint2D spring;
	private Transform slingshot;
	private Ray rayToMouse;
	private Ray leftSlingToProjectile;
	private float maxStretchSqr;
	private bool clickedOn;
	private Vector2 prevVelocity;
	private float baddyRadius;
	private CircleCollider2D circle;
	private Vector3 lastCloud;
	private ProjectileHandler projectileHandler;
	private SpriteRenderer spriteRenderer;
	private Vector3 startPosition;
	private float killSpeedSqr;
	private BaddyStateListener baddyStateListener;

	void Awake () {
		spring = GetComponent <SpringJoint2D> ();
		slingshot = spring.connectedBody.transform;
	}
	
	void Start () {
		spriteRenderer = GetComponentInChildren <SpriteRenderer> ();
		spriteRenderer.sortingOrder = 5;

		groundHit = false;

		rayToMouse = new Ray (slingshot.position, Vector3.zero);
		leftSlingToProjectile = new Ray (slingLineFront.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
		circle = collider2D as CircleCollider2D;
		baddyRadius = circle.radius;
		lastCloud = transform.position;
		startPosition = transform.position;
		killSpeedSqr = killSpeed * killSpeed;

		baddyStateListener = GetComponentInChildren<BaddyStateListener>();
		projectileHandler = clouds.GetComponent<ProjectileHandler>();
		LineRendererSetup ();
	}

	void Update () {
		if (clickedOn) {
			Dragging ();
		}

		if (groundHit && transform.rigidbody2D.velocity.sqrMagnitude < killSpeedSqr) {
			StartCoroutine("killBaddy");
			baddyStateListener.onStateChange(BaddyController.baddyStates.dying);
		}

		if (spring != null) {
			if (!rigidbody2D.isKinematic && prevVelocity.sqrMagnitude > rigidbody2D.velocity.sqrMagnitude) {
				Destroy(spring);
				rigidbody2D.velocity = prevVelocity;
			}

			if (!clickedOn) {
				prevVelocity = rigidbody2D.velocity;
			}
			LineRendererUpdate();
		} else if (!groundHit) {

			if ((Mathf.Round(transform.position.x) > lastCloud.x + cloudDistance) || (Mathf.Round(transform.position.y) > lastCloud.y + cloudDistance)) {
				lastCloud = transform.position;
				projectileHandler.DrawNewCloud(transform.position);
			}
		}
	}

	void LineRendererSetup () {
		slingLineFront.SetPosition (0, slingLineFront.transform.position);
		slingLineBack.SetPosition (0, slingLineBack.transform.position);

		slingLineFront.sortingLayerName = "Default";
		slingLineBack.sortingLayerName = "Default";

		slingLineFront.sortingOrder = 6;
		slingLineBack.sortingOrder = 4;

		slingLineFront.enabled = true;
		slingLineBack.enabled = true;
	}

	void OnMouseDown () {
		if (PlayerPrefs.GetInt ("Sound") == 1 || !PlayerPrefs.HasKey("Sound")) {
			AudioSource.PlayClipAtPoint (dragSlingSound, transform.position);
		}
		spring.enabled = false;
		clickedOn = true;
	}

	void OnMouseUp () {
		Vector3 resetPosition = transform.position - startPosition;
		if (resetPosition.sqrMagnitude > 1.5f) {
			if (PlayerPrefs.GetInt ("Sound") == 1 || !PlayerPrefs.HasKey ("Sound")) {
				AudioSource.PlayClipAtPoint (releaseSlingSound, transform.position);
			}
			ProjectileFollow.projectile = ProjectileHandler.activeBaddy;
			circle.radius = 0.7f;
			ProjectileFollow.fly = true;
			spring.enabled = true;
			rigidbody2D.isKinematic = false;
			clickedOn = false;
			projectileHandler.DeleteClouds ();
			slingLineFront.enabled = false;
			slingLineBack.enabled = false;
			baddyStateListener.onStateChange(BaddyController.baddyStates.flying);
		} else {
			transform.position = startPosition;
			spring.enabled = true;
			clickedOn = false;
		}

	}

	void OnCollisionEnter2D(Collision2D other) { 
		groundHit = true;
		baddyStateListener.onStateChange (BaddyController.baddyStates.rolling);
	}

	void OnCollisionStay2D(Collision2D other) {
		if (other.collider.name == "Platform") {
			if(rigidbody2D.velocity.x > 0.4f || rigidbody2D.velocity.x < -0.4f) {
				rigidbody2D.velocity = rigidbody2D.velocity * 0.97f;
			} else {
				rigidbody2D.velocity = rigidbody2D.velocity * 0.0f;
			}		
		}
	}

	void Dragging () {
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 slingshotToMouse = mouseWorldPoint - slingshot.position;

		if (slingshotToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = slingshotToMouse;
			mouseWorldPoint = rayToMouse.GetPoint (maxStretch);
		}

		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}

	void LineRendererUpdate () {
		Vector2 slingshotToProjectile = transform.position - slingLineFront.transform.position;
		leftSlingToProjectile.direction = slingshotToProjectile;

		Vector3 holdPoint = leftSlingToProjectile.GetPoint (slingshotToProjectile.magnitude + baddyRadius - (circle.radius - 0.4f));
		slingLineFront.SetPosition (1, holdPoint);
		slingLineBack.SetPosition (1, holdPoint);
	}

	IEnumerator killBaddy () {
		yield return new WaitForSeconds (2);
		transform.gameObject.SetActive (false);
		ProjectileFollow.resetCamera = true;
		ProjectileFollow.fly = false;
	}
}
