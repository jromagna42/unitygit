using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerController : MonoBehaviour {

	public event System.Action OnReachedEndOfLevel;

	public float MoveSpeed = 7;
	public float smoothMoveTime = .1f;

	float smoothInputMagnitude;
	float smoothMoveVelocity;
	Vector3 velocity;

	float angle;
	float turnSpeed = 8;

	Rigidbody rigidbody;
	bool disabled;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		GuardController.OnGuardAsSpottedPlayer += Disable;
	}
	
	void Update () {
		Vector3 inputDirection = Vector3.zero;
		if (!disabled)
		{
			inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
		}
		float inputMagnitude = inputDirection.magnitude;
		smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);


		float targetAngle = Mathf.Atan2 (inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
		angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);
		transform.eulerAngles = Vector3.up * angle;
		velocity = transform.forward * MoveSpeed * smoothInputMagnitude;
		//transform.Translate(transform.forward * MoveSpeed * Time.deltaTime * smoothInputMagnitude, Space.World);

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Finish")
		{
			Disable();
			if (OnReachedEndOfLevel != null)
			{
				OnReachedEndOfLevel();
			}
		}
	}
	void Disable()
	{
		disabled = true;
	}

	void FixedUpdate()
	{
		rigidbody.MoveRotation (Quaternion.Euler(Vector3.up * angle));
		rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
	}

	void OnDestroy()
	{
		GuardController.OnGuardAsSpottedPlayer -= Disable;
	}
}
