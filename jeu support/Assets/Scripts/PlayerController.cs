﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Rigidbody				rbody;
	// public MouseKeybord mouseKeybordRef = new MouseKeybord();
	// public Manette manetteRef = new Manette();
	public GameObject DiscPrefab;
	// Use this for initialization

	public int playerNumber;
	Vector3 diff = new Vector3();
	Vector3 oldPosition;
	float speed = 15;
	float spawnDist = 2;	
	Vector3 input;
	Vector3 direction;
	Vector3 velocity;
	Vector3 moveAmount;
	
	Color playerColor;
	int dashing = 0;
	Vector3 dashingDir;
	public float dashingTime = 1f;
	float dashedTime = 0f;
	RaycastHit hitInfo;
	public float dashMulti = 1;
	void GivePlayerBoomerang()
	{
		int i = 0;
		while (i < DataStorage.playerCount)
		{
			print("i = " + i);
			DataStorage.playersBoomerangCount[i] = DataStorage.startingBoomerang;
			i++;
		}
	}
	void Start ()
	{
		GivePlayerBoomerang();
		playerNumber = DataStorage.nextPlayer++;
		DataStorage.playersControlType[playerNumber] = playerNumber;
		print(DataStorage.playersBoomerangCount[playerNumber]);
		playerColor = DataStorage.playerColors[playerNumber];
		rbody = GetComponent< Rigidbody >();
		DataStorage.playersGameObject[playerNumber] = gameObject;
		GetComponent<Renderer>().material.color = playerColor;
		GetComponent<Renderer>().material.SetColor("_EmissionColor",playerColor);
		transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = playerColor;
		transform.GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor",playerColor);
	}

	void Update()
	{
		Debug.DrawRay(transform.position, transform.forward * 4, Color.blue);
		Debug.DrawLine(oldPosition, transform.position, Color.green, 0.7f);
		oldPosition = transform.position;
		if (Input.GetKey/*Down*/("joystick button 5") && DataStorage.playersBoomerangCount[playerNumber] > 0 && DataStorage.playersControlType[playerNumber] == 1)
		{
			GameObject boomref = GameObject.Instantiate(DiscPrefab, transform.position + transform.forward * spawnDist, transform.rotation * Quaternion.Euler(90, 0, 0), DataStorage.assetParent.transform);
			

            DiscController boomrefscript = boomref.GetComponent< DiscController >();
            boomrefscript.SetPlayerNumber(playerNumber);
			boomref.GetComponent<Renderer>().material.color = playerColor;
			boomref.GetComponent<Renderer>().material.SetColor("_EmissionColor",playerColor);
			// Rigidbody2D	ds = boomrang.GetComponent< Rigidbody2D >();
			// ds.AddForce(diff * boomrangInitialSpeed, ForceMode2D.Impulse);
			DataStorage.playersBoomerangCount[playerNumber]--;
		}
		else if (Input.GetMouseButtonDown(0) && DataStorage.playersBoomerangCount[playerNumber] > 0  && DataStorage.playersControlType[playerNumber] == 0)
		{
			GameObject boomref = GameObject.Instantiate(DiscPrefab, transform.position + transform.forward * spawnDist, transform.rotation * Quaternion.Euler(90, 0, 0), DataStorage.assetParent.transform);
			

            DiscController boomrefscript = boomref.GetComponent< DiscController >();
            boomrefscript.SetPlayerNumber(playerNumber);
			boomref.GetComponent<Renderer>().material.color = playerColor;
			boomref.GetComponent<Renderer>().material.SetColor("_EmissionColor",playerColor);
			// Rigidbody2D	ds = boomrang.GetComponent< Rigidbody2D >();
			// ds.AddForce(diff * boomrangInitialSpeed, ForceMode2D.Impulse);
	        DataStorage.playersBoomerangCount[playerNumber]--;
    	}
		if (Input.GetKeyDown("joystick button 4") && DataStorage.playersControlType[playerNumber] == 1)
		{
			dashingDir = direction;
			dashing = 1;
		}
		if (Input.GetKeyDown("space")&& DataStorage.playersControlType[playerNumber] == 0)
		{
			dashingDir = direction;
			dashing = 1;
		}

	}
	

    public void MannetteMovement(Rigidbody rbody, int playerNumber, GameObject DiscPrefab)
    {
		
		input = new Vector3(Input.GetAxisRaw("Horizontal2"), 0, Input.GetAxisRaw("Vertical2"));
	//	print("x = " + input.x + "z = " + input.z);
		direction = input/*.normalized*/;
		velocity = direction * speed;
		moveAmount = velocity * Time.fixedDeltaTime;
		rbody.MovePosition(transform.position + moveAmount);
    	
		Vector3 dir2 = new Vector3 (Input.GetAxisRaw("Horizontal3"), 0, Input.GetAxisRaw("Vertical3"));
		
		if (dir2.z != 0 || dir2.x != 0)
			diff  = dir2;
		// print("x = " + dir2.x + "z = " + dir2.z);
		// Vector3 diff = dir2 - gO.transform.position;
        diff.Normalize();
		float rot_y = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, rot_y, 0f);
		// rbody.velocity = Vector3.zero;

	

	}


    public void MouseMovement(Rigidbody rbody, int playerNumber, GameObject DiscPrefab)
    {
		input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		direction = input.normalized;
		velocity = direction * speed;
		moveAmount = velocity * Time.fixedDeltaTime;
		
		rbody.MovePosition(transform.position + moveAmount);
    	
		diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
		float rot_y = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, rot_y, 0f);
		rbody.velocity = Vector3.zero;



    }

	// Update is called once per frame
	void FixedUpdate()
	{
		int i = 0;
		if (DataStorage.playersControlType[playerNumber] == 0 && dashing != 1)
			MouseMovement(rbody, playerNumber, DiscPrefab);
		else if (DataStorage.playersControlType[playerNumber] == 1 && dashing != 1)
			MannetteMovement(rbody, playerNumber, DiscPrefab);
		if (dashing == 1)
		{
			if(Physics.Raycast(transform.position, dashingDir , out hitInfo))
			{
				// print("dist hit = " + hitInfo.distance + " thing = " + hitInfo.collider.gameObject.tag);
				if (hitInfo.distance < dashingDir.magnitude * dashMulti)
					{
						
						dashedTime = 0f;
						dashing = 0;
						i = 1;
					}
			}
			if (i == 0)
			{
				dashedTime += Time.fixedDeltaTime;
				rbody.MovePosition(transform.position + dashingDir * dashMulti);
			}
			if (dashedTime > dashingTime)
			{
				dashedTime = 0f;
				dashing = 0;
			}
		}
	}


	void OnCollisionEnter(Collision coll)
	{
		// print("PLAYER COLIDING");
		if (coll.gameObject.tag == "wall")
		{
			dashedTime = 0;
			dashing = 0;
		}
	}
	
}
