using UnityEngine;
using System.Collections;

public class ThrowProjectile : MonoBehaviour {
	
	//Fireball
	public Transform Fireball;
	public Controller cont;
	public Animator anim;
	
	
	
	void Start () {
		anim = GetComponentInParent<Animator> ();
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	
	
	
	
	
	
	
	
	
	public void ThrowFireball(){
		
		// Create a new shot
		var fireballTransform = Instantiate (Fireball) as Transform;
		
		
		// Assign position
		fireballTransform.position = transform.position;
		
		ProjectileScript shot = fireballTransform.gameObject.GetComponent<ProjectileScript> ();
		//direction of the shot
		shot.facingRight = cont.facingRight;
		// Assign position
		fireballTransform.position = new Vector3 (transform.position.x, transform.position.y, -0.2f);
		
		
		
	}
	
}
