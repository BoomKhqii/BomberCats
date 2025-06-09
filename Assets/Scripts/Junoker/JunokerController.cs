using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.InputSystem.XR;
using static UnityEditor.PlayerSettings;

public class JunokerController : MonoBehaviour
{
    public GeneralPlayerController player;
    [SerializeField]
    private Transform cloneLocation;
    public MeshRenderer invis;
    public UIAnnouncerLogic announcerLogic;

    // Juno Jo's
    private float cooldownJunoJos = 3;
    private bool isJunoJosActive = true;
    [SerializeField]
    private GameObject junosJoCloneObject;

    // Juno Where I Go? 
    private float cooldownJunoWhereIGo = 5;
    private bool isJunoWhereIGoActive = true;
    public LayerMask blocked;
    private CharacterController controller;
    [SerializeField]
    private GameObject junoWhereIGoCloneObject;
    private float origDurationInvis = 1f;
    private float durationInvis; //1, 2, 3 
    private bool isinvisOn = false;

    // Juno Jos Jes Juatro
    private float cooldownJunoJosJesJuatro = 30;
    private bool isJunoJosJesJuatroActive = true;
    private float spawnRadius = 5f;
    [SerializeField]
    private GameObject JunoJosJesJuatroClonesObject;
    float minXZ = -7f, maxXZ = 7f;

