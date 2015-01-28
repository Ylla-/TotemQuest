using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	public int DMG = 20;
	public float shootDelay = 1;
	Animator anim;
	bool canShoot, shoot;
	public ThrowProjectile shoot1;
	Controller controller;
	public AudioClip attackSound;
	
	void Start () {
		controller = (Controller)gameObject.GetComponent ("Controller");
		anim = GetComponentInParent<Animator> ();
		canShoot = true;
		//Assigned .wav file to public AudioClip attackSound in properties instead of resources.load
		//if you can get resources.load method working feel free to change!
		//attackSound = (AudioClip)Resources.Load ("Resources/Audio/Michelle_Shoot.wav", typeof (AudioClip));

	}
	
	// Update is called once per frame
	void Update () {
		if( (Input.GetButtonDown("Fire")) && canShoot){
			if(controller.totem != 0) {
				if(controller.orbs.currentEnergy > 0) {
					controller.orbs.RemoveEnergy(1);
					Shoot ();
				}
			} else {
				Shoot();
			}

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

	void Shoot() {
		shoot = true;
		StartCoroutine(FiringRate ());
		audio.PlayOneShot (attackSound,1);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other is BoxCollider2D) {
			//Debug.Log ("HIT - PLAYER ATTACK");
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

