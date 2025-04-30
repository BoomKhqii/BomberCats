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
    private bool tpCoroutine = false;
    private float tpRadius = 20f;
    float minXZ = -7f, maxXZ = 7f;
    public LayerMask blocked;
    private CharacterController tp;

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

        if (!tpCoroutine && curseEnergy.CEReduction(250))
        {
            StartCoroutine(LeviscapedActions());
            return; // click twice
        }

        if (tpCoroutine)
        {
            if(leviscapedAmountCasted < 2)
                tpAction();
        }
    }

    IEnumerator LeviscapedActions()
    {
        tpCoroutine = true;

        yield return new WaitForSeconds(5f);

        tpCoroutine = false;;
        isLeviscapedActive = false;
    }

    public void tpAction()
    {
        Debug.Log("tp'd actions");

        // used Junoker ultimate code
        bool untilTp = false;
        while (!untilTp)
        {
            Vector3 center = transform.position;
            // Step 1: random point in circle
            Vector2 random2D = Random.insideUnitCircle * tpRadius;
            Vector3 randomPosition = center + new Vector3(random2D.x, 0, random2D.y);

            // Step 2: adjust for height, or use terrain height here
            randomPosition.y = 1.38f;

            // Step 3: check if space is not blocked
            if (!Physics.CheckSphere(randomPosition, 0.4f, blocked) &&
                randomPosition.x >= minXZ && randomPosition.x <= maxXZ &&
                randomPosition.z >= minXZ && randomPosition.z <= maxXZ)
            {
                // Step 4: instantiate and break out of loop
                tp = GetComponent<CharacterController>();
                tp.enabled = false;
                gameObject.transform.position = randomPosition;
                tp.enabled = true;
                untilTp = true;
                leviscapedAmountCasted++;
            }
        }
        if(leviscapedAmountCasted == 2)
        {
            StopCoroutine(LeviscapedActions());
            tpCoroutine = false; ;
            isLeviscapedActive = false;
        }
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
