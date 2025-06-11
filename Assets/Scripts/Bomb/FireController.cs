using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class FireController : MonoBehaviour
{
    private Transform fire;
    private ParticleSystem parts;
    public LayerMask playerLayer;

    void Start()
    {
        fire = GetComponent<Transform>();
        parts = GetComponent<ParticleSystem>();

        DidKill();
        Destroy(gameObject, parts.duration + parts.startLifetime);
        //StartCoroutine(waiter());
    }

    void DidKill()
    {
        Collider hit = Physics.OverlapBox(fire.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, playerLayer).FirstOrDefault();
        if (hit != null && hit.CompareTag("Player"))
        {
            ObjectStatus objectStatus = hit.GetComponent<ObjectStatus>();
            if (objectStatus != null)
            {
                objectStatus.StatusUpdate(false); // Call to update player status to false (dead)
            }
            else
            {
                Debug.LogWarning("No PlayerController found on: " + hit.name);
            }
        }
    }

    IEnumerator waiter()
    {
        //Debug.Log(player.isPlayerAlive);
        DidKill();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
