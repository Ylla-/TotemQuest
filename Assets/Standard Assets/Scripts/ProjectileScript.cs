using UnityEngine;
using System.Collections;


public class ProjectileScript : MonoBehaviour
{
	public int DMG = 20;
	public float speed = 12f;
	
	public bool facingRight = true;
	
	void Start()
	{
		Destroy(gameObject, 10); 
		
		
		
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
		
		if (other is BoxCollider2D && other.tag == "Enemy") {
			Debug.Log ("HIT FIREBALL");
			Health h = (Health)other.gameObject.GetComponent ("Health");
			
			h.AdjustCurrentHealth (-DMG);
			Destroy(gameObject);
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