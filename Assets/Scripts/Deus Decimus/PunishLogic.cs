using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunishLogic : MonoBehaviour
{
    private float playerTargetRadius = 15f;
    private float speed = 5f;

    public GameObject player;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    private Transform target;
    private Vector3 currentDirection;
    private Vector3 moveTarget;
    private bool isMoving = false;

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

    void SearchForTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, playerTargetRadius, playerLayer);

        foreach (Collider hit in hits)
        {
            // Skip the spawner player
            //Debug.Log(hit.gameObject + " & " + player);
            if (hit.gameObject == player) continue;

            target = hit.transform;
            ChooseInitialDirection();
            break;
        }
        /*
        Collider[] hits = Physics.OverlapSphere(transform.position, playerTargetRadius, playerLayer);

        if (hits.Length > 0)
        {
            //Debug.Log(hits[0].gameObject.name);
            //GameObject cc = hits[0].gameObject;
            //if(hits[0].gameObject == player) { return; }

            target = hits[0].transform;
            ChooseInitialDirection();
        }
        */
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
        Gizmos.DrawWireSphere(transform.position, playerTargetRadius);
    }
}
