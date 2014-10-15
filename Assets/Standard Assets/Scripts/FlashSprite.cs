using UnityEngine;
using System.Collections;

public class FlashSprite : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	public SpriteRenderer parentRenderer; //Renderer which this object is copying
	// Use this for initialization
	void Start () {
		parentRenderer = transform.parent.GetComponent<SpriteRenderer> ();
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		spriteRenderer.color = new Color (0,0,0,0);
		spriteRenderer.enabled = false;
		PositionBeforeForeground ();
	}

	
	// Update is called once per frame
	void Update () {

	}

	void PositionBeforeForeground (){
		transform.position = new Vector3 (transform.position.x,
		                                  transform.position.y,
		                                  transform.position.z - 0.05f);
	}

	void GetParentSprite (){
		spriteRenderer.sprite = parentRenderer.sprite;
	}

	public void Flash(Color FlashColor){
		if(parentRenderer != null) GetParentSprite ();
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
