using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public int maxHealth = 10;
	public int curHealth = 10;
	public float healthBarLength;
	public int pos = 40;
	// Use this for initialization
	void Start () {
		healthBarLength = (Screen.width / 3) * (curHealth / (float)(maxHealth));
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
	}
	
	void OnGUI(){
		GUI.Box (new Rect (10, pos, healthBarLength, 20), curHealth + "/" + maxHealth);
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
