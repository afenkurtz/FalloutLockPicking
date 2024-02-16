using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// basic WASD-style movement control
// commented out line demonstrates that transform.Translate instead of charController.Move doesn't have collision detection

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
	public float speed = 6.0f;

	public float jumpSpeed = 15.0f;
	public float gravity = -9.8f;
	public float terminalVelocity = -10.0f;
	public float minFall = -1.5f;

	private float vertSpeed;

	private CharacterController charController;

	void Start()
	{
		vertSpeed = minFall;
		charController = GetComponent<CharacterController>();
	}

	void Update()
	{
		//transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, Input.GetAxis("Vertical") * speed * Time.deltaTime);
		float deltaX = Input.GetAxis("Horizontal") * speed;
		float deltaZ = Input.GetAxis("Vertical") * speed;
		Vector3 movement = new Vector3(deltaX, 0, deltaZ);
		movement = Vector3.ClampMagnitude(movement, speed);

		if (charController.isGrounded)
		{
			if (Input.GetButtonDown("Jump"))
				vertSpeed = jumpSpeed;
			else
				vertSpeed = minFall;
		}
		else
		{
			vertSpeed += gravity * 5 * Time.deltaTime;
			if (vertSpeed < terminalVelocity)
				vertSpeed = terminalVelocity;
		}
		movement.y = vertSpeed;

		movement *= Time.deltaTime;
		movement = transform.TransformDirection(movement);
		charController.Move(movement);
	}
}