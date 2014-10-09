using UnityEngine;
using System.Collections;

public class Behaviour_Guardian : MonoBehaviour {

	/// <summary>
	/// AI For the Guardian enemy. Guardian waits for player to get near or far to either melee attack or call lighting upon him. Immune to front attacks while immune.
	/// 
	/// Mouvement : Stays Still
	/// 
	/// Attack : -Damages the player if he touches him. 
	/// 		 -Swing his sword if player is near
	/// 		 -Call lighting if the player is far
	/// 
	/// How To Kill : Hit him with projectiles when he doesn't guard, or from behind.
	/// 
	/// </summary>

	public bool facingRight = true;
	public int meleeDamage = 2;
	public float engageDistance = 15f;
	public float meleeDistance = 1.5f;
	public GameObject guardianShield;
	public GameObject lightningProjectile;
	public GameObject lightningPrediction;
	public GameObject meleeAttackPrefab;


	private int previousHp; //Previous hp of ennemy
	private float heightTest = 7; //Height to test lightning ray
	private bool wasAttacked = false; //Becomes true when the enemy has been damaged
	private bool isDying = false; //Is currently dying
	private bool behaviourActivated = false; //Has behaviour been activated
	private int collisionLayermask = 1 << 11; //Collision layermask

	private Health hp;
	private GuardianState _state;
	private Controller playerController;

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
		//Run this once when activated 
		if(behaviourActivated == false) {
			rigidbody2D.isKinematic = false;
			behaviourActivated = true;
		}


