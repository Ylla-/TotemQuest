using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	public int totem; // 0 = normal, 1 = rabbit, 2 = mole, 3 = mantis

	// in order: maxSpeed, soloJumpForce, maxHealth, fireRate, shotDMG
	float[] rabbitStats = {6.5f,460f,8f, 0.5f,  4f};
	float[] moleStats   = {3f,  440f,15f,1.5f,  11f};
	float[] mantisStats = {5f,  445f,8f, 0.75f, 4f};
	float[] normalStats = {5f,  445f,10f,0.75f, 5f};
	bool canDash, canGlide, canShield, canSlowTime, canFloat, canSmash;


	Health health;

	//for horizontal movement
	public float maxSpeed = 5f;
	public float move;
	public bool moveAllowed;
	//public float HorizontalForceonAir = 8f;

	//for non fixed jump UNUSUED
	//float JumpDuration = 0.150f;
	float JumpPressedTime, JumpingPressed;
	//float jumpForce = 50f;
	//for fixed jump
	public float soloJumpForce = 440;
	public bool jumpAllowed = true;

	//for animation
	public bool facingRight = true;
	public Animator anim;

	//for dash
	public float dashDuration = 0.1f, dashSpeed = 20f;
	float dashStartTime; bool dash, dashing;

	//for gliding
	public float glideDuration = 0.25f, glideSpeed = 10f;
	float glideStartTime; bool glide; 
	bool airCounter = true;

	//for slo-mo
	bool slowMo = false;

	// for floating
	float gravityMod = 20f;
	bool Floating;

	// for shielding
	public float shieldRatio = 0.5f;
	public bool Shield;

	//for mole melee
	bool moleSmash;
	int moleDamage;
	float moleStrength;
	GameObject ExplosionPrefab;

	// ground check
	public bool onGround = false;
	public Transform groundCheckL, groundCheckR;
	float groundRadius = 0.01f;
	public LayerMask theGround;

	//for invincibility frame
	public float invincibilityTime = 0.3f; //Time for invulnerability when hit.
	private bool isInvincible = false; //Is the player currently invincible ?

	//on moving platform
	public bool onMovingPlatform;
	public Rigidbody2D movingRigidbody2D;

	PlayerAttack att;

	void Start () {
		anim = GetComponent<Animator> ();
		moveAllowed = true;
		att = (PlayerAttack)gameObject.GetComponent ("PlayerAttack");
		health = (Health)gameObject.GetComponent ("Health");

		//if you start as Mabellle, which I think should be default
		TurnIntoNormal ();
	}
	void Awake(){

				
		}
	
	
	void FixedUpdate () {
		if (isInvincible == true) return; //While the knockback method is not properly implemented, this will stop the update function while the player  is hit for invincibility time

		//on ground check
		if (Physics2D.OverlapCircle (groundCheckL.position, groundRadius, theGround) == true) {
			onGround = true;
				}
		else {
			onGround = Physics2D.OverlapCircle(groundCheckR.position, groundRadius, theGround);
		}




		if (canDash) {RabbitDash ();}
		if (canGlide) {MantisGlide ();}

		//moving in the x axis, moveAllowed allows to take away the movement control from the player
		move = Input.GetAxis ("Horizontal");
		if(moveAllowed){
			if(!onMovingPlatform){
				 //if(onGround)
				rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
			}
			else if (onMovingPlatform && onGround) {
				rigidbody2D.velocity = new Vector2 ((move * maxSpeed) + movingRigidbody2D.velocity.x, rigidbody2D.velocity.y);
			}
			else if(onMovingPlatform){
				rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
			}

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
	// it seems you cant use GetButtonDown in the FixedUpdate() 
	
	//For totem transformations
	void Update(){
	
	

	if (Input.GetButtonDown ("Normal")||Input.GetAxis("DPadYAxis")>0) totem = 0;
		if (Input.GetButtonDown ("Bunny")||Input.GetAxis("DPadXAxis")>0) totem = 1;
		if (Input.GetButtonDown ("Mole")||Input.GetAxis("DPadXAxis")<0) totem = 2;
		if (Input.GetButtonDown ("Mantis")||Input.GetAxis("DPadYAxis")<0) totem = 3;
	if (Input.GetButtonDown ("TransformForward")) totem=(totem+1)%4;
	if (Input.GetButtonDown ("TransformBackward")) totem=(totem+3)%4;
		//"DPadXAxis">0
	//Fixed jump
	if (Input.GetButtonDown("Jump") && onGround){
			Jump ();
		}

		//DASH
		if (canDash) {
			if (( (Input.GetButtonDown ("Ability1") || Input.GetButtonDown ("Fire1")) ) && onGround) {
				dash = true;
				}
		}
		// GLIDE
		if (canGlide) {
			if (( (Input.GetButtonDown ("Ability1") || Input.GetButtonDown ("Fire1")) ) && (!onGround)) {
				if(airCounter){
				glide = true;
				airCounter = false;
				}
			}
			if(onGround){
				airCounter = true;
			}
		}

		// time slow
		if (canSlowTime) {
			if (Input.GetButtonDown ("Ability2")) {
				RabbitSlowMotion ();
					}
				}
		// float mabelle
		if (canFloat) {
			if (Input.GetButtonDown ("Ability1") && onGround == false) {
				Floating = !Floating;
				MaBellesFloat (Floating);  
				}
			if(onGround){
				Floating = false;
				MaBellesFloat (Floating);
			}
			}

		//shield mole
		if (canShield) {
			if ( (Input.GetButtonDown ("Ability1") || Input.GetButtonDown ("Fire1")) ) {
				Shield = !Shield;
				MoleShield(Shield);
			}
			}
		//mole smash
		if (canSmash) {
						if ((Input.GetButtonDown ("Ability2") || Input.GetButtonDown ("Fire2"))) {
				moleSmash = true;
				moveAllowed = false;
				//MoleExplo();
						}
				}


		/*animator variables
		anim.SetFloat ("VerticalVelocity", rigidbody2D.velocity.y);
		anim.SetBool ("Jump", JumpingPressed);
		*/
		anim.SetBool ("Grounded", onGround);
		anim.SetBool ("Dash", dash);
		anim.SetBool ("Glide", glide);
		anim.SetBool ("Smash", moleSmash);

		anim.SetFloat ("Speed", Mathf.Abs (move));
		anim.SetInteger ("Totem", totem);
		
	}
	public void Jump(){ //Jump
		if(jumpAllowed == true) {
			StartCoroutine(JumpTimer ());
			rigidbody2D.AddForce (new Vector2 (0, soloJumpForce));
		}
	}

	public void Knockback(Vector2 hitDirection) {
		//TODO : This function is called when the player is hit to give a knockback to the player.
		// The vector2 hitDirection is the normalized vector of the direction from the ennemy to the player. (direction player should be knocked in, Or not if hit from top !)  

		//THis is a placeholder to a better knockback that should be implemented later :
		if(hitDirection.x > 0) {
			rigidbody2D.velocity = new Vector2( 4f, 6f);
		} else {
			rigidbody2D.velocity = new Vector2( -4f, 6f);
		}


	}
	
	void Flip(){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void RabbitDash(){
		if (dash && anim.GetCurrentAnimatorStateInfo(0).nameHash == HashIDs.rabbitDashState){
			Debug.Log ("Dash");
			moveAllowed = false;
			gameObject.layer = 16;
		   //if and else just to dash in the right direction
			if (facingRight)rigidbody2D.velocity = new Vector2 (dashSpeed, 0); 
			else rigidbody2D.velocity = new Vector2 (-dashSpeed, 0);
			dash = false; // set dash to false so this if block only executes once
		}
		else if (anim.GetCurrentAnimatorStateInfo(0).nameHash == HashIDs.rabbitDashState) { //executes ever frame when dashing
				if (facingRight) rigidbody2D.velocity = new Vector2 (dashSpeed, 0);
				else rigidbody2D.velocity = new Vector2 (-dashSpeed, 0);
			}
		}
	void RabbitSlowMotion(){
				if(!slowMo){
					Time.timeScale = 0.5f;
					slowMo=true;
			        Debug.Log ("slowmo");
				}else{
					Time.timeScale = 1f;
					slowMo=false;
				}
			}

	void MaBellesFloat(bool Floating){
		if (Floating) {
				rigidbody2D.drag = gravityMod;
				//Floating = true;
				} else {
					rigidbody2D.drag = 0f;
					//Floating = false;
				}
	}

	void MantisGlide(){

		if (glide && anim.GetCurrentAnimatorStateInfo(0).nameHash == HashIDs.mantisGlideState){
			Debug.Log ("Glide");
			moveAllowed = false;
			//if and else just to glide in the right direction
			if (facingRight)rigidbody2D.velocity = new Vector2 (glideSpeed, 0); 
			else rigidbody2D.velocity = new Vector2 (-glideSpeed, 0);
			glide = false; // set glide to false so this if block only executes once
		}
		else if (anim.GetCurrentAnimatorStateInfo(0).nameHash == HashIDs.mantisGlideState) { //executes ever frame when gliding
			if (facingRight) rigidbody2D.velocity = new Vector2 (glideSpeed, 0);
			else rigidbody2D.velocity = new Vector2 (-glideSpeed, 0);
		}
	}

	void MoleShield(bool shielding){
		if (Shield) {
			Debug.Log("shield");
			health.shield = true;
			health.shieldRatio = shieldRatio;
				}
		if (!Shield) {
			health.shield = false;
				}
		}
	void MoleMeleeDeactivate(){
		moleSmash = false;
		AllowMovement ();
	}
	void MoleExplo(){
		ExplosionPrefab = new GameObject();
		}

	//TOTEM TRANSFORMATIONS

	public void TurnIntoRabbit(){
		Debug.Log ("rabbit");
		maxSpeed = rabbitStats [0];
		soloJumpForce = rabbitStats [1];
		health.maxHealth = (int)rabbitStats [2];
		canDash = true;
		canGlide = false;
		canShield = false;
		canSlowTime = true;
		canFloat = false;
		canSmash = false;


		slowMo = true; //so it can be set to false with the method
		Floating = false; //deactivates mabelle float
		rigidbody2D.drag = 0f;
		RabbitSlowMotion ();

		att.Damage = (int)rabbitStats[4];
		att.ShootDelay = rabbitStats[3];

	}
	public void TurnIntoMole(){
		Debug.Log ("mole");
		maxSpeed = moleStats [0];
		soloJumpForce = moleStats [1];
		health.maxHealth = (int)moleStats [2];
		canDash = false;
		canGlide = false;
		canShield = true;
		canSlowTime = false;
		canFloat = false;
		canSmash = true;


		slowMo = true;
		Floating = false;
		rigidbody2D.drag = 0f;
		RabbitSlowMotion ();

		att.Damage = (int)moleStats[4];
		att.ShootDelay = moleStats[3];

	}
	public void TurnIntoMantis(){
		Debug.Log ("mantis");
		maxSpeed = mantisStats [0];
		soloJumpForce = mantisStats [1];
		health.maxHealth = (int)mantisStats [2];
		canDash = false;
		canGlide = true;
		canShield = false;
		canSlowTime = false;
		canFloat = false;
		canSmash = false;


		slowMo = true;
		Floating = false;
		rigidbody2D.drag = 0f;
		RabbitSlowMotion ();

		att.Damage = (int)mantisStats[4];
		att.ShootDelay = mantisStats[3];

	}
	public void TurnIntoNormal(){
		Debug.Log ("normal");
		maxSpeed = normalStats [0];
		soloJumpForce = normalStats [1];
		health.maxHealth = (int)normalStats [2];
		canDash = false;
		canGlide = false;
		canShield = false;
		canSlowTime = false;
		canFloat = true;
		canSmash = false;

		slowMo = true;
		Floating = false;
		rigidbody2D.drag = 0;
		RabbitSlowMotion ();

		att.Damage = (int)normalStats[4];
		att.ShootDelay = normalStats[3];
	}

	public void AllowMovement(){
		moveAllowed = true;
	                            }
	public void Invulnerable(){
		gameObject.layer = 16;
		}
	public void Vulnerable(){
		gameObject.layer = 13;
	}
	public void PreventMovement(){
		moveAllowed = false;
	}

	//Function Used when taking Damage. When hit, Call THIS function instead of changing the player's health through Health Script.
	public void DamagePlayer(int damage) {
		if(isInvincible == false) {
			health.AdjustCurrentHealth(-damage);
			StartCoroutine(InvincibilityTimer());
		}
	}

	IEnumerator InvincibilityTimer() {
		isInvincible = true;
		gameObject.layer = 16;
		yield return new WaitForSeconds (invincibilityTime);
		isInvincible = false;
		gameObject.layer = 13;
	}
	IEnumerator JumpTimer() { //makes sure the Jump dont get called multiple times (ex: stomping on two enemies at once would trigger jump twice).
		jumpAllowed = false;
		yield return new WaitForSeconds(0.1f);
		jumpAllowed = true;
	}



}


	