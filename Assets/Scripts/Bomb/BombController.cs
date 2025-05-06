using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class BombController : MonoBehaviour
{
    public float rangePlayer = 3;

    public GameObject explosionSpawner;
    public LayerMask fire;


    // ghost
    private bool isPlayerInside = true;
    private GameObject spawningPlayer;
    private Collider blockCollider;

    //public float skill = 0;

    public void SetSpawningPlayer(GameObject player, float upgrade) 
    { 
        spawningPlayer = player; // Debug.Log("Spawning player set to: " + spawningPlayer);
        rangePlayer += upgrade;
    }

    void Start()
    {
        blockCollider = GetComponent<Collider>();

        StartCoroutine(waiter());
    }

    void Update()
    {
        if (Physics.CheckSphere(transform.position, 0.1f, fire))
        {
            StopCoroutine(waiter());
            Spawn();
            Destroy(gameObject);
        }
    }
    /*
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInside = false;
            blockCollider.isTrigger = false;
        }
    }
    */
    IEnumerator waiter()
    {
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(0.2f);
        Spawn();
        Destroy(gameObject);
    }

    public void Spawn()
    {
        GameObject spawner = Instantiate(explosionSpawner, transform.position, Quaternion.identity);

        int bombLocationX = Mathf.RoundToInt(transform.position.x);
        spawner.GetComponent<ExplodeSpawner>().locationX = bombLocationX;
        int bombLocationZ = Mathf.RoundToInt(transform.position.z);
        spawner.GetComponent<ExplodeSpawner>().locationZ = bombLocationZ;

        spawner.GetComponent<ExplodeSpawner>().range = rangePlayer;
    }
}