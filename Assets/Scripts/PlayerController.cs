using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public GameObject currentVisual; // what they currently look like
    public Transform visualHolder;   // optional, to spawn the character model cleanly

    public void SetCharacter(GameObject characterPrefab)
    {
        if (currentVisual != null)
        {
            foreach (Transform child in visualHolder)
            {
                Destroy(child.gameObject);
            }
            PlayerInput player = gameObject.GetComponent<PlayerInput>();
            player.enabled = false;
        }

        currentVisual = Instantiate(characterPrefab, visualHolder != null ? visualHolder.position : transform.position, Quaternion.identity);
        currentVisual.transform.SetParent(visualHolder != null ? visualHolder : transform);

        Debug.Log("Character instantiated: " + characterPrefab.name);
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
