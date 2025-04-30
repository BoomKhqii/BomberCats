using System.Collections;
using System.Collections.Generic;
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

    // Heavy - Leviscaped
    private float cooldownLeviscaped = 15;
    private bool isLeviscapedActive = true;

    // Ultimate - Levi change it up!
    private float cooldownLeviChangeItUp = 60;
    private bool isLeviChangeItUpActive = true;

    void Start()
    {
        player = GetComponent<GeneralPlayerController>();
        curseEnergy = GameObject.Find("CE Pool of Levi").GetComponent<CurseEnergyLogic>();
    }

    public void EffectsEffects(InputAction.CallbackContext context)
    {
        if (!context.performed || !isEffectsEffectsActive || !curseEnergy.CEReduction(150)) return;
    }

    public void Leviscaped(InputAction.CallbackContext context)
    {
        if (!context.performed || !isLeviscapedActive || !curseEnergy.CEReduction(250)) return;
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
