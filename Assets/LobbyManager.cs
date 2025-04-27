using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject[] spawnPoints;

    public static LobbyManager instance;
    private List<PlayerJoinLobby> players = new List<PlayerJoinLobby>();

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player joined: " + playerInput.playerIndex);
        // Your logic here (like setting their corner, etc)

        Debug.Log("Player joined: " + playerInput.playerIndex);

        if (playerInput.playerIndex < spawnPoints.Length)
        {
            // Move the spawned Player to their assigned spawn point
            playerInput.transform.position = spawnPoints[playerInput.playerIndex].transform.position;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterPlayer(PlayerJoinLobby player)
    {
        players.Add(player);
    }

    public void UpdateReadyState()
    {
        foreach (var player in players)
        {
            if (!player.IsReady())
                return;
        }

        // All players ready
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("All players ready. Starting game...");
        // SceneManager.LoadScene("YourGameSceneName");
    }
}
