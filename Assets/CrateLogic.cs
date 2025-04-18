using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateLogic : MonoBehaviour
{
    public LayerMask destroy;

    public void CrateMechanic()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & destroy) != 0)
        {
            Destroy(gameObject);
        }
    }

    public void CrateDrop()
    {
        /* 
            Potential Upgrades
                Bomb
                Signature
                Heavy
                Ultimate
        */
    }
}
