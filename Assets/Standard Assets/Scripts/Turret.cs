using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
	
	public GameObject TurretProjectile;
	public float fireRate = 1f;
	public float projSpeedX, projSpeedY;
	
	
	void Start () {

	}

	void FixedUpdate(){
			//InvokeRepeating ("FireProjectile", 0f, fireRate); saved for future references (i always foget it exists)
		if (Time.fixedTime % fireRate == 0) {
			FireProjectile();
				}
	}


	public void FireProjectile(){
		
		// Create a new shot
		Transform projectile;
		projectile = ((GameObject)Instantiate(TurretProjectile)).transform;
		
		ProjectileScript script = projectile.gameObject.GetComponent<ProjectileScript> ();
		script.speed = projSpeedX;
		script.speedY = projSpeedY;
		
		projectile.position = transform.position;
		
		
	}
	
}
