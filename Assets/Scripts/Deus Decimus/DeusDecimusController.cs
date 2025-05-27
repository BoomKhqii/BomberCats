using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class DeusDecimusController : MonoBehaviour
{
    [SerializeField]
    private GeneralPlayerController player;

    // Signature - Deus Almighty
    private float cooldownDeusAlmight = 5f;
    private bool isDeusAlmightyActive = true;
    private Vector3 origin;
    private CharacterController controller;
    // skill
    private float levelDeusAlmighty = 0;
    private float durationTime = 2f;

    // Heavy - Punish
    private float cooldownEndsOfTheUniverse = 15f;
    private bool isEndsOfTheUniverseActive = true;
    public GameObject punishObject;

    // Ultimate - End's of the Universe
    private float cooldownPunish = 15f;
    private bool isPunishActive = true;
    public GameObject blackHole;

    void Start() { StartCoroutine(DelayedStart()); }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3f); // Waits for 3 seconds
        player = GetComponent<GeneralPlayerController>();
        controller = GetComponent<CharacterController>();
    }

    public void Upgrade(float level)
    {
        if (level < 2)     // 1
            return;
        else if (level < 3)     // 2
        {
            durationTime = 3f;
        }
        else if (level < 4)     // 3
        {
            durationTime = 3.5f;
        }
        else                    // 4 +
        {
            durationTime = 4f;
        }
    }


    public void DeusAlmighty(InputAction.CallbackContext context)
    {
        if (!context.performed || player.signatureSkill == 0 || !isDeusAlmightyActive || !player.curseEnergy.CEReduction(200)) return;
        origin = transform.position;
        StartCoroutine(DeusAlmightyAction());

        StartCoroutine(player.UISignature.FadeIn(cooldownDeusAlmight));
        isDeusAlmightyActive = false;
    }
    IEnumerator DeusAlmightyAction()
    {
        yield return new WaitForSeconds(durationTime);
        controller.enabled = false;     // disables the character controller
        transform.position = origin;    // tp
        controller.enabled = true;      // enable
        player.playerSpeed = 0;         // player stops after tp back
        yield return new WaitForSeconds(0.5f);
        player.playerSpeed = 4.5f;
    }

    public void Punish(InputAction.CallbackContext context)
    {
        Vector3 spawnOffset = player.transform.forward.normalized;
        Vector3 spawnPos = new Vector3(
            Mathf.RoundToInt(player.transform.position.x + spawnOffset.x),
            1.32f,
            Mathf.RoundToInt(player.transform.position.z + spawnOffset.z)
        );

        GameObject punish = Instantiate(punishObject, spawnPos, Quaternion.identity);
        punish.GetComponent<PunishLogic>().deusDecimus = this.gameObject;

        if (!context.performed || player.heavySkill == 0 || !isPunishActive || !punish.GetComponent<PunishLogic>().hasTarget || !player.curseEnergy.CEReduction(500)) { /*Destroy(punish);*/ return;  }

        // code
        StartCoroutine(player.UIHeavy.FadeIn(cooldownPunish));
        isPunishActive = false;
    }

    public void EndsOfTheUniverse(InputAction.CallbackContext context)
    {
        if (!context.performed || player.ultimateSkill == 0 || !isEndsOfTheUniverseActive || !player.curseEnergy.CEReduction(2000)) return;
        Instantiate(blackHole);

        StartCoroutine(player.UIUltimate.FadeIn(cooldownEndsOfTheUniverse));
        isEndsOfTheUniverseActive = false;
    }

    void Update()
    {
        UpdateDeusAlmightyCooldown();
        UpdatePunishCooldown();
        UpdateEndsOfTheUniverseCooldown();

        // Signature skill
        GeneralPlayerController skill = gameObject.GetComponent<GeneralPlayerController>(); // Accessing the skill upgrade
        levelDeusAlmighty += skill.signatureSkill;
        Upgrade(levelDeusAlmighty);
    }

    void UpdateDeusAlmightyCooldown()
    {
        if (isDeusAlmightyActive == false)
        {
            cooldownDeusAlmight -= Time.deltaTime;
            if (cooldownDeusAlmight <= 0)
            {
                cooldownDeusAlmight = 5f;
                isDeusAlmightyActive = true;
            }
        }
    }

    void UpdatePunishCooldown()
    {
        if (isPunishActive == false)
        {
            cooldownPunish -= Time.deltaTime;
            if (cooldownPunish <= 0)
            {
                cooldownPunish = 15f;
                isPunishActive = true;
            }
        }
    }

    void UpdateEndsOfTheUniverseCooldown()
    {
        if (isEndsOfTheUniverseActive == false)
        {
            cooldownEndsOfTheUniverse -= Time.deltaTime;
            if (cooldownEndsOfTheUniverse <= 0)
            {
                cooldownEndsOfTheUniverse = 120f;
                isEndsOfTheUniverseActive = true;
            }
        }
    }
}
