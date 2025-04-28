using System.Collections;
using System.Collections.Generic;
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
        playerData = new PlayerData[players.Count];
        for (int i = 0; i < players.Count; i++)
        {
            playerData[i] = new PlayerData(players[i].playerIndex, players[i].GetSelectedCharacter());
        }

        // Load the game scene and wait for it to load completely
        SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to the sceneLoaded event
        SceneManager.LoadScene("SampleScene");

        // Store the player data so it can be used after the scene is loaded
        //this.playerData = playerData; // Assuming you store this as a class-level variable

        // Load the game scene
        //SceneManager.LoadScene("SampleScene");

        // After loading the scene, spawn characters
        //SpawnCharacters(playerData);
    }

    // This method will be called when the scene is finished loading
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("called");
        // Only perform the character spawning if the correct scene has been loaded
        if (scene.name == "SampleScene")
        {
            SpawnCharacters(playerData);  // Spawn the characters
            SceneManager.sceneLoaded -= OnSceneLoaded;  // Unsubscribe from the event to avoid multiple calls
        }
    }

    private void SpawnCharacters(PlayerData[] playerData)
    {
        Debug.Log("Spawning " + playerData.Length + " players.");

        for (int i = 0; i < playerData.Length; i++)
        {
            // Ensure we're not trying to spawn more than available players
            if (i < inGameSpawnPoints.Length)
            {
                PlayerData data = playerData[i];
                Debug.Log("Spawning player " + i + ": Character ID = " + data.characterID + " at spawn point " + i);

                // Spawn character at the correct spawn point
                GameObject character = Instantiate(characterPrefabs[data.characterID],
                    inGameSpawnPoints[data.playerIndex].transform.position, Quaternion.identity);

                // Additional properties can be set for each character here
            }
            else
            {
                Debug.LogWarning("Player index " + i + " is out of bounds of spawn points!");
            }
        }
        /*
        Debug.Log(playerData.Length);
        for (int i = 0; i < playerData.Length; i++)
        {
            PlayerData data = playerData[i];
            GameObject character = Instantiate(characterPrefabs[data.characterID], inGameSpawnPoints[data.playerIndex].transform.position, Quaternion.identity);
            // You can also set additional properties for each character here
        }*/
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
