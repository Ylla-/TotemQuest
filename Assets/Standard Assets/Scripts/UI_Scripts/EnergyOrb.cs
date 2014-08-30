using UnityEngine;
using System.Collections;

public class EnergyOrb : MonoBehaviour {

	/// <summary>
	/// This Script dictates the follow behaviour of the orbs. Work In Progress !
	/// </summary>
	/// 
	public Transform target;
	public int orbIndex; //index in OrbList of EnergyBar.cs

	public float maximumSpeedRange = 1f; //Distance at which orb speed is at his maximum
	public float speed; //current Speed
	public bool playerFacingLeft = true;

	private Vector3 direction; //Current Direction
	private Vector3 orbToModelStartDistance; //Distance between the orb and the player
	private Vector3 orbToModelAdditionnalDistance; //Additionnal distance added for each orb

	void Start () {
		if(playerFacingLeft) {
			orbToModelStartDistance = new Vector3 (-0.2f, 0.15f, 0);
			orbToModelAdditionnalDistance = new Vector3( -0.1f, Random.Range (0f,0.1f),0);
		}
	}
	
	// Update is called once per frame
	void Update () {

		direction = SetOrbDirection (); //Get Direction 
		speed = SetOrbSpeed (); //Get Speed

		transform.Translate(direction*speed*Time.deltaTime);  //Move
	}

	Vector3 SetOrbDirection() { //Find Direction to go in
		Vector3 targetPosition = target.position; //Target's position
		if(playerFacingLeft) {
			targetPosition += new Vector3 (orbToModelStartDistance.x,orbToModelStartDistance.y,0); //Basic Target Position
			targetPosition += new Vector3 (orbToModelAdditionnalDistance.x * orbIndex, orbToModelAdditionnalDistance.y, 0);// Target Position based on current Index
		} else {
			targetPosition += new Vector3 (-orbToModelStartDistance.x,orbToModelStartDistance.y,0); //Basic Target Position
			targetPosition += new Vector3 (-orbToModelAdditionnalDistance.x * orbIndex, orbToModelAdditionnalDistance.y, 0);// Target Position based on current Index
		}

		Vector3 newDirection = (targetPosition - transform.position).normalized;
		return newDirection;
	}

	float SetOrbSpeed() {
		float targetRange = 0.08f * (orbIndex+1); //Set Different ranges for every orb.
		Vector3 range = transform.position - target.position;
		range -= (targetRange * range.normalized);

		float speedStep = Mathf.Clamp ((range.magnitude / maximumSpeedRange),0,1);
		float newSpeed = Mathf.Lerp (0.1f, 35f, speedStep);

		if(range.magnitude > targetRange) {
			return newSpeed;
		} else {
			return 0f;
		}


	}

}
