using UnityEngine;
using System.Collections;

public class ArtObjectDamage : MonoBehaviour {
	
	public int hitPoints = 2;
	public float damageImpactSpeed;
	
	private int currentHitPoints;
	private float damageImpactSpeedSqr;

	void Start () {
		currentHitPoints = hitPoints;
		damageImpactSpeedSqr = damageImpactSpeed * damageImpactSpeed;
	}
	
	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.collider.tag != "Damager" && collision.collider.tag != "Baddy")
			return;		
		if (collision.relativeVelocity.sqrMagnitude < damageImpactSpeedSqr)
			return;

		currentHitPoints--;
		
		if (currentHitPoints <= 0) {
			Kill ();
		}
		
	}
	
	void Kill () {
		ScoreManager.score += 2500;
		Destroy (transform.gameObject);
	}
}
