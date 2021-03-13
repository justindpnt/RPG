using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;
    public Terrain ground;


    public float speed = 1f;
    public float animationSpeed = 1f;
    public float verticalSpeed = 0f;
    public float gravity = 20f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f) 
        {
            //Vector math to get rotation angle

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //What moves the xz
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Debug.Log(transform.position.y <= .1f);

            //if (transform.position.y <= .1f) verticalSpeed = Terrain.activeTerrain.SampleHeight(transform.position);
            ////verticalSpeed = Terrain.activeTerrain.SampleHeight(transform.position);
            //else verticalSpeed -= gravity * Time.deltaTime;
            ////moveDir.y = verticalSpeed;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            //What move the y (Want to stay grounded)
            Vector3 pos = transform.position;
            pos.y = Terrain.activeTerrain.SampleHeight(pos);
            transform.position = pos;

            //move = transform.InverseTransformDirection(direction)

            UpdateAnimator(moveDir.magnitude);
        }
    }
    private void UpdateAnimator(float animationFloat)
    {
        //Debug.Log(speed.magnitude);

        //Debug.Log(animationFloat * animationSpeed);
        GetComponent<Animator>().SetFloat("forwardSpeed", animationFloat * animationSpeed);
    }
}
