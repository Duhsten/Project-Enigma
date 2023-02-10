using ProjectEnigma.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGamePlayer : MonoBehaviour
{
    public HeroCard[] playerBoard = new HeroCard[5];
    public int playerHealth = 20;
    private Stack<CardBase> playerDeck = new Stack<CardBase>();
    public List<CardBase> playerHand;
    public bool isPlayer;
    private int maxHandCount = 5;
    private CardGameManager manager;

    public void Start()
    {
        manager = GameObject.FindGameObjectWithTag("CardGameManager").GetComponent<CardGameManager>();
    }
    public bool UpdateHealth(int h)
    {
        if ((playerHealth - h) < 0)
        {
            return false;
        }
        else
        {
            playerHealth += h;
            return true;
        }
    }

    public void InitHand()
    {
        playerHand = new List<CardBase>();
        for (int i = 0; i < maxHandCount; i++)
        {
            PullCardFromDeckToHand();
        }

    }
    public void InitDeck()
    {
        playerDeck = new Stack<CardBase>();
        foreach (var card in LoadCards())
        {
            playerDeck.Push(card);
        }

    }
    public List<CardBase> LoadCards()
    {
        List<CardBase> cards = new List<CardBase>();
        if (System.IO.File.Exists("Assets/Data/Cards/cards.cpk"))
        {
            using (System.IO.Stream stream = System.IO.File.Open("Assets/Data/Cards/cards.cpk", System.IO.FileMode.Open))
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                cards = (List<CardBase>)bin.Deserialize(stream);
            }
        }
        return cards;
    }
    public void PullCardFromDeckToHand()
    {
        if (playerHand.Count < maxHandCount && playerDeck.Count > 0)
        {
            playerHand.Add(playerDeck.Pop());
        }
        if (isPlayer)
        {
            RenderHand();
        }
    }
    public void RenderHand()
    {
        int i = 0;
        foreach (CardBase card in playerHand)
        {
            manager.playerHandObjects[i].SetCard((HeroCard)card);
            i++;
        }
    }
}
