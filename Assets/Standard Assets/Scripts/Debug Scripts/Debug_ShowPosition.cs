using UnityEngine;
using System.Collections;

public class Debug_ShowPosition : MonoBehaviour {

	float posX, posY;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = Camera.main.transform.position;
		posX = position.x;
		posY = position.y;

		posX = Mathf.Round (posX);
		posY = Mathf.Round (posY);
	}

	void OnGUI() {
		GUI.Label (new Rect (0, 0, 100, 50), "X:"+ posX + " , Y:" + posY);
	}
}
