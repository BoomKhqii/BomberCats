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
    public float wallCheckDistance = 0.6f; // how far ahead to check for walls

    // upgradable
    private float duration = 10f;

    // Clone spawns Clone
    public GameObject junosJoCloneObject;
    public Transform cloneLocation;
    private CurseEnergyLogic curseEnergy;
    //public string ceName;
    public bool isPlayerSignatureActive = false;
    private float playerSignatureCooldown = 3f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        ChooseStraightDirection();
        timer = changeDirectionTime;
        basicAbility = gameObject.GetComponent<CloneBasicAbility>();

        curseEnergy = GameObject.Find("CE Pool of Junoker").GetComponent<CurseEnergyLogic>();

        Destroy(gameObject, duration);
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
            basicAbility.SpawnBomb(0);
        }

        // Cooldown
        if (!isPlayerSignatureActive)
        {
            playerSignatureCooldown -= Time.deltaTime;
            if (playerSignatureCooldown <= 0f)
            {
                isPlayerSignatureActive = true;
                playerSignatureCooldown = 3f;
            }
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
