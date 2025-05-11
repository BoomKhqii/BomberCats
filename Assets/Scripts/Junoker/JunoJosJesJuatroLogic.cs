using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;
using Pathfinding;
using System;

public class JunoJosJesJuatroLogic : MonoBehaviour
{
    private CharacterController controller;
    private CloneBasicAbility basicAbility;

    private float speed = 10f;
    private Vector3 playerVelocity;
    private float changeDirectionTime = 1f;
    private Vector3 moveDirection;
    private float timer;

    [SerializeField]
    private float wallCheckDistance = 0.6f; // how far ahead to check for walls
    public LayerMask everything;

    // upgradable
    private float duration = 10f;

    // modes
    bool isNeutral = true, isPassive = false, isAggro = false;
    public GameObject clone;
    public LayerMask obstacles;
    public LayerMask modes;
    // passive
    float playerDistance = Mathf.Infinity; // looking for the closest player
    float crateDistance = Mathf.Infinity; // closest crate
    // aggro
    private Vector3 previousTargetPosition;

    // A*
    private Seeker seeker;
    private Path path;
    public Transform targetPlayer = null;
    public Transform targetCrate = null;

    private int currentWaypoint = 0;
    public float nextWaypointDistance = 0.5f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        ChooseStraightDirection();
        timer = changeDirectionTime;
        basicAbility = gameObject.GetComponent<CloneBasicAbility>();

        Neutral(isNeutral);

        // A*
        seeker = GetComponent<Seeker>();
        if (seeker == null)
        {
            Debug.LogError("Seeker component is missing!");
        }

        //Destroy(gameObject, duration);
    }

    void Neutral(bool isActive)
    {
        if (!isActive) return;

        Debug.Log("NI");

        //if (Physics.Raycast(transform.position, moveDirection, 0.6f, obstacles)) ChooseStraightDirection();
        // RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.forward, 15f, modes);

        try {
            Collider[] hits = Physics.OverlapSphere(transform.position, 30f, modes);
            HashSet<GameObject> modeDup = new HashSet<GameObject>();
            foreach (Collider hit in hits)
            {
                if (!modeDup.Contains(hit.gameObject))
                    modeDup.Add(hit.gameObject);

                if (hit.gameObject == clone) continue;

                if (hit.CompareTag("Player"))
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (distance < playerDistance)
                    {
                        playerDistance = distance;
                        targetPlayer = hit.transform;
                    }
                }
                else if (hit.CompareTag("Breakable"))
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (distance < crateDistance)
                    {
                        crateDistance = distance;
                        targetCrate = hit.transform;
                    }
                }
            }

            prioritize();
        }
        catch (Exception ex)
        {
            Debug.LogError("Neutral() exception: " + ex.Message + "\n" + ex.StackTrace);
        }
    }

    void prioritize()
    {
        // need to shorten this
        // Now prioritize target selection
        if (targetPlayer != null)
        {
            if (seeker != null && targetPlayer != null)
            {
                path = seeker.StartPath(transform.position, targetPlayer.position, OnPathComplete);
                previousTargetPosition = targetPlayer.position;
            }
            else
            {
                Debug.LogWarning("Seeker or targetPlayer is null in prioritize().");
            }

            if (path == null)
            {
                Debug.LogWarning("Path returned null!");
            }

            if (path != null && !path.error)
            {
                SwitchMode(0);  // Attack Mode
            }
            else if (targetCrate != null)
            {
                Debug.Log("in 1");
                SwitchMode(1);  // Passive Mode (Go to crate)
            }
            else
            {
                SwitchMode(2);  // Neutral Mode
            }
        }
        else if (targetCrate != null)
        {
            SwitchMode(1);  // Passive Mode (Go to crate)
        }
        else
        {
            SwitchMode(2);  // Neutral Mode
        }
    }

    void Aggressive(bool isActive)
    {
        if (!isActive) return;
        
        if (Vector3.Distance(targetPlayer.position, previousTargetPosition) > 1f) // Threshold for movement
        {
            seeker.StartPath(transform.position, targetPlayer.position, OnPathComplete);
            previousTargetPosition = targetPlayer.position;
        }
        
        Debug.Log("ag");
    }
    void Passive(bool isActive)
    {
        if (!isActive) return;

        Debug.Log("pas");
    }

    void SwitchMode(int modeType)
    {
        isNeutral = false;
        switch (modeType)
        {
            case 0:
                isAggro = true ; break;
            case 1:
                isPassive = true ; break;
            case 2:
                isNeutral = true; break;
        }

        Debug.Log("Agg: " + isAggro + " pass: " + isPassive + " neutr: " + isNeutral + " mode type: " + modeType);
    }

    void Update()
    {
        if (path != null)
        {
            if (currentWaypoint < path.vectorPath.Count)
            {
                Debug.Log(path.vectorPath[currentWaypoint] + " " + currentWaypoint);

                Vector3 nextWaypoint = path.vectorPath[currentWaypoint];
                Vector2 nextWaypoint2D = new Vector2(nextWaypoint.x, nextWaypoint.z);
                nextWaypoint.y = 1.38f; // Lock Y position to prevent vertical drift

                Vector3 playerPosition = transform.position;
                Vector2 playerPosition2D = new Vector2(playerPosition.x, playerPosition.z);

                Vector2 direction2D = (nextWaypoint2D - playerPosition2D).normalized;
                
                //Vector3 direction = (nextWaypoint - playerPosition).normalized;
                //Vector2 direction2D = new Vector2(direction.x, direction.z); // Convert to 2D
                
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
                {
                    currentWaypoint++;
                }
            }
        }

        // Modes
        Neutral(isNeutral);
        Aggressive(isAggro);
        Passive(isPassive);
}

    void ChooseStraightDirection()
    {
        List<Vector3> validDirections = new List<Vector3>();

        // Define all 4 cardinal directions
        Vector3[] directions = new Vector3[]
        {
        Vector3.forward,  // Z+
        Vector3.back,     // Z-
        Vector3.left,     // X-
        Vector3.right     // X+
        };

        // Offset origin slightly forward from character to avoid self-hit
        Vector3 origin = transform.position + Vector3.up * 0.5f; // Slightly above ground

        // Check each direction
        foreach (var dir in directions)
        {
            Vector2 rayStart = origin + dir.normalized * 0.6f; // small offset in that direction
            Debug.DrawRay(rayStart, dir.normalized * 1f, Color.cyan, 1f);

            if (!Physics.Raycast(rayStart, dir, 1f, everything))
            {
                validDirections.Add(dir);
            }
        }

        if (validDirections.Count > 0)
        {
            moveDirection = validDirections[UnityEngine.Random.Range(0, validDirections.Count)];
        }
        else
        {
            // If all are blocked, just reverse current direction (or stay still)
            moveDirection = -moveDirection;
            Debug.LogWarning("All directions blocked! Reversing.");
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
            Debug.Log($"Path successfully created! Waypoints: {p.vectorPath.Count}");
            path = p;
            currentWaypoint = 0;
        }
    }

    void OnTriggerEnter(Collider other) // bomb ghost thing
    {
        if (other.TryGetComponent(out GhostableBlock ghostBlock))
        {
            ghostBlock.AddGhost(GetComponent<Collider>());
        }
    }
}
