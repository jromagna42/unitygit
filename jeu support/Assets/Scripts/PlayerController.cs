using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Rigidbody				rbody;

	public float boomrangInitialSpeed = 30;
	public GameObject DiscPrefab;
	// Use this for initialization

	public int playerNumber;
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
	
	// Update is called once per frame
	void Update () 
	{
		if (DataStorage.playersControlType[playerNumber] == 0)
			MouseKeybord.movement(rbody, gameObject, playerNumber, DiscPrefab);
		else if (DataStorage.playersControlType[playerNumber] == 1)
			Manette.movement(rbody, gameObject, playerNumber, DiscPrefab);


		// Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		// Vector3 direction = input.normalized;
		// Vector3 velocity = direction * speed;
		// Vector3 moveAmount = velocity * Time.deltaTime;
		// rbody.MovePosition(transform.position + moveAmount);
    	
		// Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // diff.Normalize();
		// float rot_y = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(90f, rot_y - 90, 0f);
		// rbody.velocity = Vector3.zero;

		// if (Input.GetMouseButtonDown(0) && DataStorage.playersBoomerangCount[playerNumber] > 0)
		// {
		// 	Instantiate(DiscPrefab, transform.position, transform.rotation, assetParent.transform);
		// 	DataStorage.playersBoomerangCount[playerNumber]--;
		// 	// Rigidbody2D	ds = boomrang.GetComponent< Rigidbody2D >();
		// 	// ds.AddForce(diff * boomrangInitialSpeed, ForceMode2D.Impulse);
		// }

		// if (Input.GetKeyDown("space"))
		// {
		// 	rbody.AddForce(diff * dashMulti, ForceMode.Impulse);
		// }
	}
}
