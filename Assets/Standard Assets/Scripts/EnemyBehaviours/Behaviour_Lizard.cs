using UnityEngine;
using System.Collections;

public class Behaviour_Lizard : MonoBehaviour {

	/// <summary>
	/// AI For the Lizard enemy. The lizard enemy moves up and down to reach the player's height and throws fireball. When hit or too far from the player, he fades
	/// away and teleport behind the player, throwing a fireball at the same time.
	/// 
	/// Mouvement : When ready to fire, moves to the player's height to shoot. Otherwise, moves to a position a little higher than player's position.
	/// 			When hit, teleport behind player at player's height and fire. Then moves a little upward.
	/// 
	/// Attack : Damages the player if he touches him. 
	/// 		 Throws Fireball at regular intervals.
	/// 
	/// How To Kill : Use Player's Projectiles.
	/// 
	/// </summary>

	public float speed = 1f; //Mouvement Speed. seconds it takes to move 1 unit.
	public int rangedDamage= 2; //damage done to the player when hit by projectile
	public int meleeDamage = 1; //damage done to the player when hit by lizard's model
	public float fadeTime = 2f; // Fade time before teleporting

	public GameObject Projectile;
	public GameObject ParticleSmoke;
	public GameObject ParticleDeath;
	public Controller playerController;

	public bool wasAttacked = false;

	private int CollisionLayerMask = 1 << 11; //Create a layermask with Only Collision Layer
	private float previousHp;
	private float distanceToTeleport = 10f; //Distance between player and lizard at which he teleports instead of attacking.
	private float safeDistanceFromPlayer = 1f; //Vertical distance to move away from player after a shot.
	private float teleportDistance = 2.5f; // Distance to teleport to behind the player
	private bool isDying = false;
	private bool hasSpawnedDeathParticles = false;

	private Health hp;
	private LizardState _state;


	void Awake() {
		hp = gameObject.GetComponent<Health> ();
	}
	void Start () {
		previousHp = hp.curHealth;
		if(playerController == null) playerController = (Controller) GameObject.FindGameObjectWithTag ("Player").GetComponent<Controller> ();

		_state = new IdleState(this,2f,-1);
	}
	
	// Update is called once per frame
	void Update () {
		// Verify if health has changed to verify is lizard was attacked.
		if(previousHp != hp.curHealth) {
			previousHp = hp.curHealth;
			wasAttacked = true;
		}
		//verify if enemy is dying
		if(hp.curHealth <= 0) {
			isDying = true;
			StopAllCoroutines();
			if(hasSpawnedDeathParticles == false && ParticleDeath != null) {
				Instantiate (ParticleDeath, transform.position, Quaternion.identity);
				hasSpawnedDeathParticles = true;
			}
		}
		//Execute current State's update
		_state.Update();
	}

	void  OnTriggerEnter2D(Collider2D other) { 
		//Damage Player when touching
		if(other.gameObject.layer == 13 && isDying == false) { //If it hits the player
			playerController.DamagePlayer(meleeDamage);
			Vector3 positionDiff = playerController.transform.position - transform.position; 
			playerController.Knockback((new Vector2(positionDiff.x,positionDiff.y).normalized)); //Not implemented yet.
		}
	}

	void ConfigureProjectile(Lizard_Projectile proj){
		proj.DMG = rangedDamage;
	}
	



	/*********************************************
	 ************* STATE MACHINE *****************
	 ********************************************/
	public abstract class LizardState {
		// Attributs
		protected Behaviour_Lizard _lizard;

		// Constructors
		private LizardState() {}
		public LizardState(Behaviour_Lizard lizard) {
			_lizard = lizard;
		}
		
		// Methods
		public abstract void Update();
	}

	/*********************************************
	 ************* Idle Behaviour *****************
	 ***** Used for waiting inbetween shots ******
	 ********************************************/

	//Constructor can specify which state to transition to after the idling. 
	// nothing : Attack
	// 0 : Hide
	// 1 : Teleport
	// Anything Else : Attack

	public class IdleState : LizardState {
		// Constructors
		public IdleState(Behaviour_Lizard lizard, float time) : base(lizard) {idleTime = time; nextState = -1;}
		public IdleState(Behaviour_Lizard lizard, float time, int next) : base(lizard) {idleTime = time; nextState = next;}
		// Variables
		float idleTime;
		float currentTime = 0f;
		int nextState;


		// Methods
		public override void Update() {
			//If Lizard was attacked, transition into Teleport
			if(_lizard.wasAttacked == true) {
				_lizard.wasAttacked = false;
				_lizard._state = new TeleportState(_lizard);
			}

			//Look if idle time has elapsed :
			if(currentTime > idleTime) {
				GoToNextState();
			} else {
				currentTime += Time.deltaTime;
			}
			Debug.Log (_lizard.name + " : I AM IDLE");
		}

