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
        if (scene.name == "SampleScene")
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            var playerInputs = FindObjectsOfType<PlayerInput>();
            for (int i = 0; i < playerInputs.Length; i++)
            {
                var input = playerInputs[i];
                var joinLobby = input.GetComponent<PlayerJoinLobby>();

                if (joinLobby != null)
                {
                    int chosenCharacter = playerData[i].characterID;
                    var controller = input.GetComponent<PlayerController>();
                    if (controller != null)
                    {
                        controller.SetCharacter(LobbyManager.instance.GetCharacterPrefab(chosenCharacter));
                        Debug.Log($"Spawning player {i}: char id {chosenCharacter} at spawn point {i}");
                    }
                }
            }
        }
    }

    public GameObject GetCharacterPrefab(int id)
    {
        return characterPrefabs[id];
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
