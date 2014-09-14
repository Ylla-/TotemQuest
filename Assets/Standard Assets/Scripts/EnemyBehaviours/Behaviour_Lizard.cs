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

	public float speed = 1f; //Mouvement Speed
	public int damage = 1; // Damage done to the player when hit

	private Controller playerController;
	private Health hp;
	private LizardState _state;

	void Awake() {
		hp = gameObject.GetComponent<Health> ();
	}
	void Start () {
		_state = new IdleState(this);
	}
	
	// Update is called once per frame
	void Update () {
		_state.Update();
	}



	/*********************************************
	 ************* STATE MACHINE *****************
	 ********************************************/
	public abstract class LizardState {
		// Attributs
		protected Behaviour_Lizard _lizard;
		
		// Constructeurs
		private LizardState() {}
		public LizardState(Behaviour_Lizard lizard) {
			_lizard = lizard;
		}
		
		// Méthodes
		public abstract void Update();
	}

	/*********************************************
	 ************* Idle Behaviour *****************
	 ***** Used for waiting inbetween shots ******
	 ********************************************/

	public class IdleState : LizardState {
		// Constructeurs
		public IdleState(Behaviour_Lizard lizard) : base(lizard) {}
		
		// Méthodes
		public override void Update() {

			Debug.Log (_lizard.name + " : I AM IDLE");
		}
	}


	/*********************************************
	 ************* Shoot Behaviour *****************
	 * Used for positioning and shooting the player *
	 ********************************************/

	public class ShootState : LizardState {
		// Constructeurs
		public ShootState(Behaviour_Lizard lizard) : base(lizard) {}
		
		// Méthodes
		public override void Update() {
			
			Debug.Log (_lizard.name + " : I AM SHOOTING");
		}
	}


	/*********************************************
	 ************* Teleport Behaviour *****************
	 * Used for teleporting behind player and shooting him *
	 ********************************************/
	
	public class TeleportState : LizardState {
		// Constructeurs
		public TeleportState(Behaviour_Lizard lizard) : base(lizard) {}
		
		// Méthodes
		public override void Update() {
			
			Debug.Log (_lizard.name + " : I AM TELEPORTING");
		}
	}
}