		private void GoToNextState(){
			if(nextState == 0) { //GO to  Hide State
				_lizard._state = new HideState(_lizard);
			} else if(nextState == 1 ) { //Go to Teleport State
				_lizard._state = new TeleportState(_lizard);
			} else { //Go to Attack State
				_lizard._state = new ShootState(_lizard);
			}
		}
	}


	/*********************************************
	 ************* Shoot Behaviour *****************
	 * Used for positioning and shooting the player *
	 ********************************************/

	public class ShootState : LizardState {
		// Constructors
		public ShootState(Behaviour_Lizard lizard) : base(lizard) {}

		//Variables 
		Vector3 targetPosition; //target position for lizard's mouvement
		float mouvTime; //time to do lizard's mouvement
		bool mouvementStarted = false;
		bool mouvementFinished = false;


		// Methods
		public override void Update() {

			if(mouvementStarted == false) {
				mouvementStarted = true;
				if(PlayerInRange() == false) { //If player is too far, lizard will instead teleport behind him.
					_lizard._state = new TeleportState(_lizard);
				}
				FindAttackPlayerPosition (); //If player isn't too far, acquire Target
				_lizard.StartCoroutine(GoToPosition (_lizard.transform.position,targetPosition,mouvTime, this));
			} 
			if (mouvementFinished == true) { //Once mouvement is done, Lizard fires
				mouvementFinished = false;
				Shoot ();
				_lizard._state = new IdleState(_lizard,0.4f,0);
			}
			
			Debug.Log (_lizard.name + " : I AM SHOOTING");
		}

		//Determine if the player is in range and returns the result
		private bool PlayerInRange() {
			if(Vector2.Distance(_lizard.transform.position, _lizard.playerController.transform.position) > _lizard.distanceToTeleport) {
				return false;
			} else {
				return true;
			}
		}

		void Shoot() {
			Lizard_Projectile projectile = ((GameObject) Instantiate (_lizard.Projectile, _lizard.transform.position, Quaternion.identity)).GetComponent<Lizard_Projectile> ();
			projectile.DMG = _lizard.rangedDamage; // Set Correct Damage
			if(_lizard.transform.position.x > _lizard.playerController.transform.position.x) {
				projectile.facingRight = false;
			} else {
				projectile.facingRight = true;
			}
		}


		private void FindAttackPlayerPosition() { //find the position the lizard must be in to hit the player
			targetPosition = new Vector3 (_lizard.transform.position.x,
			                             _lizard.playerController.transform.position.y,
			                             _lizard.transform.position.z);
			mouvTime = Mathf.Abs(targetPosition.y - _lizard.transform.position.y) / _lizard.speed; //Find time for the movement
		}


		public IEnumerator GoToPosition(Vector3 startPosition, Vector3 endPosition, float t, ShootState state) { //Makes the log rotate around the axis for x degrees over t seconds
			float step = 0f; //raw step
			float smoothStep = 0f; //current smooth step
			float lastStep = 0f; //previous smooth step
			Transform lizardTransform = _lizard.transform;
			while(step < 1f) { // until we're done
				step += Time.deltaTime / t; // for t seconds 
				smoothStep = Mathf.SmoothStep(0f, 1f, step); // finding smooth step

				//Do Smooth Translation to position
				lizardTransform.position = Vector3.Lerp(startPosition,endPosition,smoothStep);

				lastStep = smoothStep; //get previous last step
				yield return null;
			}
			//once mouvement is over, Tell the state
			state.mouvementFinished = true;
		}
	}


	/*********************************************
	 ************* Teleport Behaviour *****************
	 * Used for teleporting behind player and shooting him *
	 ********************************************/
	
	public class TeleportState : LizardState {
		// Constructors
		public TeleportState(Behaviour_Lizard lizard) : base(lizard) {currentTime = 0f; isFading = true; hasTeleported = false;}

		//Variables
		Vector3 teleportPosition;
		bool isFading = true;
		bool hasTeleported = false;
		bool hasStartedFading = false;
		float currentTime = 0f;

		// Methods
		public override void Update() {
			if (hasStartedFading == false) {
				hasStartedFading = true;
				_lizard.StartCoroutine(FadeAway ());
			}

			if(isFading == true) {
				if(currentTime > _lizard.fadeTime) {
					isFading = false;
				} else {
					currentTime += Time.deltaTime;
				}
			} else if(hasTeleported == false){
				hasTeleported = true;
				FindTeleportLocation();
				TeleportToNewLocation();
				Shoot();
				MakeVisible();
				_lizard._state = new IdleState(_lizard,0.4f,0);
			}
			_lizard.wasAttacked = false;


			Debug.Log (_lizard.name + " : I AM TELEPORTING");
		}

