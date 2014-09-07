using UnityEngine;
using System.Collections;

public class Behaviour_Goomba : MonoBehaviour {

	/// <summary>
	/// AI for the Basic "Goomba" enemy which just walks in a dicrection until it hits a wall or object, then turn back. 
	/// 
	/// Mouvement : Walks until it hits an object, then turn back.
	/// Attack : None, Damage the player if he touches the side of it.
	/// How To Kill : Jump on hit OR kill it with Attacks.
	/// 
	/// </summary>

	public float speed = 1f;
	public bool goingLeft = true; //which direction is the enemy moving
	public CircleCollider2D frontCollider;
	public LayerMask theGround;

	private bool canRotate = true;
	private Vector3 overlapSpherePosition;

	// Use this for initialization
	void Start () {;
		updateDirection ();
	}

	void FixedUpdate() {
	

		//Check Wall Collision
		if (Physics2D.OverlapCircle (transform.position + overlapSpherePosition, frontCollider.radius, theGround) == true
		    && canRotate == true) {
			StartCoroutine (Flip ());
			Debug.Log ("collision !");
		}

		//Move()
		if(goingLeft == true) {
			rigidbody2D.velocity = new Vector2(-speed,rigidbody2D.velocity.y);
		} else {
			rigidbody2D.velocity = new Vector2(speed,rigidbody2D.velocity.y);
		}

	}

	void updateDirection(){
		if(goingLeft == true) {
			transform.localScale = new Vector3(-1,1,1);
			overlapSpherePosition = new Vector3 (-frontCollider.center.x, frontCollider.center.y,0);
		} else {
			transform.localScale = new Vector3(1,1,1);
			overlapSpherePosition = new Vector3 (frontCollider.center.x, frontCollider.center.y,0);
		}
	}

	IEnumerator Flip(){ //Flip enemy position and mouvement
		goingLeft = !goingLeft;//change direction
		updateDirection(); //Update direction

		//This flip can only happen every 0.5 seconds to avoid constant flipping.
		canRotate = false;
		yield return new WaitForSeconds (0.5f);
		canRotate = true;
	}

}
