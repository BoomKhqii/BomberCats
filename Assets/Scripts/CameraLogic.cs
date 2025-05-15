using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLogic : MonoBehaviour
{
    public GameObject playerInputManager;
    void Start()
    {
        StartCoroutine(StartCamera());
    }

    IEnumerator StartCamera()
    {
        Time.timeScale = 10f;
        yield return new WaitForSeconds(30f);
        Time.timeScale = 1f;
        playerInputManager.SetActive(true);
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
