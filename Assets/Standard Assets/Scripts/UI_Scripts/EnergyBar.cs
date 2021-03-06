﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnergyBar : MonoBehaviour {
	/// <summary>
	/// This Script manages the List of orbs that are following the player. Work In Progress !
	/// </summary>
	/// 
	public Transform target; //Target the sprites are gonna follow
	public GameObject energySprite; // Prefab to be instantiated as orbs
	public Controller controller;

	public Color color1;
	public Color color2; 
	public Color color3; 
	public Color color4; 
	private Color CurrentColor; //CurrentColor of the orb

	public int energyPerOrb = 5; //Maximum amount of energy per orb	
	public int maxOrb = 5; //Maximum amount of orb

	public int currentEnergy = 0;
	public List<EnergyOrb> orbList;

	//Different positions of the orb depending on which totem is being used
	private int currentTotem = 0;
	private Vector2 totemOrbPosition_0;
	private Vector2 totemOrbPosition_1;
	private Vector2 totemOrbPosition_2;
	private Vector2 totemOrbPosition_3;

	//Used to correctly destroy orbs on death and on clising the app.
	private bool isDestroying = false; //True when this object is being destroyed




	void Awake () {
		transform.tag = "EnergyBar";
		orbList = new List<EnergyOrb>();
		CurrentColor = color1;
		if(controller == null) GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
	
	}

	void Start () {
		//Setting up the positions of the orbs for the different totems
		totemOrbPosition_0 = new Vector2 (0.3f, 0.00f);
		totemOrbPosition_1 = new Vector2 (0.2f, 0.15f);
		totemOrbPosition_2 = new Vector2 (0.2f, 0.05f);
		totemOrbPosition_3 = new Vector2 (0.2f, 0.15f);;
	}
	
	// Update is called once per frame
	void Update () {
		CheckTotemType ();

		//Controls for testing purposes only :
		//******* Feel Free to comment this out if you need those keys. This is purely for testing as this will be linked with the controller later ! *********//
		if(Input.GetKeyDown (KeyCode.Z)){
			Debug.Log ("Test Purpose : Added energy (Key Z)");
			AddEnergy(1,transform.position);
		}
		if(Input.GetKeyDown (KeyCode.X)){
			Debug.Log ("Test Purpose : Removed energy (Key X)");
			RemoveEnergy(1);
		}
		if(Input.GetKeyDown (KeyCode.C)){
			Debug.Log ("Test Purpose : Removing an Orb (Key C)");
			RemoveEnergyOrb();
		}

	}

	void OnDestroy() {
		isDestroying = true;
	}

	void CheckTotemType() { //TODO : Add different orb position for each transformation;
		if(currentTotem != controller.totem) {
			currentTotem = controller.totem;
			if(currentTotem == 0) { // 0 = normal
				ChangeOrbsColor(color1);
				CurrentColor = color1;
				ChangeOrbsPosition(totemOrbPosition_0);
			} else if(currentTotem == 1) { //1 = rabbit
				ChangeOrbsColor(color2);
				CurrentColor = color2;
				ChangeOrbsPosition(totemOrbPosition_1);
			} else if(currentTotem == 2) { //2 = mole
				ChangeOrbsColor(color3);
				CurrentColor = color3;
				ChangeOrbsPosition(totemOrbPosition_2);
			} else { //3 = mantis
				ChangeOrbsColor(color4);
				CurrentColor = color4;
				ChangeOrbsPosition(totemOrbPosition_3);
			}
		}
	}


	public void UpdateList(){ //update the list of orbs. 
		for(int i=0; i<orbList.Count; i++) {
			orbList[i].orbIndex = i;
		}
	}

	public void AddEnergy(int amount, Vector3 OrbCreationPosition){ //Add "amount" energy to the bar
		for(int i=0; i< amount; i++) {
			if(currentEnergy < (maxOrb*energyPerOrb) && currentEnergy%5 == 0) { //If this equals 0, we need to create a new orb.
				CreateOrb (OrbCreationPosition);
				currentEnergy++;
			} else if(currentEnergy < (maxOrb*energyPerOrb)) {
				orbList[orbList.Count-1].AddEnergy();
				currentEnergy++;
			} else {
				Debug.Log ("Trying to add energy, but is maxed out !");
			}
		}
	}
	public void RemoveEnergy(int amount) { //Remove "amount" energy to the bar
		for(int i=0; i< amount; i++) {
			if(currentEnergy > 0 && (currentEnergy-1)%5 == 0) { //If this equals 0, we need to remove an orb
				RemoveOrb();
				currentEnergy--;
			} else if(currentEnergy > 0) {
				orbList[orbList.Count-1].RemoveEnergy();
				currentEnergy--;
			} else {
				Debug.Log ("Trying to remove energy, but has none !");
			}
		}
	}
	


	public void RemoveEnergyOrb(){ //Remove a Complete Orb (for exemple, when using an totem ability)
		if(currentEnergy >= 5) {
			GameObject orbToDestroy = orbList[0].gameObject;
			orbList.RemoveAt (0); //Note : Once orbs work, try changing this for RemoveAt(0) for cool effect.
			Destroy (orbToDestroy); //Here, it would be possible to add an animation on destroy.
			currentEnergy -= energyPerOrb;

			UpdateList();
		}
	}

	private void CreateOrb(Vector3 CreationPosition) { //Create an Orb and add energy
		if(orbList.Count < maxOrb && isDestroying == false) {
			orbList.Add ((Instantiate(energySprite, CreationPosition, Quaternion.identity) as GameObject).GetComponent<EnergyOrb>()); //Create Orb & add to List
			//Debug.Log (orbList[orbList.Count-1]);
			orbList[orbList.Count-1].target = target; // Set Orb's Target
			orbList[orbList.Count-1].energyBar = this; 
			orbList[orbList.Count-1].controller = controller;
			orbList[orbList.Count-1].SetColor(CurrentColor);
			orbList[orbList.Count-1].maximumEnergy = energyPerOrb;
			orbList[orbList.Count-1].InitializeEnergy();
			orbList[orbList.Count-1].totemOrbPosition = totemOrbPosition_0;
			UpdateList();
		}
	}

	private void RemoveOrb() { //Remoe an Orb and remove energy
		if(orbList.Count > 0) {
			GameObject orbToDestroy = orbList[orbList.Count-1].gameObject;
			orbList.RemoveAt (orbList.Count-1); //Note : Once orbs work, try changing this for RemoveAt(0) for cool effect.
			Destroy (orbToDestroy); //Here, it would be possible to add an animation on destroy.

			UpdateList();
		}
	}

	public void ChangeOrbsColor(Color newColor) {
		for(int i = 0; i < orbList.Count; i++) {
			orbList[i].LerpColor(newColor);

		}
	}

	private void ChangeOrbsPosition(Vector2 newPosition){
		for(int i = 0; i < orbList.Count; i++) {
			orbList[i].totemOrbPosition = newPosition;
		}
	}

}
