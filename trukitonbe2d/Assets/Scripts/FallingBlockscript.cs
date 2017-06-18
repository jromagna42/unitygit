using UnityEngine;
using System.Collections;

public class FallingBlockscript : MonoBehaviour {

	// Use this for initialization
	Vector2 ranDir;
	Vector3 ranRot;
	Vector3 ranScale;
	float screenSize;
	float	speed = 8;
	void Start () {
		screenSize = Camera.main.orthographicSize;
		ranRot = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
		ranDir =  new Vector2(Random.Range(-0.6f, 0.6f), -1);
		ranScale = new Vector3(Random.Range(-0.5f, 1.5f), Random.Range(-0.5f, 1.5f), Random.Range(-0.5f, 1.5f));
		transform.localRotation = Quaternion.Euler(ranRot);
		transform.localScale += ranScale;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(speed * ranDir * Time.deltaTime, Space.World);
		if (transform.position.y < -screenSize - transform.localScale.x)
			Destroy(transform.gameObject);
	}

}
