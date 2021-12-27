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
    private List<Card> hand;
    public Transform handTransform;
    
    void Start()
    {
        hand = new List<Card>();
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
        hand.Add(gameManager.cardOnTheTable);
        UpdateHand();
        //gameManager.cardOnTheTable = null;
        gameManager.PassTurnToNextPlayer();
        gameManager.ChangeTurnState(TurnState.DrawingNextCard);
    }

    private void UpdateHand()
    {
        Debug.LogError($"<color=yellow>{hand.Count} cards in hand</color>");
        foreach (Card card in hand)
        {
            Debug.Log(card.gameObject);
            Debug.Log(card.gameObject.transform);
            Debug.Log(handTransform.position);
            card.transform.position = handTransform.position;
            card.transform.rotation = handTransform.rotation;
        }
    }
}
