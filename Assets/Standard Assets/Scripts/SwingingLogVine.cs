using UnityEngine;
using System.Collections;

public class SwingingLogVine : MonoBehaviour {

	public SwingingLog logScript;
	Health health;
	SpriteRenderer vineSprite;

	private bool wasActivated = false;

	void Start () {
		health = gameObject.GetComponent<Health> ();
		if(logScript == null) logScript = gameObject.GetComponentInParent<SwingingLog> ();
		vineSprite = gameObject.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(health.curHealth <= 0 && wasActivated == false) {
			StartCoroutine(DestroyVine ());
		}
	}

	IEnumerator DestroyVine() {
		wasActivated = true;
		logScript.Activate (); //Activate Log
		transform.parent = null;

		for(float i = 0; i < 1; i += Time.deltaTime/0.5f) {
			vineSprite.color = new Color(1,1,1,1-i);
			yield return null;
		}

		Destroy (gameObject);
		
	}



}
