using UnityEngine;
using System.Collections;

public class StopParticlesOnDelay : MonoBehaviour {

	public float delay = 1f;
	private ParticleSystem particleSys;

	
	void Start () {
		particleSys = gameObject.particleSystem;
		StartCoroutine (StopParticles ());
	}

	IEnumerator StopParticles(){
		yield return new WaitForSeconds(delay);
		particleSys.emissionRate = 0;
	}

}
