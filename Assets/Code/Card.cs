using ProjectEnigma.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public MinionCard CurrentCard;

    public void Start()
    {
        
    }

    public void SetCard(MinionCard card)
    {
        Debug.Log("Hit");
        CurrentCard = card;
        GetComponent<CardRenderer>().RenderCard(card);
    }
    public MinionCard GetCard()
    {
        return CurrentCard;
    }

    public void SelectCard()
    {
        GameObject.FindGameObjectWithTag("CardGameManager").GetComponent<CardGameManager>().CardSelectedInHand(CurrentCard);
    }
    public void PlaceCard(int i)
    {
        GameObject.FindGameObjectWithTag("CardGameManager").GetComponent<CardGameManager>().PlaceSelectedCardOnBoard(i);
    }
}
