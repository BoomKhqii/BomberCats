using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawning : MonoBehaviour
{
    public GameObject bomb;
    public Transform playerLocation;
    BoxCollider location;
    private bool isCoroutineRunning = false;
    public LayerMask playerOnBomb;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("q") && !isCoroutineRunning && !Physics.CheckSphere(playerLocation.position, 0.6f, playerOnBomb))
        {
            StartCoroutine(waiter());
        }
    }

    IEnumerator waiter()
    {
        isCoroutineRunning = true;
        Instantiate(bomb, new Vector3(Mathf.RoundToInt(playerLocation.position.x), 0.9160001f, Mathf.RoundToInt(playerLocation.position.z)), bomb.transform.rotation);
        yield return new WaitForSeconds(0.2f);
        isCoroutineRunning = false;
    }
}
