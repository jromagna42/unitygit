using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

	public float speed = 10;
	// Use this for initialization
	// Update is called once per frame
	void Update () {
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		Vector3 direction = input.normalized;
		Vector3 velocity = direction * speed;
		Vector3 moveAmount = velocity * Time.deltaTime;

		// transform.position += moveAmount;
		transform.Translate(moveAmount);
	}
}
