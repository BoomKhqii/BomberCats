using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeviController : MonoBehaviour
{
    private CurseEnergyLogic curseEnergy;
    private GeneralPlayerController player;

    //passive
    private bool isPassiveCountDown = false;
    private bool isAwakened = true;
    private ObjectStatus leviscaped2;

    // Singature - Effects! Effects!
    private float cooldownEffectsEffects = 15;
    private bool isEffectsEffectsActive = true;
    private bool oneEffect = false;
    private BasicAbility bombAbility; // two & three effect
    public GameObject effect2Bomb;
    private GameObject bomb;

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
    private bool[] corner = { false, false, false, false };
    public GameObject bigBomb;

    void Start()
    {
        bombAbility = GetComponent<BasicAbility>();
        player = GetComponent<GeneralPlayerController>();
        curseEnergy = GameObject.Find("CE Pool of Levi").GetComponent<CurseEnergyLogic>();
    }

    public float ProbabilityChance()
    {
        return UnityEngine.Random.value;
    }

    public void EffectsEffects(InputAction.CallbackContext context)
    {
        if (!context.performed || !isEffectsEffectsActive || !curseEnergy.CEReduction(150)) return;

        if(isAwakened)
        {
            StartCoroutine(NOMORECHANCE());
            return;
        }

        float effectsEffectsValue = ProbabilityChance();
        if (effectsEffectsValue < 0.3333f)
        {
            Debug.Log("1");
            if (curseEnergy.CEReduction(150))
                oneEffect = true;
            else
            {
                curseEnergy.currentPool = 0;
            }
        }
        else if (effectsEffectsValue < 0.6666f)
        {
            Debug.Log("2");
            StartCoroutine(twoEffectsTimer());
        }
        else
        {
            Debug.Log("3");
            StartCoroutine(threeEffectsTimer());
        }
    }

    IEnumerator NOMORECHANCE()
    {
        Debug.Log("nomorechacnes");
        // effect 3
        bombAbility = gameObject.GetComponent<BasicAbility>();
        bombAbility.bombCost = 0;
        // effect 2
        this.bomb = bombAbility.bomb;
        bombAbility.bomb = effect2Bomb;

        yield return new WaitForSeconds(5f);

        //effect 3
        bombAbility.bombCost = 100;
        //effect 2
        bombAbility.bomb = this.bomb;
    }

    IEnumerator twoEffectsTimer()
    {
        this.bomb = bombAbility.bomb;
        bombAbility.bomb = effect2Bomb;
        yield return new WaitForSeconds(10f);
        bombAbility.bomb = this.bomb;
    }

    IEnumerator threeEffectsTimer()
    {
        bombAbility = gameObject.GetComponent<BasicAbility>();
        bombAbility.bombCost = 0;
        yield return new WaitForSeconds(15f);
        bombAbility.bombCost = 100;
    }

    /*
    // click once for activation
    // click again for random tp
    // click again for ANOTHER random tp, however for the second second click the timer will automatically end
    // timer ends after 5 seconds
    */
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
            if (leviscapedAmountCasted < 2)
                tpAction();
        }
    }

    IEnumerator LeviscapedActions()
    {
        tpCoroutine = true;

        yield return new WaitForSeconds(10f);

        tpCoroutine = false;
        isLeviscapedActive = false;
    }

    public void tpAction()
    {
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

                if (isAwakened)
                    StartCoroutine(Leviscaped2Invulnerability());
            }
        }
        if (leviscapedAmountCasted == 2)
        {
            StopCoroutine(LeviscapedActions());
            tpCoroutine = false;
            isLeviscapedActive = false;
        }
    }

    IEnumerator Leviscaped2Invulnerability()
    {
        Debug.Log("Leviscaped2Invulnerability");
        leviscaped2 = GetComponent<ObjectStatus>();
        leviscaped2.isInvulnerable = true;
        yield return new WaitForSeconds(3f);
        leviscaped2.isInvulnerable = false;
    }

    public void LeviChangeItUp(InputAction.CallbackContext context)
    {
        if (!context.performed || !isLeviChangeItUpActive || !curseEnergy.CEReduction(2500)) return;

        float rand = ProbabilityChance();
        if (rand < 0.25f && !corner[0])           // 1: top right
        {
            corner[0] = true;             // -3.5     3.5
            Instantiate(bigBomb, new Vector3(-3.5f, 20, 3.5f), bigBomb.transform.rotation);
        }
        else if (rand < 0.50f && !corner[1])      // 2: bottom right
        {
            corner[1] = true;             // -3.5     -3.5
            Instantiate(bigBomb, new Vector3(-3.5f, 20, -3.5f), Quaternion.identity);
        }
        else if (rand < 0.65f && !corner[2])      // 3: top left
        {
            corner[2] = true;             // 3.5     3.5
            Instantiate(bigBomb, new Vector3(3.5f, 20, 3.5f), Quaternion.identity);
        }
        else if (!corner[3])                      // 4: bottom left
        {
            corner[3] = true;             // 3.5     -3.5
            Instantiate(bigBomb, new Vector3(3.5f, 20, -3.5f), Quaternion.identity);
        }
        leviChangeItUpAmountCasted++;
    }

    void Update()
    {
        if(!isPassiveCountDown && !isAwakened)
            StartCoroutine(PassiveCountDown());

        UpdateEffectsEffectsCooldown();
        UpdateLeviscapedCooldown();
        UpdateLeviChangeItUpCooldown();
    }

    private void Unlevictable()
    {
        if (ProbabilityChance() < 0.10f)
        {
            isAwakened = true;
        }
    }

    IEnumerator PassiveCountDown()
    {
        isPassiveCountDown = true;
        yield return new WaitForSeconds(10f);
        Unlevictable();
        isPassiveCountDown = false;
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