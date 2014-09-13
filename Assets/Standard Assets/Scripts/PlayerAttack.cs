using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	public int DMG = 20;
	public float shootDelay = 1;
	Animator anim;
	bool canShoot, shoot;
	public ThrowProjectile shoot1;
	Controller controller;
	
	void Start () {
		controller = (Controller)gameObject.GetComponent ("Controller");
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
			shoot1.ThrowFireball(DMG);
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
		yield return new WaitForSeconds(shootDelay);
		canShoot = true;
	}
	

	public int Damage{
		get{ return DMG;}
		set{ DMG = value;}
	}
	

	public float ShootDelay{
		get{ return shootDelay;}
		set{ shootDelay = value;}
	}
	

	
	
}

