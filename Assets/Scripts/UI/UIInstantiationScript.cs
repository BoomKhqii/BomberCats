using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInstantiationScript : MonoBehaviour
{
    public Transform positionUI;
    private bool isOnce = true;

    private void Start() 
    { 
        StartCoroutine(Delay()); // Enable the collider after 5 seconds
        Destroy(gameObject, 10f); 
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject uiPrefab = other.GetComponent<GeneralPlayerController>().UIGameObject; // Instantiate the UI prefab at the player's position
            if (uiPrefab != null)
            {
                other.GetComponent<GeneralPlayerController>().UIComponents(positionUI, isOnce);
                isOnce = false; // Ensure this only happens once
            }
            else
                Debug.LogError("UI prefab not found!");
        }
    }
}
