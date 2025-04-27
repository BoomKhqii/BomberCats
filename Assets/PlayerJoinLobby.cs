using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinLobby : MonoBehaviour
{
    public GameObject[] spawnPoints;

    private bool isReady = false;
    private int playerIndex;

    private void Start()
    {
        playerIndex = GetComponent<PlayerInput>().playerIndex;
        LobbyManager.instance.RegisterPlayer(this);
    }

    public void OnReadyButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isReady = !isReady;
            LobbyManager.instance.UpdateReadyState();
            Debug.Log("Player " + playerIndex + (isReady ? " is Ready" : " is Not Ready"));
        }
    }

    public bool IsReady()
    {
        return isReady;
    }

    //Spawn point
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int index = playerInput.playerIndex;
        Debug.Log(index);
        if (index < spawnPoints.Length)
        {
            playerInput.transform.position = spawnPoints[index].transform.position;
        }
    }
}
