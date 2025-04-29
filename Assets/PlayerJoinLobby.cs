using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinLobby : MonoBehaviour
{
    public int playerIndex;
    private bool isReady = false;
    private int selectedCharacter;

    // This method runs when the player joins the lobby
    private void Start()
    {
        playerIndex = GetComponent<PlayerInput>().playerIndex;
        LobbyManager.instance.OnPlayerJoined(GetComponent<PlayerInput>());
    }

    // This method is called when the ready button is pressed
    public void OnReadyButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isReady = !isReady; // Toggle the readiness state
            LobbyManager.instance.UpdateReadyState(); // Notify the lobby manager to check readiness
            Debug.Log("Player " + playerIndex + (isReady ? " is Ready" : " is Not Ready"));
        }
    }

    public bool IsReady()
    {
        return isReady;
    }

    public void SetSelectedCharacter(int characterID)
    {
        selectedCharacter = characterID;
    }

    public int GetSelectedCharacter()
    {
        return selectedCharacter;
    }
}
