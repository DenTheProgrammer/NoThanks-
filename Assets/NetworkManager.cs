using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    private int minPlayersToStart = 2;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogError("<color=green>Connected to Master server</color>");
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        UpdateRoomInfo();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdateRoomInfo();
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdateRoomInfo();
    }

    private void UpdateRoomInfo()
    {
        Debug.LogError($"<color=green>Number of players in the room: {PhotonNetwork.CurrentRoom.PlayerCount}</color>");
        Debug.LogError($"<color=green>Ready to start - {IsReadyToStart()}</color>");
    }

    private bool IsReadyToStart()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount >= minPlayersToStart;
    }

    public void TryStartGame()
    {
        if (IsReadyToStart())
        {
            //gameManager.StartGame();
            gameManager.view.RPC("StartGame", RpcTarget.All);
            //hide non-game UI
            GameObject.Find("StartGameButton").SetActive(false);
        }
        else
        {
            Debug.LogError("Not Ready to start...");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
