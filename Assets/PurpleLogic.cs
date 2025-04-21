using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PurpleLogic : MonoBehaviour
{
    public float purpleRadius = 0.4f;
    public LayerMask affectedLayers;

    public Transform objectPurple;
    public GameObject ottoGojo;
    private OttoGojoController buttonOutput;

    //public float skillIncrement = 0;

    public Vector3 direction; // Direction to move in
    private float speed = 30f;                  // Movement speed
    private float moveDistance = 20;            // How far to move
    private float duration = 5f;
    private float levelPurple = 0;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = true;

    public bool held;


    private void Start()
    {
        buttonOutput = ottoGojo.GetComponent<OttoGojoController>();

        PlayerController skill = ottoGojo.GetComponent<PlayerController>(); // Accessing the skill upgrade

        HeldUpdate(buttonOutput.isHoldingHollowPurple);

        levelPurple += skill.ultimateSkill;

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

    void Update()
    {
        //Debug.Log(buttonOutput.HowLongHeld());
        Debug.Log(levelPurple);

        if (levelPurple >= 0 && buttonOutput.HowLongHeld() < 1.5f)
        {
            //Debug.Log("Level 1");
            objectPurple.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            speed = 30f;                  // Movement speed
            moveDistance = 20;            // How far to move
            duration = 20f;
            purpleRadius = 0.4f;
        }
        else if (levelPurple >= 1 && buttonOutput.HowLongHeld() <= 1.5f)
        {
            //Debug.Log("Level 2");
            objectPurple.localScale = new Vector3(3f, 3f, 3f);
            speed = 10f;                  // Movement speed
            moveDistance = 20;            // How far to move
            duration = 20f;
            purpleRadius = 1.2f;
        }
        else if (levelPurple >= 2 && buttonOutput.HowLongHeld() >= 3f)
        {
            //Debug.Log("Level 3");
            objectPurple.localScale = new Vector3(5f, 5f, 5f);
            speed = 5f;                  // Movement speed
            moveDistance = 20;            // How far to move
            duration = 20f;
            purpleRadius = 2f;
        }

        if (isMoving && !held)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false; // Stop moving once destination is reached
            }
        }
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

            CharacterController cc = col.GetComponent<CharacterController>();
            if (cc != null)
            {
                Destroy(cc.gameObject);
            }

            BombController bb = col.GetComponent<BombController>();
            if (bb != null)
            {
                Destroy(bb.gameObject);
            }
        }
    }

    public bool HeldUpdate(bool update)
    {
        return held = update;
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
