using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStatus : MonoBehaviour
{
    private bool currentStatus;

    public bool StatusUpdate(bool currentStatus)
    {

        if (currentStatus == false)
        {
            Destroy(gameObject);
            return false;
        }
        else
            return true;
    }
}
