using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleLogic : MonoBehaviour
{
    public float purpleRadius = 0.5f;
    public LayerMask affectedLayers;

    public GameObject ottoGojo;

    //public float skillIncrement = 0;

    public Vector3 direction; // Direction to move in
    private float speed = 2f;                  // Movement speed
    private float moveDistance = 2;            // How far to move
    private float duration = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = true;


    private void Start()
    {
        PlayerController skill = ottoGojo.GetComponent<PlayerController>(); // Accessing the skill upgrade
        speed += skill.ultimateSkill;
        duration += skill.ultimateSkill;
        moveDistance += skill.ultimateSkill;

        direction.Normalize(); // Always normalize to ensure consistent distance
        startPosition = transform.position;
        targetPosition = startPosition + direction * moveDistance;
        Destroy(gameObject, duration); // optional: auto-destroy after 3 seconds
    }

    public void SkillUpdate(float increment)
    {
        if (increment == 0)
            return;

        speed = speed + increment;
        moveDistance = moveDistance + increment;
        duration = duration + increment;
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, purpleRadius, affectedLayers);

        foreach (Collider col in colliders)
        {
            if (col.gameObject == ottoGojo) continue; // Wont pull the caster

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

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, purpleRadius);
    }
}
