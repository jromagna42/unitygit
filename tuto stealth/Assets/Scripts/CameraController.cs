using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject	limitQuad;

	public Rect		worldLimit;
	public Rect		deadZone;
	public float	deadZoneSize = 3;

	public float 	cameraDist = 15;
	Transform		playerTransform;
	Vector3 currentVelocity = new Vector3(0, 0, 0);
	public float smoothTime = 0.5f;

	bool	followPlayer = false;
	float camHalf;
	Vector3 camCorrection;

	void Start()
	{
		Transform limitQuadTransform = limitQuad.GetComponent<Transform>();
		playerTransform = Object.FindObjectOfType<PLayerController>().transform;

		worldLimit.xMin = limitQuadTransform.position.x - limitQuadTransform.localScale.x / 2;
		worldLimit.xMax = limitQuadTransform.position.x + limitQuadTransform.localScale.x / 2;
		worldLimit.yMin = limitQuadTransform.position.z - limitQuadTransform.localScale.y / 2;
		worldLimit.yMax = limitQuadTransform.position.z + limitQuadTransform.localScale.y / 2;
		 camHalf = GetComponent<Camera>().orthographicSize;

	}
	
	bool PlayerOutOfDeadzone()
	{
		if (playerTransform && !deadZone.Contains(playerTransform.position))
			return (true);
		return (false);
	}

	bool CheckFollowCondition()
	{
		if (playerTransform && ((playerTransform.position.x < transform.position.x + 0.5) &&
		(playerTransform.position.x > transform.position.x - 0.5) &&
		(playerTransform.position.z < transform.position.z + 0.5) &&
		(playerTransform.position.z > transform.position.z - 0.5)))
			return (true);
		return (false);
	}

	void RectUpdate()
	{
		deadZone.xMax = transform.position.x + deadZoneSize;
		deadZone.xMin = transform.position.x - deadZoneSize;
		deadZone.yMax = transform.position.z + deadZoneSize;
		deadZone.yMin = transform.position.z - deadZoneSize;
	}

	
	void LateUpdate () 
	{
		RectUpdate();
		if (PlayerOutOfDeadzone())
		{
			followPlayer = true;
		}
		
		if (followPlayer)
		{
			Vector3 moveTo;
			if (playerTransform)
			{
				moveTo = Vector3.SmoothDamp(transform.position, playerTransform.position, ref(currentVelocity), smoothTime);
				// Debug.Log(moveTo);

				moveTo.x = ((moveTo - (Vector3.right * camHalf / 9 * 16)).x - worldLimit.xMin < 0.001)? transform.position.x : moveTo.x;
				moveTo.x = (worldLimit.xMax - (moveTo + (Vector3.right * camHalf / 9 * 16)).x < 0.001)? transform.position.x : moveTo.x;
				moveTo.z = ((moveTo - (Vector3.up * camHalf)).z - worldLimit.yMin < 0.001)? transform.position.z : moveTo.z;
				moveTo.z = (worldLimit.yMax - (moveTo + (Vector3.up * camHalf)).z < 0.001)? transform.position.z : moveTo.z;
				
				moveTo.y = cameraDist;
				transform.position = moveTo;
				if (CheckFollowCondition())
				{
					currentVelocity = Vector3.zero;
					followPlayer = false;
				}
			}
		}
	}

}
