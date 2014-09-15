using UnityEngine;
using System.Collections;

public class DestroyOnDelay : MonoBehaviour {

	/// <summary>
	/// This Script is used to Destroy an object after a delay after they're created.
	/// For exemple, this can be used on projectiles or particles effect.
	/// </summary>

	public float TimeBeforeDestroy = 5f;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, TimeBeforeDestroy);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
