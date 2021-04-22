using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimensionAnimationStateController : MonoBehaviour
{

    Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;

    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void updateAnimator(float x, float z)
    {
        /*
        if (z > .01f)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        if (z < .01f)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }
        if (x < .01f)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        if (x > .01f)
        {
            velocityX += Time.deltaTime * acceleration;
        }
        */

        animator.SetFloat("Veloctiy Z", x * acceleration);
        animator.SetFloat("Velocity X", z * acceleration);


    }
}
