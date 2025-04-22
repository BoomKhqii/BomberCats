using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunoJosLogic : MonoBehaviour
{
    private CharacterController controller;

    private float speed = 4.5f;
    private float changeDirectionTime = 2f;
    private Vector3 moveDirection;
    private float timer;
    public LayerMask bedrockLayer;
    public float wallCheckDistance = 0.6f; // how far ahead to check for walls

    // upgradable
    private float duration = 5;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        ChooseStraightDirection();
        timer = changeDirectionTime;

        Destroy(gameObject, duration);
    }

    void Update()
    {
        // Check if a wall is ahead
        if (Physics.Raycast(transform.position, moveDirection, wallCheckDistance, bedrockLayer))
        {
            ChooseStraightDirection();
            timer = changeDirectionTime;
            return; // skip moving this frame to avoid wall sticking
        }

        // Move clone
        transform.Translate(moveDirection * Time.deltaTime * speed);

        // Timer-based direction change
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ChooseStraightDirection();
            timer = changeDirectionTime;
        }
    }

    void ChooseStraightDirection()
    {
        int direction = Random.Range(0, 4); // 0 = forward, 1 = backward, 2 = left, 3 = right

        switch (direction)
        {
            case 0:
                moveDirection = Vector3.forward;  // Z+
                break;
            case 1:
                moveDirection = Vector3.back;     // Z-
                break;
            case 2:
                moveDirection = Vector3.left;     // X-
                break;
            case 3:
                moveDirection = Vector3.right;    // X+
                break;
        }
    }
}
