using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	const string			assetParentName = "SceneAssets";
	GameObject				assetParent;
	Rigidbody				rbody;

	public float speed = 7;
	public float boomrangInitialSpeed = 30;
	public GameObject DiscPrefab;
	// Use this for initialization
	public float dashMulti = 1;
	public int boomerangCount = 1;
	void Start ()
	{
		if ((assetParent = GameObject.Find(assetParentName)) == null)
			assetParent = new GameObject(assetParentName);
		rbody = GetComponent< Rigidbody >();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		Vector3 direction = input.normalized;
		Vector3 velocity = direction * speed;
		Vector3 moveAmount = velocity * Time.deltaTime;
		rbody.MovePosition(transform.position + moveAmount);
    	
		Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
		float rot_y = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90f, rot_y - 90, 0f);
		rbody.velocity = Vector3.zero;

		if (Input.GetMouseButtonDown(0) && boomerangCount > 0)
		{
			Instantiate(DiscPrefab, transform.position, transform.rotation, assetParent.transform);
			boomerangCount--;
			// Rigidbody2D	ds = boomrang.GetComponent< Rigidbody2D >();
			// ds.AddForce(diff * boomrangInitialSpeed, ForceMode2D.Impulse);
		}

		if (Input.GetKeyDown("space"))
		{
			rbody.AddForce(diff * dashMulti, ForceMode.Impulse);
		}
	}
}
