using UnityEngine;
using System.Collections;

public class UnlockTotem : MonoBehaviour {

	bool isColliding = false; //is the player colliding with this ?
	bool contextArrowActive = false;
	public int totemIndex = 0; //index of the totem to unlock. 0 = normal, 1 = rabbit, 2 = mole, 3 = mantis

	GameManager gameManager;
	//Context Arrow
	private GameObject ContextArrow;
	private SpriteRenderer arrowRenderer;


	// Use this for initialization
	void Start () {
		GameObject manager_Obj = GameObject.FindGameObjectWithTag ("GameManager");
		if (manager_Obj != null) gameManager = (GameManager)manager_Obj.GetComponent<GameManager> ();
		else Debug.Log ("MANAGER OBJECT NOT FOUND !");

		
		GameObject ArrowPrefab = (GameObject) Resources.Load ("GameObjects/ArrowKeyContext");
		ContextArrow = (GameObject) Instantiate (ArrowPrefab, transform.position + new Vector3 (0, 1.2f, -1), Quaternion.identity);
		ContextArrow.transform.parent = transform;
		arrowRenderer = ContextArrow.GetComponent<SpriteRenderer> ();
		arrowRenderer.color = new Color (0, 0, 0, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetAxis ("Vertical") > 0.25 && isColliding) {
			UnlockPower();
		}
		//Remove collision
		isColliding = false;

	}
	void LateUpdate() {
		if(isColliding == false && contextArrowActive == true) {
			StopCoroutine ("FadeInContextMenu");
			StartCoroutine ("FadeOutContextMenu");
			contextArrowActive = false;
		} else if(isColliding == true && contextArrowActive == false){
			StartCoroutine ("FadeInContextMenu");
			contextArrowActive = true;
		}
	}

	void OnTriggerStay2D(Collider2D other)	{
		if(other.gameObject.layer == 13) { //If it hits the player
			if(isColliding == false) {
				isColliding = true;

			}
		}
	}


	void UnlockPower(){
		gameManager.totemUnlocks [totemIndex] = true;
		Destroy (gameObject); 
	}

	IEnumerator FadeInContextMenu(){
		yield return new WaitForSeconds(1f);
		StopCoroutine ("FadeOutContextMenu");
		for(float i = 0; i < 1; i += Time.deltaTime/2f) {
			arrowRenderer.color = new Color (1, 1, 1, i);
			yield return null;
		}
		arrowRenderer.color = new Color (1, 1, 1, 1);
	}

	IEnumerator FadeOutContextMenu(){
		for(float i = arrowRenderer.color.a; i > 0; i -= Time.deltaTime/2f) {
			arrowRenderer.color = new Color (1, 1, 1, i);
			yield return null;
		}
		arrowRenderer.color = new Color (1, 1, 1, 0);
	}

}
