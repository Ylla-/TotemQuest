using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	public string ForestLevel;
	public string PlainsLevel;
	public string CavesLevel;
	public string FinalLevel;
	public string Title;

	public SceneTransitionGUI sceneTransition;
	private GameManager manager;

	// Use this for initialization
	void Start () {
		if (sceneTransition == null) sceneTransition = GameObject.Find ("SceneTransitionGUI").GetComponent<SceneTransitionGUI> ();
		if(manager == null) manager = GameObject.Find ("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// 2014-08AUG-28 ACP Buttons properties are hardcoded should be re-written to be easier to edit
	void OnGUI () {
		//plains
		if 
			(GUI.Button(new Rect(10,10,100,100),"Plains")) StartCoroutine (LoadNextScene(PlainsLevel));
		//forest
		if 
			(GUI.Button(new Rect(10,120,100,100),"Forest")) StartCoroutine (LoadNextScene(ForestLevel));
		//caves
		if 
			(GUI.Button(new Rect(10,230,100,100),"Caves")) StartCoroutine (LoadNextScene(CavesLevel));

		//final level starts disabled
		if(manager.lastLevelUnlocked == false) GUI.enabled=false;
		if 
			(GUI.Button(new Rect(10,340,100,100),"Final")) StartCoroutine (LoadNextScene(FinalLevel));
		GUI.enabled=true;

		//back to title
		if
			(GUI.Button(new Rect(Screen.width-100-10,10,100,100),"Back To Title")) StartCoroutine (LoadNextScene(Title));
	}

	IEnumerator LoadNextScene(string nextScene) {
		if(sceneTransition != null) {  //Fade UI , Then load next level
			sceneTransition.FadeUI ();
			yield return new WaitForSeconds(sceneTransition.standardFadeInTime);
		}
		Application.LoadLevel(nextScene);
	}


}
