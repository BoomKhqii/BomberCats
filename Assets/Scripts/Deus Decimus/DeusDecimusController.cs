using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class DeusDecimusController : MonoBehaviour
{

    [SerializeField]
    private CurseEnergyLogic curseEnergy;
    [SerializeField]
    private GeneralPlayerController player;

    void Start()
    {
        player = GetComponent<GeneralPlayerController>();
        curseEnergy = GameObject.Find("CE Pool of Deus Decimus").GetComponent<CurseEnergyLogic>();
    }

    void Update()
    {
        
    }
}
