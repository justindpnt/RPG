
using UnityEngine;
using System.Collections;

public class TestInputScript : MonoBehaviour
{

	public Transform cam;

	public float walkSpeed = 2;
	public float runSpeed = 6;

	public float turnSmoothTime = 0.2f;
	float turnSmoothVelocity;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;

	Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
	}

	void FixedUpdate()
	{

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

		if (direction != Vector3.zero)
		{
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);

			//What moves the xz
			Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

			float running = Input.GetAxis("Sprint");
			float targetSpeed = ((running > 0) ? runSpeed : walkSpeed) * moveDir.magnitude;
			currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

			transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

			float animationSpeedPercent = ((running > 0) ? 4 : 2) * direction.magnitude;

			animator.SetFloat("forwardSpeed", animationSpeedPercent);
			//animator.SetFloat("forwardSpeed", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
		}




	}
}