using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlowController : MonoBehaviour
{
    private Transform slow;
    public LayerMask playerLayer;

    private float slowEffect = 3.5f;

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
            }
            else
            {
                Debug.LogWarning("No PlayerController found on: " + hit.name);
            }
        }
    }

    void UpdateSlowEffect()
    {
        Collider hit = Physics.OverlapBox(slow.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, playerLayer).FirstOrDefault();
        if (hit != null && hit.CompareTag("Player"))
        {
            GeneralPlayerController movement = hit.GetComponent<GeneralPlayerController>();
            if (movement != null)
            {
                movement.playerSpeed = 4.5f;
            }
            else
            {
                Debug.LogWarning("Slow | No PlayerController found on: " + hit.name);
            }
        }
    }

    private void Update()
    {
        
    }

    IEnumerator waiter()
    {
        //Debug.Log(player.isPlayerAlive);
        DidKill();
        yield return new WaitForSeconds(1);
        UpdateSlowEffect();
        Destroy(gameObject);
    }
}
