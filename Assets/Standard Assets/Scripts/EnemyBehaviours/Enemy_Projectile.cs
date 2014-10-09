using UnityEngine;
using System.Collections;

public class Enemy_Projectile : MonoBehaviour {

	/// <summary>
	///  Basic Projectile for an enemy.
	/// </summary>

	public int DMG = 2;
	public float speed = 12f;
	public float knockbackAmpliX = 1f; //Multiply force X of knockback by this.
	public float knockbackAmpliY = 1f; //Multiply force Y of knockback by this.
	
	public bool KnockBackPlayer = false; //Does this projectile make the player knockbback
	public bool facingRight = true;
	public bool destroyOnHit = true;
	
	private Controller playerController;
	
	// Use this for initialization
	void Start () {
		if (facingRight == false) {
			Flip ();
			speed = -speed;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rigidbody2D.velocity = new Vector2 (speed, rigidbody2D.velocity.y);
	}
	
	void OnTriggerEnter2D(Collider2D other)	{

		if(other.gameObject.layer == 13) { //If it hits the player
			if(playerController == null) playerController = other.gameObject.GetComponent<Controller>();
			playerController.DamagePlayer(DMG);
			Vector3 positionDiff = playerController.transform.position - transform.position; 
			if(KnockBackPlayer == true) {
				if(facingRight == true){
					playerController.Knockback(new Vector2(1,0),knockbackAmpliX,knockbackAmpliY); //Not implemented yet.
				} else {
					playerController.Knockback(new Vector2(-1,0),knockbackAmpliX,knockbackAmpliY); //Not implemented yet.
				}

			}
			if(destroyOnHit == true) Destroy(gameObject);
		}
	}
	
	void Flip(){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		
		Vector3 angles = transform.localEulerAngles;
		angles.x *= -1; angles.y *= -1; angles.z *= -1;
		transform.localEulerAngles = angles;
	}

}
