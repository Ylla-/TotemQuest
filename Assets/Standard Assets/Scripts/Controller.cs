using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	public int totem; // 0 = normal, 1 = rabbit, 2 = mole, 3 = mantis

	// in order: maxSpeed, soloJumpForce, maxHealth, fireRate, shotDMG
	float[] rabbitStats = {6.5f,460f,8f, 0.75f, 10f};
	float[] moleStats   = {3f,  440f,15f,1.5f,  14f};
	float[] mantisStats = {5f,  445f,8f, 0.20f, 6f};
	float[] normalStats = {5f,  445f,10f,0.75f, 5f};
	bool canDash, canGlide, canShield, canSlowTime, canFloat, canSmash;


	Health health;

	//for horizontal movement
	public float maxSpeed = 5f;
	public float move;
	public bool moveAllowed;
	//public float HorizontalForceonAir = 8f;


	//CharacterController myCC;


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
	float orbSlowMoRatio = 0.35f;

	// for floating
	float gravityMod = 20f;
	bool Floating;

	// for shielding
	public float shieldRatio = 0.5f;
	public bool Shield;
	float orbShieldRatio = 0.30f;

	//for mole melee
	bool moleSmash;
	int moleDamage;
	float moleStrength;
	public GameObject ExplosionPrefab;
	Vector2 origin;

	// ground check
	public bool onGround = false;
	public Transform groundCheckL, groundCheckR;
	float groundRadius = 0.01f;
	public LayerMask theGround;

	//for invincibility frame
	public float knockbackTime = 0.5f; //Time where the player loses control when hit
	private bool isKnockbacked = false; //Is the player currently being Knockbacked ?
	public float invincibilityTime = 0f; //Time for invulnerability when hit.
	private bool isInvincible = false; //Is the player currently invincible ?

	//on moving platform
	public bool onMovingPlatform;
	public Rigidbody2D movingRigidbody2D;

	//GameManager and Camera
	private GameManager gameManager;
	private CameraScript cameraScript;

	PlayerAttack att;
	public EnergyBar orbs;

	//declaration of audio clips
	public AudioClip playerWalk;
	public AudioClip playerStandJump;
	public AudioClip playerWalkJump;
	public AudioClip playerDamageTaken;
	public AudioClip playerTransform;

	//DEBUG : INVINCIBILITY MODE
	bool DEBUG_INVINCIBLE = false;

	void Start () {
		moveAllowed = true;
		health.shieldRatio = shieldRatio;
		//assign corresponding audio files to their audio variables
		//playerWalk = (AudioClip)Resources.Load ("Audio/Michelle_Walk.wav", typeof(AudioClip));
		//playerStandJump=(AudioClip)Resources.Load ("Audio/Michelle_Jump_Standing.wav",typeof(AudioClip));
		//playerWalkJump = (AudioClip)Resources.Load ("Audio/Michelle_Jump_Walking.wav", typeof(AudioClip));
		//playerTransform = (AudioClip)Resources.Load ("Audio/transformation.wav", typeof(AudioClip));

		//if you start as Mabellle, which I think should be default'

		TurnIntoNormal ();
		//CharacterController cc = GetComponent(typeof(CharacterController)) as CharacterController;
		CharacterController myCC = GetComponent<CharacterController>();
		if (myCC != null) {
						myCC.enabled = true; // Turn on the component
				} else
						Debug.Log ("Broken controller");
	}
	void Awake(){
		//Get the different components. Try and do this in Awake in case some other scripts require those components in their start()	
		att = gameObject.GetComponent<PlayerAttack>();
		health = gameObject.GetComponent <Health>();
		anim = GetComponent<Animator> ();
		GameObject manager_Obj = GameObject.FindGameObjectWithTag ("GameManager");
		if (manager_Obj != null) gameManager = (GameManager)manager_Obj.GetComponent<GameManager> ();
		else Debug.Log ("MANAGER OBJECT NOT FOUND !");
		GameObject camera_Obj = GameObject.FindGameObjectWithTag ("MainCamera");
		if (camera_Obj != null) cameraScript = (CameraScript)camera_Obj.GetComponentInParent<CameraScript> ();
		else Debug.Log ("CAMERA OBJECT NOT FOUND !");
	}
	
	
	void FixedUpdate () {
		//DEBUG INVINCIBLE 
		if(Input.GetKey(KeyCode.Alpha0)) DEBUG_INVINCIBLE = !DEBUG_INVINCIBLE;
		if(DEBUG_INVINCIBLE == true) health.curHealth = health.maxHealth;
						


		if (isKnockbacked == true) return; //While the knockback method is not properly implemented, this will stop the update function while the player  is hit for invincibility time

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
				rigidbody2D.velocity = new Vector2 ((move * maxSpeed) + movingRigidbody2D.velocity.x, movingRigidbody2D.velocity.y);
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
	int nextTotem(){
		int x=totem;
		while(true){
			x=(x+1)%4;
			if(gameManager.totemUnlocks[x] == true) return x;
		}
	}
	int previousTotem(){
		int x=totem;
		while(true){
			x=(x+3)%4;
			if(gameManager.totemUnlocks[x] == true) return x;
		}
	}
	//For totem transformations
	void Update(){


	if (Input.GetButtonDown ("Normal")) if(gameManager.totemUnlocks[0] == true) totem = 0;
		if (Input.GetButtonDown ("Bunny"))if(gameManager.totemUnlocks[1] == true)totem = 1;
	if (Input.GetButtonDown ("Mole")) if(gameManager.totemUnlocks[2] == true)totem = 2;
	if (Input.GetButtonDown ("Mantis")) if(gameManager.totemUnlocks[3] == true)totem = 3;
	if (Input.GetButtonDown ("TransformForward")) totem=nextTotem();
	if (Input.GetButtonDown ("TransformBackward")) totem=previousTotem();

	//Fixed jump
	if (Input.GetButtonDown("Jump") && onGround){
			Jump ();
		}

		//DASH
		if (canDash) {
			if (( (Input.GetButtonDown ("Ability1")) ) && onGround) {
				dash = true;
				}
		}
		// GLIDE
		if (canGlide) {
			if (( (Input.GetButtonDown ("Ability1")) ) && (!onGround)) {
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
			if ((Input.GetButtonDown ("Ability1")) && onGround == false) {
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
			if ( Input.GetButtonDown ("Ability1")) {
				MoleShield();
			}
			}
		//mole smash
		if (canSmash) {
			if (Input.GetButtonDown ("Ability2")){// || Input.GetButtonDown ("Fire2")) {
				moleSmash = true;
				moveAllowed = false;
				MoleExplo();
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
		anim.SetBool ("Floating", Floating);


		anim.SetFloat ("Speed", Mathf.Abs (move));
		anim.SetInteger ("Totem", totem);
		
	}
	public void Jump(){ //Jump
		if(jumpAllowed == true) {
			StartCoroutine(JumpTimer ());
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,0); //Added this to reset y velocity before jump. Fixes some issues where jump would have way too much force because of current velocity
			rigidbody2D.AddForce (new Vector2 (0, soloJumpForce));
			audio.PlayOneShot (playerWalkJump);

		}
	}

	public void Knockback(Vector2 hitDirection) {
		// The vector2 hitDirection is the normalized vector of the direction from the ennemy to the player. (direction player should be knocked in, Or not if hit from top !)  
		//THis is a placeholder to a better knockback that should be implemented later :
		isKnockbacked = true;
		if(hitDirection.x > 0) {
			rigidbody2D.velocity = new Vector2( 4f, 8f);
		} else {
			rigidbody2D.velocity = new Vector2( -4f, 8f);
		}
	}
	public void Knockback(Vector2 hitDirection, float xAmplification, float yAmplification) { //can add X and Y values that multiply the force of the push
		isKnockbacked = true;
		if(hitDirection.x > 0) {
			rigidbody2D.velocity = new Vector2( 4f*xAmplification, 8f*yAmplification);
		} else {
			rigidbody2D.velocity = new Vector2( -4f*xAmplification, 8f*yAmplification);
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
				if(!slowMo && orbs.currentEnergy > 0){
					Time.timeScale = 0.5f;
					slowMo=true;
			        StartCoroutine(SlowMoTimer());
			        Debug.Log ("slowmo");
				}else if(slowMo){
					Time.timeScale = 1f;
					slowMo=false;
				}
			}
	IEnumerator SlowMoTimer() {
		if (slowMo) orbs.RemoveEnergy(1);
		yield return new WaitForSeconds(orbSlowMoRatio);
		if (orbs.currentEnergy <= 0) RabbitSlowMotion ();
		if (slowMo) StartCoroutine(SlowMoTimer());
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

	void MoleShield(){
		if (!Shield && orbs.currentEnergy > 0) {
			Debug.Log("shield");
			Shield = true;
			health.shield = Shield;
			StartCoroutine(MoleShieldTimer());
				}
		else if (Shield) {
			Shield = false;
			health.shield = Shield;
			Debug.Log("NO SHIELD");
				}
		}
	IEnumerator MoleShieldTimer() {
		if (Shield) orbs.RemoveEnergy(1);
		yield return new WaitForSeconds(orbShieldRatio);
		if (orbs.currentEnergy <= 0) MoleShield();
		if (Shield) StartCoroutine(MoleShieldTimer());
	}

	void MoleMeleeDeactivate(){
		moleSmash = false;
		AllowMovement ();
	}
	void MoleExplo(){
		if (orbs.currentEnergy >= 5) {
			origin = new Vector2 (transform.position.x, (transform.position.y - 0.411f));
			Instantiate (ExplosionPrefab, origin, transform.rotation);
			orbs.RemoveEnergyOrb();
		}
	}

	//TOTEM TRANSFORMATIONS

	public void TurnIntoRabbit(){
		Debug.Log ("rabbit");
		audio.PlayOneShot (playerTransform,1);
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
		audio.PlayOneShot (playerTransform,1);
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
		audio.PlayOneShot (playerTransform,1);
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
		audio.PlayOneShot (playerTransform,1);
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
			health.AdjustCurrentHealth(-damage); //deal dmg
			StartCoroutine(InvincibilityTimer()); //Start Invincibility
			gameManager.FreezeGame(0.075f); //Freeze the game for a small moment
			if(cameraScript != null) cameraScript.ScreenShake(); //Shake Screen
			audio.PlayOneShot (playerDamageTaken,1);
		}
	}

	IEnumerator InvincibilityTimer() {
		bool wasFloating = false;
		//Player is being knockbacked
		isInvincible = true;
		isKnockbacked = false;
		if(Floating == true) { //If float is active, remove it for the knockback (because of the drag)
			Floating = false;
			MaBellesFloat(Floating);
			wasFloating = true;
		}
		gameObject.layer = 16; //put on layer without enemies

		yield return new WaitForSeconds (knockbackTime);
		//Player Get Controls back but is still invincible
		isKnockbacked = false;
		if(wasFloating == true) { //If float was active, set it back after knockback.
			Floating = true;
			MaBellesFloat(Floating);
		}

		yield return new WaitForSeconds (invincibilityTime);
		//player returns to normal
		isInvincible = false;
		gameObject.layer = 13; //put back on layer with enemies
	}
	IEnumerator JumpTimer() { //makes sure the Jump dont get called multiple times (ex: stomping on two enemies at once would trigger jump twice).
		jumpAllowed = false;
		yield return new WaitForSeconds(0.08f);
		jumpAllowed = true;
	}



}


	