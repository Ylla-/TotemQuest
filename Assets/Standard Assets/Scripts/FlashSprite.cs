using UnityEngine;
using System.Collections;

public class FlashSprite : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		spriteRenderer.color = new Color (0,0,0,0);
		spriteRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.V)){
			Debug.Log ("Test Purpose : Testing Flash Animation on Log (KEY V)");
			Flash(Color.white);
		}
	}

	public void Flash(Color FlashColor){
		StartCoroutine (FlashAnimation (FlashColor));
	}

	IEnumerator FlashAnimation(Color newColor) {
		float flashTime = 0.02f;

		spriteRenderer.enabled = true;
		spriteRenderer.color = new Color (newColor.r, newColor.g, newColor.b, 0);
		for (float i = 0; i < 1; i += Time.deltaTime/flashTime) {
			spriteRenderer.color = new Color (newColor.r, newColor.g, newColor.b, i);
			yield return null;
		}
		for (float i = 0; i < 1; i += Time.deltaTime/flashTime) {
			spriteRenderer.color = new Color (newColor.r, newColor.g, newColor.b, 1-i);
			yield return null;
		}
		spriteRenderer.enabled = false;
	}

}
