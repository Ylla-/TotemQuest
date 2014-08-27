using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	public int DMG = 20;
	Animator anim;
	bool fire;
	public ThrowProjectile shoot1;
	
	void Start () {
		//controller = (Controller)gameObject.GetComponent ("Controller");
		anim = GetComponentInParent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire")){
			fire = true;
		}
	}

	void FixedUpdate (){
			if (fire){
			fire = false;
			shoot1.ThrowFireball();

			           }
		}
	
	void Attack(){
		
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other is BoxCollider2D) {
			Debug.Log ("HIT");
			Health h = (Health)other.gameObject.GetComponent ("Health");
			
		}
	}
	
	
	
	
}

