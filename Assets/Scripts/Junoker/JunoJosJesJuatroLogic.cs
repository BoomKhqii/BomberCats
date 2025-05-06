using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunoJosJesJuatroLogic : MonoBehaviour
{
    private CharacterController controller;
    private CloneBasicAbility basicAbility;

    private float speed = 4.5f;
    private Vector3 playerVelocity;
    private float changeDirectionTime = 2f;
    private Vector3 moveDirection;
    private float timer;

    [SerializeField]
    private float wallCheckDistance = 0.6f; // how far ahead to check for walls
    public LayerMask bedrockLayer;
    public LayerMask bombThere;

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
        /*
        // Check if a breakable,unbreakble
        if (Physics.Raycast(transform.position, moveDirection, wallCheckDistance, bedrockLayer))
        {

            ChooseStraightDirection();
            //timer = changeDirectionTime; // comment so clone will spawn more bombs often
            return;
        }

        if (Physics.Raycast(transform.position, moveDirection, 6f, bombThere))
        {
            Debug.DrawRay(transform.position, moveDirection * wallCheckDistance, Color.red);

            ChooseStraightDirection();
            //timer = changeDirectionTime; // comment so clone will spawn more bombs often
            return;
        }

        // Movement shi
        transform.Translate(moveDirection * Time.deltaTime * speed);

        // timer x < 0 : change direction
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ChooseStraightDirection();
            timer = changeDirectionTime;
            basicAbility.SpawnBomb(0);
        }
        */
        RaycastHit hit;
        //Vector3 rayOrigin = transform.position + moveDirection.normalized * 0.6f;

        if (Physics.Raycast(transform.position, moveDirection, out hit, wallCheckDistance, bedrockLayer))
        {
            Debug.DrawRay(transform.position, moveDirection * wallCheckDistance, Color.red);
            Debug.Log("Raycast hit: " + hit.collider.name + " | Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));

            ChooseStraightDirection();
            return;
        }
        else
        {
            Debug.Log("Raycast missed");
        }

        // Movement
        if (moveDirection != Vector3.zero)
        {
            // Determine the angle of the movement input
            //float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
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

            if (!Physics.Raycast(rayStart, dir, 1f, bedrockLayer))
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
    Vector3 RoundToGrid(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
    }
}
