using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float initialSpeed = 1f;
    private float playerSpeed = 0;
    public float stun = 1;
    public bool isDead = false;
    public bool isMoving = false;

    private Rigidbody rb;

    void Start()
    {
        playerSpeed = initialSpeed;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            rb.velocity += transform.right * Input.GetAxisRaw("Horizontal") * playerSpeed;
            isMoving = true;
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            rb.velocity += transform.forward * Input.GetAxisRaw("Vertical") * playerSpeed;
            isMoving = true;
        }

        if(isDead)
        {
            //play animation
            // wait
            // death
        }

    }
}
