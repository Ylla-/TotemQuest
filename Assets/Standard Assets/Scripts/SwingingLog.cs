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
	[HideInInspector] public int health;

	private bool isActive = false;
	private int startHealth = 10;
	private int logDamage = 4;


	// Use this for initialization
	void Start () {
		if (player == null) player = GameObject.FindGameObjectWithTag ("Player");
		if (flashSprite == null) gameObject.GetComponentInChildren<FlashSprite>();
		health = startHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if(health <= 0) {
			DestroyLog();
		}


		//FOR TESTING :
		if(Input.GetKeyDown (KeyCode.B)){
			Debug.Log ("Test Purpose : Breaking Vine (KEY B)");
			Activate ();
		}
	}

	void  OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.layer == 13 && isActive == true) { //If it hits the player
			Health playerHp = other.gameObject.GetComponent<Health>();
			playerHp.AdjustCurrentHealth(-logDamage);
			DestroyLog();
		}
	}

	public void DestroyLog() {
		isActive = false;
		StopAllCoroutines ();
		Flash (Color.white);
		StartCoroutine (DestroyLogAnimation ());
	}

	public void Activate() {
		gameObject.tag = "Enemy"; //changes the tag once the vine is broken so the log cant be broken before.
		DestroyableVine.transform.parent = null;
		isActive = true; 
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



	IEnumerator DestroyLogAnimation(){
		SpriteRenderer[] logSprites = gameObject.GetComponentsInChildren<SpriteRenderer> ();
		for(float i = 0; i < 1; i += Time.deltaTime/0.5f) {
			for(int j = 0; j < logSprites.Length; j++) {
				logSprites[j].color = new Color(1,1,1,1-i);
			}
			yield return null;
		}
		Destroy (gameObject);
	}

	void Flash(Color flashColor) {
		flashSprite.Flash (flashColor);
	}


}
