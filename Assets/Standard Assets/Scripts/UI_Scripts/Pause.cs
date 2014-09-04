using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	static GUIStyle menuFont;
	public bool isPaused = false;

	private float buttonWidth;
	private float buttonHeight;
	private float previousTimeScale;
	private GUITexture GuiTexture;

	void Awake(){
		GuiTexture = (GUITexture) gameObject.GetComponent<GUITexture> ();
	}

	void Start () {
		buttonWidth = 0.2f*Screen.width;
		buttonHeight = 0.1f * Screen.height;
		SetMenuFont ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P) || Input.GetKeyDown (KeyCode.Escape)) {
			if(isPaused == true) UnpauseGame();
			else PauseGame();
		}
	}

	void OnGUI() {
		if (isPaused == true) {
			GUI.Label (new Rect((Screen.width/2)-(buttonWidth/2), (Screen.height/4f)-(buttonHeight/2),buttonWidth, buttonHeight), "Game Is Paused", menuFont);
			if(GUI.Button (new Rect((Screen.width/2)-(buttonWidth/2), (Screen.height/2)-(buttonHeight/2),buttonWidth, buttonHeight), "Main Menu")){
				UnpauseGame();
				Time.timeScale = 1f;
				Application.LoadLevel (2);
			}
		}
	}

	void SetMenuFont(){
		menuFont = new GUIStyle ();
		menuFont.font = (Font)Resources.Load ("Fonts/CruseoText-Regular");
		menuFont.fontSize = 30;
		menuFont.alignment = TextAnchor.MiddleCenter;
		menuFont.normal.textColor = Color.white;
	}

	void PauseGame() {
		previousTimeScale = Time.timeScale;
		isPaused = true;
		Time.timeScale = 0f;
		GuiTexture.color = new Color (0,0,0,0.35f);

	}

	void UnpauseGame() {
		isPaused = false;
		Time.timeScale = previousTimeScale;
		GuiTexture.color = new Color (0,0,0,0);

	}

}
