using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	static bool isLoaded = false;
	public bool[] levelFinished;
	public bool lastLevelUnlocked = false;

	void Awake(){
		if(isLoaded == false) {
			GameObject.DontDestroyOnLoad (gameObject);
			isLoaded = true;
			gameObject.name = "GameManager";
			gameObject.tag = "GameManager";
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
		if(levelFinished[0] == true && levelFinished[1] == true && levelFinished[2] == true){
			lastLevelUnlocked = true;
		}
	}

	//Freezes the game for an amount of time. This is used to freeze the game for really small intervals when certains events happens
	//to add "game feel". For exemple, this is called when the player is hit to emphasize the event.
	//Try to keep those freeze VERY short. They must be barely noticeable.
	public void FreezeGame(float time) {
		StartCoroutine(Freeze (time)); 
	}

	IEnumerator Freeze(float freezeTime){
		//We cannot use Time.deltaTime here since we're modifying the time scale.
		//Time.realtimeSinceStartup returns the time elapsed since the start, without taking TimeScale into account.
		float timeToEndFreeze = Time.realtimeSinceStartup + freezeTime;
		Time.timeScale = 0;
		while (Time.realtimeSinceStartup < timeToEndFreeze) { //Wait until we reached desired time to resume timeScale
			yield return 0;
		}
		Time.timeScale = 1;
	}


}
