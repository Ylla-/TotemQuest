﻿using UnityEngine;
using System.Collections;

public class Lizard_Projectile : MonoBehaviour {

	public int DMG = 20;
	public float speed = 12f;
	
	public bool facingRight = true;
	
	void Start()
	{
		gameObject.layer = 19; //This is the player's attack layer
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
		//TODO : Change this to hit player instead of lizard !
		/*
		if (other is BoxCollider2D && other.tag == "Enemy") {
			Debug.Log ("HIT FIREBALL");
			//Get HealthScript and remove HP
			Health h = (Health)other.gameObject.GetComponent ("Health");
			h.AdjustCurrentHealth (-DMG);
			//Activate the MonoBehaviour of the enemy (if the enemy requires a specific condition to activate, getting hit by the player will fulfill it).
			ActivateMonos(other.gameObject); //Activates the monobehaviours on target
			
			Destroy(gameObject);
		}*/
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
	

}