using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] characterPrefabs; // Reference to the character prefabs to spawn after scene load
    public static LobbyManager instance;
    private List<PlayerJoinLobby> players = new List<PlayerJoinLobby>();

    private void Awake()
    {
        if (instance == null)
            instance = this; // Singleton setup to ensure only one instance exists
        else
            Destroy(gameObject);
    }

    // Called when a player joins the lobby
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player joined: " + playerInput.playerIndex);

        if (playerInput.playerIndex < spawnPoints.Length)
        {
            // Assign spawn point for each player
            playerInput.transform.position = spawnPoints[playerInput.playerIndex].transform.position;
        }

        // Register player (create new PlayerJoinLobby object)
        PlayerJoinLobby newPlayer = playerInput.GetComponent<PlayerJoinLobby>();
        players.Add(newPlayer);
    }

    // Called when a player presses the ready button
    public void UpdateReadyState()
    {
        foreach (var player in players)
        {
            if (!player.IsReady()) // If any player is not ready
                return;
        }

        // All players ready, start the game
        StartGame();
    }

    // When all players are ready, start the game and load the next scene
    private void StartGame()
    {
        Debug.Log("All players ready. Starting game...");

        // Store player selections and spawn points
        PlayerData[] playerData = new PlayerData[players.Count];
        for (int i = 0; i < players.Count; i++)
        {
            playerData[i] = new PlayerData(players[i].playerIndex, players[i].GetSelectedCharacter());
        }

        // Load the game scene
        SceneManager.LoadScene("SampleScene");

        // After loading the scene, spawn characters
        SpawnCharacters(playerData);
    }

    private void SpawnCharacters(PlayerData[] playerData)
    {
        for (int i = 0; i < playerData.Length; i++)
        {
            PlayerData data = playerData[i];
            GameObject character = Instantiate(characterPrefabs[data.characterID], spawnPoints[data.playerIndex].transform.position, Quaternion.identity);
            // You can also set additional properties for each character here
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public int playerIndex;
    public int characterID;

    public PlayerData(int playerIndex, int characterID)
    {
        this.playerIndex = playerIndex;
        this.characterID = characterID;
    }

    /*
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
        SceneManager.LoadScene("SampleScene");
    }
    */
}
