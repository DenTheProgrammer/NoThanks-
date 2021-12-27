using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class Deck : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;
    private List<int> valuesList;
    private GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        GenerateDeck();
        Shuffle();
        //LogDeck();
        ThrowAwayCards(9);
        LogDeck();
    }


    private void GenerateDeck()//generate 1-35 cards deck
    {
        valuesList = new List<int>();
        for (int i = 1; i < 36; i++)
        {
            valuesList.Add(i);
            /*Card newCard = new Card();
            newCard.SetValue(i);
            valuesList.Add(newCard);*/
        }
    }

    public void DealTopCard(Transform dealWhere)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        GameObject newCard = PhotonNetwork.Instantiate(cardPrefab.name, dealWhere.position, dealWhere.rotation);
        newCard.GetComponent<Card>().SetValue(valuesList[0]);
        valuesList.RemoveAt(0);
        Debug.LogError($"<color=black>Card with value {newCard.GetComponent<Card>().Value} dealt on the table</color>");
    }

    private void Shuffle()
    {
        System.Random rnd = new System.Random();
        List<int> randomized = valuesList.OrderBy(item => rnd.Next()).ToList();
        valuesList = randomized;
    }

    private void ThrowAwayCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            valuesList.RemoveAt(0);
        }
    }

    private void LogDeck()
    {
        foreach (int value in valuesList)
        {
            Debug.LogError($"<color=black>{value}</color>");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState == GameState.InProgress && !PhotonNetwork.IsMasterClient)//only master client deals with the deck!
        {
            Destroy(this);//only master client have this script 
        }
    }
}
