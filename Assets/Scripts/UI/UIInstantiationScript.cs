using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInstantiationScript : MonoBehaviour
{
    public Transform positionUI;

    private void Start() { Destroy(gameObject, 5f); }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Instantiate the UI prefab at the player's position
            GameObject uiPrefab = other.GetComponent<GeneralPlayerController>().UIGameObject;
            if (uiPrefab != null)
            {
                other.GetComponent<GeneralPlayerController>().UIComponents(positionUI);
                //Instantiate(uiPrefab, positionUI.position, Quaternion.Euler(80, 0, 0));
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("UI prefab not found!");
            }
        }
    }
}
