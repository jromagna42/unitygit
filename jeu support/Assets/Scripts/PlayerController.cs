using System.Collections;
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

		rbody = GetComponent< Rigidbody >();
		DataStorage.playersGameObject[playerNumber] = gameObject;
	}

	void Update()
	{
		Debug.DrawRay(transform.position, transform.forward * 4, Color.blue);
		Debug.DrawLine(oldPosition, transform.position, Color.green, 0.7f);
		oldPosition = transform.position;
		if (Input.GetKey ("joystick button 5") && DataStorage.playersBoomerangCount[playerNumber] > 0)
		{
			GameObject boomref = GameObject.Instantiate(DiscPrefab, transform.position + transform.forward * spawnDist, transform.rotation * Quaternion.Euler(90, 0, 0), DataStorage.assetParent.transform);
			DataStorage.playersBoomerangCount[playerNumber]--;

            DiscController boomrefscript = boomref.GetComponent< DiscController >();
            boomrefscript.SetPlayerNumber(playerNumber);
			// Rigidbody2D	ds = boomrang.GetComponent< Rigidbody2D >();
			// ds.AddForce(diff * boomrangInitialSpeed, ForceMode2D.Impulse);
		}
		if (Input.GetMouseButtonDown(0) && DataStorage.playersBoomerangCount[playerNumber] > 0)
		{
			GameObject boomref = GameObject.Instantiate(DiscPrefab, transform.position + transform.forward * spawnDist, transform.rotation * Quaternion.Euler(90, 0, 0), DataStorage.assetParent.transform);
			

            DiscController boomrefscript = boomref.GetComponent< DiscController >();
            boomrefscript.SetPlayerNumber(playerNumber);
			// Rigidbody2D	ds = boomrang.GetComponent< Rigidbody2D >();
			// ds.AddForce(diff * boomrangInitialSpeed, ForceMode2D.Impulse);
	        DataStorage.playersBoomerangCount[playerNumber]--;
    	}
	}
	

    public void MannetteMovement(Rigidbody rbody, GameObject gO, int playerNumber, GameObject DiscPrefab)
    {
		
		input = new Vector3(Input.GetAxisRaw("Horizontal2"), 0, Input.GetAxisRaw("Vertical2"));
		// print("x = " + input.x + "z = " + input.z);
		direction = input.normalized;
		velocity = direction * speed;
		moveAmount = velocity * Time.fixedDeltaTime;
		rbody.MovePosition(gO.transform.position + moveAmount);
    	
		Vector3 dir2 = new Vector3 (Input.GetAxisRaw("Horizontal3"), 0, Input.GetAxisRaw("Vertical3"));
		
		if (dir2.z != 0 && dir2.x != 0)
			diff  = dir2;
		// print("x = " + dir2.x + "z = " + dir2.z);
		// Vector3 diff = dir2 - gO.transform.position;
        diff.Normalize();
		float rot_y = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
        gO.transform.rotation = Quaternion.Euler(0f, rot_y, 0f);
		// rbody.velocity = Vector3.zero;

	

		if (Input.GetKeyDown("space"))
		{
             float dashMulti = 1;
			rbody.AddForce(diff * dashMulti, ForceMode.Impulse);
		}
	}


    public void MouseMovement(Rigidbody rbody, GameObject gO, int playerNumber, GameObject DiscPrefab)
    {
		input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		direction = input.normalized;
		velocity = direction * speed;
		moveAmount = velocity * Time.fixedDeltaTime;
		rbody.MovePosition(gO.transform.position + moveAmount);
    	
		diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gO.transform.position;
        diff.Normalize();
		float rot_y = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
        gO.transform.rotation = Quaternion.Euler(0f, rot_y, 0f);
		rbody.velocity = Vector3.zero;


		if (Input.GetKeyDown("space"))
		{
             float dashMulti = 1;
			rbody.AddForce(diff * dashMulti, ForceMode.Impulse);
		}
    }

	// Update is called once per frame
	void FixedUpdate()
	{
		if (DataStorage.playersControlType[playerNumber] == 0)
			MouseMovement(rbody, gameObject, playerNumber, DiscPrefab);
		else if (DataStorage.playersControlType[playerNumber] == 1)
			MannetteMovement(rbody, gameObject, playerNumber, DiscPrefab);
	}
	
}
