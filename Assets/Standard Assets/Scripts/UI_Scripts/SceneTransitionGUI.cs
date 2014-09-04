using UnityEngine;
using System.Collections;

public class SceneTransitionGUI : MonoBehaviour {

	public Color fadeColor;
	public float standardFadeInTime = 0.8f; //In seconds
	public float standardfadeOutTime = 0.6f; //In seconds

	static bool isCreated = false; //Is there already a SceneTransitionGUI in the scene ?
	private GUITexture GuiTexture;
	

	void Awake() {
		if(isCreated == false) { //If GUI already exists, destroy this instance.
			isCreated = true; 
			GameObject.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent("Pause");
			gameObject.tag = "TransitionGUI";
		} else {
			Debug.Log ("Destroying UI");
			Destroy (gameObject);
		}
	}

	void Start () {
		GuiTexture = gameObject.GetComponent<GUITexture> (); //Store GUITexture
		transform.position = new Vector3 (0.5f, 0.5f, 50f); //Set Up Position and Dimension of the screen
		transform.localScale = new Vector3 (1f, 1f);
		GuiTexture.color = fadeColor;
	}
	
	// Update is called once per frame
	void Update () {
	}
	 
	public void FadeUI() {
		StartCoroutine (FadeIn (standardFadeInTime,standardfadeOutTime,fadeColor)); //Simply fade in with the default time
	}
	public void FadeUI(float fadeInTime, float fadeOutTime){ //function called to have this texture Fade In and Fade Out with a custom time
		StartCoroutine (FadeIn (fadeInTime,fadeOutTime,fadeColor));
	}
	public void FadeUI(float fadeInTime, float fadeOutTime, Color customColor){ //function called to have this texture Fade In and Fade Out with a custom time
		StartCoroutine (FadeIn (fadeInTime,fadeOutTime,customColor));
	}



	IEnumerator FadeIn(float fadeInTime, float fadeOutTime,Color customColor) {
		Color StartColor = new Color (customColor.r, customColor.g, customColor.b, 0f);
		Color targetColor = new Color (StartColor.r, StartColor.g, StartColor.b, 1f);

		for(float i = 0; i < 1; i += (Time.deltaTime/fadeInTime)){ //Change Alpha over time
			GuiTexture.color = Color.Lerp(StartColor,targetColor,i);
			yield return null;
		}
		GuiTexture.color = targetColor; //Complete the Color Change
		StartCoroutine (FadeOut(fadeOutTime));
	}

	IEnumerator FadeOut(float fadeOutTime) {
		Color StartColor = GuiTexture.color;
		Color targetColor = new Color (StartColor.r, StartColor.g, StartColor.b, 0f);

		for(float i = 0; i < 1; i += (Time.deltaTime/fadeOutTime)){ //Change Alpha over time
			GuiTexture.color = Color.Lerp(StartColor,targetColor,i);
			yield return null;
		}

		GuiTexture.color = targetColor; //Complete the Color Change
	}

}
