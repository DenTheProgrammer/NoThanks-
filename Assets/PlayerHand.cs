using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerHand : MonoBehaviour
{

    private List<Card> hand;
    private List<CardPile> cardPiles;
    public Transform handTransform;
    public GameObject cardPrefab;
    private float cardWidth;
    private float cardLength;
    private float cardThickness;
    public float pilesGapWidth = 0.03f;
    // Start is called before the first frame update
    void Start()
    {
        hand = new List<Card>();
        cardPiles = new List<CardPile>();
        cardWidth = cardPrefab.gameObject.transform.localScale.x;
        cardThickness = cardPrefab.gameObject.transform.localScale.y;
        cardLength = cardPrefab.gameObject.transform.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToHand(Card card)
    {
        hand.Add(card);
        
        UpdateHand();
        LogHand();
        Debug.LogError($"<color=yellow>There are {cardPiles.Count} piles currently</color>");
    }

    private void UpdateHand()
    {
        /*Debug.LogError($"<color=yellow>{hand.Count} cards in hand</color>");
        foreach (Card card in hand)
        {
            Debug.Log(card.gameObject);
            Debug.Log(card.gameObject.transform);
            Debug.Log(handTransform.position);
            card.transform.position = handTransform.position;
            card.transform.rotation = handTransform.rotation;
        }*/
        SortHandToPiles();
        PlacePilesOnTheTable();
    }

    private void LogHand()
    {
        Debug.LogError($"<color=yellow>Cards in hand:</color>");
        foreach (var card in hand)
        {
            Debug.LogError($"<color=yellow>{card.Value}</color>");
        }
    }

    private void SortHandToPiles()
    {
        hand = hand.OrderBy(card => card.Value).ToList();
        List<CardPile> newCardPileList = new List<CardPile>();//!!
        foreach (Card card in hand)
        {
            bool newPileNeeded = true;
            foreach (CardPile pile in newCardPileList)
            {
                if (pile.lowestValue-card.Value == 1 || card.Value-pile.highestValue == 1)//adjusted numbers
                {
                    pile.AddCardToPile(card);
                    newPileNeeded = false;
                }
            }
            if (newPileNeeded)//not one pile is suitable for this card
            {
                CardPile newCardPile = new CardPile();
                newCardPile.AddCardToPile(card);
                newCardPileList.Add(newCardPile);
            }
        }
        cardPiles = newCardPileList;
    }

    private void PlacePilesOnTheTable()
    {
        float allPilesWidth = (cardPiles.Count * cardWidth) + ((cardPiles.Count - 1) * pilesGapWidth);
        Vector3 nextPilePosition = handTransform.position;
        Quaternion nextPileRotation = handTransform.rotation;
        //nextPilePosition -= new Vector3((allPilesWidth - cardWidth)/2f, 0, 0);
        nextPilePosition -= handTransform.right.normalized * ((allPilesWidth - cardWidth) / 2f);
        foreach (CardPile pile in cardPiles)
        {
            PlaceOnePileOnTable(pile, nextPilePosition, nextPileRotation);
            //nextPilePosition += new Vector3(cardWidth + pilesGapWidth, 0, 0);
            nextPilePosition += handTransform.right.normalized * (cardWidth + pilesGapWidth);
        }
    }

    private void PlaceOnePileOnTable(CardPile pile, Vector3 pos, Quaternion rot)
    {
        List<Card> cards = pile.cards;
        foreach (Card card in cards)
        {
            card.transform.position = pos;
            card.transform.rotation = rot;
        }
    }
}

class CardPile
{
    public List<Card> cards;
    public int lowestValue;
    public int highestValue;

    public CardPile()
    {
        cards = new List<Card>();
    }

    public void AddCardToPile(Card card)
    {
        cards.Add(card);
        cards = cards.OrderBy(card => card.Value).ToList();
        lowestValue = cards[0].Value;
        highestValue = cards[cards.Count - 1].Value;
    }
}
