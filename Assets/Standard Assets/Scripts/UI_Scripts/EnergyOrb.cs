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
	[HideInInspector] public EnergyBar energyBar;

	private Vector3 direction; //Current Direction
	private Vector3 orbToModelStartDistance; //Distance between the orb and the player
	private Vector3 orbToModelAdditionnalDistance; //Additionnal distance added for each orb
	private Vector3 currentTargetPosition;


	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		UpdatePlayerFacing ();

		direction = SetOrbDirection (); //Get Direction 
		speed = SetOrbSpeed (); //Get Speed

		transform.Translate(direction*speed*Time.deltaTime);  //Move
	}

	void UpdatePlayerFacing() { //TODO: Link this with the controller script
		if(playerFacingLeft) {
			orbToModelStartDistance = new Vector3 (-0.2f, 0.15f, 0);
			orbToModelAdditionnalDistance = new Vector3( -0.14f, Random.Range (-0.05f,0.05f),0);
		} else {
			orbToModelStartDistance = new Vector3 (0.2f, 0.15f, 0);
			orbToModelAdditionnalDistance = new Vector3( 0.14f, Random.Range (-0.05f,0.05f),0);
		}
	}
	

	Vector3 SetOrbDirection() { //Get Direction in which the orb must move

		Transform newTarget;

		newTarget = FindNewTarget ();
		
		currentTargetPosition = newTarget.position; //Target's position
		if(playerFacingLeft) {
			if(orbIndex == 0) currentTargetPosition += new Vector3 (-orbToModelStartDistance.x,orbToModelStartDistance.y,0); //Basic Target Position
			else currentTargetPosition = currentTargetPosition + new Vector3 (-orbToModelAdditionnalDistance.x, orbToModelAdditionnalDistance.y, 0);// Target Position based on current Index
		} else {
			if(orbIndex == 0) currentTargetPosition += new Vector3 (orbToModelStartDistance.x,orbToModelStartDistance.y,0); //Basic Target Position
			else currentTargetPosition = currentTargetPosition + new Vector3 (orbToModelAdditionnalDistance.x, orbToModelAdditionnalDistance.y, 0);// Target Position based on current Index
		}
		if (orbIndex == 0)
						Debug.Log (currentTargetPosition + "        " + playerFacingLeft);
		Vector3 newDirection = (currentTargetPosition - transform.position).normalized;
		return newDirection;
	}

	Transform FindNewTarget() { //Find the new orb's target
		if(orbIndex == 0) {
			return target;
		} else {
			return (energyBar.orbList[orbIndex -1].transform);
		}
	}

	float SetOrbSpeed() { //Find the Orb's speed

		Vector3 range = transform.position - currentTargetPosition;
		
		float speedStep = Mathf.Clamp ((range.magnitude / maximumSpeedRange),0,1);
		float newSpeed = Mathf.Lerp (0.5f, 35f, speedStep);
		
		if(range.magnitude >  0.04f) {
			Debug.Log ("orb > 0.04f");
			return newSpeed;
		} else if(range.magnitude > 0.015f) {
			Debug.Log ("orb > 0.015f");
			return newSpeed/3f;
		} else {
			transform.position = currentTargetPosition;
			return 0;
		}

	}

	IEnumerator changeOrbMouvement(float time) {
		yield return new WaitForSeconds (time);
		StopAllCoroutines ();
	}

}
