using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manette : MonoBehaviour{

    public void movement(Rigidbody rbody, GameObject gO, int playerNumber, GameObject DiscPrefab)
    {
        float speed = 15;
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal2"), 0, Input.GetAxisRaw("Vertical2"));
		print("x = " + input.x + "z = " + input.z);
		Vector3 direction = input.normalized;
		Vector3 velocity = direction * speed;
		Vector3 moveAmount = velocity * Time.fixedDeltaTime;
		rbody.MovePosition(gO.transform.position + moveAmount);
    	
		Vector3 dir2 = new Vector3 (Input.GetAxisRaw("Horizontal3"), 0, Input.GetAxisRaw("Vertical3"));
		print("x = " + dir2.x + "z = " + dir2.z);
		Vector3 diff = dir2 - gO.transform.position;
        diff.Normalize();
		float rot_y = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
        gO.transform.rotation = Quaternion.Euler(90f, rot_y - 90, 0f);
		// rbody.velocity = Vector3.zero;

		if (Input.GetMouseButtonDown(0) && DataStorage.playersBoomerangCount[playerNumber] > 0)
		{
			GameObject boomref =Instantiate(DiscPrefab, gO.transform.position, gO.transform.rotation, DataStorage.assetParent.transform);
			DataStorage.playersBoomerangCount[playerNumber]--;

            DiscController boomrefscript = boomref.GetComponent< DiscController >();
            boomrefscript.SetPlayerNumber(playerNumber);
			// Rigidbody2D	ds = boomrang.GetComponent< Rigidbody2D >();
			// ds.AddForce(diff * boomrangInitialSpeed, ForceMode2D.Impulse);
		}

		if (Input.GetKeyDown("space"))
		{
             float dashMulti = 1;
			rbody.AddForce(diff * dashMulti, ForceMode.Impulse);
		}
    }
}
