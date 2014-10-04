 using UnityEngine;
using System.Collections;


public class ProjectileScript : MonoBehaviour
{
	public int DMG = 20;
	public float speed = 12f;
	public GameObject destroyedParticlesObj;
	
	public bool facingRight = true;
	
	void Start()
	{
		gameObject.layer = 15; //This is the player's attack layer
		Destroy(gameObject, 4); 
	}
	
	
	void FixedUpdate(){
		//flipping 
		if (facingRight == false) {
			Flip ();
			speed = -speed;
			facingRight = true;
		}
		rigidbody2D.velocity = new Vector2 (speed, rigidbody2D.velocity.y);
		
		
	}
	
	
	
	void OnTriggerEnter2D(Collider2D other)	{



		if (other is BoxCollider2D && (other.gameObject.layer == 14 || other.gameObject.layer == 17)) { //Changed the collision requirements from being a tag to a layer. It will now hit everything in enemy layer.
			Debug.Log ("HIT FIREBALL");
			//Get HealthScript and remove HP
			Health h = (Health)other.gameObject.GetComponent ("Health");
			if(h != null) h.AdjustCurrentHealth (-DMG);
			//Activate the MonoBehaviour of the enemy (if the enemy requires a specific condition to activate, getting hit by the player will fulfill it).
			ActivateMonos(other.gameObject); //Activates the monobehaviours on target

			Destroy(gameObject);
		} else if (other.gameObject.layer == 11) {
			if(destroyedParticlesObj != null) Instantiate (destroyedParticlesObj,transform.position,Quaternion.identity);
			Destroy (gameObject);

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

	void ActivateMonos(GameObject obj){
		MonoBehaviour[] behaviours = obj.GetComponentsInChildren<MonoBehaviour> ();
		for(int i =0; i< behaviours.Length; i++) {
			if(behaviours[i].enabled == false) behaviours[i].enabled = true;
		}
	}

	public int Damage{ 
		get{ return DMG;}
		set{ DMG = value;}
	}
}