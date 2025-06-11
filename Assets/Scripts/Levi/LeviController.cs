using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeviController : MonoBehaviour
{
    [SerializeField]
    private GeneralPlayerController player;
    private UIAnnouncerLogic uiAnnouncerLogic;

    //passive
    private bool isPassiveCountDown = false;
    private bool isAwakened = false;
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
    private int max = 1; // max amount of teleportations allowed
    private float tpWindow = 3f; // time window to click again

    // Ultimate - Levi change it up!
    private float cooldownLeviChangeItUp = 60;
    private bool isLeviChangeItUpActive = true;
    private int leviChangeItUpAmountCasted = 0;
    private bool[] corner = { false, false, false, false };
    public GameObject bigBomb;
    private int? initialCorner = null;

    void Start() { StartCoroutine(DelayedStart()); }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3f); // Waits for 3 seconds
        player = GetComponent<GeneralPlayerController>();
        bombAbility = GetComponent<BasicAbility>();
        uiAnnouncerLogic = player.UIGameObject.GetComponent<UIAnnouncerLogic>();
    }

    public float ProbabilityChance()
    {
        return UnityEngine.Random.value;
    }

    public void EffectsEffects(InputAction.CallbackContext context) // this guy has no cooldown i think
    {
        if (!context.performed || player.signatureSkill == 0 || !isEffectsEffectsActive || !player.curseEnergy.CEReduction(150)) return;

        if (isAwakened)
        {
            StartCoroutine(NOMORECHANCE());
            return;
        }

        float effectsEffectsValue = ProbabilityChance();
        if (effectsEffectsValue < 0.3333f)
        {
            Debug.Log("1");
            uiAnnouncerLogic.Announce("Effect 1!");
            if (player.curseEnergy.CEReduction(150))
                oneEffect = true;
            else
            {
                player.curseEnergy.currentPool = 0;
            }

            StartCoroutine(player.UISignature.FadeIn(3f));
            cooldownEffectsEffects = 3f; // reset cooldown to 3 seconds
            isEffectsEffectsActive = false; // reset cooldown
        }
        else if (effectsEffectsValue < 0.6666f)
        {
            Debug.Log("2");
            uiAnnouncerLogic.Announce("Effect 2!");
            player.UISignature.Faded();

            StartCoroutine(twoEffectsTimer());
        }
        else
        {
            Debug.Log("3");
            uiAnnouncerLogic.Announce("Effect 3!");
            player.UISignature.Faded();

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

        int temp_range = player.bombSkill;
        player.bombSkill = 20;

        yield return new WaitForSeconds(15f);

        player.bombSkill = temp_range; // reset bomb skill
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

        StartCoroutine(player.UISignature.FadeIn(cooldownEffectsEffects));
        isEffectsEffectsActive = false; // reset cooldown
    }

    IEnumerator threeEffectsTimer()
    {
        bombAbility = gameObject.GetComponent<BasicAbility>();
        bombAbility.bombCost = 0;
        yield return new WaitForSeconds(15f);
        bombAbility.bombCost = 100;

        StartCoroutine(player.UISignature.FadeIn(cooldownEffectsEffects));
        isEffectsEffectsActive = false; // reset cooldown
    }

    /*
    // click once for activation
    // click again for random tp
    // click again for ANOTHER random tp, however for the second second click the timer will automatically end
    // timer ends after 5 seconds
    */

    public void UpgradeLeviscaped(float level)
    {
        if (level < 2)     // 1
            return;
        else if (level < 3)     // 2
        {
            tpWindow = 5f; // time window to click again
        }
        else if (level < 4)     // 3
        {
            tpWindow = 5f;
            max = 2; // max amount of teleportations allowed
        }
        else if (level < 5)     // 4
        {
            tpWindow = 10f;
            max = 2;
        }
        else                    // 5
        {
            tpWindow = 10f;
            max = 3; // max amount of teleportations allowed
        }
    }

    public void Leviscaped(InputAction.CallbackContext context)
    {
        if (!context.performed || player.heavySkill == 0 || !isLeviscapedActive) return;

        UpgradeLeviscaped(player.heavySkill);
        if (!tpCoroutine && player.curseEnergy.CEReduction(250))
        {
            StartCoroutine(LeviscapedActions());
            return; // click twice
        }

        if (tpCoroutine)
        {
            if (leviscapedAmountCasted < max)
                tpAction();
        }
    }

    IEnumerator LeviscapedActions()
    {
        tpCoroutine = true;

        yield return new WaitForSeconds(tpWindow);

        tpCoroutine = false;

        StartCoroutine(player.UIHeavy.FadeIn(cooldownLeviscaped));
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

            StartCoroutine(player.UIHeavy.FadeIn(cooldownLeviscaped));
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

    private readonly Dictionary<int, int> diagonalPairs = new Dictionary<int, int>
    {
        { 0, 3 }, { 3, 0 },
        { 1, 2 }, { 2, 1 }
    };

    private readonly Vector3[] cornerPositions = new Vector3[]
    {
        new Vector3(-4f, 20, 4f),   // 0: top right
        new Vector3(-4f, 20, -4f),  // 1: bottom right
        new Vector3(4f, 20, 4f),    // 2: top left
        new Vector3(4f, 20, -4f)    // 3: bottom left
    };

    public void LeviChangeItUp(InputAction.CallbackContext context)
    {
        if (!context.performed || player.ultimateSkill == 0 || !isLeviChangeItUpActive || !player.curseEnergy.CEReduction(2500)) return;

        int destroyedCount = 0;
        for (int i = 0; i < corner.Length; i++)
            if (corner[i]) destroyedCount++;

        // Collect valid corners
        List<int> availableCorners = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if (corner[i]) continue; // already destroyed
            if (initialCorner.HasValue && i == diagonalPairs[initialCorner.Value]) continue; // prevent diagonal
            availableCorners.Add(i);
        }

        if (availableCorners.Count == 0)
        {
            Debug.Log("No valid corners left.");
            return;
        }

        // Awakened Mode: Destroy 2 corners if possible
        if (isAwakened && destroyedCount <= 1 && availableCorners.Count >= 2)
        {
            int first = availableCorners[Random.Range(0, availableCorners.Count)];
            availableCorners.Remove(first);

            // Filter second options: not diagonally opposite to the first
            availableCorners.Remove(diagonalPairs[first]);
            if (availableCorners.Count == 0)
            {
                Debug.Log("No second valid corner after diagonal filter.");
                return;
            }

            int second = availableCorners[Random.Range(0, availableCorners.Count)];

            // Destroy both
            corner[first] = true;
            corner[second] = true;
            GameObject ult1 = Instantiate(bigBomb, cornerPositions[first], Quaternion.identity);
            ult1.GetComponent<BigBombController>().Upgrade(player.ultimateSkill);

            GameObject ult2 = Instantiate(bigBomb, cornerPositions[second], Quaternion.identity);
            ult2.GetComponent<BigBombController>().Upgrade(player.ultimateSkill);

            initialCorner = first;
            leviChangeItUpAmountCasted++;
            return;
        }

        // Normal behavior: destroy 1 corner
        int chosenIndex = availableCorners[Random.Range(0, availableCorners.Count)];
        corner[chosenIndex] = true;

        GameObject ult = Instantiate(bigBomb, cornerPositions[chosenIndex], Quaternion.identity);
        ult.GetComponent<BigBombController>().Upgrade(player.ultimateSkill);

        if (!initialCorner.HasValue)
            initialCorner = chosenIndex;

        leviChangeItUpAmountCasted++;

        player.UIUltimate.FadeIn(cooldownEffectsEffects);
        isLeviChangeItUpActive = false; // reset cooldown
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
        if (ProbabilityChance() < 0.1f)
        {
            isAwakened = true;
            uiAnnouncerLogic.Announce("Levi has awakened!"); // Announce Levi's awakening
        } else uiAnnouncerLogic.Announce("Roll failed! Another 10 seconds for retry to awaken"); // Announce Levi's passive failed
    }

    IEnumerator PassiveCountDown()
    {
        Debug.Log("Countdown");
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
            if (cooldownEffectsEffects <= 0)
            {
                cooldownEffectsEffects = 15;
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