using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLogic : MonoBehaviour
{
    // Copy Pasted

    [Header("Push Settings")]
    [SerializeField]
    private float pushStrength = 150;
    [SerializeField]
    private float pushRadius = 4.5f;
    [SerializeField]
    private LayerMask affectedLayers;

    public GameObject ottoGojo;

    public Vector3 direction;
    private float speed = 5f; // 40f
    [SerializeField]
    private float moveDistance = 5;
    private float duration = 1f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = true;
    private bool didExplode = false; // this is not the issue

    private void Start()
    {
        GeneralPlayerController skill = ottoGojo.GetComponent<GeneralPlayerController>();
        speed += skill.heavySkill;
        duration += skill.heavySkill;
        moveDistance += skill.heavySkill;

        direction.Normalize();
        startPosition = transform.position;
        targetPosition = startPosition + direction * moveDistance;
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
        if (!didExplode) // this is not the issue
        {
            // Optional: ignore specific objects like the player
            if (collision.gameObject == ottoGojo) return;

            didExplode = true;
            Explode();
            Destroy(gameObject); // Boom, self-destruct
        }
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pushRadius, affectedLayers);
        HashSet<GameObject> stunnedObjects = new HashSet<GameObject>(); // uses hash to avoid double calling

        foreach (Collider col in colliders)
        {
            GameObject obj = col.gameObject;

            if (col.gameObject == ottoGojo) continue;

            // Move character controllers
            if (!stunnedObjects.Contains(obj))
            {
                stunnedObjects.Add(obj);

                CharacterController cc = obj.GetComponent<CharacterController>();
                GeneralPlayerController enemy = obj.GetComponent<GeneralPlayerController>();

                if (cc != null && enemy != null)
                {
                    Debug.Log(cc.gameObject.name);
                    enemy.PlayerStun(1.5f);
                    Vector3 direction = (obj.transform.position - transform.position).normalized;
                    cc.Move(direction * pushStrength * Time.fixedDeltaTime);
                }
            }

            /*
            // Move character controllers
            CharacterController cc = col.GetComponent<CharacterController>();
            GeneralPlayerController enemy = cc.gameObject.GetComponent<GeneralPlayerController>();
            if (cc != null && enemy != null)
            {
                Debug.Log(cc.gameObject.name);
                enemy.PlayerStun(1.5f);
                Vector3 direction = (col.transform.position - transform.position).normalized;
                cc.Move(direction * pushStrength * Time.fixedDeltaTime);
            }
            */

            // Push rigidbodies
            Rigidbody rb = col.attachedRigidbody;
            if (rb != null && !rb.isKinematic)
            {
                Vector3 direction = (col.transform.position - transform.position).normalized;
                rb.AddForce(direction * pushStrength, ForceMode.Impulse); // Impulse for explosion burst
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
        else 
        {
            Explode();
            Destroy(gameObject);
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
