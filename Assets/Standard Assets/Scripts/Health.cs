using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public int maxHealth = 10;
	public int curHealth = 10;
	public float healthBarLength = 200;
	public int pos = 40;

	private float healthBarMaxLength;
	private GUIStyle centeredText;


	void Start () {
		healthBarLength = (Screen.width / 3) * (curHealth / (float)(maxHealth)); //current health Lenght
		healthBarMaxLength = (Screen.width / 3); //Health Bar Background Lenght
		//Creating a new GUISTYLE
		centeredText = new GUIStyle ();
		centeredText.alignment = TextAnchor.MiddleCenter;

	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnGUI(){
		GUI.Box (new Rect (10, pos, healthBarMaxLength, 20),"");
		GUI.Box (new Rect (10, pos, healthBarLength, 20),"");
		GUI.Label (new Rect(10, pos, healthBarMaxLength, 20),curHealth + "/" + maxHealth,centeredText);
	}
	
	public void AdjustCurrentHealth(int value){
		curHealth += value;
		
		if (curHealth < 1) {
						curHealth = 0;
						Die();
				}
		if (curHealth > maxHealth) {
			curHealth = maxHealth;
				}
		
		healthBarLength = (Screen.width / 3) * (curHealth / (float)(maxHealth));	
	}

	void Die(){
		if (gameObject.tag == "Player") {
						Application.LoadLevel (Application.loadedLevel);
				} else {
						DestroyObject (gameObject);
				}
		}

}
