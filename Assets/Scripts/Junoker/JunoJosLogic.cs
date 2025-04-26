using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JunoJosLogic : MonoBehaviour
{
    private CloneBasicAbility basicAbility;

    private float speed = 4.5f;
    private float changeDirectionTime = 2f;
    private Vector3 moveDirection;
    private float timer;
    public LayerMask bedrockLayer;
    public float wallCheckDistance = 0.6f; // how far ahead to check for walls

    // upgradable
    private float duration = 5f;

    // Level for signature
    private float levelJunoJos = 0;
    public GameObject junoker;

    void Start()
    {
        ChooseStraightDirection();
        timer = changeDirectionTime;
        basicAbility = gameObject.GetComponent<CloneBasicAbility>();

        // level
        GeneralPlayerController skill = junoker.gameObject.GetComponent<GeneralPlayerController>(); // Accessing the skill upgrade
        levelJunoJos += skill.signatureSkill;
        Upgrade(levelJunoJos);

        Destroy(gameObject, duration);
    }

    public void Upgrade(float level)
    {
        Debug.Log(level);
        if (level < 1)           // 0
            return;
        else if (level < 2)     // 1
            duration = 7f;
        else                    // 2 +
            duration = 10f;
    }

    void Update()
    {
        // Check if a breakable,unbreakble, bomb is ahead
        if (Physics.Raycast(transform.position, moveDirection, wallCheckDistance, bedrockLayer))
        {
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
            basicAbility.SpawnBomb(50);
        }
    }

    void ChooseStraightDirection()
    {
        int direction = Random.Range(0, 4); // 0 = forward, 1 = backward, 2 = left, 3 = right

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