		// Verify if health has changed to verify is lizard was attacked.
		if(previousHp != hp.curHealth) {
			previousHp = hp.curHealth;
			wasAttacked = true;
		}
		//verify if enemy is dying
		if(hp.curHealth <= 0) {
			isDying = true;
			StopAllCoroutines();
		}
		//Execute current State's update
		_state.Update();
	}


	
	/*********************************************
	 ************* STATE MACHINE *****************
	 ********************************************/
	/****
	 * States : 
	 * Idle : Used to make enemy idle
	 * ReadyToHit : Ready to launch an attack
	 * Melee : Melee Attack
	 * Ranged : Ranged Attack
	 * TurnAround :Turn to face the other side
	 */

	public abstract class GuardianState {
		// Attributs
		protected Behaviour_Guardian _guardian;
		
		// Constructors
		private GuardianState() {}
		public GuardianState(Behaviour_Guardian guardian) {
			_guardian = guardian;
			Start(); //Calls the Start method of the state on entering
		}
		
		// Methods
		public abstract void Update();
		public abstract void Start();
	}



	/*********************************************
	 ************* Idle Behaviour *****************
	 ***** Used for waiting inbetween actions ******
	 ********************************************/
	
	//Constructor can specify which state to transition to after the idling. 
	// nothing : Ready to launch an attack 
	// 0 : Melee Attack
	// 1 : Ranged Attack
	// 2 : Turn Around
	// Anything Else : Ready to launch an attack

	
	public class IdleState : GuardianState {
		// Constructors
		public IdleState(Behaviour_Guardian guardian, float time) : base(guardian) {idleTime = time; nextState = -1;}
		public IdleState(Behaviour_Guardian guardian, float time, int next) : base(guardian) {idleTime = time; nextState = next;}
		// Variables
		float idleTime;
		float currentTime = 0f;
		int nextState;
		

		// Methods
		public override void Start (){
			Debug.Log (_guardian.name + " : I AM IDLE");
			_guardian.guardianShield.SetActive (true);
		}
		public override void Update() {
			//If Lizard was attacked, transition into Teleport
			if(_guardian.wasAttacked == true) {
				_guardian.wasAttacked = false;
				currentTime = idleTime; //interrupt wait on hit
			}
			
			//Look if idle time has elapsed :
			if(currentTime > idleTime) {
				GoToNextState();
			} else {
				currentTime += Time.deltaTime;
			}

		}

		private void GoToNextState(){
			if(nextState == 0) { //GO to  Hide State
				_guardian._state = new MeleeState(_guardian);
			} else if(nextState == 1 ) { //Go to Teleport State
				_guardian._state = new RangeState(_guardian);
			} else if(nextState == 2 ) { //Go to Teleport State
				_guardian._state = new TurnAroundState(_guardian);
			} else {
				_guardian._state = new ReadyState(_guardian);
			}
		}
	}

	/***********************************************
	 **** Ready to launch an attack  Behaviour *****
	 ***** Used for waiting inbetween actions ******
	 ***********************************************/

	public class ReadyState : GuardianState {
		// Constructors
		public ReadyState(Behaviour_Guardian guardian) : base(guardian) {}
		// Variables
		float currentTime = 0f;
		float idleTime = 0.7f; //Time before attack
		// Methods
		public override void Start (){
			Debug.Log (_guardian.name + " : I AM READYING AN ATTACK");
			_guardian.guardianShield.SetActive (false);
		}

		public override void Update() {

			//If Guardian is attacked, throw attack now instead of waiting.
			if(_guardian.wasAttacked == true) {
				_guardian.wasAttacked = false;
				currentTime = idleTime;
			}
			
			//Look if idle time has elapsed :
			if(currentTime > idleTime) {
				if(PlayerIsBehind() == true){
					_guardian.guardianShield.SetActive (true);
					_guardian._state = new TurnAroundState(_guardian,true);
					
				} else {
					ChooseAttack();

				}

			} else {
				currentTime += Time.deltaTime;
			}

		}

		void ChooseAttack(){
			//If player is too far, idle :
			Vector3 playerDistance = _guardian.playerController.transform.position - _guardian.transform.position;
			if (playerDistance.magnitude > _guardian.engageDistance) {
				_guardian._state = new IdleState(_guardian,1.5f);
			} else {
				//Choose an attack depending on player position
				if(playerDistance.magnitude <= _guardian.meleeDistance){
					_guardian._state = new MeleeState(_guardian);
				} else {
					_guardian._state = new RangeState(_guardian);
				}
			}
		}

		bool PlayerIsBehind(){
			//IF player is behind, return TRUE
			if(_guardian.facingRight == true) {
				if(_guardian.playerController.transform.position.x < _guardian.transform.position.x){
					return true;
				}
			} else {
				if(_guardian.playerController.transform.position.x > _guardian.transform.position.x){
					return true;
				}
			}
			return false;
		}
	}


	/***********************************************
	 *********** Melee Attack Behaviour ************
	 ***** Used for waiting inbetween actions ******
	 ***********************************************/
	
	public class MeleeState : GuardianState {
		// Constructors
		public MeleeState(Behaviour_Guardian guardian) : base(guardian) {}
		// Variables
		float idleTime = 0.3f; //Time before attack launches nd leave state
		float currentTime = 0f;
		// Methods
		public override void Start (){Debug.Log (_guardian.name + " : I AM MELEE ATTACKING");}
		public override void Update() {
			//If Guardian is attacked, throw attack now instead of waiting.
			if(_guardian.wasAttacked == true) {
				_guardian.wasAttacked = false;
				//Do something on attack :
			}
			if(currentTime > idleTime) {
				Attack ();
			} else {
				currentTime += Time.deltaTime;
			}
		}

		void Attack(){

			CreateProjectile ();
			_guardian._state = new IdleState(_guardian,1.5f);
		}

		void CreateProjectile(){
			Vector3 newPosition;
			if(_guardian.facingRight == true) {
				newPosition = _guardian.transform.position + new Vector3 (1f,0,0);
				_guardian.rigidbody2D.AddForce (new Vector2 (25000f, 0f));
			} else {
				newPosition = _guardian.transform.position - new Vector3 (1f,0,0);
				_guardian.rigidbody2D.AddForce (new Vector2 (-25000f, 0f));
			}
			GameObject proj = (GameObject)Instantiate (_guardian.meleeAttackPrefab, newPosition, Quaternion.identity);
			Enemy_Projectile projScript = proj.GetComponent<Enemy_Projectile> ();
			projScript.DMG = _guardian.meleeDamage;
			projScript.facingRight = _guardian.facingRight;
		}
	}


	/***********************************************
	 ********** Ranged Attack Behaviour ************
	 ***** Used for waiting inbetween actions ******
	 ***********************************************/
	
	public class RangeState : GuardianState {
		// Constructors
		public RangeState(Behaviour_Guardian guardian) : base(guardian) {}
		// Variables
		float idleTime = 1f; //Time before attack launches nd leave state
		float lightningPositionTime; //Time at which lightning position is chosen
		float currentTime = 0f;

		bool lightningPositionChosen = false; // Has attack position been chosen yet ?

		GameObject predictionEffect;
		Vector3 lightningPosition;
		// Methods
		public override void Start (){
			Debug.Log (_guardian.name + " : I AM RANGE ATTACKING");
			lightningPositionTime = (idleTime - 0.8f);
		}
		public override void Update() {
			//If Guardian is attacked, throw attack now instead of waiting.
			if(_guardian.wasAttacked == true) {
				_guardian.wasAttacked = false;
				//Do something on attack :
			} 
			if(currentTime > idleTime) {
				Attack ();
			} else if(lightningPositionChosen == false && currentTime > lightningPositionTime){
				//Choose ligthning position on start, so player has a delay to dodge it
				lightningPosition = ChooseAttackPosition();
				//Spawn a marker on the ground to show the player lightning is gonna strike there.
				predictionEffect = (GameObject) Instantiate(_guardian.lightningPrediction, lightningPosition, Quaternion.identity); 
				predictionEffect.GetComponentInChildren<StopParticlesOnDelay>().delay = idleTime - currentTime;
			} else {
				currentTime += Time.deltaTime;
			}
		}

		void Attack(){
			Instantiate (_guardian.lightningProjectile,lightningPosition , Quaternion.identity); //Create Lightning
			_guardian._state = new IdleState(_guardian,1.5f);
		}

		Vector3 ChooseAttackPosition(){
			Vector3 newPosition = new Vector3 (_guardian.playerController.transform.position.x, _guardian.playerController.transform.position.y-0.6f, 0);
			Vector3 RayPosition = new Vector3 (newPosition.x, newPosition.y + _guardian.heightTest, newPosition.z);
		
			RaycastHit2D hit = Physics2D.Linecast (RayPosition, RayPosition - new Vector3(0,20,0), _guardian.collisionLayermask);
			Debug.DrawLine (RayPosition, hit.point, Color.yellow, 4f);

			if (hit.point != null) {
				newPosition = new Vector3 (newPosition.x, hit.point.y, newPosition.z);
			}

			lightningPositionChosen = true;
			return newPosition;
		}

		
	}


	/***********************************************
	 ************ TurnAround Behaviour *************
	 ***** Used for waiting inbetween actions ******
	 ***********************************************/
	
	public class TurnAroundState : GuardianState {
		// Constructors
		public TurnAroundState(Behaviour_Guardian guardian) : base(guardian) {}
		public TurnAroundState(Behaviour_Guardian guardian, bool wasAttacking) : base(guardian) {wasReadyToAttack = wasAttacking;}
		// Variables
		float idleTime = 1f;
		float currentTime = 0f;
		bool wasReadyToAttack;
		// Methods
		public override void Start (){Debug.Log (_guardian.name + " : I AM Turning Around");}
		public override void Update() {
			//If Guardian is attacked, throw attack now instead of waiting.
			if(_guardian.wasAttacked == true) {
				_guardian.wasAttacked = false;
				currentTime = idleTime;
			}
			//timer
			if(currentTime > idleTime) {
				Turn();
				if(wasReadyToAttack){ //If was attacking,attack now.
					_guardian._state = new ReadyState(_guardian);
				} else { //Else go idle and ready attack
					_guardian._state = new IdleState(_guardian,0.5f);
				}

			} else {
				currentTime += Time.deltaTime;
			}

		}

		void Turn(){
			if(_guardian.facingRight == true){
				_guardian.facingRight = false;
				_guardian.transform.localScale = new Vector3(-1,1,1);
			} else {
				_guardian.facingRight = true;
				_guardian.transform.localScale = new Vector3(1,1,1);
			}
		}	
	}












}
