using UnityEngine;
using System.Collections;

public class SwingingLog : MonoBehaviour {

	public GameObject rotationAxis;  //Axis from which the log will rotate
	public GameObject player; //Player's GameObject
	public GameObject activationCollider; //Object that will activate the log when the player collides with it.,=
	public float rotationDegrees = 70;
	public float swingTime; //time it takes to swing RotationDegrees degrees

	private bool isIdle = true;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Activate() {
		isIdle = false; 
	}

}
