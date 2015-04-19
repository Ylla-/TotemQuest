using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	static bool isLoaded = false;
	public bool[] levelFinished;
	public bool[] totemUnlocks;//0 = normal, 1 = rabbit, 2 = mole, 3 = mantis
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
		InitializeTables ();
		//DEBUG_UNLOCKTOTEMS (); //TODO :Remove this for release !
	}
	
	// Update is called once per frame
	void Update () {
		if(levelFinished[0] == true && levelFinished[1] == true && levelFinished[2] == true){
			lastLevelUnlocked = true;
		}
		if (Input.GetKeyDown (KeyCode.U)) {
			DEBUG_UNLOCKTOTEMS ();
		}
	}

	void InitializeTables(){
		levelFinished = new bool[3];
		for(int i=0; i<levelFinished.Length; i++) 	levelFinished [i] = false;
		totemUnlocks = new bool[4];
		//for(int i=0; i<totemUnlocks.Length; i++) 	totemUnlocks [i] = false;
		//totemUnlocks [0] = true; //Can transform into basic form
		DEBUG_UNLOCKTOTEMS ();
	}
	void DEBUG_UNLOCKTOTEMS(){
		for(int i=0; i<totemUnlocks.Length; i++) 	totemUnlocks [i] = true;
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