    void Start() { StartCoroutine(DelayedStart()); }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3f); // Waits for 3 seconds
        player = GetComponent<GeneralPlayerController>();
        controller = GetComponent<CharacterController>();
    }

    public void JunoJos(InputAction.CallbackContext context)
    {
        if (!context.performed || player.signatureSkill == 0 || !isJunoJosActive || !player.curseEnergy.CEReduction(150)) return;

        GameObject junojos = Instantiate(junosJoCloneObject, new Vector3(
            Mathf.RoundToInt(cloneLocation.position.x),
            1.38f,
            Mathf.RoundToInt(cloneLocation.position.z)), Quaternion.identity);

        junojos.GetComponent<JunoJosLogic>().junoker = gameObject;

        // cooldown
        StartCoroutine(player.UISignature.FadeIn(cooldownJunoJos));
        isJunoJosActive = false;
    }

    public void JunoWhereIGo(InputAction.CallbackContext context)
    {
        //levelJunoWhereIGo = player.heavySkill;
        Upgrade(player.heavySkill);

        if (!context.performed || player.heavySkill == 0 || !isJunoWhereIGoActive || !player.curseEnergy.CEReduction(200)) return;

        Vector3 dashTarget = new Vector3(
            Mathf.RoundToInt(cloneLocation.position.x),
            1.38f,
            Mathf.RoundToInt(cloneLocation.position.z)) 
            + transform.forward * 2f;

        // Check only the target position for bedrock
        bool isBlocked = Physics.CheckSphere(dashTarget, 0.3f, blocked);

        if (!isBlocked &&
            dashTarget.x >= minXZ && dashTarget.x <= maxXZ &&
            dashTarget.z >= minXZ && dashTarget.z <= maxXZ)
        {
            Instantiate(junoWhereIGoCloneObject, new Vector3(
            Mathf.RoundToInt(cloneLocation.position.x),
            1.38f,
            Mathf.RoundToInt(cloneLocation.position.z)), Quaternion.identity);
            durationInvis = origDurationInvis;
            isinvisOn = true;

            controller.enabled = false;
            transform.position = dashTarget;
            controller.enabled = true;

            // cooldown
            StartCoroutine(player.UIHeavy.FadeIn(cooldownJunoWhereIGo)); // Fade in the UI
            isJunoWhereIGoActive = false;
        } else { return; }
    }

    public void IsJunoInvis(bool onGoing)
    {
        if(onGoing)
        {
            invis.enabled = false;
            durationInvis -= Time.deltaTime;
            if (durationInvis <= 0)
            {
                durationInvis = origDurationInvis; // with skill increment
                invis.enabled = true;
                isinvisOn = false;
            }
        }
    }

    public void Upgrade(float level)
    {
        if (level < 2)                          // 1
            return;
        else if (level < 3)                     // 2
            origDurationInvis = 1.2f;
        else if (level < 4)                    // 3
            origDurationInvis = 1.5f;
        else if (level < 5)                     // 4
            origDurationInvis = 1.8f;
        else                                    // 5
            origDurationInvis = 3f;
    }

    public void JunoJosJesJuatro(InputAction.CallbackContext context)
    {
        if (!context.performed || player.ultimateSkill == 0 || !isJunoWhereIGoActive || !player.curseEnergy.CEReduction(800)) return;

        StartCoroutine(player.UIUltimate.FadeIn(cooldownJunoJosJesJuatro)); // Fade in the UI
        /*
        Vector3 center = transform.position;

        for (int i = 0; i < 3; i++)
        {
            bool hasUltCloneSpawn = false;
            while (!hasUltCloneSpawn)
            {
                // Step 1: random point in circle
                Vector2 random2D = Random.insideUnitCircle * spawnRadius;
                Vector3 randomPosition = center + new Vector3(random2D.x, 0, random2D.y);

                // Step 2: adjust for height, or use terrain height here
                randomPosition.y = 1.38f;

                // Step 3: check if space is not blocked
                if (!Physics.CheckSphere(randomPosition, 0.4f, blocked))
                {
                    // Step 4: instantiate and break out of loop
                    Instantiate(junosJoCloneObject, randomPosition, Quaternion.identity);
                    hasUltCloneSpawn = true;
                    break;
                }
            }
        }
        */

        // cooldown
        StartCoroutine(UltimateAction());
        isJunoJosJesJuatroActive = false;
    }

    IEnumerator UltimateAction()
    {
        Vector3 center = transform.position;
        invis.enabled = false;
        yield return new WaitForSeconds(1.5f);
        invis.enabled = true;

        for (int i = 0; i < 1; i++)
        {
            bool hasUltCloneSpawn = false;
            while (!hasUltCloneSpawn)
            {
                // Step 1: random point in circle
                Vector2 random2D = Random.insideUnitCircle * spawnRadius;
                Vector3 randomPosition = center + new Vector3(random2D.x, 0, random2D.y);

                // Step 2: adjust for height, or use terrain height here
                randomPosition.y = 1.38f;

                // Step 3: check if space is not blocked
                if (!Physics.CheckSphere(randomPosition, 0.4f, blocked) && 
                    randomPosition.x >= minXZ && randomPosition.x <= maxXZ && 
                    randomPosition.z >= minXZ && randomPosition.z <= maxXZ)
                {
                    // Step 4: instantiate and break out of loop
                    Instantiate(JunoJosJesJuatroClonesObject, randomPosition, Quaternion.identity);
                    hasUltCloneSpawn = true;
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    void Update()
    {
        IsJunoInvis(isinvisOn);

        UpdateJunoJosCooldown();
        UpdateJunoWhereIGoCooldown();
        UpdateJunoJosJesJuatroCooldown();
    }
    void UpdateJunoJosCooldown()
    {
        if (isJunoJosActive == false)
        {
            cooldownJunoJos -= Time.deltaTime;
            if (cooldownJunoJos <= 0)
            {
                cooldownJunoJos = 3;
                isJunoJosActive = true;
            }
        }
    }

    void UpdateJunoWhereIGoCooldown()
    {
        if (isJunoWhereIGoActive == false)
        {
            cooldownJunoWhereIGo -= Time.deltaTime;
            if (cooldownJunoWhereIGo <= 0)
            {
                cooldownJunoWhereIGo = 5;
                isJunoWhereIGoActive = true;
            }
        }
    }

    void UpdateJunoJosJesJuatroCooldown()
    {
        if (isJunoJosJesJuatroActive == false)
        {
            cooldownJunoJosJesJuatro -= Time.deltaTime;
            if (cooldownJunoJosJesJuatro <= 0)
            {
                cooldownJunoJosJesJuatro = 30;
                isJunoJosJesJuatroActive = true;
            }
        }
    }
}
