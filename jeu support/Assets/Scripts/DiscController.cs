using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscController : MonoBehaviour {
float finalSpeed = 0;
public float travelTime = 1f;
public float initialSpeed = 20;
Rigidbody	ds;
Vector3 dir;
public float acceleration;
public float moveAmount;
float timeFlying = 0;
int playerNumber = -1;
Vector3 mousePos;

	public void SetPlayerNumber(int i)
	{
		playerNumber = i;
	}


	void	Manette()
	{


	}

	void	MouseKeybord()
	{

	}
	void Start () {
		// playerNumber = -1;
		if (DataStorage.debug >= 2)
		{
			if (playerNumber != 0)
				print("playernumber" + playerNumber);
		}
		if (DataStorage.playersControlType[playerNumber] == 0)
		{
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.y = transform.position.y;
			dir = mousePos - transform.position;
		}
		if (DataStorage.playersControlType[playerNumber] == 1)
		{
			mousePos = new Vector3 (Input.GetAxisRaw("Horizontal3"), 0, Input.GetAxisRaw("Vertical3"));
			dir = mousePos;
		}
		dir.Normalize();
		dir.y = 0;
		// initialSpeed = ((2 * dir.magnitude) / travelTime) - finalSpeed;
		
		ds = gameObject.GetComponent< Rigidbody >();
		// ds.MovePosition(transform.position + dir.normalized * initialSpeed);
		acceleration = (finalSpeed- initialSpeed) / (2 * travelTime);
		moveAmount = initialSpeed;

		if (DataStorage.debug >= 2)
		{
			print("dist:" + dir.magnitude);
			print("init speed:" + initialSpeed);
			print("accel:" + acceleration);
		}

		//ds.AddForce(dir.normalized * initialSpeed, ForceMode2D.Impulse);
	}
	
	float botTabTimer = 0;
	// Update is called once per frame
	int oldi = 0;
	int oldj = 0;
	int i = 0;
	int j = 0;

	void CleanBotTab()
	{
		DataStorage.botTab[oldi, oldj] = 0f;
		DataStorage.botTab[oldi, oldj + 1] = 0f;
		DataStorage.botTab[oldi, oldj + 2] = 0f;
		DataStorage.botTab[oldi + 1, oldj] = 0f;
		DataStorage.botTab[oldi + 1, oldj + 1] = 0f;
		DataStorage.botTab[oldi + 1, oldj + 2] = 0f;
		DataStorage.botTab[oldi + 2, oldj] = 0f;
		DataStorage.botTab[oldi + 2, oldj + 1] = 0f;
		DataStorage.botTab[oldi + 2, oldj + 2] = 0f;
	}

	void RunAwayProtect()
	{
		if (i < -1 || i > DataStorage.tabHNumber + 1 || j < -1 || j > DataStorage.tabVNumber + 1)
		{
			DataStorage.playersBoomerangCount[playerNumber]++;
			CleanBotTab();
			Destroy(gameObject);
		}
	}
	void UpdateBotTab()
	{
		i = (int)(Mathf.Abs(DataStorage.tabStartPos.x - transform.position.x) / DataStorage.tabHSize);
		j = (int)(Mathf.Abs(DataStorage.tabStartPos.z - transform.position.z) / DataStorage.tabVSize);
		RunAwayProtect();
		i = (i > 0)? i - 1: i;
		j = (j > 0)? j - 1: j;
		i = (i >= DataStorage.tabHNumber - 3)? DataStorage.tabHNumber - 3: i;
		j = (j >= DataStorage.tabVNumber - 3)? DataStorage.tabVNumber - 3: j;
		DataStorage.botTab[i, j] = 1f;

		DataStorage.botTab[i, j + 1] = 1f;
		DataStorage.botTab[i, j + 2] = 1f;
		DataStorage.botTab[i + 1, j] = 1f;
		DataStorage.botTab[i + 1, j + 1] = 1f;
		DataStorage.botTab[i + 1, j + 2] = 1f;
		DataStorage.botTab[i + 2, j] = 1f;
		DataStorage.botTab[i + 2, j + 1] = 1f;
		DataStorage.botTab[i + 2, j + 2] = 1f;
		oldi = i;
		oldj = j;
	//	print("x = " + i + " z = " + j );
	}
	void Update () {
			
			botTabTimer += Time.deltaTime;
			if (botTabTimer > 0.02f)
			{
				CleanBotTab();
				UpdateBotTab();
				botTabTimer = 0;
			}
			
			
						//ds.AddForce(dir.normalized * (acceleration * Time.deltaTime), ForceMode2D.Impulse);
			// transform.Rotate(new Vector3(0, 0, 10));
			// ds.MovePosition(transform.position + dir.normalized * (moveAmount * Time.deltaTime));
			// moveAmount += acceleration * Time.deltaTime;
			// Debug.DrawLine(transform.position, transform.position + (dir.normalized * moveAmount), Color.green, Time.deltaTime );
			// Debug.DrawLine(transform.position, transform.position + (dir.normalized * acceleration), Color.red, Time.deltaTime );
			// print("speed:" + moveAmount);
	}

	void FixedUpdate()
	{
		timeFlying += Time.fixedDeltaTime;
		transform.Rotate(new Vector3(0, 0, 10));
		ds.MovePosition(transform.position + dir * (moveAmount * Time.fixedDeltaTime));
		moveAmount += acceleration * Time.fixedDeltaTime;
		//Debug.Log(" movam =  " + moveAmount + " acel = " + acceleration);
		if (moveAmount < 0)
		{
			moveAmount = -moveAmount;
			acceleration = -acceleration;
			dir = -dir;
		}
		if (DataStorage.debug == 1)
		{
			Debug.DrawLine(transform.position, transform.position + (dir * moveAmount), Color.green, Time.fixedDeltaTime );
			Debug.DrawLine(transform.position, transform.position + (dir * acceleration), Color.red, Time.fixedDeltaTime );
		}
	}
	string lastWallHit = null;

	int	botSmallTrigger(Vector3 pos)
	{
		float dist = Mathf.Abs((pos - transform.position).magnitude);
		print(dist);
		if (dist < 3f)
			return(1);
		else
			return (0);
	}

	int collnumber = 0;
	void OnTriggerEnter(Collider coll)
	{
		if ((coll.gameObject.tag == "Player") && playerNumber != -1)
		{
			PlayerController collScript = coll.gameObject.GetComponent< PlayerController >();
		// Debug.Log("hit = " + coll.gameObject.tag);
			if (collScript.playerNumber == playerNumber && timeFlying > 0.2f || collScript.dashing == 1)
			{
				// Debug.Log("PLAYER HIT");
				DataStorage.playersBoomerangCount[collScript.playerNumber]++;
				if (playerNumber != collScript.playerNumber)
					DataStorage.playersBoomerangCount[playerNumber]++;
				CleanBotTab();
				Destroy(gameObject);
			}
			else if (collScript.playerNumber != playerNumber)
			{
				//print("PLAYER no" + collScript.playerNumber + " DIEDED!");
				collScript.OnPlayerDeath += collScript.KillPlayer;
			}
		}
		if ((coll.gameObject.tag == "bot") && playerNumber != -1)
		{
			print("BOT COLLIDED" + collnumber);
			collnumber++;
			BotController collScript = coll.gameObject.GetComponent< BotController >();
		// Debug.Log("hit = " + coll.gameObject.tag);
			if (collScript.playerNumber == playerNumber && timeFlying > 0.2f || collScript.dashing == 1 && botSmallTrigger(coll.gameObject.transform.position) == 1)
			{
				// Debug.Log("PLAYER HIT");
				DataStorage.playersBoomerangCount[collScript.playerNumber]++;
				if (playerNumber != collScript.playerNumber)
					DataStorage.playersBoomerangCount[playerNumber]++;
				CleanBotTab();
				Destroy(gameObject);
			}
			else if (collScript.playerNumber != playerNumber && botSmallTrigger(coll.gameObject.transform.position) == 1)
			{
				//print("PLAYER no" + collScript.playerNumber + " DIEDED!");
				collScript.OnPlayerDeath += collScript.KillPlayer;
			}
		}
	}
	void OnCollisionEnter(Collision coll)
	{
		RaycastHit hitInfo;
		int i = 0;
			if (coll.gameObject.tag == "wall" && !string.Equals(lastWallHit, coll.gameObject.name) )
			{
				// while ()
				// {
					// print("just hit "  + coll.gameObject.name);
					// print("last wall " + lastWallHit);
					// ds.MovePosition(transform.position + dir.normalized * (moveAmount * Time.deltaTime));
					Vector3 inNormal = Camera.main.transform.position - coll.gameObject.transform.position;
					inNormal.y = 0f;
					inNormal = inNormal.normalized;
					// Debug.Log("pre dir  = " + dir);
					// Debug.Log("norm  = " + inNormal);
					if (DataStorage.debug >= 2)
					{
						DrawArrow.Debug_arrow(gameObject.transform.position - dir * 5 , gameObject.transform.position, Color.blue, 2f);
					}
					dir = Vector3.Reflect(dir, inNormal).normalized;

					if (DataStorage.debug >= 2)
					{
						DrawArrow.Debug_arrow(gameObject.transform.position, gameObject.transform.position + (inNormal * 5), Color.red, 2f);
						DrawArrow.Debug_arrow(gameObject.transform.position, gameObject.transform.position + dir * 5, Color.green, 2f);
					}

					dir.y = 0;
					// if ((moveAmount < 0 && acceleration < 0) || (moveAmount > 0 && acceleration > 0))
					// Debug.Log("posdt dir  = " + dir);
					acceleration = Mathf.Abs(acceleration);
					moveAmount /= 1.8f;
					moveAmount = Mathf.Abs(moveAmount);
					lastWallHit = coll.gameObject.name;
				// }
			}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(Vector3.zero, 3);
	}

}
