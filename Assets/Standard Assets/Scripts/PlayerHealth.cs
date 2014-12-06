using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	//self explanatory - max health level for player along with the length of the healthbar
	public int maxHealth = 10;
	public int curHealth = 10;
	public float healthBarLength = 200;
	public int pos = 40;
	public bool ShowHealthOnScreen = false;
	
	
	
	//mole shield
	public bool shield;
	public float shieldRatio;
	
	private float healthBarMaxLength;
	public GUISkin skin;
	private FlashSprite flashSprite;


	//Animation
	Animator anim;
	public bool isDead = false;


	// Use this for initialization
	void Start () {
		healthBarLength = (Screen.width / 3) * (curHealth / (float)(maxHealth)); //current health Lenght
		healthBarMaxLength = (Screen.width / 3); //Health Bar Background Lenght
	}

	void Awake(){
		anim = GetComponent<Animator> ();

		}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool ("isDead", false);

	}

	public void AdjustcurHealth(int adj){
		curHealth += adj;

		if(curHealth <0)
			curHealth = 0;
			Die ();
		if(curHealth>maxHealth)
			curHealth = maxHealth;
		isDead = false;
		if(maxHealth<1)
			maxHealth=1;
		isDead = false;
		healthBarLength = (Screen.width/2)*(curHealth/(float)maxHealth);


		}



	void Die(){
		DeathAnimation ();
		if (anim.GetNextAnimatorStateInfo(0).IsName("isDead")) {
			anim.SetBool("isDead", true);
		}
		}

	Coroutine DeathAnimation(){

		return null;
	}





}
