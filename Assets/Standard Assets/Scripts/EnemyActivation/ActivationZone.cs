using UnityEngine;
using System.Collections;

public class ActivationZone : MonoBehaviour {

	public bool activated = false;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void  OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.layer == 13) { //If it hits the player
			activated = true;
		} 
	}

}
