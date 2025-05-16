using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PunishLogic : MonoBehaviour
{
    private float playerTargetRadius = 20f;
    private float pullRadius = 4.5f; // not used
    private float killRadius = 0.1f;
    private float speed = 4.5f;
    private float duration = 3f;
    private float originSpeed;

    public GameObject deusDecimus;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    private Transform target;
    private Vector3 currentDirection;
    private Vector3 moveTarget;
    private bool isMoving = false;

    private float levelPunish = 0;
    //public GameObject deusDecimus;

    private void Start()
    {
        GeneralPlayerController skill = deusDecimus.GetComponent<GeneralPlayerController>(); // Accessing the skill upgrade
        levelPunish += skill.signatureSkill;

        Upgrade(levelPunish);

        Destroy(gameObject, duration);
    }

    public void Upgrade(float level)
    {
        if (level < 2)     // 1
            return;
        else if (level < 3)     // 2
        {
            speed = 5.5f;
            duration = 7f;
        }
        else if (level < 4)     // 3
        {
            speed = 6f;
            duration = 10f;
        }
        else                    // 4 +
        {
            speed = 10f;
            duration = 15f;
        }
    }



    /*
    void Update()
    {
        if (target == null)
        {
            SearchForTarget();
            return;
        }

        if (!isMoving)
        {
            Vector3 roundedPos = RoundToGrid(transform.position);
            Vector3 targetPos = RoundToGrid(target.position);
            transform.position = roundedPos;

            // Recalculate direction if aligned
            if (Mathf.Approximately(roundedPos.x, targetPos.x))
            {
                currentDirection = new Vector3(0, 0, Mathf.Sign(targetPos.z - roundedPos.z));
            }
            else if (Mathf.Approximately(roundedPos.z, targetPos.z))
            {
                currentDirection = new Vector3(Mathf.Sign(targetPos.x - roundedPos.x), 0, 0);
            }

            // Check if the direction is blocked
            if (IsObstacleAhead(roundedPos))
            {
                TryTurn();
            }

            moveTarget = roundedPos + currentDirection;

            // Final obstacle check before moving
            if (!Physics.Raycast(roundedPos + Vector3.up * 0.5f, currentDirection, 1f, obstacleLayer))
            {
                isMoving = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, moveTarget) < 0.01f)
            {
                transform.position = moveTarget;
                isMoving = false;
            }
        }
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, killRadius, playerLayer);

        foreach (Collider col in colliders)
        {
            if (col.gameObject == deusDecimus) continue; // Wont pull the caster

            GeneralPlayerController enemy = col.gameObject.GetComponent<GeneralPlayerController>();
            if (enemy != null)
            {
                enemy.PlayerStun(2f);
                Destroy(gameObject);
            }

            BombController bb = col.GetComponent<BombController>();
            if (bb != null)
            {
                Destroy(bb.gameObject);
                Destroy(gameObject);
            }
        }
    }

    void SearchForTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, playerTargetRadius, playerLayer);

        foreach (Collider hit in hits)
        {
            // Skip the spawner player
            if (hit.gameObject == deusDecimus) continue;

            target = hit.transform;
            ChooseInitialDirection();
            break;
        }
    }

    void ChooseInitialDirection()
    {
        Vector3 diff = target.position - transform.position;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
            currentDirection = new Vector3(Mathf.Sign(diff.x), 0, 0);
        else
            currentDirection = new Vector3(0, 0, Mathf.Sign(diff.z));
    }

    void TryTurn()
    {
        Vector3 alt1 = new Vector3(currentDirection.z, 0, currentDirection.x);
        Vector3 alt2 = -alt1;

        Vector3 pos = RoundToGrid(transform.position);

        if (!Physics.Raycast(pos + Vector3.up * 0.5f, alt1, 1f, obstacleLayer))
            currentDirection = alt1;
        else if (!Physics.Raycast(pos + Vector3.up * 0.5f, alt2, 1f, obstacleLayer))
            currentDirection = alt2;
    }

    bool IsObstacleAhead(Vector3 origin)
    {
        return Physics.Raycast(origin + Vector3.up * 0.5f, currentDirection, 1f, obstacleLayer);
    }

    Vector3 RoundToGrid(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
    */
}
