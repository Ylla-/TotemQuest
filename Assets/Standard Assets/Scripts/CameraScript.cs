using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	//followPlayer
	public Transform target;
	public float distance = 3.0f;
	public float height = 0f;
	public float damping = 5.0f;
	public bool smoothRotation = false;
	public bool lockRotation = true;
	public float rotationDamping = 10.0f;
	public float aboveTarget = 0;

	//Shake Camera variables
	private float shakeTime = 0.1f;
	private float shakeAmount = 0.08f;
	private Camera camera;

	void Awake() {
		camera = gameObject.GetComponentInChildren<Camera> ();
	}
	void Start () {
		//camera.transparencySortMode = TransparencySortMode.Orthographic;
	}
	

	void LateUpdate () {
		Vector3 wantedPosition = target.TransformPoint(0, height+aboveTarget, -distance);
		transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * damping);
		
		if (smoothRotation) {
			Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
			transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
		}
		
		else transform.LookAt (target, target.up);

		if (lockRotation) {
			transform.localRotation = Quaternion.Euler(0,0,0);
		}
	}

	//Shakes the screen. Used to add some punch to the game. This is used, for exemple, when taking a hit, hitting an ennemy or doing a action that 
	//would feel better with more punch (breaking door, landing from high jump, ect.)
	public void ScreenShake(){
		StartCoroutine (Shake (shakeTime,shakeAmount));
	}
	public void ScreenShake(float customTime, float customShake){ //Method for custom shake
		StartCoroutine (Shake (customTime,customShake));
	}

	//The function does not use Time.Deltatime since it is often gonna be used with the freeze method. 
	//Instead, we use Time.realtimeSinceStartup. Which is a timer independent of timescale.
	IEnumerator Shake(float time, float amount){ // time = shake time in seconds,  amount = units the shake is gonna move of
		float timeToEndShake = Time.realtimeSinceStartup + time;
		while (Time.realtimeSinceStartup < timeToEndShake) {   //Time independant from timeScale
			camera.transform.localPosition = Random.insideUnitSphere * amount;
			yield return 0;
		}
		camera.transform.localPosition = Vector3.zero;
	}

}
