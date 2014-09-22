using UnityEngine;
using System.Collections;

public class CompleteLevel : MonoBehaviour {

	public bool isLastLevel = false;
	public int levelToComplete = 0;
	GameManager manager;
	GameObject player;
	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Vertical")||Input.GetAxis ("Vertical")>0.25) {
			if(gameObject.renderer.bounds.Intersects(player.renderer.bounds)){
				if(isLastLevel == false) FinishLevel();
				else Application.LoadLevel (7); //GO TO CREDITS
			} 
		}
	}

	void FinishLevel(){
		manager.levelFinished [levelToComplete] = true;
		Application.LoadLevel (2);
	}

}
