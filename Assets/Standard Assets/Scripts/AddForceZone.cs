using UnityEngine;
using System.Collections;

/// <summary>
/// This script makes the player currently colliding with this object's collider to be affected by a force in a certain direction.
/// </summary>


public class AddForceZone : MonoBehaviour {

	public Vector2 ZoneForce;
	public int layerToForce = 13; //layer of the player

	private Rigidbody2D playerRigidbody;
	private bool playerIsColliding = false;

	// Use this for initialization
	void Start () {
		
	}

	void LateUpdate () {
		if(playerIsColliding == true) {

			playerRigidbody.AddForce(ZoneForce);
			playerIsColliding = false;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.layer == layerToForce){
			if(playerRigidbody == null) playerRigidbody = other.rigidbody2D;
			playerIsColliding = true;
		}
	}
}
