using UnityEngine;
using System.Collections;

public class GiveEnergyOnDeath : MonoBehaviour {

	public int amountGiven = 1;

	private EnergyBar energyBar;
	private bool hasGivingEnergy = false;


	// Use this for initialization
	void Start () {
		energyBar = (EnergyBar) GameObject.FindGameObjectWithTag ("EnergyBar").GetComponent<EnergyBar> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GiveEnergy() { //This function is called when the gameobject is destroyed.
		if(hasGivingEnergy == false) {
			energyBar.AddEnergy (amountGiven, transform.position);
			hasGivingEnergy = true;
		}
	}
}
