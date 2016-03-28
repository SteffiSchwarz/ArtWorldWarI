using UnityEngine;
using System.Collections;

public class TargetDamage : MonoBehaviour {

	public string nameOfTarget;
	public float damageImpactSpeed;
	public int hitPoints = 2;

	private int currentHitPoints;
	private float damageImpactSpeedSqr;
	private Transform enemy;
	private Transform throne;
	private Animator anim;
	
	void Start () {
		enemy = transform.Find (nameOfTarget); 
		anim = enemy.GetComponent<Animator> ();
		currentHitPoints = hitPoints;
		damageImpactSpeedSqr = damageImpactSpeed * damageImpactSpeed;
	}
	
	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.collider.tag != "Damager" && collision.collider.tag != "Baddy")
			return;		
		if (collision.relativeVelocity.sqrMagnitude < damageImpactSpeedSqr)
			return;

		anim.SetBool ("Hit", true);
		ScoreManager.score += 5000;
		currentHitPoints--;
		
		if (currentHitPoints <= 0) {
			Kill ();
		}
			
	}
	
	void Kill () {
		Destroy (transform.gameObject);
		Resetting.numberOfEnemies -= 1;
		GameController.control.SetCharacterDestroyed (nameOfTarget);
	}
}
