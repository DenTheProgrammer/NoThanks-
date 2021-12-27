using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UImanager : MonoBehaviourPunCallbacks
{
    public Button StartGameButton;
    
    void Start()
    {
        StartGameButton.gameObject.SetActive(false);
    }


    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
            StartGameButton.gameObject.SetActive(true);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
            StartGameButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
