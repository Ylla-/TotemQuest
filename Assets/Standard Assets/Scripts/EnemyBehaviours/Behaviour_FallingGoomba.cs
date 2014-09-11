﻿using UnityEngine;
using System.Collections;

public class Behaviour_FallingGoomba : MonoBehaviour {

	/// <summary>
	/// AI for the Basic "Goomba" enemy which just walks in a direction until it hits a wall or object, then turn back. But this 
	/// Falling Goomba actually starts in the air (on a branch or something), then falls when the player comes near.
	/// 
	/// Mouvement : Starts in the air, fall when player approches, then Walks until it hits an object, then turn back.
	/// Attack : None, Damage the player if he touches the side of it.
	/// How To Kill : Jump on hit OR kill it with Attacks.
	/// 
	/// </summary>
	
	
	public float speed = 1f; //Mouvement Speed
	public int damage = 1; // Damage done to the player when hit
	
	public bool goingLeft = true; //which direction is the enemy moving
	public bool isGrounded = false;
	public CircleCollider2D frontCollider; //Collider used to turn when hitting wall/object
	public BoxCollider2D stompCollider; //Collider used to verify if player is jumping on this enemy
	
	//Layers
	public LayerMask theGround;
	public LayerMask playerLayer;
	
	
	private int stompDamage; //Damage the enemy takes when stomped
	private bool canRotate = true; //Can the enemy rotate right now ?
	private bool canStomp = true; //Can the enemy be stomped right now ?
	private bool hasHitGround = false;
	private Vector3 overlapSpherePosition; 
	private Controller playerController;
	private Health hp;
	
	
	
	void Awake() {
		hp = gameObject.GetComponent<Health> ();
		stompDamage = hp.maxHealth;
	}
	
	void Start () {;
		updateDirection ();
		CreateStompCollider ();
	}
	
	void FixedUpdate() {
		if(isGrounded == true) { //dont do while still suspended in the wait

			//Check Wall Collision
			if (Physics2D.OverlapCircle (transform.position + overlapSpherePosition, frontCollider.radius, theGround) == true && canRotate == true) {
				StartCoroutine (Flip ());
			}
			
			
			//Move()
			if(goingLeft == true) {
				rigidbody2D.velocity = new Vector2(-speed,rigidbody2D.velocity.y);
			} else {
				rigidbody2D.velocity = new Vector2(speed,rigidbody2D.velocity.y);
			}
		} else {
			Fall();
		}
		
	}
	void  OnTriggerEnter2D(Collider2D other) { //Damage Player when touching him
		if(other.gameObject.layer == 13) { //If it hits the player
			if(playerController == null) playerController = other.gameObject.GetComponent<Controller>();
			playerController.DamagePlayer(damage);
			Vector3 positionDiff = playerController.transform.position - transform.position; 
			playerController.Knockback((new Vector2(positionDiff.x,positionDiff.y).normalized)); //Not implemented yet.
		} 
		if(other.gameObject.layer == 11) {
			hasHitGround = true;
		}
	}
	void  OnCollisionEnter2D(Collision2D coll) { //Check if player is stomping the enemy
		if(coll.collider.gameObject.layer == 13 && canStomp == true) { //If it hits the player
			foreach (ContactPoint2D contact in coll.contacts) {
				if( stompCollider.GetInstanceID() == contact.otherCollider.GetInstanceID()) {
					StartCoroutine (stompTimer()); //Start timer for next stomp
					Debug.Log ("StompCollider hit !");
					if(playerController == null) playerController = coll.collider.gameObject.GetComponent<Controller>(); //Get controller and Jump
					playerController.Jump();
					hp.AdjustCurrentHealth(-stompDamage);
				}
			}
		}
	}


	void CreateStompCollider(){
		stompCollider = gameObject.AddComponent<BoxCollider2D> ();
		stompCollider.isTrigger = false;
		stompCollider.size = new Vector2 (0.45f, 0.1f);
		stompCollider.center = new Vector2 (0, 0.25f);
	}
	
	void updateDirection(){
		if(goingLeft == true) {
			transform.localScale = new Vector3(-1,1,1);
			overlapSpherePosition = new Vector3 (-frontCollider.center.x, frontCollider.center.y,0);
		} else {
			transform.localScale = new Vector3(1,1,1);
			overlapSpherePosition = new Vector3 (frontCollider.center.x, frontCollider.center.y,0);
		}
	}

	void Fall(){ // This will make the enemy start falling. Then become a normal goomba when the ground is hit.
		if(hasHitGround == true) {
			Debug.Log ("f");
			rigidbody2D.velocity = new Vector2(0,0);
			isGrounded = true;
		}
	}
	
	IEnumerator Flip(){ //Flip enemy position and mouvement
		goingLeft = !goingLeft;//change direction
		updateDirection(); //Update direction
		
		//This flip can only happen every 0.5 seconds to avoid constant flipping.
		canRotate = false;
		yield return new WaitForSeconds (0.5f);
		canRotate = true;
	}
	
	IEnumerator stompTimer() {
		canStomp = false;
		yield return new WaitForSeconds(0.5f);
		canStomp = true;
	}
}