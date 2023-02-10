using ProjectEnigma.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public HeroCard CurrentCard;

    public void Start()
    {
        
    }

    public void SetCard(HeroCard card)
    {
        CurrentCard = card;
        GetComponent<CardRenderer>().RenderCard(card);
    }
    public HeroCard GetCard()
    {
        return CurrentCard;
    }


}
