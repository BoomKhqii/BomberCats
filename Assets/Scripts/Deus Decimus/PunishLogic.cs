using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PunishLogic : MonoBehaviour
{
    //private float playerTargetRadius = 20f;
    private float pullRadius = 4.5f; // not used
    //private float killRadius = 0.1f;
    private float speed = 4.5f;
    private Vector3 playerVelocity;
    private float duration = 3f;
    private float originSpeed;

    private CharacterController controller;

    public GameObject deusDecimus;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    private Transform target;
    private Vector3 currentDirection;
    private Vector3 moveTarget;
    private bool isMoving = false;

    private float levelPunish = 0;
    //public GameObject deusDecimus;

    // A-Star AI
    private Seeker seeker;
    private Path path;
    private Path playerPath;
    public Transform targetPlayer = null;
    private bool reachDestination = false;

    private int currentWaypoint = 0;
    public float nextWaypointDistance = 0.5f;

    private Vector3 previousTargetPosition;

    // Find Target
    public LayerMask findingPlayers;
    float playerDistance = Mathf.Infinity;

    public bool hasTarget = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        seeker = GetComponent<Seeker>();
        if (seeker == null)
        {
            Debug.LogError("Seeker component is missing!");
        } else Debug.Log("Seeker component found!");

        FindTarget();

        GeneralPlayerController skill = deusDecimus.GetComponent<GeneralPlayerController>(); // Accessing the skill upgrade
        levelPunish += skill.signatureSkill;

        Upgrade(levelPunish);

        //TargetChangedPosition(FindTarget());

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

    void Update()
    {
        if (path != null && !reachDestination)
        {
            if (currentWaypoint < path.vectorPath.Count)
            {
                Vector2 nextWaypoint2D = new Vector2(path.vectorPath[currentWaypoint].x, path.vectorPath[currentWaypoint].z);
                Vector2 playerPosition2D = new Vector2(transform.position.x, transform.position.z);
                Vector2 direction2D = (nextWaypoint2D - playerPosition2D).normalized;

                if (direction2D != Vector2.zero)
                {
                    float angle = Mathf.Atan2(direction2D.x, direction2D.y) * Mathf.Rad2Deg;
                    float snappedAngle = Mathf.Round(angle / 90f) * 90f;
                    Vector3 moveDir = Quaternion.Euler(0, snappedAngle, 0) * Vector3.forward;
                    controller.Move(moveDir * Time.deltaTime * speed);
                    transform.forward = moveDir;
                }

                // Apply gravity
                playerVelocity.y += -9.81f * Time.deltaTime;
                controller.Move(playerVelocity * Time.deltaTime);

                if (Vector2.Distance(playerPosition2D, nextWaypoint2D) < .1f)
                    currentWaypoint++;

                /*
                // just a parameter to stop the pathing
                if (Vector2.Distance
                    (
                        playerPosition2D,
                        new Vector2(path.vectorPath[path.vectorPath.Count - 1].x, path.vectorPath[path.vectorPath.Count - 1].z
                    )) < 0.01f)
                {
                    Debug.Log("Destination Reached!");
                    reachDestination = true;
                    path = null;

                }
                */
            }
        }

    }

    public bool FindTarget()
    {
        Debug.Log("Finding Target...");


        Collider[] hits = Physics.OverlapSphere(transform.position, 30f, findingPlayers);
        HashSet<GameObject> modeDup = new HashSet<GameObject>();
        foreach (Collider hit in hits)
        {
            Debug.Log(hit.gameObject.name);

            if (!modeDup.Contains(hit.gameObject))
                modeDup.Add(hit.gameObject);

            if (hit.gameObject == deusDecimus) continue;

            Transform temp = hit.transform;

            /*
            Debug.Log("Targetplayer: " + temp != null);
            Debug.Log("seeker: " + seeker != null);
            */

            if (seeker != null)
            {
                /*
                Debug.Log("Entered targetPlayer != null && seeker != null");
                playerPath = seeker.StartPath(transform.position, temp.position, OnPathComplete);
                Debug.Log("created path");
                */

                //if (playerPath == null)
                if(seeker.StartPath(transform.position, temp.position, OnPathComplete) == null)
                    Debug.LogWarning("Player Path returned null!");
                else
                {
                    float distance = Vector3.Distance(transform.position, temp.position);
                    if (distance < playerDistance)
                    {
                        playerDistance = distance;
                        hasTarget = true;
                        gameObject.GetComponent<MeshRenderer>().enabled = true;
                        targetPlayer = hit.transform;
                    }
                }
            }
            else
            {
                Debug.Log("failed to enter ");
                /*
                Debug.Log("Targetplayer: " + targetPlayer != null);
                Debug.Log("seeker: " + seeker != null);
                */
            }
        }

        //Debug.Log(hasTarget + " " + modeDup.Count + " " + targetPlayer.name);

        if(!hasTarget) { return false; }
        return hasTarget;
    }

    private void TargetChangedPosition()
    {
        if(!hasTarget) return; // No target found, exit early

        if (Vector3.Distance(targetPlayer.position, previousTargetPosition) > 1f) // Threshold for movement
        {
            AstarPath.active.Scan();
            seeker.StartPath(transform.position, targetPlayer.position, OnPathComplete);
            previousTargetPosition = targetPlayer.position;
        }
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f, playerLayer);

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

    void OnPathComplete(Path p)
    {
        if (p.error)
        {
            Debug.LogError($"Pathfinding error: {p.errorLog}");
        }
        else
        {
            path = p;
            currentWaypoint = 0;
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
