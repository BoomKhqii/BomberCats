using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OttoGojoController : MonoBehaviour
{
    public CurseEnergyLogic curseEnergy;

    // Blue
    //private int signatureSkill = 3;         
    [SerializeField]
    private GameObject objectBlue;
    private float cooldownBlue = 5;
    private bool isBlueActive = true;
    
    // Red
    [SerializeField]
    private GameObject objectRed;
    private float cooldownRed = 0;
    private bool isRedActive = true;

    // Hollow purple
    private int ultimateSkill = 12;
    [SerializeField]
    private GameObject objectHollowPurple;

    //[SerializeField]
    public PlayerController player;

    void Start()
    {
        curseEnergy = GameObject.Find("CE Pool of Otto Gojo").GetComponent<CurseEnergyLogic>();
        player = GetComponent<PlayerController>();
    }

    public bool InfinityProbabilityChance()
    {
        return Random.value < 0.35f;
    }

    public void BlueSkill(InputAction.CallbackContext context)
    {
        if (!context.performed || !isBlueActive || !curseEnergy.CEReduction(250)) return;

        //Vector3 spawnPos = new Vector3(Mathf.RoundToInt(player.transform.position.x), 1.32f, Mathf.RoundToInt(player.transform.position.z));
        Vector3 spawnOffset = player.transform.forward.normalized;
        Vector3 spawnPos = new Vector3(
            Mathf.RoundToInt(player.transform.position.x + spawnOffset.x),
            1.32f,
            Mathf.RoundToInt(player.transform.position.z + spawnOffset.z)
        );
        GameObject blue = Instantiate(objectBlue, spawnPos, Quaternion.identity);

        blue.GetComponent<BlueLogic>().ottoGojo = this.gameObject;
        BlueLogic blueLogic = blue.GetComponent<BlueLogic>();
        blueLogic.SkillUpdate(player.signatureSkill);
        blueLogic.SetDirection(player.transform.forward);

        isBlueActive = false;
    }

    public void RedSkill(InputAction.CallbackContext context)
    {
        if (!context.performed || !isRedActive || !curseEnergy.CEReduction(500)) return;

        Vector3 spawnOffset = player.transform.forward.normalized;
        Vector3 spawnPos = new Vector3(
            Mathf.RoundToInt(player.transform.position.x + spawnOffset.x),
            1.32f,
            Mathf.RoundToInt(player.transform.position.z + spawnOffset.z)
        );
        GameObject red = Instantiate(objectRed, spawnPos, Quaternion.identity);

        red.GetComponent<RedLogic>().ottoGojo = this.gameObject;
        RedLogic redLogic = red.GetComponent<RedLogic>();
        redLogic.SkillUpdate(player.signatureSkill);
        redLogic.SetDirection(player.transform.forward);

        isRedActive = false;
    }
    public void HollowPurpleSkill(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Button just started being pressed.");
        }

        if (context.performed)
        {
            Debug.Log("Button press performed.");
        }

        if (context.canceled)
        {
            Debug.Log("Button released.");
        }
        /*
        if (context.performed && curseEnergy.CEReduction(0))
        {
            Debug.Log("being held");
        }*/
    }

    private void Update()
    {
        UpdateBlueCooldown();
        UpdateRedCooldown();
    }

    void UpdateBlueCooldown()
    {
        if (isBlueActive == false)
        {
            cooldownBlue -= Time.deltaTime;
            if (cooldownBlue <= 0)
            {
                cooldownBlue = 5;
                isBlueActive = true;
            }
        }
    }

    void UpdateRedCooldown()
    {
        if (isRedActive == false)
        {
            cooldownRed -= Time.deltaTime;
            if (cooldownRed <= 0)
            {
                cooldownRed = 5;
                isRedActive = true;
            }
        }
    }
}
