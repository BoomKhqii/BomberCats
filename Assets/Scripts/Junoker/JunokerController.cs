using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunokerController : MonoBehaviour
{
    public CurseEnergyLogic curseEnergy;
    public PlayerController player;

    // Cooldown
    private float cooldownAbility = 5f;
    private bool isAbilityActive = true;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }



    void Update()
    {
        UpdateAbilityCooldown();
    }
    void UpdateAbilityCooldown()
    {
        if (isAbilityActive == false)
        {
            cooldownAbility -= Time.deltaTime;
            if (cooldownAbility <= 0)
            {
                cooldownAbility = 5;
                isAbilityActive = true;
            }
        }
    }
}
