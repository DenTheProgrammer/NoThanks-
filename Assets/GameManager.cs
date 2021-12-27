using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Transform[] sitPoints;
    [SerializeField]
    private GameObject playerPrefab;
    public Player[] players;
    public GameState gameState;
    //public Player currentTurnPlayer;
    public int currentTurnPlayerIndex;
    public PhotonView view;
    
    void Start()
    {
        gameState = GameState.WaitingToStart;
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.WaitingToStart:
                break;
            case GameState.SpawningPlayers:

                Player[] spawnedPlayers = FindObjectsOfType<Player>();
                Debug.LogError($"spawned players count - {spawnedPlayers.Length}");
                if (PhotonNetwork.CurrentRoom.PlayerCount == spawnedPlayers.Length)
                {
                    //All players spawned
                    players = spawnedPlayers;
                    Debug.LogError($"<color=green>All {PhotonNetwork.CurrentRoom.PlayerCount} players spawned</color>");
                    SortByViewNumber();
                    currentTurnPlayerIndex = 0;
                    PlacePlayers();
                    ChangeGameState(GameState.InProgress);

                }
                break;
            
            case GameState.InProgress:
                
                break;
            case GameState.Scoring:
                break;
            default:
                break;
        }
    }

    private void SortByViewNumber()
    {
        /*Debug.LogError($"<color=black>Unsorted view ID's</color>");
        for (int i = 0; i < players.Length; i++)
        {
            Debug.LogError($"<color=black>{i}: {players[i].GetComponent<PhotonView>().ViewID}</color>");
        }*/
        players = players.OrderBy(player => player.GetComponent<PhotonView>().ViewID).ToArray();
        /*Debug.LogError($"<color=black>Sorted ID's</color>");
        for (int i = 0; i < players.Length; i++)
        {
            Debug.LogError($"<color=black>{i}: {players[i].GetComponent<PhotonView>().ViewID}</color>");
        }*/
    }

    private void PassTurnToNextPlayer()
    {
        currentTurnPlayerIndex = (currentTurnPlayerIndex + 1) % players.Length;
    }

    private void PlacePlayers()//for master client to execute?
    {
        
        for (int i = 0; i < players.Length; i++)
        {
            players[i].gameObject.transform.position = sitPoints[i].position;
            players[i].gameObject.transform.rotation = sitPoints[i].rotation;
        }
        Debug.LogError($"<color=green>Player placement is complete</color>");
    }

    [PunRPC]
    public void StartGame()
    {
        ChangeGameState(GameState.SpawningPlayers);
        //photon instantiate player
        GameObject newPlayerObject = PhotonNetwork.Instantiate(playerPrefab.name, sitPoints[0].position, sitPoints[0].rotation);
   
        Debug.LogError("<color=green>The Game is Started!</color>");
    }

    private void ChangeGameState(GameState newState)
    {
        Debug.LogError($"<color=blue>Game state has been changed from {gameState} to {newState}</color>");
        gameState = newState;
        
    }

 
}

public enum GameState
{
    WaitingToStart,
    SpawningPlayers,
    //PlacingPlayers,
    InProgress,
    Scoring
}
