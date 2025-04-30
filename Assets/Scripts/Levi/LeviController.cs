using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeviController : MonoBehaviour
{
    private CurseEnergyLogic curseEnergy;
    private GeneralPlayerController player;

    // Singature - Effects! Effects!
    private float cooldownEffectsEffects = 15;
    private bool isEffectsEffectsActive = true;
    private bool oneEffect = false;
    public bool twoEffect = false;
    private BasicAbility bombAbility; // three effect

    // Heavy - Leviscaped
    private float cooldownLeviscaped = 15;
    private bool isLeviscapedActive = true;
    private int leviscapedAmountCasted = 0;
    private bool isLeviscapedIntervalActive = false;
    private bool tpCoroutine = false;

    // Ultimate - Levi change it up!
    private float cooldownLeviChangeItUp = 60;
    private bool isLeviChangeItUpActive = true;
    private int leviChangeItUpAmountCasted = 0;

    void Start()
    {
        player = GetComponent<GeneralPlayerController>();
        curseEnergy = GameObject.Find("CE Pool of Levi").GetComponent<CurseEnergyLogic>();
    }

    public void EffectsEffects(InputAction.CallbackContext context)
    {
        if (!context.performed || !isEffectsEffectsActive || !curseEnergy.CEReduction(150)) return;

        float effectsEffectsValue = ProbabilityChanceEffectsEffects();
        if(effectsEffectsValue < 0.3333f)
        {
            // -100
            oneEffect = true; 
        }
        else if(effectsEffectsValue < 0.6666f)
        {
            StartCoroutine(twoEffectsTimer());
        }
        else
        {
            StartCoroutine(threeEffectsTimer());
        }
    }

    IEnumerator twoEffectsTimer()
    {
        twoEffect = true;
        yield return new WaitForSeconds(5f);
        twoEffect = false;
    }

    IEnumerator threeEffectsTimer()
    {
        bombAbility = gameObject.GetComponent<BasicAbility>();
        bombAbility.bombCost = 0;
        yield return new WaitForSeconds(15f);
        bombAbility.bombCost = 100;
    }

    public float ProbabilityChanceEffectsEffects()
    {
        return UnityEngine.Random.value;
    }

    // click once for activation
    // click again for random tp
    // click again for ANOTHER random tp
    // timer ends after 15 seconds
    public void Leviscaped(InputAction.CallbackContext context)
    {
        if (!context.performed || !isLeviscapedActive) return;

        if (curseEnergy.CEReduction(250) && !isLeviscapedIntervalActive)
        {
            isLeviscapedIntervalActive = true;
            tpCoroutine = true;
            StartCoroutine(LeviscapedActions());

            return; // click twice
        }
        else if (!curseEnergy.CEReduction(250) && !isLeviscapedIntervalActive) return;

        if(tpCoroutine)
        {
            tpAction();
        }
    }

    IEnumerator LeviscapedActions()
    {
        yield return new WaitForSeconds(15f);
        tpCoroutine = false;
    }

    public void tpAction()
    {

    }

    public void LeviChangeItUp(InputAction.CallbackContext context)
    {
        if (!context.performed || !isLeviChangeItUpActive || !curseEnergy.CEReduction(2500)) return;
    }

    void Update()
    {
        UpdateEffectsEffectsCooldown();
        UpdateLeviscapedCooldown();
        UpdateLeviChangeItUpCooldown();
    }

    // Cooldown - Signature
    void UpdateEffectsEffectsCooldown()
    {
        if (isEffectsEffectsActive == false)
        {
            cooldownEffectsEffects -= Time.deltaTime;
            if (cooldownEffectsEffects <= 0 & !oneEffect)
            {
                cooldownEffectsEffects = 15;
                isEffectsEffectsActive = true;
            }
            else
            {
                cooldownEffectsEffects = 3;
                isEffectsEffectsActive = true;
                oneEffect = false;
            }
        }
    }

    // Cooldown - Heavy
    void UpdateLeviscapedCooldown()
    {
        if (isLeviscapedActive == false)
        {
            cooldownLeviscaped -= Time.deltaTime;
            if (cooldownLeviscaped <= 0)
            {
                cooldownLeviscaped = 15;
                isLeviscapedActive = true;
            }
        }
    }

    // Cooldown - Ultimate
    void UpdateLeviChangeItUpCooldown()
    {
        if (isLeviChangeItUpActive == false)
        {
            cooldownLeviChangeItUp -= Time.deltaTime;
            if (cooldownLeviChangeItUp <= 0)
            {
                cooldownLeviChangeItUp = 60;
                isLeviChangeItUpActive = true;
            }
        }
    }
}
