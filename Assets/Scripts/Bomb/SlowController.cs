using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlowController : MonoBehaviour
{
    private Transform slow;
    public LayerMask playerLayer;

    private float slowEffect = 3f;
    private bool didSlow = false;

    void Start()
    {
        slow = GetComponent<Transform>();
        StartCoroutine(waiter());
    }

    void DidKill()
    {
        Collider hit = Physics.OverlapBox(slow.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, playerLayer).FirstOrDefault();
        if (hit != null && hit.CompareTag("Player"))
        {
            GeneralPlayerController movement = hit.GetComponent<GeneralPlayerController>();
            if (movement != null)
            {
                movement.playerSpeed -= slowEffect;
                didSlow = true;
                UpdateSlowEffect(movement, movement.playerSpeed);
            }
            else
            {
                Debug.LogWarning("No PlayerController found on: " + hit.name);
            }
        }
    }

    void UpdateSlowEffect(GeneralPlayerController component, float speed)
    {
        /*
        if (didSlow == true)
        {
            speed += .5f * Time.deltaTime;
            if (speed == 4.5f)
            {
                component.
                didSlow = false;
            }
        }
        */
    }

    private void Update()
    {
        
    }

    IEnumerator waiter()
    {
        //Debug.Log(player.isPlayerAlive);
        DidKill();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
