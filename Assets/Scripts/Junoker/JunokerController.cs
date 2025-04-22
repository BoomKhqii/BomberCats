using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class JunokerController : MonoBehaviour
{
    [SerializeField]
    private CurseEnergyLogic curseEnergy;
    [SerializeField]
    private GeneralPlayerController player;
    [SerializeField]
    private Transform cloneLocation;
    [SerializeField]
    private GameObject junosJoCloneObject;

    // Juno Jo's
    private float cooldownJunoJos = 3;
    private bool isJunoJosActive = true;

    // Juno Where I Go?
    [SerializeField]
    private float cooldownJunoWhereIGo = 5;
    private bool isJunoWhereIGoActive = true;

    void Start()
    {
        player = GetComponent<GeneralPlayerController>();
        curseEnergy = GameObject.Find("CE Pool of Junoker").GetComponent<CurseEnergyLogic>();
    }

    public void JunoJos(InputAction.CallbackContext context)
    {
        if (!context.performed || !isJunoJosActive || !curseEnergy.CEReduction(150)) return;

        Instantiate(junosJoCloneObject, new Vector3(
            Mathf.RoundToInt(cloneLocation.position.x),
            0.9160001f,
            Mathf.RoundToInt(cloneLocation.position.z)), Quaternion.identity);

        isJunoJosActive = false;
    }

    public void JunoWhereIGo(InputAction.CallbackContext context)
    {
        if (!context.performed || !isJunoWhereIGoActive || !curseEnergy.CEReduction(200)) return;
        //instatiate clone


        isJunoWhereIGoActive = false;
    }

    void Update()
    {
        UpdateJunoJosCooldown();
    }
    void UpdateJunoJosCooldown()
    {
        Debug.Log(cooldownJunoJos);
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
}
