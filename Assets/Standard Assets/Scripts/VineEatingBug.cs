using UnityEngine;
using System.Collections;

public class VineEatingBug : MonoBehaviour {

	public Health vineHealth;

	private bool isActive = true;
	private int critterDamage = 100;

	/// <summary>
	/// This is a script for the bug which eat the vine of the first log in the forest stage.
	/// 
	/// Once the player collide with the collider, it breaks the vine.
	/// </summary>


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.layer == 13 && isActive == true) { //If it hits the player
			isActive = false;
			vineHealth.AdjustCurrentHealth(-critterDamage);

		}
	}


}
