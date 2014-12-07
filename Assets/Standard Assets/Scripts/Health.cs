using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public int maxHealth = 10;
	public int curHealth = 10;
	public float healthBarLength = 200;
	public int pos = 40;
	public bool ShowHealthOnScreen = false;
	public Animator anim;


	//mole shield
	public bool shield;
	public float shieldRatio;

	private float healthBarMaxLength;
	public GUISkin skin;
	private FlashSprite flashSprite;

	void Awake () {
		flashSprite = gameObject.GetComponentInChildren<FlashSprite> ();
		curHealth = maxHealth;
		if (gameObject.tag == "Player")
			anim = GetComponent<Animator> ();
	}
	void Start () {
		healthBarLength = (Screen.width / 3) * (curHealth / (float)(maxHealth)); //current health Lenght
		healthBarMaxLength = (Screen.width / 3); //Health Bar Background Lenght
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.tag == "Player")
			anim.SetBool("isDead", false);
	}
	
	void OnGUI(){
		if(ShowHealthOnScreen == true) {
			GUI.Box (new Rect (10, pos, healthBarMaxLength, 20),"");
			GUI.Box (new Rect (10, pos, healthBarLength, 20),"");
			GUI.skin = skin;
			GUI.Label (new Rect(10, pos, healthBarMaxLength, 20),curHealth + "/" + maxHealth);
		}
	}
	
	public void AdjustCurrentHealth(int value){
		if(value < 0 && flashSprite != null) { //If damage taken is >1. Flash Sprite.
			flashSprite.Flash(Color.white);
		}
		if (shield)	{
			curHealth = curHealth + (int)(value * shieldRatio); //for mole shielding
		} else  {  
			curHealth += value;
		}

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
			StartCoroutine (DeathAnimation)();
			Application.LoadLevel (Application.loadedLevel);
		} else {
			GiveEnergyOnDeath giveEnergy = gameObject.GetComponent<GiveEnergyOnDeath>();
			if(giveEnergy != null) giveEnergy.GiveEnergy();
			StartCoroutine (DestroyAnimation ());
		}
	}

	IEnumerator DestroyAnimation(){
		gameObject.layer = 0; //Change layer to default so it doesnt trigger/collides with other object when dead
		SpriteRenderer[] objectSprites = gameObject.GetComponentsInChildren<SpriteRenderer> ();
		for(float i = 0; i < 1; i += Time.deltaTime/0.5f) {
			for(int j = 0; j < objectSprites.Length; j++) {
				objectSprites[j].color = new Color(1,1,1,1-i);
			}
			yield return null;
		}
		Destroy (gameObject);
	}

	Coroutine DeathAnimation(){
		anim.SetBool ("isDead", true);
		while (AnimationState.time >= AnimationState.length)
		{
			yield return null;
		}
		return null;
	}


}
