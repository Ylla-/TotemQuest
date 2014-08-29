 using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	//NOW, THIS IS GONNA LOOK UGLY, BUT BEAR WITH ME, AND WATCH IT WORK;
	public int totem; // 0 = normal, 1 = rabbit, 2 = mole, 3 = mantis

	// in order: maxSpeed, soloJumpForce, maxHealth,
	public float[] rabbitStats = {5f,440f,8f};
	public float[] moleStats   = {3f,400f,15f};
	public float[] mantisStats = {6f,440f,8f};
	public float[] normalStats = {5f,440f,10f};


	Health health;

	//for horizontal movement
	public float maxSpeed = 5f;
	public float move;
	bool moveAllowed;
	//public float HorizontalForceonAir = 8f;

	//for non fixed jump UNUSUED
	public float JumpDuration = 0.150f;
	float JumpPressedTime, JumpingPressed;
	public float jumpForce = 50f;
	//for fixed jump
	public float soloJumpForce = 440;


	//for animation
	public bool facingRight = true;
	public Animator anim;

	//for dash
	public float dashDuration = 0.1f, dashSpeed = 20f;
	float dashStartTime; bool dash, dashing;

	//for gliding
	public float glideDuration = 0.25f, glideSpeed = 10f;
	float glideStartTime; bool glide, gliding;

	// ground check
	public bool onGround = false;
	public Transform groundCheckL, groundCheckR;
	float groundRadius = 0.1f;
	public LayerMask theGround;


	
	void Start () {
		anim = GetComponent<Animator> ();
		moveAllowed = true;
		health = (Health)gameObject.GetComponent ("Health");

	}
	
	
	void FixedUpdate () {
		//on ground check
		if (Physics2D.OverlapCircle (groundCheckL.position, groundRadius, theGround) == true) {
			onGround = true;
				}
		else {
			onGround = Physics2D.OverlapCircle(groundCheckR.position, groundRadius, theGround);
		}

		RabbitDash();
		MantisGlide();


		//moving in the x axis, moveAllowed allows to take away the movement control from the player
		if(moveAllowed){
		move = Input.GetAxis ("Horizontal");
		//if(onGround)
			rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
		//if(!onGround)
		//	rigidbody2D.AddForce(new Vector2 (move * HorizontalForceonAir, 0));
	    }

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

	
 

		//flipping for running left or right
		if (move > 0 && !facingRight) {
			Flip ();
			facingRight = !facingRight;
		} else if (move < 0 && facingRight) {
			Flip ();
			facingRight = !facingRight;
		}

	}
	
	//For totem transformations
	void Update(){
	if (Input.GetButtonDown ("Normal")) {
			totem = 0;
				} else if (Input.GetButtonDown ("Bunny")) {
			totem = 1;
				} else if (Input.GetButtonDown ("Mole")) {
			totem = 2;
				} else if (Input.GetButtonDown ("Mantis")) {
			totem = 3;
				}
						

	
	//Fixed jump
	if (Input.GetButtonDown("Jump") && onGround){
		rigidbody2D.AddForce (new Vector2 (0, soloJumpForce));
		}

		// it seems you cant use GetButtonDown in the FixedUpdate() DASH
	if ((Input.GetButtonDown ("Dash")||Input.GetButtonDown("Fire1")) && onGround) {
						dash = true;
				}
		// GLIDE
	if ((Input.GetButtonDown ("Dash")||Input.GetButtonDown("Fire1")) && !onGround) {
			glide = true;
		}
		/*animator variables
		anim.SetFloat ("VerticalVelocity", rigidbody2D.velocity.y);
		anim.SetBool ("Jump", JumpingPressed);
		*/
		anim.SetBool ("Grounded", onGround);
		anim.SetBool ("Dash", dash);
		anim.SetBool ("Glide", glide);
		anim.SetFloat ("Speed", Mathf.Abs (move));
		anim.SetInteger ("Totem", totem);
		
	}
	
	
	void Flip(){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void RabbitDash(){
		if (dash){
			Debug.Log ("Dash");
			dash = false;
			moveAllowed = false;
			dashStartTime = Time.time;            //if and else just to dash in the right direction
			if (facingRight)rigidbody2D.velocity = new Vector2 (dashSpeed, rigidbody2D.velocity.y); 
			else rigidbody2D.velocity = new Vector2 (-dashSpeed, rigidbody2D.velocity.y);
			dashing = true;
		}
		if (dashing == true) {
			if ((Time.time - dashStartTime) < dashDuration) {
				if (facingRight) rigidbody2D.velocity = new Vector2 (dashSpeed, rigidbody2D.velocity.y);
				else rigidbody2D.velocity = new Vector2 (-dashSpeed, rigidbody2D.velocity.y);
			} else {
				rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
				moveAllowed = true;
				dashing = false;
			}
		}
	}

	void MantisGlide(){
		// Glide mantis
		if (glide){
			Debug.Log ("Glide");
			glide = false;
			moveAllowed = false;
			glideStartTime = Time.time;            //if and else just to dash in the right direction
			if (facingRight)rigidbody2D.velocity = new Vector2 (glideSpeed, 0); 
			else rigidbody2D.velocity = new Vector2 (-glideSpeed, 0);
			rigidbody2D.gravityScale = 0f;
			gliding = true;
		}
		if (gliding == true){
			if ((Time.time - glideStartTime) < glideDuration) {
				if (facingRight)rigidbody2D.velocity = new Vector2 (glideSpeed, 0);
				else rigidbody2D.velocity = new Vector2 (-glideSpeed, 0);
			} else {
				rigidbody2D.velocity = new Vector2 (move*maxSpeed, rigidbody2D.velocity.y);
				moveAllowed = true;
				rigidbody2D.gravityScale = 1f;
				gliding = false;
			} 
		}

	}

	void TurnIntoRabbit(){
		maxSpeed = rabbitStats [0];
		soloJumpForce = rabbitStats [1];
		health.maxHealth = (int)rabbitStats [2];

	}
	void TurnIntoMole(){
	}
	void TurnIntoMantis(){
	}
	void TurnIntoNormal(){
	}



}


	