﻿using UnityEngine;
using System.Collections;

public class SpaceForNextScene : MonoBehaviour {

	public string nextScene;
	public SceneTransitionGUI sceneTransition;

	// Use this for initialization
	void Start () {
		if (sceneTransition == null) sceneTransition = GameObject.Find ("SceneTransitionGUI").GetComponent<SceneTransitionGUI> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Jump")) 
				StartCoroutine (LoadNextScene());
	}

	IEnumerator LoadNextScene() {
		if(sceneTransition != null) {  //Fade UI , Then load next level
			sceneTransition.FadeUI ();
			yield return new WaitForSeconds(sceneTransition.standardFadeInTime);
		}
		Application.LoadLevel(nextScene);
	}
}
