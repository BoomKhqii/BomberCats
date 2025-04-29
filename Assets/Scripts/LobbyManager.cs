using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject[] spawnPoints;       // for lobby spawn positions
    public GameObject[] inGameSpawnPoints;  // for in-game spawn positions
    public GameObject[] characterPrefabs;   // reference to character prefabs

    public static LobbyManager instance;

    private List<PlayerJoinLobby> players = new List<PlayerJoinLobby>();
    private PlayerData[] playerData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // important: lobby manager survives scene change
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.playerIndex < spawnPoints.Length)
        {
            playerInput.transform.position = spawnPoints[playerInput.playerIndex].transform.position;
        }

        PlayerJoinLobby newPlayer = playerInput.GetComponent<PlayerJoinLobby>();
        players.Add(newPlayer);
        DontDestroyOnLoad(playerInput.gameObject); // <--- very important! survive scene change
        Debug.Log("Players in lobby: " + players.Count);
    }

    public void UpdateReadyState()
    {
        foreach (var player in players)
        {
            if (!player.IsReady())
                return; // if anyone is not ready, do nothing
        }
        StartGame();
    }

    private void StartGame()
    {
        // Save the selections
        playerData = new PlayerData[players.Count];
        for (int i = 0; i < players.Count; i++)
        {
            playerData[i] = new PlayerData(players[i].playerIndex, players[i].GetSelectedCharacter());
        }

        // VERY IMPORTANT: prevent new players from joining
        var inputManager = FindObjectOfType<PlayerInputManager>();
        if (inputManager != null)
        {
            inputManager.enabled = false;
        }

        // Subscribe to when the scene finishes loading
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("SampleScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            // Find all surviving players
            PlayerJoinLobby[] savedPlayers = FindObjectsOfType<PlayerJoinLobby>();
            Debug.Log("Players found after scene load: " + playerData.Length);

            for (int i = 0; i < players.Count; i+=1)
            {
                //var joinLobby = savedPlayers[i];
                var input = players[i].GetComponent<PlayerInput>();

                int chosenCharacter = playerData[i].characterID;
                var controller = input.GetComponent<PlayerController>();
                if (controller != null)
                {
                    Debug.Log("Assigning character to player " + i);
                    controller.SetCharacter(LobbyManager.instance.GetCharacterPrefab(chosenCharacter));
                }

                // OPTIONAL: move players to in-game spawn points
                if (i < inGameSpawnPoints.Length)
                {
                    input.transform.position = inGameSpawnPoints[i].transform.position;
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
