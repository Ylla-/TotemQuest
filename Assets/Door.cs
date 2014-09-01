﻿using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public GameObject targetAnchor; //Object where the character will come out.
	public GameObject player;
	public float fadeTime = 1f;

	private SceneTransitionGUI transitionGUI;
	private KeyCode key1; //Keys to activate
	private KeyCode key2; 


	void Start () { 
		//If for some reasons public link to GameObject are misssing :
		if(player == null) player = (GameObject) GameObject.FindGameObjectWithTag("Player");
		if (targetAnchor == null) targetAnchor = (gameObject.GetComponentsInChildren<Transform> ()) [1].gameObject; 

		transitionGUI = (SceneTransitionGUI) GameObject.FindGameObjectWithTag ("TransitionGUI").GetComponent<SceneTransitionGUI> ();

		key1 = KeyCode.UpArrow;
		key2 = KeyCode.UpArrow;
	}
	

	void Update () {
		if(Input.GetKeyDown (key1) || Input.GetKeyDown (key2)) {
			if(gameObject.renderer.bounds.Intersects(player.renderer.bounds)){
				UseDoor ();
			} 
		}
	}

	void UseDoor() { //Could Add animtion here (or maybe a fade ?)
		Debug.Log ("Used Door to position" + targetAnchor.transform.position);
		StartCoroutine (PlayDoorTransition ());

	}

	IEnumerator PlayDoorTransition(){
		transitionGUI.FadeUI (fadeTime / 2, fadeTime / 2);
		yield return new WaitForSeconds(fadeTime/2);
		player.transform.position = targetAnchor.transform.position;
	}

}