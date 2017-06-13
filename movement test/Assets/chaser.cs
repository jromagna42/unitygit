using UnityEngine;
using System.Collections;

public class chaser : MonoBehaviour {


	public Transform targetTransform;
	public	float speed = 7;
	void Update () {
		Vector3 displacementFromTarget = targetTransform.position - transform.position;
		Vector3 directrionToTarget = displacementFromTarget.normalized;
		Vector3 velocity = directrionToTarget * speed;

		float distanceToTarget = displacementFromTarget.magnitude;
		if (distanceToTarget > 1.5f){
			transform.Translate(velocity * Time.deltaTime);
		}
	}
}
