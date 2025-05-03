using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class LunaController : MonoBehaviour
{
    [SerializeField]
    private CurseEnergyLogic curseEnergy;
    [SerializeField]
    private GeneralPlayerController player;

    private bool isTrapActive = true;
    private float cooldownTrap = 1f;
    public GameObject trap;
    private int maxSpawned = 2;
    private Queue<GameObject> queueSpawning = new Queue<GameObject>();

    private bool isHookActive = true;
    private float cooldownHook = 1f;

    private bool isSplashActive = true;
    private float cooldownSplash = 1f;

    void Start()
    {
        player = GetComponent<GeneralPlayerController>();
        curseEnergy = GameObject.Find("CE Pool of Luna").GetComponent<CurseEnergyLogic>();

        // Passive
        player.playerSpeed += 1f;
    }

    public void Trap(InputAction.CallbackContext context)
    {
        if (!context.performed || player.signatureSkill == 0 || !isTrapActive || !curseEnergy.CEReduction(150)) return;

        GameObject trapSpawner = Instantiate(trap, new Vector3(
            Mathf.RoundToInt(transform.position.x),
            0.5195f,
            Mathf.RoundToInt(transform.position.z)), Quaternion.identity);
        queueSpawning.Enqueue(trapSpawner);

        if (queueSpawning.Count > maxSpawned)
        {
            GameObject oldest = queueSpawning.Dequeue();
            Destroy(oldest);
        }

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
