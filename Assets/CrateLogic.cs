using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateLogic : MonoBehaviour
{
    public LayerMask destroy;

    public void CrateDrop()
    {
        /* 
            Potential Upgrades
                Bomb
                Signature
                Heavy
                Ultimate
        */
        // Instantiate();

        Destroy(this.gameObject);
    }
}
