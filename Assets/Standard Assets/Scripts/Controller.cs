using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	public float maxSpeed = 5f;
	public float HorizontalForceonAir = 8f;
	
	public float JumpDuration = 0.150f;
	public float JumpPressedTime;
	public bool JumpingPressed;
	
	public bool facingRight = true;
	public float move;
	public Animator anim;

	
	public bool onGround = false;
	public Transform groundCheck;
	float groundRadius = 0.05f;
	public LayerMask theGround;
	public float jumpForce = 50f;
	public float soloJumpForce = 440;


	
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	
	void FixedUpdate () {
		//on ground check
		onGround = Physics2D.OverlapCircle(groundCheck.position, groundRadius, theGround);
		
		//moving in the x axis
		move = Input.GetAxis ("Horizontal");
		//if(onGround)
			rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
		//if(!onGround)
		//	rigidbody2D.AddForce(new Vector2 (move * HorizontalForceonAir, 0));
		
		
		/*jumping hold to go higher
		if (Input.GetButton ("Jump")) {
			if (onGround) {
				JumpPressedTime = Time.time;
				JumpingPressed = true;
				rigidbody2D.AddForce (new Vector2 (0, jumpForce));
			} else {
				if (JumpingPressed && ((Time.time - JumpPressedTime) >= JumpDuration)) {
					JumpingPressed = false;
				} else {
					if (JumpingPressed) {
						rigidbody2D.AddForce (new Vector2 (0, jumpForce));
					}
				}
			}
		}   */

		//Fixed jump
		if (Input.GetButtonDown ("Jump")) {
			if (onGround) {
			    rigidbody2D.AddForce (new Vector2 (0, soloJumpForce));
			}
		}


		//flipping for running left or right
		if (move > 0 && facingRight) {
			Flip ();
			facingRight = !facingRight;
		} else if (move < 0 && !facingRight) {
			Flip ();
			facingRight = !facingRight;
		}

	}
	
	
	void Update(){


		
		
		/*animator variables
		anim.SetFloat ("Speed", Mathf.Abs (move));
		anim.SetFloat ("VerticalVelocity", rigidbody2D.velocity.y);
		anim.SetBool ("Jump", JumpingPressed);
		anim.SetBool ("Grounded", onGround);
		*/
	
		
	}
	
	
	void Flip(){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}


	