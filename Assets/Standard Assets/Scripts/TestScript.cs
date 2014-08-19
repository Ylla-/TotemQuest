using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	public GUIStyle tempStyle;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//2014-08aug-19 ACP adding gui event to draw title
	void OnGUI() {
		GUI.Label(new Rect(100,100,1000,100),"<color=orange>TOTEM QUEST</color>",tempStyle);
	}



}
