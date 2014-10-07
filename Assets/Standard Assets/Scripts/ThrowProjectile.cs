using UnityEngine;
using System.Collections;

public class ThrowProjectile : MonoBehaviour {
	
	//Fireballs
	public GameObject Fireball;
	public GameObject Mantis_Fireball;
	public GameObject Mole_Fireball;
	public GameObject Rabbit_Fireball;


	private Controller controller;
	private Animator anim;
	
	

	void Start () {
		anim = GetComponentInParent<Animator> ();
		controller = GetComponentInParent<Controller>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ThrowFireball(int dmg){
		
		// Create a new shot
		Transform fireballTransform;
		if(controller.totem == 0) { //If Normal
			fireballTransform = ((GameObject)Instantiate(Fireball)).transform;

		} else if (controller.totem == 1) { //If Rabbit
			fireballTransform = ((GameObject)Instantiate(Rabbit_Fireball)).transform;
		} else if (controller.totem == 2) { //If Mole
			fireballTransform = ((GameObject)Instantiate(Mole_Fireball)).transform;
		} else { //If Mantis
			fireballTransform = ((GameObject)Instantiate(Mantis_Fireball)).transform;
		}

		//Assign Damage
		ProjectileScript shot = fireballTransform.gameObject.GetComponent<ProjectileScript> ();
		shot.Damage = dmg;

		// Assign position and direction
		//fireballTransform.position = transform.position;
		fireballTransform.position = new Vector3 (transform.position.x, transform.position.y, -0.2f);
		shot.facingRight = controller.facingRight;

	
	}
	
}
