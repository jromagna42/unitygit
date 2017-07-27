using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BotController : MonoBehaviour {

	// public MouseKeybord mouseKeybordRef = new MouseKeybord();
	// public Manette manetteRef = new Manette();
	public GameObject DiscPrefab;
	public int botType = 1;
	public int dashing = 0;
	public float dashingTime = 1f;
	public float dashMulti = 1;
	public int limit = 5;
	public int playerNumber;

	int fireBoom = 0;
	float fireBoomTimer = 0f;
	float nextFireBoomTimer = 1f;
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
//			print("i = " + i);
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
		playerNumber = DataStorage.nextPlayer++;
		DataStorage.playersControlType[playerNumber] = 2;
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

	void Update()
	{
		// Debug.DrawRay(transform.position, transform.forward * 4, Color.blue);
		Debug.DrawLine(oldPosition, transform.position, Color.green, 0.7f);
	//	CastDetectionRay();
		oldPosition = transform.position;
		fireBoomTimer += Time.deltaTime;
		if (fireBoomTimer > nextFireBoomTimer)
		{
			nextFireBoomTimer = UnityEngine.Random.Range(0.05f, 4f);
			fireBoomTimer = 0f;
			fireBoom = 1;
		}
		if (fireBoom == 1 && DataStorage.playersBoomerangCount[playerNumber] > 0)
		{
			GameObject boomref = GameObject.Instantiate(DiscPrefab, transform.position + transform.forward * spawnDist, transform.rotation * Quaternion.Euler(90, 0, 0), DataStorage.assetParent.transform);
            DiscController boomrefscript = boomref.GetComponent< DiscController >();
            boomrefscript.SetPlayerNumber(playerNumber);
			boomrefscript.SetBotDir(diff);
			boomref.GetComponent<Renderer>().material.color = playerColor;
			boomref.GetComponent<Renderer>().material.SetColor("_EmissionColor",playerColor);
			// Rigidbody2D	ds = boomrang.GetComponent< Rigidbody2D >();
			// ds.AddForce(diff * boomrangInitialSpeed, ForceMode2D.Impulse);
			DataStorage.playersBoomerangCount[playerNumber]--;
			fireBoom = 0;
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
			Vector3 closestPlayer  = FindClosestPlayer();
			botDir = UnityEngine.Random.insideUnitSphere;
			while (closestPlayer.magnitude < 15f && Vector3.Angle(botDir, closestPlayer) < 60f)
			{
				botDir = UnityEngine.Random.insideUnitSphere;
			}
			botChangeDirTime = 0;
			botChangeDir = 0;
			botDir.y = 0;
		}
		return(botDir);
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
	Vector3 emergencyDash(Vector3 dir)
	{
		dashingDir = -dir.normalized;
		dashing = 1;
		dashUp = 0;
		SetAltColor();
		return (-dir);
	}
	Vector3 findClosestBoomerang()
	{
		int i = (int)(Mathf.Abs(DataStorage.tabStartPos.x - transform.position.x) / DataStorage.tabHSize);
		int j = (int)(Mathf.Abs(DataStorage.tabStartPos.z - transform.position.z) / DataStorage.tabVSize);
		int x = -1;
		int z = -1;
		int actualRadius = 1;
		Vector3 ret;

		while (actualRadius < 5/*10 / DataStorage.tabVSize*/)
		{
			if (InTab(i + x, j + z) == true && DataStorage.botTab[i + x, j + z] == 1 && DataStorage.botTab[i + x, j + z] != playerNumber + 1)
				break;
				// }		// while (i + x < 0 || j + z < 0 || i + x < DataStorage.tabHSize || j + z < DataStorage.tabVSize)
			// {
			//	Debug.Log("x = " + x + " z = " + z);
				if (x <= actualRadius)
					x++;
				else
				{
					x = -actualRadius;
					z++;
				}
				if (z > actualRadius)
				{
					actualRadius++;
					z = -actualRadius;
					x = -actualRadius;
				}
		}
		ret = transform.position - new Vector3((i + x) * DataStorage.tabHSize,0 , (j + z) * DataStorage.tabVSize);
//		Debug.Log("RET MAG " + ret.magnitude);
		if (ret.magnitude < 2f && dashUp == 1 && DataStorage.botTab[i + x, j + z] != playerNumber + 1)
			return (emergencyDash(ret));
		return ((transform.position - ret).normalized);
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
				if (InTab(i + x, j + z) == true)
				{
					if (DataStorage.botTab[i + x, j + z] == playerNumber + 1)
					{
						// Vector3 debugRay = transform.position - new Vector3((i + x) * DataStorage.tabHSize,0 , (j + z) * DataStorage.tabVSize);
					//	Debug.DrawLine(transform.position, transform.position + debugRay, Color.yellow, 1f);
						// ret -= debugRay;
					}
					else if (DataStorage.botTab[i + x, j + z] != 0)
					{
						Vector3 debugRay = transform.position - new Vector3((i + x) * DataStorage.tabHSize,0 , (j + z) * DataStorage.tabVSize);
					//	Debug.DrawLine(transform.position, transform.position + debugRay, Color.yellow, 1f);
						if (debugRay.magnitude < 3f && dashUp == 1)
							return (emergencyDash(debugRay));
						ret += debugRay;
					}
				}
				else
				{
					Vector3 debugRay = transform.position - new Vector3((i + x) * DataStorage.tabHSize,0 , (j + z) * DataStorage.tabVSize);
				//	Debug.DrawLine(transform.position, transform.position + debugRay, Color.yellow, 1f);
					ret += debugRay;
				}
				x++;
			}
			z++;
		}
		return (ret.normalized);
	}


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
				//		print("dashstop1");
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
		//		print("dashstop2");
				SetMainColor();
			}
		}
	}

	void OnTriggerStay(Collider coll)
	{
		if (coll.gameObject.tag == "boomerang")
		{
			// DiscController collScript = coll.gameObject.GetComponent< DiscController >();
			// if (collScript.GetPlayerNumber() != playerNumber)
				evade = 1;
			//print("ESCAAAAAAAAAAAAAAAAAAAPE");
		}
		else if (coll.gameObject.tag == "Player")
		{
			Vector3 dir = (coll.transform.position - transform.position);
		//	Debug.Log((coll.transform.position - transform.position).magnitude);
			if (dir.magnitude < 4.5f && dashUp == 1)
			{
				emergencyDash(-dir);
				nextFireBoomTimer = UnityEngine.Random.Range(0.05f, 2f);
				fireBoomTimer = 0f;
				fireBoom = 1;
			}
		}
		//	Debug.Log(coll.gameObject.tag);
		
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
	//	Gizmos.DrawWireSphere(transform.position, DataStorage.tabVSize * limit);
	}
}
