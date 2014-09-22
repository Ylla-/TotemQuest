using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	static bool isLoaded = false;
	public bool[] levelFinished;

	void Awake(){
		if(isLoaded == false) {
			GameObject.DontDestroyOnLoad (gameObject);
			isLoaded = true;
			gameObject.name = "GameManager";
		} else {
			Destroy (gameObject);
		}
	}

	void Start () {
		levelFinished = new bool[3];
		levelFinished [0] = false;
		levelFinished [1] = false;
		levelFinished [2] = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
