using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
		
	public GameObject TurretProjectile;
	public float fireRate, projSpeedX, projSpeedY;
		
		
		void Start () {
		fireRate = 1f;
		}
		
	public void FixedUpdate(){
		if (Time.time % fireRate == 0) {
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
