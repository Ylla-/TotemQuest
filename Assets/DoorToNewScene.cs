using UnityEngine;
using System.Collections;

public class DoorToNewScene : MonoBehaviour {

	public string SceneName;
	public GameObject player;
	public float fadeTime = 1f;
	
	private SceneTransitionGUI transitionGUI;

	void Start () { 
		//If for some reasons public link to GameObject are misssing :
		if(player == null) player = (GameObject) GameObject.FindGameObjectWithTag("Player");
		
		transitionGUI = (SceneTransitionGUI) GameObject.FindGameObjectWithTag ("TransitionGUI").GetComponent<SceneTransitionGUI> ();
		
		
	}
	
	
	void Update () {
		if(Input.GetAxis ("Vertical")>0.25) {
			if(gameObject.renderer.bounds.Intersects(player.renderer.bounds)){
				UseDoor ();
			} 
		}
	}
	
	void UseDoor() { //Could Add animtion here (or maybe a fade ?)
		StartCoroutine (PlayDoorTransition ());

		
	}
	
	IEnumerator PlayDoorTransition(){
		transitionGUI.FadeUI (fadeTime / 2, fadeTime / 2);
		yield return new WaitForSeconds(fadeTime/2);
		Application.LoadLevel (SceneName);
	}

}
