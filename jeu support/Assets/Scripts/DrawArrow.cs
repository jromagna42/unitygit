using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DrawArrow {

	static public float arrowHeadAngle = 20f;
	static public void Debug_arrow(Vector3 start, Vector3 end, Color color, float duration)
	{
		Debug.DrawLine(start, end, color, duration);
		Vector3 left = Quaternion.LookRotation(end - start) * Quaternion.Euler(0, 180+ arrowHeadAngle , 0) * new Vector3(0,0,1);
		Vector3 right = Quaternion.LookRotation(end - start) * Quaternion.Euler(0, 180- arrowHeadAngle , 0) * new Vector3(0,0,1);
		Debug.DrawLine(end, end + left, color, duration);
		Debug.DrawLine(end, end + right, color, duration);
	}
}
