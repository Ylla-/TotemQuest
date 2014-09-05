using UnityEngine;
using System.Collections;

public class SwingingLog : MonoBehaviour {

	public GameObject rotationAxis;  //Axis from which the log will rotate
	public GameObject player; //Player's GameObject
	public GameObject DestroyableVine; //Object that will activate the log when the player collides with it.,=
	public FlashSprite flashSprite;

	public float rotationDegrees = 70;
	public float swingTime = 3f; //time it takes to swing RotationDegrees degrees
	public int rotateDirection = 1; //-1 for ClockWise, 1 for CounterClockwise.

	private bool isIdle = true;


	// Use this for initialization
	void Start () {
		if (player == null) player = GameObject.FindGameObjectWithTag ("Player");
		if (flashSprite == null) gameObject.GetComponentInChildren<FlashSprite>();

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.V)){
			Debug.Log ("Test Purpose : Testing Flash Animation on Log (KEY V)");
			Flash(Color.white);
		}
		if(Input.GetKeyDown (KeyCode.B)){
			Debug.Log ("Test Purpose : Breaking Vine (KEY B)");
			Activate ();
		}
	}

	public void Activate() {
		isIdle = false; 
		StartCoroutine (DestroyVine ()); //Start Vine destroy animation
		StartCoroutine (LogFalling (rotationAxis.transform.position,rotationDegrees,swingTime,rotateDirection)); //Start Log Falling animation
	}

	IEnumerator LogFalling(Vector3 rotAxis, float x, float t, int direction) { //Makes the log rotate around the axis for x degrees over t seconds
		float step = 0f; //raw step
		float smoothStep = 0f; //current smooth step
		float lastStep = 0f; //previous smooth step
		while(step < 1f) { // until we're done
			step += Time.deltaTime / t; // for t seconds 
			smoothStep = Mathf.SmoothStep(0f, 1f, step); // finding smooth step

			//Do Rotation on Z axis
			transform.RotateAround(rotAxis, Vector3.forward, (direction*x) * (smoothStep - lastStep));
			lastStep = smoothStep; //get previous last step
			yield return null;
		}
		StartCoroutine (LogFalling (rotationAxis.transform.position,rotationDegrees,swingTime,-direction)); //Start Log Falling animation
	}

	IEnumerator DestroyVine() {
		DestroyableVine.transform.parent = null;
		SpriteRenderer vineSprite = DestroyableVine.GetComponent<SpriteRenderer> ();
		for(float i = 0; i < 1; i += Time.deltaTime/0.5f) {
			vineSprite.color = new Color(1,1,1,1-i);
			yield return null;
		}
		Destroy (DestroyableVine);

	}

	void Flash(Color flashColor) {
		flashSprite.Flash (flashColor);
	}


}
