using ProjectEnigma.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardGamePlayer : MonoBehaviour
{
    public Card[] playerBoard;
    public int playerHealth = 20;
    private Stack<CardBase> playerDeck = new Stack<CardBase>();
    public List<CardBase> playerHand;
    public bool isPlayer;
    private int maxHandCount = 5;
    private CardGameManager manager;
    private int boardMaxCount = 5;
    private bool hasPlacedCardThisRound;

    public void Start()
    {
        manager = GameObject.FindGameObjectWithTag("CardGameManager").GetComponent<CardGameManager>();
        foreach(var card in playerBoard)
        {
            card.CurrentCard = null;
        }
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
    public List<int> GetFreeBoardIndexes()
    {
        List<int> indexes = new List<int>();
        for(int i = 0; i < playerBoard.Length; i++)
        {
            if (playerBoard[i].CurrentCard is null)
            {
                indexes.Add(i);
            }
        }
        return indexes;

    }
    public List<int> GetOpponentCardIndexes()
    {
        List<int> indexes = new List<int>();
        for(int i = 0; i < manager.enemyPlayer.playerBoard.Length; i++)
        {
            if (manager.enemyPlayer.playerBoard[i] is not null)
            {
                indexes.Add(i);
            }
        }
        return indexes;
    }
    public bool AddCardFromHandToBoard(MinionCard hand, int boardIndex)
    {
        if(CanAddCardToBoard() && playerHand.Count > 0 && playerHand.Contains(hand) && playerBoard[boardIndex].CurrentCard is null)
        {
            playerHand.Remove(hand);
            playerBoard[boardIndex].SetCard(hand);
            return true;
        }
        return false;
    }
    public void RenderHand()
    {
        int i = 0;
        foreach (CardBase card in playerHand)
        {
            manager.playerHandObjects[i].SetCard((MinionCard)card);
            i++;
        }
    }

    public bool CanAddCardToBoard()
    {
        if (hasPlacedCardThisRound)
            return false;
        foreach(var card in playerBoard)
        {
            if (card.CurrentCard is null)
                return true;
        }
        return false;
    }
    public bool CanAddCardToHand()
    {
        return playerHand.Count <= maxHandCount-1;
    }
    public List<MinionCard> GetAttackableCards()
    {
        List<MinionCard> result = new List<MinionCard>();
        List<Card> enemyBoard = GetEnemyBoard();
            enemyBoard = manager.localPlayer.playerBoard.ToList();
        foreach(var card in enemyBoard)
        {
            if (card.CurrentCard is not null)
                result.Add(card.CurrentCard);

        }
        return result;
    }
    public List<Card> GetEnemyBoard()
    {
        if (isPlayer)
            return manager.enemyPlayer.playerBoard.ToList();
        else
            return  manager.localPlayer.playerBoard.ToList();
    }

    public List<MinionCard> GetCardsThatCanAttack() 
    {
        List<MinionCard> result = new List<MinionCard>();
        foreach(var card in playerBoard)
        {
            //TODO: Add the logic check for if the card has just been placed down or not.
            if(card.CurrentCard is not null)
                result.Add(card.CurrentCard);
        }
        return result;
    }
}
