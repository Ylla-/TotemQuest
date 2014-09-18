using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	public float explosionDelay = 0f;
	public float explosionRate = 1f;
	public float explosionMaxSize = 5f;
	public float explosionSpeed = 1f;
	public float currentRadius = 0f;
	public float explosionMultiplier = 400f;
	bool exploded = false;
	CircleCollider2D explosionRadius;
	Transform origin;


	// Use this for initialization
	void Start () {
		explosionRadius = gameObject.GetComponent<CircleCollider2D> ();

	}
	
	// Update is called once per frame
	void Update () {
		explosionDelay -= Time.deltaTime;
		if (explosionDelay < 0) {
			exploded  = true;
				}
			
	}

	void FixedUpdate(){
		if (exploded == true) {
						if (currentRadius < explosionMaxSize) {
								currentRadius += explosionRate;
						} else
								//Object.Destroy (this.gameObject);

			explosionRadius.radius = currentRadius;
				}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (exploded) {

			if(col.gameObject.rigidbody2D != null){
				Vector2 target = col.gameObject.transform.position;
				Vector2 origin = gameObject.transform.position;

				Vector2 direction = explosionMultiplier * (target - origin); //invert to make the bomb implode)

				col.gameObject.rigidbody2D.AddForce(direction);
			}
				}
	}


}

















