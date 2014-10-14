using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public GameObject targetAnchor; //Object where the character will come out.
	public GameObject player;
	public float fadeTime = 1f;

	private SceneTransitionGUI transitionGUI;


	void Start () { 
		//If for some reasons public link to GameObject are misssing :
		if(player == null) player = (GameObject) GameObject.FindGameObjectWithTag("Player");
		if (targetAnchor == null) targetAnchor = (gameObject.GetComponentsInChildren<Transform> ()) [1].gameObject; 

		transitionGUI = (SceneTransitionGUI) GameObject.FindGameObjectWithTag ("TransitionGUI").GetComponent<SceneTransitionGUI> ();


	}
	

	void Update () {
		if(Input.GetAxis ("Vertical")>0.25) {
			if(gameObject.renderer.bounds.Intersects(player.renderer.bounds)){
				Debug.Log ("yes");
				UseDoor ();
			} 
		}
	}

	void UseDoor() { //Could Add animtion here (or maybe a fade ?)
		//Debug.Log ("Used Door to position" + targetAnchor.transform.position);
		StartCoroutine (PlayDoorTransition ());

	}

	IEnumerator PlayDoorTransition(){
		transitionGUI.FadeUI (fadeTime / 2, fadeTime / 2);
		yield return new WaitForSeconds(fadeTime/2);
		player.transform.position = new Vector3(targetAnchor.transform.position.x,targetAnchor.transform.position.y,player.transform.position.z);
	}

}
