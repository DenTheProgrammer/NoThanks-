using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class UImanager : MonoBehaviourPunCallbacks
{
    private NetworkManager networkManager;
    public Button StartGameButton;
    public Button NoThanksButton;
    public Button TakeCardButton;
    public TextMeshProUGUI passTokensCountText;
    public TextMeshProUGUI playerTokensCount;
    private GameManager gameManager;
    void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();
        gameManager = FindObjectOfType<GameManager>();
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

    public void OnNoThanksButtonPress()
    {
        gameManager.CurrentPlayer.PlayPassToken();
    }

    public void OnTakeCardButtonPress()
    {
        gameManager.CurrentPlayer.TakeCard();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState == GameState.WaitingToStart)
        {
            if (networkManager.IsReadyToStart())
            {
                StartGameButton.GetComponent<CanvasRenderer>().SetAlpha(1);
            }
            else
            {
                StartGameButton.GetComponent<CanvasRenderer>().SetAlpha(0.1f);
            }
        }
        if (gameManager.gameState == GameState.InProgress)
            passTokensCountText.text = gameManager.passTokensOnCurrentCard.ToString();
        if (gameManager.gameState == GameState.InProgress && gameManager.IsMyClientTurn())
        {
            playerTokensCount.text = "Tokens:" + gameManager.CurrentPlayer.passTokens.ToString();
            //show action buttons
            NoThanksButton.gameObject.SetActive(true);
            TakeCardButton.gameObject.SetActive(true);
            if (gameManager.CurrentPlayer.passTokens > 0)
                NoThanksButton.GetComponent<CanvasRenderer>().SetAlpha(1);
            else
                NoThanksButton.GetComponent<CanvasRenderer>().SetAlpha(0.1f);
        }
        else
        {
            //or hide action buttons
            NoThanksButton.gameObject.SetActive(false);
            TakeCardButton.gameObject.SetActive(false);
        }
    }
}
