 using UnityEngine;
using System.Collections;


public class ProjectileScript : MonoBehaviour
{
	public int DMG = 20;
	public float speed = 12f;
	public float speedY = 0f;
	public GameObject destroyedParticlesObj;
	public LayerMask interactsWith;
	
	public bool facingRight = true;

	//For Screen shake
	private CameraScript cameraScript;
	private GameManager gameManager;

	void Awake() {
		gameObject.layer = 15; //This is the player's attack layer
		Destroy(gameObject, 4); 
	}
	void Start() {
		GameObject camera_Obj = GameObject.FindGameObjectWithTag ("MainCamera");
		if (camera_Obj != null) cameraScript = (CameraScript)camera_Obj.GetComponentInParent<CameraScript> ();
		else Debug.Log ("CAMERA OBJECT NOT FOUND !");
		GameObject manager_Obj = GameObject.FindGameObjectWithTag ("GameManager");
		if (manager_Obj != null) gameManager = (GameManager)manager_Obj.GetComponent<GameManager> ();
		else Debug.Log ("MANAGER OBJECT NOT FOUND !");
	}
	
	
	void FixedUpdate(){
		//flipping 
		if (facingRight == false) {
			Flip ();
			speed = -speed;
			speedY = -speedY;
			facingRight = true;
		}
		rigidbody2D.velocity = new Vector2 (speed, speedY);
	}
	
	
	//14-17
	void OnTriggerEnter2D(Collider2D other)	{
		if (other is BoxCollider2D && (((1<<other.gameObject.layer) & interactsWith) != 0)) { //Changed the collision requirements from being a tag to a layer. It will now hit everything in enemy layer.
			Debug.Log ("HIT FIREBALL");
			//Get HealthScript and remove HP
			Health h = (Health)other.gameObject.GetComponent ("Health");
			if(h != null) h.AdjustCurrentHealth (-DMG);
			//Activate the MonoBehaviour of the enemy (if the enemy requires a specific condition to activate, getting hit by the player will fulfill it).
			ActivateMonos(other.gameObject); //Activates the monobehaviours on target

			DestroyProjectile();
		} else if (other.gameObject.layer == 11) {
			if(destroyedParticlesObj != null) Instantiate (destroyedParticlesObj,transform.position,Quaternion.identity);
			Destroy (gameObject);
		}
	}

	void DestroyProjectile() { //Destroy Projectile and add freeze and shake effect
		gameManager.FreezeGame(0.02f);
		if(cameraScript != null) cameraScript.ScreenShake(0.08f,0.02f); //Shake Screen
		Destroy (gameObject);
	}
	
	void Flip(){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		
		Vector3 angles = transform.localEulerAngles;
		angles.x *= -1; angles.y *= -1; angles.z *= -1;
		transform.localEulerAngles = angles;
	}

	void ActivateMonos(GameObject obj){
		MonoBehaviour[] behaviours = obj.GetComponentsInChildren<MonoBehaviour> ();
		for(int i =0; i< behaviours.Length; i++) {
			if(behaviours[i].enabled == false) behaviours[i].enabled = true;
		}
	}

	public int Damage{ 
		get{ return DMG;}
		set{ DMG = value;}
	}
}