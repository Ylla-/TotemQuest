using UnityEngine;
using System.Collections;

public class EnergyOrb : MonoBehaviour {

	/// <summary>
	/// This Script dictates the follow behaviour of the orbs. Work In Progress !
	/// </summary>
	/// 
	public Transform target;
	public int orbIndex; //index in OrbList of EnergyBar.cs
	public Controller controller;

	public float sizePerEnergy = 0.15f;
	public float maximumSpeedRange = 1f; //Distance at which orb speed is at his maximum
	public float speed; //current Speed
	public bool playerFacingRight = true;
	[HideInInspector] public EnergyBar energyBar;
	[HideInInspector] public int maximumEnergy;
	 public Vector2 totemOrbPosition; //Position of the orb on current totem

	private SpriteRenderer spriteRenderer;
	private Vector3 direction; //Current Direction
	private Vector3 orbToModelStartDistance; //Distance between the orb and the player
	private Vector3 orbToModelAdditionnalDistance; //Additionnal distance added for each orb
	private Vector3 currentTargetPosition;
	public int currentEnergy;



	void Awake() {
		spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
	}
	void Start () {
		UpdatePlayerFacing (); //Update the facing so the orb position themselves correctly on spawn.


	}
	
	// Update is called once per frame
	void Update () {

		if(playerFacingRight != controller.facingRight) { 
			playerFacingRight = !playerFacingRight;
			UpdatePlayerFacing ();
		}

		direction = SetOrbDirection (); //Get Direction 
		speed = SetOrbSpeed (); //Get Speed

		transform.Translate(direction*speed*Time.deltaTime);  //Move
	}

	public void InitializeEnergy() {
		currentEnergy = 1; //Adjust size at initialization
		ChangeOrbSize ();
	}

	public void LerpColor(Color newColor) {  //Lerp to the new color
		StartCoroutine (LerpColor (spriteRenderer.color,newColor,2f));
	}
	public void SetColor(Color newColor) { //Change to the new color, WITHOUT lerp.
		spriteRenderer.color = newColor;
	}

	public void AddEnergy() {
		if(currentEnergy <= maximumEnergy) {
			Debug.Log ("Before Adding Energy. Current Energy : " + currentEnergy); 
			currentEnergy++;
			Debug.Log ("Added Energy. Current Energy : " + currentEnergy + "      maximum Energy : " + maximumEnergy);
			ChangeOrbSize();
		}
	}
	public void RemoveEnergy() {
		if(currentEnergy > 1) {
			currentEnergy--;
			ChangeOrbSize();
		}
	}

	void ChangeOrbSize(){
		float finalSize = (sizePerEnergy * currentEnergy) + 0.15f;
		transform.localScale = new Vector3 (finalSize, finalSize, 0);
	}


	void UpdatePlayerFacing() { 

		if(playerFacingRight) { //Change Orb Standard position
			orbToModelStartDistance = new Vector3 (-totemOrbPosition.x, totemOrbPosition.y, 0);
			orbToModelAdditionnalDistance = new Vector3( -0.14f, Random.Range (-0.05f,0.05f),0);
		} else {
			orbToModelStartDistance = new Vector3 (totemOrbPosition.x, totemOrbPosition.y, 0);
			orbToModelAdditionnalDistance = new Vector3( 0.14f, Random.Range (-0.02f,0.02f),0);
		}
	}
	

	Vector3 SetOrbDirection() { //Get Direction in which the orb must move
		//TODO: Give a y value to the orb depending on its orbIndex to have a straight line of orbs. 

		Transform newTarget;
		newTarget = FindNewTarget ();
		
		currentTargetPosition = newTarget.position; //Target's position

		if(orbIndex == 0) currentTargetPosition += new Vector3 (orbToModelStartDistance.x,orbToModelStartDistance.y,0); //Basic Target Position
		else currentTargetPosition = currentTargetPosition + new Vector3 (orbToModelAdditionnalDistance.x, orbToModelAdditionnalDistance.y, 0);// Target Position based on current Index

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
			//Debug.Log ("orb > 0.04f");
			return newSpeed;
		} else if(range.magnitude > 0.015f) {
			//Debug.Log ("orb > 0.015f");
			return newSpeed/2f;
		} else {
			transform.position = currentTargetPosition;
			return 0;
		}

	}


	IEnumerator LerpColor(Color startColor, Color endColor, float time) {
		for(float i=0; i < 1; i += Time.deltaTime/time) {
			spriteRenderer.color = Color.Lerp (startColor,endColor,i);
			yield return null;
		}

	}

}
