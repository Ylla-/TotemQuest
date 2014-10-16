using UnityEngine;
using System.Collections;

public class ShakeOnStart : MonoBehaviour {

	public float delay = 0f;
	public float FreezeTime = 0.02f;
	public float shakeTime = 0.08f;
	public float shakeAmount = 0.02f;

	private GameManager gameManager;
	private CameraScript cameraScript;
	
	void Start () {
		GameObject camera_Obj = GameObject.FindGameObjectWithTag ("MainCamera");
		if (camera_Obj != null) cameraScript = (CameraScript)camera_Obj.GetComponentInParent<CameraScript> ();
		else Debug.Log ("CAMERA OBJECT NOT FOUND !");
		GameObject manager_Obj = GameObject.FindGameObjectWithTag ("GameManager");
		if (manager_Obj != null) gameManager = (GameManager)manager_Obj.GetComponent<GameManager> ();
		else Debug.Log ("MANAGER OBJECT NOT FOUND !");
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (StartShake());
	}

	void Shake() {
		gameManager.FreezeGame(0.02f);
		if(cameraScript != null) cameraScript.ScreenShake(0.08f,0.02f); //Shake Screen
	}

	IEnumerator StartShake(){
		yield return new WaitForSeconds(delay);
		Shake ();
	}
}
