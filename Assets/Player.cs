using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Camera myCamera;
    public PhotonView view;
    private GameManager gameManager;
    public int passTokens = 1;
    private PlayerHand hand;
    
    void Start()
    {
        hand = GetComponent<PlayerHand>();
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


    public void PlayPassToken()
    {
        view.RPC("PlayPassTokenRPC", RpcTarget.All);
    }
    [PunRPC]
    private void PlayPassTokenRPC()
    {
        if (passTokens > 0)
        {
            passTokens--;
            gameManager.passTokensOnCurrentCard++;
            gameManager.PassTurnToNextPlayer();
        }
    }

    public void TakeCard()
    {
        view.RPC("TakeCardRPC", RpcTarget.All);
    }
    [PunRPC]
    private void TakeCardRPC()
    {
        passTokens += gameManager.passTokensOnCurrentCard;
        gameManager.passTokensOnCurrentCard = 0;
        hand.AddToHand(gameManager.cardOnTheTable);
        gameManager.ChangeTurnState(TurnState.DrawingNextCard);
    }

    
}
