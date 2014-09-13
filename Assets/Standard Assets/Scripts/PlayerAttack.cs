using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	public int DMG = 20;
	Animator anim;
	bool canShoot, shoot;
	public ThrowProjectile shoot1;
	
	void Start () {
		//controller = (Controller)gameObject.GetComponent ("Controller");
		anim = GetComponentInParent<Animator> ();
		canShoot = true;
	}
	
	// Update is called once per frame
	void Update () {
		if( (Input.GetButtonDown("Fire")||Input.GetButtonDown("Fire2")) && canShoot){
			shoot = true;
			StartCoroutine(FiringRate ());


		}
	}

	void FixedUpdate (){
			if (shoot){
			shoot = false;
			shoot1.ThrowFireball();
			           }
		}
	
	void Attack(){
		
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other is BoxCollider2D) {
			Debug.Log ("HIT - PLAYER ATTACK");
			Health h = (Health)other.gameObject.GetComponent ("Health");
			
		}
	}

	IEnumerator FiringRate() {
		canShoot = false;
		yield return new WaitForSeconds(2);
		canShoot = true;
	}
	
	
	
	
}

