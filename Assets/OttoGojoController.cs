using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OttoGojoController : MonoBehaviour
{
    //private bool infinitySkill = Random.value < 0.25f;  // Passive ability | Probability chance: 20% - 25%
    private int signatureSkill = 3;         // Blue
    private int heavySkill = 6;             // Red
    private int ultimateSkill = 12;         // Hollow purple

    void Start()
    {
        
    }

    public bool InfinityProbabilityChance()
    {
        return Random.value < 0.35f;
    }

    public void BlueSkill(int incrementSkill)
    {

    }

    void Update()
    {
        
    }
}