		void FindTeleportLocation(){
			if(_lizard.playerController.facingRight == true) {
				teleportPosition = _lizard.playerController.transform.position + new Vector3(-_lizard.teleportDistance,0,0);
			} else {
				teleportPosition = _lizard.playerController.transform.position + new Vector3(_lizard.teleportDistance,0,0);
			}

		}

		void TeleportToNewLocation() {
			_lizard.transform.position = teleportPosition;
			Instantiate (_lizard.ParticleSmoke, teleportPosition, Quaternion.identity);
		}

		void Shoot() {
			Lizard_Projectile projectile = ((GameObject) Instantiate (_lizard.Projectile, _lizard.transform.position, Quaternion.identity)).GetComponent<Lizard_Projectile> ();
			projectile.DMG = _lizard.rangedDamage; // Set Correct Damage
			if(_lizard.transform.position.x > _lizard.playerController.transform.position.x) {
				projectile.facingRight = false;
			} else {
				projectile.facingRight = true;
			}
		}

		void MakeVisible(){
			_lizard.StopAllCoroutines ();
			SpriteRenderer[] objectSprites = _lizard.gameObject.GetComponentsInChildren<SpriteRenderer> ();
			for(int j = 0; j < objectSprites.Length; j++) {
				objectSprites[j].color = new Color(1,1,1,1);
			}
		}

		IEnumerator FadeAway(){
			SpriteRenderer[] objectSprites = _lizard.gameObject.GetComponentsInChildren<SpriteRenderer> ();
			for(float i = 0; i < 1; i += Time.deltaTime/_lizard.fadeTime) {
				for(int j = 0; j < objectSprites.Length; j++) {
					objectSprites[j].color = new Color(1,1,1,1-i);
				}
				yield return null;
			}
		}


	}


	/*********************************************
	 ************* Hide Behaviour *****************
	 * Lizard will find a safe position and go there. *
	 ********************************************/
	
	public class HideState : LizardState {
		// Constructors
		public HideState(Behaviour_Lizard lizard) : base(lizard) {}

		// variables 
		bool isSafe = false;
		bool mouvementStarted = false;
		Vector3 targetPosition; //target position for lizard's mouvement
		float mouvTime; //time to do lizard's mouvement



		// Methods
		public override void Update() {
			if(_lizard.wasAttacked == true) {
				_lizard.wasAttacked = false;
				_lizard.StopAllCoroutines(); //Stop mouvement coroutine
				_lizard._state = new TeleportState(_lizard);

			}

			//Once the mouvement is over, transition to new State
			if(isSafe == true) {
				_lizard._state = new IdleState(_lizard, 0.4f, -1);
			}

			//This is only done once :
			if (mouvementStarted == false) { 
				mouvementStarted = true;
				if(FindSafePosition () == true) {
					_lizard.StartCoroutine(GoToPosition (_lizard.transform.position,targetPosition,mouvTime, this));
				}
			}
	
			Debug.Log (_lizard.name + " : I AM HIDING");
		}

		private bool FindSafePosition() { //find the position the lizard must be in to hit the player
			bool mouving = false;
	
			targetPosition = new Vector3 (_lizard.transform.position.x, _lizard.playerController.transform.position.y,_lizard.transform.position.z);

			//Verify Collisions
			if(Physics2D.Raycast(_lizard.transform.position,Vector2.up,_lizard.safeDistanceFromPlayer, _lizard.CollisionLayerMask)){
				if(Physics2D.Raycast(_lizard.transform.position,Vector2.up,_lizard.safeDistanceFromPlayer, _lizard.CollisionLayerMask)){
					//Collision Bot AND Top, Change To Teleport
					_lizard._state = new TeleportState(_lizard);
				} else { 
					//No collision Bot
					targetPosition += new Vector3 (0,-_lizard.safeDistanceFromPlayer,0);
					mouving = true;
				}
			} else { 
				//No collision Top
				targetPosition += new Vector3 (0,_lizard.safeDistanceFromPlayer,0);
				mouving = true;
			}

			mouvTime = Mathf.Abs(targetPosition.y - _lizard.transform.position.y) / _lizard.speed; //Find time for the movement
			return(mouving);
		}

		public IEnumerator GoToPosition(Vector3 startPosition, Vector3 endPosition, float t, HideState state) { //Makes the log rotate around the axis for x degrees over t seconds
			float step = 0f; //raw step
			float smoothStep = 0f; //current smooth step
			float lastStep = 0f; //previous smooth step
			Transform lizardTransform = _lizard.transform;
			while(step < 1f) { // until we're done
				step += Time.deltaTime / t; // for t seconds 
				smoothStep = Mathf.SmoothStep(0f, 1f, step); // finding smooth step
				
				//Do Smooth Translation to position
				lizardTransform.position = Vector3.Lerp(startPosition,endPosition,smoothStep);
				
				lastStep = smoothStep; //get previous last step
				yield return null;
			}
			//once mouvement is over, Tell the state
			state.isSafe = true;
		}
	}

}
