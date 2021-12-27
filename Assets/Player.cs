using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Camera myCamera;
    private PhotonView view;
    private GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        view = GetComponent<PhotonView>();
        if (view.IsMine)//enable my camera only
            myCamera.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool IsMyTurn()
    {
        return gameManager.players[gameManager.currentTurnPlayerIndex].Equals(this);
    }
}
