using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] inGameSpawnPoints;
    public GameObject[] characterPrefabs; // Reference to the character prefabs to spawn after scene load
    public static LobbyManager instance;
    private List<PlayerJoinLobby> players = new List<PlayerJoinLobby>();

    private PlayerData[] playerData;

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
            if (!player.IsReady())
                return;
        }
        StartGame();
    }

    // When all players are ready, start the game and load the next scene
    private void StartGame()
    {
        Debug.Log("All players ready. Starting game...");

        // Store player selections and spawn points
        playerData = new PlayerData[players.Count];
        for (int i = 0; i < players.Count; i++)
        {
            playerData[i] = new PlayerData(players[i].playerIndex, players[i].GetSelectedCharacter());
        }
        SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to the sceneLoaded event
        SceneManager.LoadScene("SampleScene");
    }

    // This method will be called when the scene is finished loading
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        /*
        if (scene.name == "SampleScene")
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            // Get the players that you already tracked
            for (int i = 0; i < players.Count; i++)
            {
                var playerLobby = players[i];
                var playerInput = playerLobby.GetComponent<PlayerInput>();

                // Move player to correct spawn
                playerInput.transform.position = inGameSpawnPoints[i].transform.position;

                // Change player character appearance
                var PlayerJoinLobby = playerInput.GetComponent<PlayerJoinLobby>();
                if (PlayerJoinLobby != null)
                {
                    PlayerJoinLobby.SetCharacter(characterPrefabs[playerLobby.GetSelectedCharacter()]);
                }
            }
        }
        */
    }

    private void SpawnCharacters(PlayerData[] playerData)
    {
        /*
        Debug.Log(playerData.Length);
        for (int i = 0; i < playerData.Length; i++)
        {
            PlayerData data = playerData[i];
            GameObject character = Instantiate(
                characterPrefabs[data.characterID],
                inGameSpawnPoints[i].transform.position, // notice we use i, not playerIndex
                Quaternion.identity
            );
            Debug.Log($"Spawning player {i}: char id {data.characterID}");
        }
         */

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
}
