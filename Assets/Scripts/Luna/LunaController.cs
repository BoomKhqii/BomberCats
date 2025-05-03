using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LunaController : MonoBehaviour
{
    [SerializeField]
    private CurseEnergyLogic curseEnergy;
    [SerializeField]
    private GeneralPlayerController player;

    public bool isTrapActive = true;
    private float cooldownTrap = 1f;

    public bool isHookActive = true;
    private float cooldownHook = 1f;

    public bool isSplashActive = true;
    private float cooldownSplash = 1f;

    void Start()
    {
        player = GetComponent<GeneralPlayerController>();
        curseEnergy = GameObject.Find("CE Pool of Luna").GetComponent<CurseEnergyLogic>();
    }

    public void Trap(InputAction.CallbackContext context)
    {
        if (!context.performed || player.signatureSkill == 0 || !isTrapActive || !curseEnergy.CEReduction(150)) return;

        isTrapActive = false;
    }

    public void Hook(InputAction.CallbackContext context)
    {
        if (!context.performed || player.heavySkill == 0 || !isHookActive || !curseEnergy.CEReduction(150)) return;

        isHookActive = false;
    }

    public void Splash(InputAction.CallbackContext context)
    {
        if (!context.performed || player.ultimateSkill == 0 || !isSplashActive || !curseEnergy.CEReduction(150)) return;

        isSplashActive = false;
    }

    void Update()
    {
        UpdateTrapCooldown();
    }

    void UpdateTrapCooldown()
    {
        if (isTrapActive == false)
        {
            cooldownTrap -= Time.deltaTime;
            if (cooldownTrap <= 0)
            {
                cooldownTrap = 3;
                isTrapActive = true;
            }
        }
    }
}
