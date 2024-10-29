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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey("q") && !isCoroutineRunning)
        {
            StartCoroutine(waiter());
        }
        
    }
    IEnumerator waiter()
    {
        isCoroutineRunning = true;
        Instantiate(bomb, playerLocation.position, transform.rotation);
        yield return new WaitForSeconds(0.2f);
        isCoroutineRunning = false;
    }
}
