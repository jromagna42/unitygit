using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscController2 : MonoBehaviour {
float finalSpeed = 0;
public float travelTime = 1;
public float initialSpeed = 20;
Rigidbody	ds;
Vector3 diff;
public float acceleration;
public float moveAmount;
float timeFlying = 0;
public int playerNumber;
Vector3 mousePos;
	// Use this for initialization
	void Start () {
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.y = transform.position.y;
		diff = mousePos - transform.position;
		// initialSpeed = ((2 * diff.magnitude) / travelTime) - finalSpeed;
	
		ds = gameObject.GetComponent< Rigidbody >();
		// ds.MovePosition(transform.position + diff.normalized * initialSpeed);
		acceleration = (finalSpeed- initialSpeed) / (2 * travelTime);
		moveAmount = initialSpeed;

		print("dist:" + diff.magnitude);
		print("init speed:" + initialSpeed);
		print("accel:" + acceleration);

		//ds.AddForce(diff.normalized * initialSpeed, ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
			//ds.AddForce(diff.normalized * (acceleration * Time.deltaTime), ForceMode2D.Impulse);
			// transform.Rotate(new Vector3(0, 0, 10));
			// ds.MovePosition(transform.position + diff.normalized * (moveAmount * Time.deltaTime));
			// moveAmount += acceleration * Time.deltaTime;
			// Debug.DrawLine(transform.position, transform.position + (diff.normalized * moveAmount), Color.green, Time.deltaTime );
			// Debug.DrawLine(transform.position, transform.position + (diff.normalized * acceleration), Color.red, Time.deltaTime );
			// print("speed:" + moveAmount);
	}

	void FixedUpdate()
	{
		timeFlying += Time.fixedDeltaTime;
		transform.Rotate(new Vector3(0, 0, 10));
		ds.MovePosition(transform.position + diff.normalized * (moveAmount * Time.fixedDeltaTime));
		moveAmount += acceleration * Time.fixedDeltaTime;
		if (DataStorage.debug == 1)
		{
			Debug.DrawLine(transform.position, transform.position + (diff.normalized * moveAmount), Color.green, Time.fixedDeltaTime );
			Debug.DrawLine(transform.position, transform.position + (diff.normalized * acceleration), Color.red, Time.fixedDeltaTime );
		}
	}
	string lastWallHit = null;
	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.tag == "Player" && timeFlying > 1f)
		{
			DataStorage.playersBoomerangCount[playerNumber]++;
			Destroy(gameObject);
		}
		if (coll.gameObject.tag == "wall" && !string.Equals(lastWallHit, coll.gameObject.name))
		{
			print("just hit "  + coll.gameObject.name);
			print("last wall " + lastWallHit);
			// ds.MovePosition(transform.position + diff.normalized * (moveAmount * Time.deltaTime));
			if ((moveAmount < 0 && acceleration < 0) || (moveAmount > 0 && acceleration > 0))
				acceleration = -acceleration;
			moveAmount = 0;
			lastWallHit = coll.gameObject.name;
		}
	}

}
