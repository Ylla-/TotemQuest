using UnityEngine;
using System.Collections;

public class BackgroundScrolling : MonoBehaviour {
	
	public float maxScrollX = 1f; //Maximum scrolling of the background on X Axis
	public float maxScrollY = 1f; //Maximum scrolling of the background on Y Axis.
	public float mouvementOfPlayerX = 100f;
	public float mouvementOfPlayerY = 100f;

	private Transform player;
	private Vector3 startPosition;
	private float currentXMouvement;
	private float currentYMouvement;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		startPosition = player.position;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (startPosition + "      " + player.position);
		SetNewMouvement();
		transform.localPosition = new Vector3(maxScrollX * currentXMouvement, maxScrollY * currentYMouvement,100);
	}

	void SetNewMouvement() {
		//X Mouvement
		currentXMouvement = (startPosition.x - player.position.x);
		if(Mathf.Abs(currentXMouvement) > mouvementOfPlayerX) currentXMouvement = mouvementOfPlayerX;
		currentXMouvement = currentXMouvement / mouvementOfPlayerX;
		//Y mouvement
		currentYMouvement = (startPosition.y - player.position.y);
		if(Mathf.Abs(currentYMouvement) > mouvementOfPlayerY) currentYMouvement = mouvementOfPlayerY;
		currentYMouvement = currentYMouvement / mouvementOfPlayerY;
	}
}
