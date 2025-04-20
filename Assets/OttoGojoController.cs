using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OttoGojoController : MonoBehaviour
{
    public CurseEnergyLogic curseEnergy;

    // Blue      
    [SerializeField]
    private GameObject objectBlue;
    private float cooldownBlue = 5;
    private bool isBlueActive = true;
    
    // Red
    [SerializeField]
    private GameObject objectRed;
    private float cooldownRed = 15;
    private bool isRedActive = true;

    // Hollow purple
    [SerializeField]
    private GameObject objectHollowPurple;
    public PurpleLogic purple;
    private float cooldownPurple = 0;
    private bool isPurpleActive = true;

    public bool isHoldingHollowPurple = true;
    private float holdStartTime = 0f;
    private float heldDuration = 0f;

    //[SerializeField]
    public PlayerController player;

    void Start()
    {
        curseEnergy = GameObject.Find("CE Pool of Otto Gojo").GetComponent<CurseEnergyLogic>();
        player = GetComponent<PlayerController>();
        purple = GetComponent<PurpleLogic>();
    }

    public bool InfinityProbabilityChance()
    {
        return UnityEngine.Random.value < 0.35f;
    }

    public void BlueSkill(InputAction.CallbackContext context)
    {
        if (!context.performed || !isBlueActive || !curseEnergy.CEReduction(250)) return;

        //Vector3 spawnPos = new Vector3(Mathf.RoundToInt(player.transform.position.x), 1.32f, Mathf.RoundToInt(player.transform.position.z));
        Vector3 spawnOffset = player.transform.forward.normalized;
        Vector3 spawnPos = new Vector3(
            Mathf.RoundToInt(player.transform.position.x + spawnOffset.x),
            1.32f,
            Mathf.RoundToInt(player.transform.position.z + spawnOffset.z)
        );
        GameObject blue = Instantiate(objectBlue, spawnPos, Quaternion.identity);

        blue.GetComponent<BlueLogic>().ottoGojo = this.gameObject;
        BlueLogic blueLogic = blue.GetComponent<BlueLogic>();
        blueLogic.SkillUpdate(player.signatureSkill);
        blueLogic.SetDirection(player.transform.forward);

        isBlueActive = false;
    }

    public void RedSkill(InputAction.CallbackContext context)
    {
        if (!context.performed || !isRedActive || !curseEnergy.CEReduction(500)) return;

        Vector3 spawnOffset = player.transform.forward.normalized;
        Vector3 spawnPos = new Vector3(
            Mathf.RoundToInt(player.transform.position.x + spawnOffset.x),
            1.32f,
            Mathf.RoundToInt(player.transform.position.z + spawnOffset.z)
        );
        GameObject red = Instantiate(objectRed, spawnPos, Quaternion.identity);

        red.GetComponent<RedLogic>().ottoGojo = this.gameObject;
        RedLogic redLogic = red.GetComponent<RedLogic>();
        redLogic.SkillUpdate(player.signatureSkill);
        redLogic.SetDirection(player.transform.forward);

        isRedActive = false;
    }
    public void HollowPurpleSkill(InputAction.CallbackContext context)
    {
        Debug.Log("Controller: " + isHoldingHollowPurple);
        //IsHeldUpdate(context); // call this immediately to update hold state

        if (context.started && isPurpleActive && curseEnergy.CEReduction(1000))
        {
            holdStartTime = Time.time;
            Debug.Log("Started holding Hollow Purple.");

            Vector3 spawnOffset = player.transform.forward.normalized;
            Vector3 spawnPos = new Vector3(
                Mathf.RoundToInt(player.transform.position.x + spawnOffset.x),
                1.32f,
                Mathf.RoundToInt(player.transform.position.z + spawnOffset.z)
            );

            GameObject purple = Instantiate(objectHollowPurple, spawnPos, Quaternion.identity);

            purple.GetComponent<PurpleLogic>().ottoGojo = this.gameObject;

            PurpleLogic purpleLogic = purple.GetComponent<PurpleLogic>();
            purpleLogic.SkillUpdate(player.signatureSkill);
            purpleLogic.SetDirection(player.transform.forward);
        }

        if (context.canceled)
        {
            heldDuration = Time.time - holdStartTime;
            Debug.Log($"Released Hollow Purple after {heldDuration:F2} seconds.");
            isPurpleActive = false;
        }
        /*
        if (!context.performed || !isPurpleActive || !curseEnergy.CEReduction(1000)) return;

        Vector3 spawnOffset = player.transform.forward.normalized;
        Vector3 spawnPos = new Vector3(
            Mathf.RoundToInt(player.transform.position.x + spawnOffset.x),
            1.32f,
            Mathf.RoundToInt(player.transform.position.z + spawnOffset.z)
        );
        GameObject purple = Instantiate(objectHollowPurple, spawnPos, Quaternion.identity);

        purple.GetComponent<PurpleLogic>().ottoGojo = this.gameObject;
        PurpleLogic purpleLogic = purple.GetComponent<PurpleLogic>();
        purpleLogic.SkillUpdate(player.signatureSkill);
        purpleLogic.SetDirection(player.transform.forward);

        isPurpleActive = false;
        */
    }

    public void IsHeldUpdate(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isHoldingHollowPurple = true;
            Debug.Log("Hollow Purple button held.");
        }
        else if (context.canceled)
        {
            purple.HeldUpdate(false);
            Debug.Log("Hollow Purple button released.");
        }
    }

    private void Update()
    {
        UpdateBlueCooldown();
        UpdateRedCooldown();
        UpdatePurpleCooldown();
    }

    void UpdateBlueCooldown()
    {
        if (isBlueActive == false)
        {
            cooldownBlue -= Time.deltaTime;
            if (cooldownBlue <= 0)
            {
                cooldownBlue = 5;
                isBlueActive = true;
            }
        }
    }

    void UpdateRedCooldown()
    {
        if (isRedActive == false)
        {
            cooldownRed -= Time.deltaTime;
            if (cooldownRed <= 0)
            {
                cooldownRed = 5;
                isRedActive = true;
            }
        }
    }

    void UpdatePurpleCooldown()
    {
        if (isPurpleActive == false)
        {
            cooldownPurple -= Time.deltaTime;
            if (cooldownPurple <= 0)
            {
                cooldownPurple = 5;
                isPurpleActive = true;
            }
        }
    }
}
