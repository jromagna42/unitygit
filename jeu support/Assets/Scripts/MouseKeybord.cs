using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseKeybord : MonoBehaviour{

    public static void movement(Rigidbody rbody, GameObject gO, int playerNumber, GameObject DiscPrefab)
    {
        float speed = 30;
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		Vector3 direction = input.normalized;
		Vector3 velocity = direction * speed;
		Vector3 moveAmount = velocity * Time.deltaTime;
		rbody.MovePosition(gO.transform.position + moveAmount);
    	
		Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gO.transform.position;
        diff.Normalize();
		float rot_y = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
        gO.transform.rotation = Quaternion.Euler(90f, rot_y - 90, 0f);
		rbody.velocity = Vector3.zero;

		if (Input.GetMouseButtonDown(0) && DataStorage.playersBoomerangCount[playerNumber] > 0)
		{
			GameObject boomref = Instantiate(DiscPrefab, gO.transform.position, gO.transform.rotation, DataStorage.assetParent.transform);
			

            DiscController boomrefscript = boomref.GetComponent< DiscController >();
            boomrefscript.SetPlayerNumber(playerNumber);
			// Rigidbody2D	ds = boomrang.GetComponent< Rigidbody2D >();
			// ds.AddForce(diff * boomrangInitialSpeed, ForceMode2D.Impulse);
	        DataStorage.playersBoomerangCount[playerNumber]--;
    	}

		if (Input.GetKeyDown("space"))
		{
             float dashMulti = 1;
			rbody.AddForce(diff * dashMulti, ForceMode.Impulse);
		}
    }
}
