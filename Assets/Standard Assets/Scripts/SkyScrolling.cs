using UnityEngine;
using System.Collections;

public class SkyScrolling : MonoBehaviour {
	public float speed = 0;
	public static SkyScrolling current;

	float pos = 0;
	float drift = 0;

	// Use this for initialization
	void Start () {
		current = this;
	}
	
	// Update is called once per frame
	void Update () {
		pos += 10*speed;
		drift += speed;
		if (pos > 1.0f)
						pos -= 1.0f;
		else if (pos < -1.0f)
						pos += 1.0f;
		if (drift > 1.0f)
						drift -= 1.0f;
				else if (drift < -1.0f)
						drift += 1.0f;
		renderer.material.mainTextureOffset = new Vector2 (drift, pos);

	}
	/*public void Go (){


	}*/
}
