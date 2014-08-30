using UnityEngine;
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

	public int maxEnergy = 10; //link these to the Player's energy value later
	public int currentEnergy = 0;
	public List<EnergyOrb> orbList;

	private int currentTotem = 0;


	void Awake () {
		orbList = new List<EnergyOrb>();
		CurrentColor = color1;
		if(controller == null) GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
	
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CheckTotemType ();

		//Controls for testing purposes only :
		if(Input.GetKeyDown (KeyCode.Z)){
			Debug.Log ("Test Purpose : Added Orb (Key Z)");
			CreateOrb();
		}
		if(Input.GetKeyDown (KeyCode.X)){
			Debug.Log ("Test Purpose : Removed Orb (Key X)");
			RemoveOrb();
		}
		if(Input.GetKeyDown (KeyCode.C)){
			Debug.Log ("Test Purpose : Changing to White (Key C)");
			ChangeOrbsColor(color1);
		}
		if(Input.GetKeyDown (KeyCode.V)){
			Debug.Log ("Test Purpose : Changing to Red (Key V)");
			ChangeOrbsColor(color2);
		}
		if(Input.GetKeyDown (KeyCode.B)){
			Debug.Log ("Test Purpose : Changing to Blue (Key B)");
			ChangeOrbsColor(color3);
		}
		if(Input.GetKeyDown (KeyCode.N)){
			Debug.Log ("Test Purpose : Changing to Green (Key N)");
			ChangeOrbsColor(color4);
		}


	}

	void CheckTotemType() { //TODO : Add different orb position for each transformation;
		if(currentTotem != controller.totem) {
			currentTotem = controller.totem;
			if(currentTotem == 0) { // 0 = normal
				ChangeOrbsColor(color1);
				CurrentColor = color1;
			} else if(currentTotem == 1) { //1 = rabbit
				ChangeOrbsColor(color2);
				CurrentColor = color2;
			} else if(currentTotem == 2) { //2 = mole
				ChangeOrbsColor(color3);
				CurrentColor = color3;
			} else { //3 = mantis
				ChangeOrbsColor(color4);
				CurrentColor = color4;
			}
		}
	}


	public void UpdateList(){ //update the list of orbs. 
		for(int i=0; i<orbList.Count; i++) {
			orbList[i].orbIndex = i;
		}
	}


	public void CreateOrb() { //Create an Orb and add energy
		if(orbList.Count < maxEnergy) {
			orbList.Add ((Instantiate(energySprite, target.position, Quaternion.identity) as GameObject).GetComponent<EnergyOrb>()); //Create Orb & add to List
			Debug.Log (orbList[orbList.Count-1]);
			orbList[orbList.Count-1].target = target; // Set Orb's Target
			orbList[orbList.Count-1].energyBar = this; 
			orbList[orbList.Count-1].controller = controller;
			orbList[orbList.Count-1].SetColor(CurrentColor);

			UpdateList();
			currentEnergy++;
		}
	}

	public void RemoveOrb() { //Remoe an Orb and remove energy
		if(orbList.Count > 0) {
			GameObject orbToDestroy = orbList[orbList.Count-1].gameObject;
			orbList.RemoveAt (orbList.Count-1); //Note : Once orbs work, try changing this for RemoveAt(0) for cool effect.
			Destroy (orbToDestroy); //Here, it would be possible to add an animation on destroy.
			UpdateList();
			currentEnergy--;
		}
	}

	public void ChangeOrbsColor(Color newColor) {
		for(int i = 0; i < orbList.Count; i++) {
			orbList[i].LerpColor(newColor);
		}
	}

}
