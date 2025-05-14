using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCamera());
    }

    IEnumerator StartCamera()
    {
        yield return new WaitForSeconds(10f);
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
