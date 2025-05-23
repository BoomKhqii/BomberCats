using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OttoGojoController : MonoBehaviour
{
    public GeneralPlayerController player;

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
    private PurpleLogic purpleOut;
    private float cooldownPurple = 60f;
    private bool isPurpleActive = true;

    public bool isHoldingHollowPurple = false;
    private float holdStartTime = 0f;
    private float heldDuration = 0f;

    void Start()
    {
        player = GetComponent<GeneralPlayerController>();
    }

    public bool InfinityProbabilityChance()
    {
        return UnityEngine.Random.value < 0.45f;
    }

    public void BlueSkill(InputAction.CallbackContext context)
    {
        if (!context.performed || player.signatureSkill == 0 || !isBlueActive || !player.curseEnergy.CEReduction(250)) return;

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
        blueLogic.SetDirection(player.transform.forward);
        StartCoroutine(player.UISignature.FadeIn(cooldownBlue));

        isBlueActive = false;
    }

    public void RedSkill(InputAction.CallbackContext context)
    {
        if (!context.performed || player.heavySkill == 0 || !isRedActive || !player.curseEnergy.CEReduction(500)) return;

        Vector3 spawnOffset = player.transform.forward.normalized;
        Vector3 spawnPos = new Vector3(
            Mathf.RoundToInt(player.transform.position.x + spawnOffset.x),
            1.32f,
            Mathf.RoundToInt(player.transform.position.z + spawnOffset.z)
        );
        GameObject red = Instantiate(objectRed, spawnPos, Quaternion.identity);

        red.GetComponent<RedLogic>().ottoGojo = this.gameObject;
        RedLogic redLogic = red.GetComponent<RedLogic>();
        redLogic.SetDirection(player.transform.forward);
        StartCoroutine(player.UIHeavy.FadeIn(cooldownRed));

        isRedActive = false;
    }
    public void HollowPurpleSkill(InputAction.CallbackContext context)
    {
        //&& isPurpleActive && curseEnergy.CEReduction(1000)
        if (context.started && player.ultimateSkill != 0 && isPurpleActive && player.curseEnergy.CEReduction(1000))
        {
            holdStartTime = Time.time;
            IsHeldUpdate(context);

            Vector3 spawnOffset = player.transform.forward.normalized;
            Vector3 spawnPos = new Vector3(
                Mathf.RoundToInt(player.transform.position.x + spawnOffset.x),
                1.32f,
                Mathf.RoundToInt(player.transform.position.z + spawnOffset.z)
            );
            GameObject purpleIn = Instantiate(objectHollowPurple, spawnPos, Quaternion.identity);

            purpleIn.GetComponent<PurpleLogic>().ottoGojo = this.gameObject;
            PurpleLogic purpleLogic = purpleIn.GetComponent<PurpleLogic>();
            purpleLogic.SkillUpdate(player.signatureSkill);
            purpleLogic.SetDirection(player.transform.forward);

            purpleOut = purpleLogic;
        }

        if (context.canceled && player.ultimateSkill != 0 && isPurpleActive)
        {
            heldDuration = Time.time - holdStartTime;
            Debug.Log($"Released Hollow Purple after {heldDuration:F2} seconds.");
            IsHeldUpdate(context);
            isPurpleActive = false;
        }
}

    public void IsHeldUpdate(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isHoldingHollowPurple = true;
        }
        else if (context.canceled)
        {
            isHoldingHollowPurple = false;
            purpleOut.HeldUpdate(isHoldingHollowPurple);
            StartCoroutine(player.UIUltimate.FadeIn(cooldownPurple));
        } else return;
    }

    public float HowLongHeld()
    { return heldDuration; }

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
                cooldownRed = 15;
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
                cooldownPurple = 60;
                isPurpleActive = true;
            }
        }
    }
}
