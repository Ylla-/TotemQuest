using UnityEngine;
using System.Collections;

public class UnlockTotem : MonoBehaviour {

	bool isColliding = false; //is the player colliding with this ?
	public int totemIndex = 0; //index of the totem to unlock. 0 = normal, 1 = rabbit, 2 = mole, 3 = mantis

	GameManager gameManager;
	// Use this for initialization
	void Start () {
		GameObject manager_Obj = GameObject.FindGameObjectWithTag ("GameManager");
		if (manager_Obj != null) gameManager = (GameManager)manager_Obj.GetComponent<GameManager> ();
		else Debug.Log ("MANAGER OBJECT NOT FOUND !");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetAxis ("Vertical") > 0.25 && isColliding) {
			UnlockPower();
		}
		isColliding = false;
	}

	void OnTriggerStay2D(Collider2D other)	{
		if(other.gameObject.layer == 13) { //If it hits the player
			isColliding = true;
		}
	}


	void UnlockPower(){
		gameManager.totemUnlocks [totemIndex] = true;
		Destroy (gameObject);
	}
}
