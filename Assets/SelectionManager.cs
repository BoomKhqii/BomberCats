using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;

    public PlayerJoinLobby lobby;

    private void Start()
    {
        // Initialize the lobby reference
        //lobby = gameObject.GetComponent<PlayerJoinLobby>(); // Get the PlayerJoinLobby component attached to the same GameObject
    }

    // NExt and previous
    public void Next(InputAction.CallbackContext context)
    {
        if(!context.performed) return;

        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
        lobby.SetSelectedCharacter(selectedCharacter);
    }

    public void Previous(InputAction.CallbackContext context) 
    {
        if (!context.performed) return;

        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].SetActive(true);
        lobby.SetSelectedCharacter(selectedCharacter);
    }
}
