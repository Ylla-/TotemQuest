using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {	

	public Transform thisTransform;
	Rigidbody2D thisRigidbody2D;
	public float interval = 1f;
	public float delay = 0f; //Platforms wait "delay" seconds before moving.
	public float limitX1, limitX2, limitY1, limitY2;
	float distanceX, speedX, speedY, distanceY;
	public bool moving, left;

	
	
	void Start () {
		thisTransform = GetComponent<Transform>();
		thisRigidbody2D = GetComponent<Rigidbody2D> ();


		distanceX = Mathf.Abs(limitX2 - limitX1);
		speedX = distanceX / interval;
		distanceY = Mathf.Abs(limitY2 - limitY1);
		speedY = distanceY / interval;

		StartCoroutine (StartMoving()); // New Coroutine Added To simulate what the previous code was doing.
	}



	// Update is called once per frame
	void FixedUpdate () {
		
		/* // @Ylla : 
		 * // I commented this out since this causes the platforms not to move when respawning since time is never equal to 0 again. 
		 * // Also, Why is moving not always true ? Is it to have delayed Platforms ? If so I've replaced it with a Coroutine that does
		 * // The same thing and works on respawn. 
		if ((Time.time / interval) == 0) {
			moving = true;
		}
		*/
		
		if (moving) { 
			if (!left) {
				if (limitX2 >= rigidbody2D.position.x && limitY2 >= rigidbody2D.position.y) {
					rigidbody2D.velocity = new Vector2 (speedX, rigidbody2D.velocity.y);
					rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x, +speedY);
				} else if (!left) {
					rigidbody2D.velocity = new Vector2 (0, 0);
						left = true;
				}
			}				
			if (left) {
				if (limitX1 <= rigidbody2D.position.x && limitY1 <= rigidbody2D.position.y) { 
					rigidbody2D.velocity = new Vector2 (-speedX, rigidbody2D.velocity.y);
					rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x, -speedY);
				} else if (left) {
					rigidbody2D.velocity = new Vector2 (0, 0);
					left = false;
				}
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

	//This coroutine wait for interval seconds then sets the moving bool to true
	IEnumerator StartMoving(){
		yield return new WaitForSeconds(delay);
		moving = true;
	}

	
	
}
