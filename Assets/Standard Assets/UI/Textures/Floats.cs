using UnityEngine;
using System.Collections;

public class Floats : MonoBehaviour {

	/// <summary>
	/// This script makes the object it is attached to float up and down.  
	/// </summary>

	public float floatTime = 3f; //Time to go up and down in seconds
	public float amplitude = 1f; //Amplitude of one mouvement
	public float delay = 0f; //Delay before it starts. Allow better feel when multiple floating object are near eachother.
	public bool startsOnCreation = true; //If true, will start when object is created.
	
	private float maxY;
	private float minY;

	// Use this for initialization
	void Start () {
		maxY = transform.localPosition.y + amplitude;
		minY = transform.localPosition.y - amplitude;
		if(startsOnCreation == true) StartMouvement ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartMouvement() {
		StartCoroutine(PlayAnimation());
	}
	public void StopMouvement() {
		StopAllCoroutines ();
	}

	IEnumerator PlayAnimation () {
		yield return (new WaitForSeconds (delay));
		while(true) {
			StartCoroutine(PlaySmoothAnimation(transform.localPosition.y, maxY));
			yield return (new WaitForSeconds(floatTime));
			StartCoroutine(PlaySmoothAnimation(transform.localPosition.y, minY));
			yield return (new WaitForSeconds(floatTime));
		}
	}

	IEnumerator PlaySmoothAnimation(float startingPosition, float endingPosition) {
		float step = 0f; //raw step
		float rate = 1f/floatTime; //amount to add to raw step
		float smoothStep = 0f; //current smooth step
		while(step < 1f) { // until we're done
			step += Time.deltaTime * rate; 
			smoothStep = Mathf.SmoothStep(0f, 1f, step); //Smooth The mouvement
	
			transform.localPosition = new Vector3 (transform.localPosition.x,Mathf.Lerp(startingPosition, endingPosition, (smoothStep)),transform.localPosition.z); //lerp position
			yield return null;
		}

	}

}
