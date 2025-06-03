using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private bool isHooked = false;
    private Transform hookedTarget;

    public GameObject lunaObject;
    public PlayerInput casterStun;

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

        // Move the hooked target along with the hook
        if (isHooked && hookedTarget != null)
        {
            hookedTarget.position = transform.position;
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            if (!isReturning)
            {
                targetPosition = startPosition;
                isReturning = true;
            }
            else
            {
                if (hookedTarget == null) return;

                GeneralPlayerController enemy = hookedTarget.GetComponent<GeneralPlayerController>();
                enemy.PlayerStun(0.5f);
                isMoving = false;
                Destroy(gameObject);
            }
        }
        /*
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
        */
    }

    public void Upgrade(float level)
    {
        if (level < 2)     // 1
            return;
        else if (level < 3)     // 2
        {
            
        }
        else if (level < 4)     // 3
        {
            
        }
        else if (level < 5)     // 4
        {

        }
        else                    // 5
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHooked || isReturning || other.gameObject != lunaObject) return;

        if (((1 << other.gameObject.layer) & affectedLayer) != 0)
        {
            isHooked = true;
            hookedTarget = other.transform;

            // Start returning immediately
            targetPosition = startPosition;
            isReturning = true;
        }
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }
}
