using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BotController : MonoBehaviour {

	// public MouseKeybord mouseKeybordRef = new MouseKeybord();
	// public Manette manetteRef = new Manette();
	public GameObject DiscPrefab;
	public int dashing = 0;
	public float dashingTime = 1f;
	public float dashMulti = 1;
	public int limit = 5;
	public int playerNumber;

	Vector3 diff = new Vector3();
	Vector3 oldPosition;
	float speed = 15;
	float spawnDist = 2;	
	Vector3 input;
	Vector3 direction;
	Vector3 velocity;
	Vector3			moveAmount;
	
	Rigidbody		rbody;
	Color playerColor;
	Vector3 dashingDir;
	float dashedTime = 0f;
	RaycastHit hitInfo;
	int botChangeDir = 0;
	// RaycastHit[] BotHitInfo = new RaycastHit[8];
	int dashUp = 1;
	float DashUpTimer = 0;
	Renderer playerRenderer;
	int evade = 0;


	public event Action OnPlayerDeath;
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

	public void KillPlayer()
	{
		Destroy(gameObject);
	}
	void Start ()
	{
		GivePlayerBoomerang();
	//	playerNumber = DataStorage.nextPlayer++;
		playerNumber = DataStorage.playerCount - 1;
		DataStorage.playersControlType[playerNumber] = playerNumber;
		print(DataStorage.playersBoomerangCount[playerNumber]);
		playerColor = DataStorage.playerColors[playerNumber].mainColor;
		rbody = GetComponent< Rigidbody >();
		DataStorage.playersGameObject[playerNumber] = gameObject;
		playerRenderer = GetComponent<Renderer>();
		SetMainColor();
	}

	void SetAltColor()
	{
		playerRenderer.material.color = DataStorage.playerColors[playerNumber].altColor;
		playerRenderer.material.SetColor("_EmissionColor",DataStorage.playerColors[playerNumber].altColor);
		transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = DataStorage.playerColors[playerNumber].altColor;;
		transform.GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor",DataStorage.playerColors[playerNumber].altColor);
	}

	void SetMainColor()
	{
		playerRenderer.material.color = playerColor;
		playerRenderer.material.SetColor("_EmissionColor",playerColor);
		transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = playerColor;
		transform.GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor",playerColor);
	}

	// void CastDetectionRay()
	// {
	// 		Dictionary<int, Vector3> RayDir = new Dictionary<int, Vector3>()
	// 	{
	// 		{ 0, transform.forward },
	// 		{ 1, transform.forward },
	// 		{ 2, transform.forward },
	// 		{ 3, transform.forward },
	// 		{ 4, transform.forward },
	// 		{ 5, transform.forward },
	// 		{ 6, transform.forward },
	// 		{ 7, transform.forward },
	// 		{ 8, transform.forward }
	// 	};
	// 	if (Physics.Raycast(transform.position,  transform.forward , out BotHitInfo[0], 8f) == true)
	// 		Debug.DrawRay(transform.position,  transform.forward * 8f, Color.red );
	// 	else
	// 		Debug.DrawRay(transform.position,  transform.forward * 8f, Color.green);
	// 	if	(Physics.Raycast(transform.position,  -transform.forward , out BotHitInfo[1], 8f) == true)
	// 		Debug.DrawRay(transform.position,  -transform.forward * 8f, Color.red );
	// 	else
	// 		Debug.DrawRay(transform.position,  -transform.forward * 8f, Color.green);
			
	// 	if	(Physics.Raycast(transform.position,  transform.right , out BotHitInfo[2], 8f) == true)
	// 		Debug.DrawRay(transform.position,  transform.right * 8f, Color.red );
	// 	else
	// 		Debug.DrawRay(transform.position,  transform.right * 8f, Color.green);
		
	// 	if	(Physics.Raycast(transform.position,  -transform.right , out BotHitInfo[3], 8f) == true)
	// 		Debug.DrawRay(transform.position,  -transform.right * 8f, Color.red );
	// 	else
	// 		Debug.DrawRay(transform.position,  -transform.right * 8f, Color.green );

	// 	if	(Physics.Raycast(transform.position,  (transform.forward + transform.right).normalized, out BotHitInfo[4], 8f) == true)
	// 		Debug.DrawRay(transform.position,  (transform.forward + transform.right).normalized * 8f, Color.red );
	// 	else
	// 		Debug.DrawRay(transform.position,  (transform.forward + transform.right).normalized * 8f, Color.green );

	// 	if	(Physics.Raycast(transform.position,  (-transform.forward + transform.right).normalized, out BotHitInfo[5], 8f) == true)
	// 		Debug.DrawRay(transform.position,  (-transform.forward + transform.right).normalized * 8f, Color.red );
	// 	else
	// 		Debug.DrawRay(transform.position,  (-transform.forward + transform.right).normalized * 8f, Color.green );

	// 	if	(Physics.Raycast(transform.position,  (transform.forward + -transform.right).normalized, out BotHitInfo[6], 8f) == true)
	// 		Debug.DrawRay(transform.position,  (transform.forward + -transform.right).normalized * 8f, Color.red );
	// 	else
	// 		Debug.DrawRay(transform.position,  (transform.forward + -transform.right).normalized * 8f, Color.green );

	// 	if	(Physics.Raycast(transform.position,  (-transform.forward + -transform.right).normalized, out BotHitInfo[7], 8f) == true)
	// 		Debug.DrawRay(transform.position,  (-transform.forward + -transform.right).normalized * 8f, Color.red );
	// 	else
	// 		Debug.DrawRay(transform.position,  (-transform.forward + -transform.right).normalized * 8f, Color.green );

	// 	// Physics.Raycast(transform.position,  + transform.forward , out hitInfo, 1f);
	// 	// Physics.Raycast(transform.position,  + transform.forward , out hitInfo, 1f);
	// 	// Physics.Raycast(transform.position,  + transform.forward , out hitInfo, 1f);
	// }
	void Update()
	{
		// Debug.DrawRay(transform.position, transform.forward * 4, Color.blue);
		Debug.DrawLine(oldPosition, transform.position, Color.green, 0.7f);
	//	CastDetectionRay();
		oldPosition = transform.position;
		if (Input.GetKeyDown("joystick button 5") && DataStorage.playersControlType[playerNumber] == 1)
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
		if (Input.GetKeyDown("joystick button 4") && dashUp == 1)
		{
			dashingDir = direction.normalized;
			dashing = 1;
			dashUp = 0;
			SetAltColor();
		}
		if (dashUp == 0)
		{
			DashUpTimer += Time.deltaTime;
			if (DashUpTimer > 0.6f)
			{
				dashUp = 1;
				DashUpTimer = 0f;
			}
		}
		if (OnPlayerDeath != null)
			OnPlayerDeath();
	}
	float botChangeDirTime = 0;
	Vector3 botDir = Vector3.zero;
	float nextDirTime = 0.5f;
	Vector3 BotDirectionator()
	{
		botChangeDirTime += Time.fixedDeltaTime;
		if(	botChangeDirTime > nextDirTime)
		{
			botChangeDir = 1;
			nextDirTime = UnityEngine.Random.Range(0.1f, 1f);
		}
		if (botChangeDir == 1)
		{
			botChangeDirTime = 0;
			botChangeDir = 0;
			botDir = UnityEngine.Random.insideUnitSphere;
			botDir.y = 0;
		}
		return(botDir);
	}

	Vector3 findClosestBoomerang()
	{
		int i = (int)(Mathf.Abs(DataStorage.tabStartPos.x - transform.position.x) / DataStorage.tabHSize);
		int j = (int)(Mathf.Abs(DataStorage.tabStartPos.z - transform.position.z) / DataStorage.tabVSize);
		int x = -1;
		int y = -1;
		int limit = 1;
		Vector3 ret;

		while (DataStorage.botTab[i + x, j + y] != 1 && limit < 5/*10 / DataStorage.tabVSize*/)
		{
				// }		// while (i + x < 0 || j + y < 0 || i + x < DataStorage.tabHSize || j + y < DataStorage.tabVSize)
			// {
			//	Debug.Log("x = " + x + " y = " + y);
				if (x <= limit)
					x++;
				else
				{
					x = -limit;
					y++;
				}
				if (y > limit)
				{
					limit++;
					y = -limit;
					x = -limit;
				}

		}
		ret = new Vector3((i + x) * DataStorage.tabHSize,0 , (j + y) * DataStorage.tabVSize);

		return ((transform.position - ret).normalized);
	}

	bool InTab(int i, int j)
	{
		//Debug.Log("i = " + i + " j = " + j);
		if (i < 0 || i >= DataStorage.tabHNumber - 1 || j < 0 || j >= DataStorage.tabVNumber - 1)
		{
			return (false);
		}
		return (true);
	}

	Vector3 findAllCloseBoomerang()
	{
		Vector3 ret = Vector3.zero;
		int i = (int)(Mathf.Abs(DataStorage.tabStartPos.x - transform.position.x) / DataStorage.tabHSize);
		int j = (int)(Mathf.Abs(DataStorage.tabStartPos.z - transform.position.z) / DataStorage.tabVSize);
		int x = -limit;
		int z = -limit;

		while (z < limit)
		{
			x = -limit;
			while (x < limit)
			{
				if (InTab(i + x, j + z) == true && DataStorage.botTab[i + x, j + z] == 1)
					ret += (transform.position - new Vector3((i + x) * DataStorage.tabHSize,0 , (j + z) * DataStorage.tabVSize));
				x++;
			}
			z++;
		}
		return (ret.normalized);
	}

	public int botType = 1;
	Vector3 evadeManeuver()
	{
		if (botType == 0)
			botDir = findClosestBoomerang();
		else if (botType == 1)
			botDir = findAllCloseBoomerang();
		
		return(botDir);
	}

	float botLookAtTime = 0;
	Vector3 botLookAtRet = Vector3.zero;
	
	Vector3 FindClosestPlayer()
	{
		Vector3 ret = Vector3.zero;
		Vector3 temp = Vector3.zero;

		for (int i = 0; i < DataStorage.playerCount ; i++)
		{
			if(DataStorage.playersGameObject[i])
			{
				if(i != playerNumber)
				{
					temp = DataStorage.playersGameObject[i].transform.position - transform.position;
				}
				if (ret == Vector3.zero || temp.sqrMagnitude < ret.sqrMagnitude)
				{
					ret = temp;
				}
			}
		}
		return(ret);
	}

	Vector3 BotLookAt()
	{
		botLookAtTime += Time.fixedDeltaTime;

		if (botLookAtTime > 0.1f)
		{
			botLookAtRet = FindClosestPlayer();
			botLookAtTime = 0f;
		}
		return(botLookAtRet);
	}

    public void BotMovement(Rigidbody rbody)
    {
		if (evade == 0)
			input = /*Vector3.zero; */BotDirectionator();
		else if (evade == 1)
		{
			evade = 0;
			input = evadeManeuver();
		}

		direction = input.normalized;
		Debug.DrawLine(transform.position, transform.position + direction*10, Color.blue , Time.fixedDeltaTime);
		velocity = direction * speed;
		moveAmount = velocity * Time.fixedDeltaTime;
		
		rbody.MovePosition(transform.position + moveAmount);
    	
		diff = BotLookAt();
        diff.Normalize();
		float rot_y = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, rot_y, 0f);
		rbody.velocity = Vector3.zero;



    }

	// Update is called once per frame
	void FixedUpdate()
	{
		int i = 0;
		BotMovement(rbody);
		// if (DataStorage.playersControlType[playerNumber] == 0 && dashing != 1)
		// 	MouseMovement(rbody, playerNumber, DiscPrefab);
		// else if (DataStorage.playersControlType[playerNumber] == 1 && dashing != 1)
		// 	MannetteMovement(rbody, playerNumber, DiscPrefab);
		if (dashing == 1)
		{
			int layerMask = 1 << 8;
			layerMask = ~layerMask;
			if(Physics.Raycast(transform.position, dashingDir , out hitInfo, 10000f, layerMask))
			{
				// print("dist hit = " + hitInfo.distance + " thing = " + hitInfo.collider.gameObject.tag);
				if (hitInfo.distance < dashingDir.magnitude * dashMulti)
					{
						
						dashedTime = 0f;
						dashing = 0;
						i = 1;
						print("dashstop1");
						SetMainColor();
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
				print("dashstop2");
				SetMainColor();
			}
		}
	}

	void OnTriggerStay(Collider coll)
	{
		if (coll.gameObject.tag == "boomerang")
		{
			evade = 1;
			//print("ESCAAAAAAAAAAAAAAAAAAAPE");
		}
	}

	// void OnCollisionEnter(Collision coll)
	// {
	// 	// print("PLAYER COLIDING");
	// 	if (coll.gameObject.tag == "wall")
	// 	{
	// 		dashedTime = 0;
	// 		dashing = 0;
	// 		print("dashstop3");
	// 		SetMainColor();
	// 	}
	// }
	
	void OnCollisionStay(Collision coll)
	{
		// print("PLAYER COLIDING");
		if (coll.gameObject.tag == "wall")
		{
			dashedTime = 0;
			dashing = 0;
		//	print("dashstop3");
			SetMainColor();
		//	print("change dir");
			botChangeDir = 1;
		}
	}
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, DataStorage.tabVSize * limit);
	}
}
