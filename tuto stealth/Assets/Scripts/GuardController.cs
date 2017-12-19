using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour {

	public static event System.Action OnGuardAsSpottedPlayer;

	public float speed = 5f;	
	float turnSpeed = 180f;
	public float waitTime = 1f;

	public float timeToSpotPLayer = 0.5f;

	public Light spotlight;
	public float viewDistance ;
	float viewAngle;

	float playerVisibleTimer;

	public Transform pathHolder;

	Transform player;
	Color originalSpotColor;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		viewAngle = spotlight.spotAngle;
		originalSpotColor = spotlight.color;
	
		Vector3[] waypoints = new Vector3[pathHolder.childCount];

		print(pathHolder.childCount);
		for (int i = 0; i < waypoints.Length; i++)
		{
			waypoints[i] = pathHolder.GetChild(i).position;
		}
		transform.position = waypoints[0];
		transform.LookAt(waypoints[1]);
		StartCoroutine(FollowPath(waypoints));
	}

	bool PlayerSpotted()
	{
		RaycastHit hitinfo;
		Vector3 toPlayer = new Vector3(player.position.x - transform.position.x, player.position.y - transform.position.y, player.position.z - transform.position.z);
		
		if (toPlayer.magnitude < viewDistance)
		{
			Debug.DrawRay(transform.position, toPlayer, Color.red);
		//	print(Vector3.Angle(transform.forward, toPlayer));
			if (Vector3.Angle(transform.forward, toPlayer) < viewAngle / 2)
			{
				print("player in cone");
				if (Physics.Raycast(transform.position, toPlayer, out hitinfo, toPlayer.magnitude))
				{
					print(hitinfo.transform.tag);
					if (hitinfo.transform.tag == "Player")
						return (true);
				}
			}
		}
		return(false);
	}

	void Update()
	{
		if (PlayerSpotted())
		{
			playerVisibleTimer += Time.deltaTime;
		}
			// spotlight.color = Color.red;
		else
		{
			playerVisibleTimer -= Time.deltaTime;
		}
		playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPLayer);
		spotlight.color = Color.Lerp(originalSpotColor, Color.red, playerVisibleTimer / timeToSpotPLayer);
		if (playerVisibleTimer >= timeToSpotPLayer)
		{
			if (OnGuardAsSpottedPlayer != null){
				OnGuardAsSpottedPlayer();
			}
		}
			//spotlight.color = originalSpotColor;
	}

	IEnumerator FollowPath(Vector3[] waypoints)
	{
		int nextWaypoint = 1;
		Vector3 nextWaypointPosition = waypoints[nextWaypoint];
		while (true)
		{
			// print(nextWaypoint);
			transform.position = Vector3.MoveTowards(transform.position, nextWaypointPosition, speed * Time.deltaTime);
			if (transform.position == nextWaypointPosition)
			{
				nextWaypoint = (nextWaypoint + 1) % waypoints.Length;
				nextWaypointPosition = waypoints[nextWaypoint];
				yield return new WaitForSeconds(waitTime);
				yield return StartCoroutine(TurnToNextWaypoint(nextWaypointPosition));
			}
			yield return null;
		}
	}

	IEnumerator TurnToNextWaypoint(Vector3 nextWaypointPosition)
	{
		Vector3 dirToNextWP = (nextWaypointPosition - transform.position).normalized;
		float angleToTurn = Vector3.Angle(transform.forward, dirToNextWP);
		if (Vector3.Angle(transform.right, dirToNextWP) > 90)
			angleToTurn = - angleToTurn;
		float angleToNextWP = transform.eulerAngles.y + angleToTurn;
		// print(angleToTurn);
		// print(transform.eulerAngles.y);
		// print(angleToNextWP);
		while (Vector3.Angle(transform.forward, dirToNextWP) > 0.05f)
		{
			//print(Vector3.Angle(transform.forward, dirToNextWP));
			//Debug.DrawLine(transform.position + Vector3.up, transform.forward, Color.red, 0.1f);
			//Debug.DrawLine(transform.position + Vector3.up, transform.forward, Color.red, 0.1f);
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y , angleToNextWP, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}
	}

	void OnDrawGizmos()
	{
		Vector3 startPosition = pathHolder.GetChild (0).position;
		Vector3 previousPosition = startPosition;
		foreach(Transform waypoint in pathHolder)
		{
			Gizmos.DrawSphere(waypoint.position, .3f);
			Gizmos.DrawLine(previousPosition, waypoint.position);
			previousPosition = waypoint.position;
		}
		Gizmos.DrawLine(previousPosition, startPosition);

		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
	}
}
