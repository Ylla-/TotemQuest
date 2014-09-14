using UnityEngine;
using System.Collections;

public class Activation_PlayerPosition : MonoBehaviour {

	/// <summary>
	///  This Activation enables the behaviour script when the player enter a certain zone in the level.
	/// </summary>
	

	public ActivationZone activationCollider;
	public bool isActivated = false;


	public MonoBehaviour[] monosToActivate;

	private Rigidbody2D rigidBody;

	//MonoBehaviour that stays on : 

	private Health health;
	private GiveEnergyOnDeath giveEnergyOnDeath;

	void Start () {
		health = (Health) transform.GetComponent<Health> ();
		giveEnergyOnDeath = (GiveEnergyOnDeath) transform.GetComponent<GiveEnergyOnDeath> ();
		monosToActivate = (MonoBehaviour[])transform.GetComponents<MonoBehaviour> ();
		rigidBody = (Rigidbody2D)transform.GetComponent<Rigidbody2D> ();
		Deactivate ();
	}
	
	// Update is called once per frame
	void Update () {
		if(isActivated == false && activationCollider.activated == true) {
			Activate ();
		}
	}

	void Activate() {
		for(int i = 0; i < monosToActivate.Length; i++) {
			if(this.GetInstanceID() != monosToActivate[i].GetInstanceID()) monosToActivate[i].enabled =  true;
		}

		isActivated = true;
	}

	void Deactivate() {
		for(int i = 0; i < monosToActivate.Length; i++) { //Deactivate each MonoBehaviour except this one and Health Script.
			if(this.GetInstanceID() != monosToActivate[i].GetInstanceID()
			   && health.GetInstanceID() != monosToActivate[i].GetInstanceID()
			   && giveEnergyOnDeath.GetInstanceID() != monosToActivate[i].GetInstanceID()) monosToActivate[i].enabled = false;
		}
		rigidBody.isKinematic = true;
		isActivated = false;
	}

}
