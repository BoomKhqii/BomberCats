using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.InputSystem.XR;
using static UnityEditor.PlayerSettings;

public class JunokerController : MonoBehaviour
{
    [SerializeField]
    private CurseEnergyLogic curseEnergy;
    [SerializeField]
    private GeneralPlayerController player;
    [SerializeField]
    private Transform cloneLocation;
    public MeshRenderer invis;

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
    private float durationInvis = 1f; //1, 2, 3 
    private bool heavyOn = false;

    // Juno Jos Jes Juatro
    private float cooldownJunoJosJesJuatro = 30;
    private bool isJunoJosJesJuatroActive = true;
    private float spawnRadius = 5f;

    void Start()
    {
        player = GetComponent<GeneralPlayerController>();
        curseEnergy = GameObject.Find("CE Pool of Junoker").GetComponent<CurseEnergyLogic>();
        controller = GetComponent<CharacterController>();
    }

    public void JunoJos(InputAction.CallbackContext context)
    {
        if (!context.performed || !isJunoJosActive || !curseEnergy.CEReduction(150)) return;

        Instantiate(junosJoCloneObject, new Vector3(
            Mathf.RoundToInt(cloneLocation.position.x),
            1.38f,
            Mathf.RoundToInt(cloneLocation.position.z)), Quaternion.identity); 

        isJunoJosActive = false;
    }

    public void JunoWhereIGo(InputAction.CallbackContext context)
    {
        if (!context.performed || !isJunoWhereIGoActive || !curseEnergy.CEReduction(200)) return;

        Vector3 dashTarget = new Vector3(
            Mathf.RoundToInt(cloneLocation.position.x),
            1.38f,
            Mathf.RoundToInt(cloneLocation.position.z)) 
            + transform.forward * 2f;

        // Check only the target position for bedrock
        bool isBlocked = Physics.CheckSphere(dashTarget, 0.3f, blocked);

        if (!isBlocked)
        {
            Instantiate(junoWhereIGoCloneObject, new Vector3(
            Mathf.RoundToInt(cloneLocation.position.x),
            1.38f,
            Mathf.RoundToInt(cloneLocation.position.z)), Quaternion.identity);
            heavyOn = true;
            IsJunoInvis(heavyOn);

            controller.enabled = false;
            transform.position = dashTarget;
            controller.enabled = true;

            isJunoWhereIGoActive = false;
        } else { return; }
    }

    public void IsJunoInvis(bool onGoing)
    {
        if(onGoing)
        {
            invis.enabled = false;
            durationInvis -= Time.deltaTime;
            if(durationInvis <= 0)
            {
                durationInvis = 1; // with skill increment
                invis.enabled = true;
                heavyOn = false;
            }
        }
    }

    public void JunoJosJesJuatro(InputAction.CallbackContext context)
    {
        if (!context.performed || !isJunoWhereIGoActive || !curseEnergy.CEReduction(600)) return;
        // Radius from when button pressed
        // invis for duration
            // spawn three clones + player

        isJunoJosJesJuatroActive = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    void Update()
    {
        IsJunoInvis(heavyOn);

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
