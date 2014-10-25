using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayDialog : MonoBehaviour {

	public List<string> StringsToShow;
	public List<Vector2> Display;
	public bool skipAutomaticly = false;
	public float charDelay = 0.08f; //Delay between characters appeareance

	string currentText;
	GUIText guiText;


	int currentString = 0; //Current String of the list
	int currentChar = 0; //Current Char of the String
	bool StringShown = false; //is the current string finished showing
	bool wantToSkip = false; //If true, skip text if possible
	bool canClickSkip = true; //if false cant click (delay to avoid double clicks!)


	void Awake() {
		guiText = gameObject.GetComponent<GUIText> ();
	}

	void Start () {
		StartCoroutine (ShowText ());
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(Input.GetMouseButtonDown(0) && canClickSkip == true){
			StartCoroutine(ClickCooldown());
			wantToSkip = true;
		}
	}

	IEnumerator ShowText(){
		for(int i=0; i<StringsToShow.Count; i++){
			StringShown = false;
			currentText = ""; //Empty String
			object[] parms = new object[1]{StringsToShow[i]};
			StartCoroutine ("ShowString",parms);
			while(StringShown==false) {
				yield return null;
			}
		}
	}

	IEnumerator ShowString(object[] parms){
		string myString = (string) parms [0];
		for(int i=0; i< myString.Length; i++) {
			currentText += myString[i];
			guiText.text = currentText;

			if(wantToSkip == false) yield return new WaitForSeconds(charDelay); //if want to skip, dont do delay
			else yield return null;
		}
		wantToSkip = false;

		//After is shown 
		if(skipAutomaticly == true) {
			yield return new WaitForSeconds (0.75f);
		} else {
			while(true) {
				if(Input.GetMouseButtonDown(0) && canClickSkip == true) {
					StartCoroutine(ClickCooldown());
					break;
				}
				yield return null;
			}
		}

		StringShown = true;
	}

	IEnumerator ClickCooldown(){
		canClickSkip = false;
		yield return new WaitForSeconds(0.1f);
		canClickSkip = true;
	}


}
