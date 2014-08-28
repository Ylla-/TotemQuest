using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	public string ForestLevel;
	public string PlainsLevel;
	public string CavesLevel;
	public string FinalLevel;
	public string Title;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// 2014-08AUG-28 ACP Buttons properties are hardcoded should be re-written to be easier to edit
	void OnGUI () {
		//plains
		if 
			(GUI.Button(new Rect(10,10,100,100),"Plains")) Application.LoadLevel(PlainsLevel);
		//forest
		if 
			(GUI.Button(new Rect(10,120,100,100),"Forest")) Application.LoadLevel(ForestLevel);
		//caves
		if 
			(GUI.Button(new Rect(10,230,100,100),"Caves")) Application.LoadLevel(CavesLevel);

		//final level starts disabled
		GUI.enabled=false;
		if 
			(GUI.Button(new Rect(10,340,100,100),"Final")) Application.LoadLevel(FinalLevel);
		GUI.enabled=true;

		//back to title
		if
			(GUI.Button(new Rect(Screen.width-100-10,10,100,100),"Back To Title")) Application.LoadLevel(Title);
	}
}
