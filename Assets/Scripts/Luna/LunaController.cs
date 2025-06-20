using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class LunaController : MonoBehaviour
{
    [SerializeField]
    private GeneralPlayerController player;

    private bool isTrapActive = true;
    private float cooldownTrap = 1f;
    public GameObject trapGameObject;
    private int maxSpawned = 2;
    private Queue<GameObject> queueSpawning = new Queue<GameObject>();

    private bool isHookActive = true;
    private float cooldownHook = 10f;
    public GameObject hookGameObject;

    private bool isSplashActive = true;
    private float cooldownSplash = 60f;
    public GameObject splashGameObject;

    void Start() { StartCoroutine(DelayedStart()); }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3f); // Waits for 3 seconds
        player = GetComponent<GeneralPlayerController>();
        player.playerSpeed += 1f;
    }

    public void Upgrade(float level)
    {
        if (level < 2)     // 1
            return;
        else
            maxSpawned = 3;
    }

    public void Trap(InputAction.CallbackContext context)
    {
        if (!context.performed || player.signatureSkill == 0 || !isTrapActive || !player.curseEnergy.CEReduction(150)) return;
        Upgrade(player.signatureSkill);

        GameObject trapSpawner = Instantiate(trapGameObject, new Vector3(
            Mathf.RoundToInt(transform.position.x),
            0.5195f,
            Mathf.RoundToInt(transform.position.z)), Quaternion.identity);
        queueSpawning.Enqueue(trapSpawner);

        trapSpawner.GetComponent<LunaTrapLogic>().lunaObject = this.gameObject;
        trapSpawner.GetComponent<LunaTrapLogic>().Upgrade(player.signatureSkill);

        if (queueSpawning.Count > maxSpawned)
        {
            GameObject oldest = queueSpawning.Dequeue();
            Destroy(oldest);
        }

        StartCoroutine(player.UISignature.FadeIn(cooldownTrap));
        isTrapActive = false;
    }

    public void Hook(InputAction.CallbackContext context) // luna is not supposed to move
    {
        if (!context.performed || player.heavySkill == 0 || !isHookActive || !player.curseEnergy.CEReduction(200)) return;

        Vector3 spawnOffset = player.transform.forward.normalized;
        Vector3 spawnPos = new Vector3(
            Mathf.RoundToInt(transform.position.x + spawnOffset.x),
            1.32f,
            Mathf.RoundToInt(transform.position.z + spawnOffset.z)
        );
        GameObject hook = Instantiate(hookGameObject, spawnPos, Quaternion.identity);

        HookLogic hookLogic = hook.GetComponent<HookLogic>();
        hookLogic.SetDirection(player.transform.forward);
        hookLogic.lunaObject = this.gameObject;
        hookLogic.Upgrade(player.heavySkill);

        StartCoroutine(player.UIHeavy.FadeIn(cooldownHook));
        isHookActive = false;
    }

    public void Splash(InputAction.CallbackContext context) // not supposed to move
    {
        if (!context.performed || player.ultimateSkill == 0 || !isSplashActive || !player.curseEnergy.CEReduction(150)) return;

        GameObject splashSpawner = Instantiate(splashGameObject, new Vector3(
            Mathf.RoundToInt(transform.position.x),
            0.5195f,
            Mathf.RoundToInt(transform.position.z)), Quaternion.identity);

        splashSpawner.GetComponent<SplashLogic>().lunaObject = this.gameObject;
        splashSpawner.GetComponent<SplashLogic>().Upgrade(player.ultimateSkill);

        StartCoroutine(player.UIUltimate.FadeIn(cooldownSplash));
        isSplashActive = false;
    }

    void Update()
    {
        UpdateTrapCooldown();
        UpdateHookCooldown();
        UpdateSplashCooldown();
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

    void UpdateHookCooldown()
    {
        if (isHookActive == false)
        {
            cooldownHook -= Time.deltaTime;
            if (cooldownHook <= 0)
            {
                cooldownHook = 10f;
                isHookActive = true;
            }
        }
    }

    void UpdateSplashCooldown()
    {
        if (isSplashActive == false)
        {
            cooldownSplash -= Time.deltaTime;
            if (cooldownSplash <= 0)
            {
                cooldownSplash = 60f;
                isSplashActive = true;
            }
        }
    }
}
