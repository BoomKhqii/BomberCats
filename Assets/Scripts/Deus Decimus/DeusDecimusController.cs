using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class DeusDecimusController : MonoBehaviour
{

    [SerializeField]
    private CurseEnergyLogic curseEnergy;
    [SerializeField]
    private GeneralPlayerController player;

    // Signature - Deus Almighty
    private float cooldownDeusAlmight = 5f;
    private bool isDeusAlmightyActive = true;
    private Vector3 origin;
    private CharacterController controller;

    // Heavy - Punish
    private float cooldownEndsOfTheUniverse = 120f;
    private bool isEndsOfTheUniverseActive = true;

    // Ultimate - End's of the Universe
    private float cooldownPunish = 15f;
    private bool isPunishActive = true;
    public GameObject blackHole;

    void Start()
    {
        player = GetComponent<GeneralPlayerController>();
        curseEnergy = GameObject.Find("CE Pool of Deus Decimus").GetComponent<CurseEnergyLogic>();

        // Signature
        controller = GetComponent<CharacterController>();
    }

    public void DeusAlmighty(InputAction.CallbackContext context)
    {
        if (!context.performed || !isDeusAlmightyActive || !curseEnergy.CEReduction(200)) return;
        origin = transform.position;
        StartCoroutine(DeusAlmightyAction());

        isDeusAlmightyActive = false;
    }
    IEnumerator DeusAlmightyAction()
    {
        yield return new WaitForSeconds(2f);
        controller.enabled = false;     // disables the character controller
        transform.position = origin;    // tp
        controller.enabled = true;      // enable
        player.playerSpeed = 0;         // player stops after tp back
        yield return new WaitForSeconds(0.5f);
        player.playerSpeed = 4.5f;
    }

    public void Punish(InputAction.CallbackContext context)
    {
        if (!context.performed || !isPunishActive || !curseEnergy.CEReduction(500)) return;

        // code
        isPunishActive = false;
    }

    public void EndsOfTheUniverse(InputAction.CallbackContext context)
    {
        if (!context.performed || !isEndsOfTheUniverseActive || !curseEnergy.CEReduction(2000)) return;
        Instantiate(blackHole);
        isEndsOfTheUniverseActive = false;
    }

    void Update()
    {
        UpdateDeusAlmightyCooldown();
        UpdatePunishCooldown();
        UpdateEndsOfTheUniverseCooldown();
    }

    void UpdateDeusAlmightyCooldown()
    {
        if (isDeusAlmightyActive == false)
        {
            cooldownDeusAlmight -= Time.deltaTime;
            if (cooldownDeusAlmight <= 0)
            {
                cooldownDeusAlmight = 120f;
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
                cooldownPunish = 120f;
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
