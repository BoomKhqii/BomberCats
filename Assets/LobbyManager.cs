using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    private List<PlayerJoinLobby> players = new List<PlayerJoinLobby>();

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
