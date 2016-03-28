using UnityEngine;
using System.Collections;

public class ProjectilDragging : MonoBehaviour {

	public float maxStretch = 3.0f;
	public LineRenderer slingLineFront;
	public LineRenderer slingLineBack;
	public AudioClip dragSlingSound;
	public AudioClip releaseSlingSound;

	private SpringJoint2D spring;
	private Transform slingshot;
	private Ray rayToMouse;
	private Ray leftSlingToProjectile;
	private float maxStretchSqr;
	private bool clickedOn;
	private Vector2 prevVelocity;
	private float baddyRadius;

	void Awake () {
		spring = GetComponent <SpringJoint2D> ();
		slingshot = spring.connectedBody.transform;
	}
	
	void Start () {
		LineRendererSetup ();
		rayToMouse = new Ray (slingshot.position, Vector3.zero);
		leftSlingToProjectile = new Ray (slingLineFront.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
		CircleCollider2D circle = collider2D as CircleCollider2D;
		baddyRadius = circle.radius;
	}

	void Update () {
		if (clickedOn) {
			Dragging ();
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
		} else {
			slingLineFront.enabled = false;
			slingLineBack.enabled = false;
		}
	}

	void LineRendererSetup () {
		slingLineFront.SetPosition (0, slingLineFront.transform.position);
		slingLineBack.SetPosition (0, slingLineBack.transform.position);

		slingLineFront.sortingLayerName = "Foreground";
		slingLineBack.sortingLayerName = "Foreground";

		slingLineFront.sortingOrder = 3;
		slingLineBack.sortingOrder = 1;
	}

	void OnMouseDown () {
		AudioSource.PlayClipAtPoint (dragSlingSound, transform.position);
		spring.enabled = false;
		clickedOn = true;
	}

	void OnMouseUp () {
		AudioSource.PlayClipAtPoint (releaseSlingSound, transform.position);
		spring.enabled = true;
		rigidbody2D.isKinematic = false;
		clickedOn = false;
	}

	void Dragging () {
		ProjectileFollow.fly = true;
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 slingshotToMouse = mouseWorldPoint - slingshot.position;

		if (slingshotToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = slingshotToMouse;
			mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
		} 

		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}

	void LineRendererUpdate () {
		Vector2 slingshotToProjectile = transform.position - slingLineFront.transform.position;
		leftSlingToProjectile.direction = slingshotToProjectile;

		Vector3 holdPoint = leftSlingToProjectile.GetPoint (slingshotToProjectile.magnitude + baddyRadius);
		slingLineFront.SetPosition (1, holdPoint);
		slingLineBack.SetPosition (1, holdPoint);
	}
}
