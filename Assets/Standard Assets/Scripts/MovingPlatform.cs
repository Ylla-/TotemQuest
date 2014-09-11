using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {
	Transform thisTransform;
	Rigidbody2D thisRigidbody2D;
	public float interval = 1f;
	public float limitX1, limitX2, limitY1, limitY2;
	public float distanceX, speedX;
	public bool moving, left;
	
	
	void Start () {
		thisTransform = GetComponent<Transform>();
		thisRigidbody2D = GetComponent<Rigidbody2D> ();
		distanceX = Mathf.Abs (limitX2 - limitX1);
		speedX = distanceX / interval;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
				if ((Time.time / interval) == 0) {
						moving = true;
				}
		
		
				if (moving) { 
			
						if (!left && (limitX2 >= rigidbody2D.position.x)) { //moving right
								rigidbody2D.velocity = new Vector2 (speedX, rigidbody2D.velocity.y);
						} else if (!left) {
								rigidbody2D.velocity = new Vector2 (0, 0);
								left = true;
						}
			
						if (left && (limitX1 <= rigidbody2D.position.x)) { //moving left
								rigidbody2D.velocity = new Vector2 (-speedX, rigidbody2D.velocity.y);
						} else if (left) {
								rigidbody2D.velocity = new Vector2 (0, 0);
								left = false;
						}
			
				}
		}



	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider is CircleCollider2D) {
						Debug.Log ("on platform");
						Controller c = (Controller)coll.gameObject.GetComponent ("Controller");	
						c.movingRigidbody2D = thisRigidbody2D;
						c.onMovingPlatform = true;
				}
		}
	void OnCollisionExit2D(Collision2D coll){
		if (coll.collider is CircleCollider2D) {
						Debug.Log ("off platform");
						Controller c = (Controller)coll.gameObject.GetComponent ("Controller");	
						c.onMovingPlatform = false;
				}
	}

	
	
}
