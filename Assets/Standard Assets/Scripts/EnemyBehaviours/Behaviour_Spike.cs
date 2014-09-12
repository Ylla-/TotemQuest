using UnityEngine;
using System.Collections;

public class Behaviour_Spike : MonoBehaviour {

	public int damage = 2; //damage dealt to player
	public int enemyDamage = 10; //damage dealt to enemy

	private Controller playerController;
	private bool canHitPlayer = true;
	private bool canHitEnemy = true;

	void Start () {
		if(damage <= 0) canHitPlayer = false;
		if(enemyDamage <= 0) canHitEnemy = false;
		gameObject.layer = 14;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void  OnTriggerEnter2D(Collider2D other) { 
		//Damages Player when touching
		if(canHitPlayer == true && other.gameObject.layer == 13) { //If it hits the player
			if(playerController == null) playerController = other.gameObject.GetComponent<Controller>();
			playerController.DamagePlayer(damage);
			Vector3 positionDiff = playerController.transform.position - transform.position; 
			playerController.Knockback((new Vector2(positionDiff.x,positionDiff.y).normalized)); //Not implemented yet.
		}
		if(canHitEnemy == true && other.gameObject.layer == 14) { //If it hits the player
			Health hp = other.GetComponent<Health>();
			hp.AdjustCurrentHealth(-enemyDamage);
		}
	}
}
