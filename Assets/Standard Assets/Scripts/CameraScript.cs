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
}
