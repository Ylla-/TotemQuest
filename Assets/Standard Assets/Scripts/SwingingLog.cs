using UnityEngine;
using System.Collections;

public class SwingingLog : MonoBehaviour {

	public GameObject rotationAxis;  //Axis from which the log will rotate
	public GameObject player; //Player's GameObject
	public GameObject activationCollider; //Object that will activate the log when the player collides with it.,=
	public float rotationDegrees = 70;
	public float swingTime = 3f; //time it takes to swing RotationDegrees degrees
	public int rotateDirection = 1; //-1 for ClockWise, 1 for CounterClockwise.

	private bool isIdle = true;
	private bool vineBreakableByPlayer = true; //If false, player's attack will not be able to break the vine (used for boss Fight ?)


	// Use this for initialization
	void Start () {
		Activate ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Activate() {
		isIdle = false; 
		StartCoroutine (LogFalling (rotationAxis.transform.position,rotationDegrees,swingTime));
	}

	IEnumerator LogFalling(Vector3 rotAxis, float x, float t) { //Makes the log rotate around the axis for x degrees over t seconds
		float step = 0f; //raw step
		float rate = 1f/t; //amount to add to raw step
		float smoothStep = 0f; //current smooth step
		float lastStep = 0f; //previous smooth step
		while(step < 1f) { // until we're done
			step += Time.deltaTime * rate; 
			smoothStep = Mathf.SmoothStep(0f, 1f, step); // finding smooth step

			//Do Rotation on Z axis
			transform.RotateAround(rotAxis, Vector3.forward, (rotateDirection*x) * (smoothStep - lastStep));
			lastStep = smoothStep; //get previous last step
			yield return null;
		}
	}
	

}
