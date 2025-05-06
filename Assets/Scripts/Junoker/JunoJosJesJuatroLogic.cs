using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunoJosJesJuatroLogic : MonoBehaviour
{
    private CharacterController controller;
    private CloneBasicAbility basicAbility;

    private float speed = 4.5f;
    private float changeDirectionTime = 2f;
    private Vector3 moveDirection;
    private float timer;
    public LayerMask bedrockLayer;
    public float wallCheckDistance = 2f; // how far ahead to check for walls

    // upgradable
    private float duration = 10f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        ChooseStraightDirection();
        timer = changeDirectionTime;
        basicAbility = gameObject.GetComponent<CloneBasicAbility>();

        Destroy(gameObject, duration);
    }

    void Update()
    {
        // Check if a breakable,unbreakble, bomb is ahead
        if (Physics.Raycast(transform.position + moveDirection.normalized, moveDirection, wallCheckDistance, bedrockLayer))
        {
            ChooseStraightDirection();
            //Debug.DrawRay(transform.position, moveDirection.normalized * wallCheckDistance, Color.red);
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
    }

    void ChooseStraightDirection()
    {
        int direction = Random.Range(0, 3); // 0 = forward, 1 = backward, 2 = left, 3 = right

        Debug.Log("cal");
        switch (direction)
        {
            case 0:
                moveDirection = Vector3.forward;  // Z+
                break;
            case 1:
                moveDirection = Vector3.back;     // Z-
                break;
            case 2:
                moveDirection = Vector3.left;     // X-
                break;
            case 3:
                moveDirection = Vector3.right;    // X+
                break;
        }
    }
}
