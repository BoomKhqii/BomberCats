using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HookLogic : MonoBehaviour
{
    public LayerMask affectedLayer;

    public Vector3 direction;
    private float forwardSpeed = 10f;
    private float returnSpeed = 20f;
    private float moveDistance = 5f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = true;
    private bool isReturning = false;

    void Start()
    {
        direction.Normalize();
        startPosition = transform.position;
        targetPosition = startPosition + direction * moveDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving) return;

        float currentSpeed = isReturning ? returnSpeed : forwardSpeed;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            if (!isReturning)
            {
                // Start returning
                targetPosition = startPosition;
                isReturning = true;
            }
            else
            {
                // Done moving
                isMoving = false;
                Destroy(gameObject);
            }
        }


    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }
}
