using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunoWhereIGoLogic : MonoBehaviour
{
    private float duration = 3; // 3f, 4f, 5f  
    void Start()
    {
        Destroy(gameObject, duration);
    }
}
