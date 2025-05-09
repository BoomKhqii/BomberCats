using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunoJosJesJuatroLogic : MonoBehaviour
{
    private CharacterController controller;
    private CloneBasicAbility basicAbility;

    private float speed = 4.5f;
    private Vector3 playerVelocity;
    private float changeDirectionTime = 1f;
    private Vector3 moveDirection;
    private float timer;

    [SerializeField]
    private float wallCheckDistance = 0.6f; // how far ahead to check for walls
    public LayerMask everything;

    // upgradable
    private float duration = 10f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        ChooseStraightDirection();
        timer = changeDirectionTime;
        basicAbility = gameObject.GetComponent<CloneBasicAbility>();

        //Destroy(gameObject, duration);
    }

    void Update()
    {
        // Movement
        if (moveDirection != Vector3.zero)
        {
            // Determine the angle of the movement input
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

            // Round angle to nearest 90 degrees (0, 90, 180, 270)
            float snappedAngle = Mathf.Round(angle / 90f) * 90f;

            // Convert the snapped angle back into a direction vector
            Vector3 moveDir = Quaternion.Euler(0, snappedAngle, 0) * Vector3.forward;

            controller.Move(moveDir * Time.deltaTime * speed);
            transform.forward = moveDir;
        }

        playerVelocity.y += -9.81f * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Action()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.forward, 10f, everything);
        int probabilityChance = randomiser(); // 0 - 100
        bool untilAction = false;
        bool breakable = false, bedrock = false, bomb = false, player = false;

        foreach (RaycastHit hit in hits) // "Breakable"/Crates : "Bedrock"/bedrock : "bomb"/bomb : "Player"/player : boundaries has no tag
        {
            if (hit.collider.CompareTag("Breakable"))
                breakable = true;
            else if (hit.collider.CompareTag("Bedrock"))
                bedrock = true;
            else if (hit.collider.CompareTag("bomb"))
                bomb = true;
            else if (hit.collider.CompareTag("Player"))
                player = true;
        }

        while (!untilAction)
        {
            if (player && probabilityChance < 50)
            {
                // try to kill the player
            }
            else if (breakable && probabilityChance < 80)
            {
                // helps juno break crates
            }
            else
            {
                // run around until it gets an assignment
            }
        }
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
            moveDirection = validDirections[Random.Range(0, validDirections.Count)];
        }
        else
        {
            // If all are blocked, just reverse current direction (or stay still)
            moveDirection = -moveDirection;
            Debug.LogWarning("All directions blocked! Reversing.");
        }
    }

    public int randomiser() // pure randomness
    {
        return Random.Range(0, 100);
    }

    void OnTriggerEnter(Collider other) // bomb ghost thing
    {
        if (other.TryGetComponent(out GhostableBlock ghostBlock))
        {
            ghostBlock.AddGhost(GetComponent<Collider>());
        }
    }
}
