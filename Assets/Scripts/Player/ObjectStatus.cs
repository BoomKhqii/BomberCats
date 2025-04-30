using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStatus : MonoBehaviour
{
    private bool currentStatus;
    private OttoGojoController player;
    public bool isOtto;
    public bool isInvulnerable = false;

    public void StatusUpdate(bool currentStatus)
    {
        if (currentStatus == false && !isInvulnerable)
        {
            // exclusive for Gojo
            if (isOtto)
            {
                player = gameObject.GetComponent<OttoGojoController>();
                if (player.InfinityProbabilityChance() == true)
                    return;
                else
                    Destroy(gameObject);

            }
            else
                Destroy(gameObject);
        }
        else
            return;
        /*
        if (currentStatus == false)
        {
            Destroy(gameObject);
            return false;
        }
        else
            return true;
        */
    }
}
