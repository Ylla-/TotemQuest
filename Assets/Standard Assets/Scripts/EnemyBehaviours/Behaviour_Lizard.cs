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
	public int damage = 1; // Damage done to the player when hit
	public GameObject Projectile;

	public bool wasAttacked = false;

	private float previousHp;
	private float distanceToTeleport = 10f; //Distance between player and lizard at which he teleports instead of attacking.

	public Controller playerController;
	private Health hp;
	private LizardState _state;

	void Awake() {
		hp = gameObject.GetComponent<Health> ();
	}
	void Start () {
		previousHp = hp.curHealth;
		if(playerController == null) playerController = (Controller) GameObject.FindGameObjectWithTag ("Player").GetComponent<Controller> ();

		_state = new IdleState(this,2f);
	}
	
	// Update is called once per frame
	void Update () {
		// Verify if health has changed to verify is lizard was attacked.
		if(previousHp != hp.curHealth) {
			previousHp = hp.curHealth;
			wasAttacked = true;
		}
		//Execute current State's update
		_state.Update();
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
		public IdleState(Behaviour_Lizard lizard, float time) : base(lizard) {idleTime = time; nextState = -1;_lizard.wasAttacked = false;}
		public IdleState(Behaviour_Lizard lizard, float time, int next) : base(lizard) {idleTime = time; nextState = next;_lizard.wasAttacked = false;}
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
				Instantiate(_lizard.Projectile,_lizard.transform.position,Quaternion.identity);
				_lizard._state = new IdleState(_lizard,0.8f,0);

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
		public TeleportState(Behaviour_Lizard lizard) : base(lizard) {}
		
		// Methods
		public override void Update() {
			
			Debug.Log (_lizard.name + " : I AM TELEPORTING");
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

		// Methods
		public override void Update() {
			isSafe = true;
			//Once the mouvement is over, transition to new State
			if(isSafe == true) {
				_lizard._state = new IdleState(_lizard, 0.8f, -1);
			}


			
			Debug.Log (_lizard.name + " : I AM HIDING");
		}
	}

}
