using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlueLogic : MonoBehaviour
{
    [Header("Pull Settings")]
    public float pullStrength = 3.5f;
    public float pullRadius = 3.5f;
    public LayerMask affectedLayers;

    public GameObject ottoGojo;

    //public float skillIncrement = 0;

    public Vector3 direction; // Direction to move in
    private float speed = 2f;                  // Movement speed
    private float moveDistance = 6;            // How far to move
    private float duration = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = true;

    public LayerMask ignoreLayers;

    private float level = 0;


    private void Start()
    {
        GeneralPlayerController skill = ottoGojo.GetComponent<GeneralPlayerController>(); // Accessing the skill upgrade
        level += skill.signatureSkill;
        Upgrade(level);

        direction.Normalize(); // Always normalize to ensure consistent distance
        startPosition = transform.position;
        targetPosition = startPosition + direction * moveDistance;
        Destroy(gameObject, duration); // optional: auto-destroy after 3 seconds
    }

    public void Upgrade(float level)
    {
        if (level < 2)     // 1
            return;
        else if (level < 3)     // 2
        {
            pullStrength = 4f;
            pullRadius = 3.5f;
            speed = 3f;                  // Movement speed
            duration = 5f;
        }
        else if (level < 4)     // 3
        {
            pullStrength = 4f;
            pullRadius = 4f;
            speed = 5f;                  // Movement speed
            duration = 7f;
        }
        else                    // 4 +
        {
            pullStrength = 5f;
            pullRadius = 5f;
            speed = 10f;                  // Movement speed
            duration = 15f;
        }
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius, affectedLayers);

        foreach (Collider col in colliders)
        {
            if (col.gameObject == ottoGojo) continue; // Wont pull the caster

            // Pull bomb
            /*
            BombController bb = col.GetComponent<BombController>();
            if (bb != null)
            {
                Destroy(bb.gameObject);
                Destroy(gameObject);
            }
            */
            Rigidbody rb = col.attachedRigidbody;
            if (rb != null && !rb.isKinematic)
            {
                Vector3 direction = (transform.position - rb.position).normalized;
                rb.AddForce(direction * pullStrength, ForceMode.VelocityChange);
            }

            // Pull CharacterController objects manually
            CharacterController cc = col.GetComponent<CharacterController>();
            if (cc != null)
            {
                Vector3 direction = (transform.position - col.transform.position).normalized;
                cc.Move(direction * pullStrength * Time.fixedDeltaTime);
            }

            CrateLogic crate = col.gameObject.GetComponent<CrateLogic>();
            if (col.gameObject.CompareTag("Breakable"))
            {
                crate.CrateDrop();
            }

        }
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false; // Stop moving once destination is reached
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isMoving) // this is not the issue
        {
            if (((1 << collision.gameObject.layer) & ignoreLayers) != 0)
                return;

            isMoving = false;
        }
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
