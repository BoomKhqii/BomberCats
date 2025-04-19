using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLogic : MonoBehaviour
{
    // Copy Pasted

    [Header("Push Settings")]
    [SerializeField]
    private float pushStrength = -100f;
    [SerializeField]
    private float pushRadius = 2f;
    [SerializeField]
    private LayerMask affectedLayers;

    public GameObject ottoGojo;

    public Vector3 direction;
    [SerializeField]
    private float speed = 40f;
    [SerializeField]
    private float moveDistance = 5;
    private float duration = 1f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = true;

    private bool didContact = false;


    private void Start()
    {
        PlayerController skill = ottoGojo.GetComponent<PlayerController>();
        speed += skill.heavySkill;
        duration += skill.heavySkill;
        moveDistance += skill.heavySkill;

        direction.Normalize();
        startPosition = transform.position;
        targetPosition = startPosition + direction * moveDistance;
        //Destroy(gameObject, duration);
    }

    public void SkillUpdate(float increment)
    {
        if (increment == 0)
            return;

        speed = speed + increment;
        moveDistance = moveDistance + increment;
        duration = duration + increment;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);

        // Optional: ignore specific objects like the player
        if (collision.gameObject == ottoGojo) return;
        
        Explode();
        Destroy(this.gameObject); // Boom, self-destruct
    }

    void Explode()
    {
        Debug.Log("called 2");

        Collider[] colliders = Physics.OverlapSphere(transform.position, pushRadius, affectedLayers);

        foreach (Collider col in colliders)
        {
            if (col.gameObject == ottoGojo) continue;

            // Push rigidbodies
            Rigidbody rb = col.attachedRigidbody;
            if (rb != null && !rb.isKinematic)
            {
                Vector3 direction = (col.transform.position - transform.position).normalized;
                rb.AddForce(direction * pushStrength, ForceMode.Impulse); // Impulse for explosion burst
            }

            // Move character controllers
            CharacterController cc = col.GetComponent<CharacterController>();
            if (cc != null)
            {
                Vector3 direction = (col.transform.position - transform.position).normalized;
                cc.Move(direction * pushStrength * Time.fixedDeltaTime);
            }

            // Break breakables
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
                isMoving = false;
            }
        }
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pushRadius);
    }
}
