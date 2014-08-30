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

	public int maxEnergy = 10; //link these to the Player's energy value later
	public int currentEnergy = 0;
	public List<EnergyOrb> orbList;

	void Awake () {
		orbList = new List<EnergyOrb>();
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Controls for testing purposes only :
		if(Input.GetKeyDown (KeyCode.Z)){
			Debug.Log ("Test Purpose : Added Orb (Key Z)");
			CreateOrb();
		}
		if(Input.GetKeyDown (KeyCode.X)){
			Debug.Log ("Test Purpose : Removed Orb (Key X)");
			RemoveOrb();
		}
	}



	public void UpdateList(){
		for(int i=0; i<orbList.Count; i++) {
			orbList[i].orbIndex = i;
		}
	}


	public void CreateOrb() {
		if(orbList.Count < maxEnergy) {
			orbList.Add ((Instantiate(energySprite, target.position, Quaternion.identity) as GameObject).GetComponent<EnergyOrb>()); //Create Orb & add to List
			orbList[orbList.Count-1].target = target; // Set Orb's Target
			UpdateList();
			currentEnergy++;
		}
	}

	public void RemoveOrb() {
		if(orbList.Count > 0) {
			GameObject orbToDestroy = orbList[orbList.Count-1].gameObject;
			orbList.RemoveAt (orbList.Count-1); //Note : Once orbs work, try changing this for RemoveAt(0) for cool effect.
			Destroy (orbToDestroy); //Here, it would be possible to add an animation on destroy.
			UpdateList();
			currentEnergy--;
		}
	}

}
