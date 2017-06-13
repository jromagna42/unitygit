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
	void Start ()
	{
		if ((assetParent = GameObject.Find(assetParentName)) == null)
			assetParent = new GameObject(assetParentName);
		rbody = GetComponent< Rigidbody >();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
		Vector3 direction = input.normalized;
		Vector3 velocity = direction * speed;
		Vector3 moveAmount = velocity * Time.deltaTime;
		rbody.MovePosition(transform.position + moveAmount);
    	
		Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
		rbody.velocity = Vector3.zero;

		if (Input.GetMouseButtonDown(0))
		{
			/*GameObject boomrang = */Instantiate(DiscPrefab, transform.position, transform.rotation, assetParent.transform);
			// Rigidbody2D	ds = boomrang.GetComponent< Rigidbody2D >();
			// ds.AddForce(diff * boomrangInitialSpeed, ForceMode2D.Impulse);
		}
	}
}
