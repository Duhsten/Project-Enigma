using ProjectEnigma.Cards;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class CardGameManager : MonoBehaviour
{
    public Animator AnimController; 
    private Vector3 position;
    private Quaternion rotation;
    private float modifer_1 = .35f;
    public GameObject[] playerBoardObjects;
    public GameObject[] opponentBoardObjects;
    public Card[] playerHandObjects;
    private System.DateTime gameStart;
    private System.TimeSpan gameClock;
    [Header("Game Info")]
    public int turn;
    public PlayerTurn playerTurn;
    public CardGamePlayer localPlayer;
    public CardGamePlayer enemyPlayer;
    public GameObject handView;
    [Header("Hand")]
    public CardBase selectedHandCard;
    [Header("Enemy AI")]
    public AIDifficulty aIDifficulty;
    [Header("UI")]
    public TMPro.TMP_Text playerHealthUI;
    public TMPro.TMP_Text opponentHealthUI;
    public TMPro.TMP_Text turnNumberUI;
    public TMPro.TMP_Text playersTurnUI;
    public TMPro.TMP_Text gameTimeUI;
    public GameObject cardDescriptionUI;
    public Card cardDescriptionCard;
    public void Start()
    {
        SetupGame();
        StartGame();
        SwitchView(GameView.Board);
    }
    public void SetupGame()
    {
        localPlayer.InitDeck();
        enemyPlayer.InitDeck();
        enemyPlayer.InitHand();
        localPlayer.InitHand();
    }
    public void StartGame()
    {
        gameStart = System.DateTime.Now;
        StartCoroutine(GameLoop());
    }
    private IEnumerator GameLoop()
    {
        while(CanGameStillRun())
        {
            if(playerTurn == PlayerTurn.Player)
            {
                AwaitPlayerTurn();
            }
            else
            {
                PlayEnemyTurn();
            }
            gameClock = System.DateTime.Now - gameStart;
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Game has ended");

        yield return new WaitForSeconds(2);
    }
    private bool CanGameStillRun()
    {
        if(localPlayer.playerHealth > 0 && enemyPlayer.playerHealth > 0)
            return true;
        else
            return false;
    }
    public void PerformAttack(CardGamePlayer localPlayer, int userCardIndex, CardGamePlayer opposerPlayer, int opposerCardIndex)
    {
        MinionCard user = localPlayer.playerBoard[userCardIndex].CurrentCard;
        MinionCard opposer = opposerPlayer.playerBoard[opposerCardIndex].CurrentCard;
        if (user.Attack > opposer.Defense)
        {
            opposer.Defense = user.Attack - opposer.Defense;
        }
        else if (user.Attack == opposer.Defense)
        {
            opposer.Defense = (int)(user.Attack * modifer_1);
            user.Defense = (int)(user.Attack * modifer_1);
        }
        else if (user.Attack < opposer.Defense)
        {
            user.Defense = user.Defense - opposer.Defense;
        }
        if (user.Defense < 0)
        {
            localPlayer.UpdateHealth(user.Defense);
            localPlayer.playerBoard[userCardIndex] = null;
        }
        if (opposer.Defense < 0)
        {
            opposerPlayer.UpdateHealth(opposer.Defense);
            opposerPlayer.playerBoard[userCardIndex] = null;
        }
    }
    void Update()
    {
        if (AnimController.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            position = Camera.main.transform.position;
            rotation = Camera.main.transform.rotation;
        }

        Camera.main.transform.position = position;
        Camera.main.transform.rotation = rotation;
        if (Input.GetKey(KeyCode.Escape))
        {
            if (handView.activeSelf)
            {
                handView.SetActive(false);
                SwitchView(GameView.Board);
            }
            if(cardDescriptionUI.activeSelf)
                cardDescriptionUI.SetActive(false);
            SwitchView(GameView.Board);

        }
        if(playerTurn == PlayerTurn.Player)
        {
            playersTurnUI.text = "Your Turn";
        }
        else
        {
            playersTurnUI.text = "Opponents Turn";
        }
        turnNumberUI.text = $"Turn {turn}";
        playerHealthUI.text = $"Your Health: {localPlayer.playerHealth}";
        opponentHealthUI.text = $"Opponent Health: {enemyPlayer.playerHealth}";
        string formattedTime = "";
        if (gameClock.Hours > 0)
        {
            formattedTime += gameClock.Hours + ":";
        }
        formattedTime += gameClock.Minutes.ToString("00") + ":" + gameClock.Seconds.ToString("00");

        gameTimeUI.text = formattedTime;
    }
    private void AwaitPlayerTurn()
    {

    }
    private void EndPlayerTurn()
    {
        playerTurn = PlayerTurn.Enemy;
        turn++;
    }
    private void PlayEnemyTurn()
    {
        if(enemyPlayer.CanAddCardToHand())
        {
            Debug.Log("EnemyAI: Added Card To Hand");
            enemyPlayer.CanAddCardToHand();
        }
        if(enemyPlayer.CanAddCardToBoard())
        {
            Debug.Log("EnemyAI: Added Card To Board");
            List<CardBase> hCards = enemyPlayer.playerHand.Where(d => d.Type == CardType.Hero).ToList();
            List<int> freeBoardSpots = enemyPlayer.GetFreeBoardIndexes();
            enemyPlayer.AddCardFromHandToBoard((MinionCard)hCards[Random.Range(0, hCards.Count-1)], freeBoardSpots[Random.Range(0, freeBoardSpots.Count-1)]);
        }
        List<MinionCard> AttackableCards = enemyPlayer.GetAttackableCards();
        List<MinionCard> CardThatCanAttack = enemyPlayer.GetCardsThatCanAttack();

        if(CardThatCanAttack.Count > 0)
            if(AttackableCards.Count > 0)
            {
                Debug.Log($"EnemyAI: Attacking Player");
                    
            }
        Debug.Log("EnemyAI: Ending Turn");
        playerTurn = PlayerTurn.Player;
    }

    public void CardSelectedInHand(CardBase card)
    {
        selectedHandCard = card;
        Debug.Log(card);
        handView.SetActive(false);
    }
    public void ViewCardDescription(MinionCard card)
    {
        cardDescriptionUI.SetActive(true);
        cardDescriptionCard.SetCard(card);
    }
    public void PlaceSelectedCardOnBoard(int boardIndex)
    {
        if (localPlayer.playerBoard[boardIndex].CurrentCard is null)
        {
            localPlayer.playerBoard[boardIndex].SetCard((MinionCard)selectedHandCard);
            selectedHandCard = null;
            Debug.Log(boardIndex);
            localPlayer.RenderHand();
        }
        else
        {
            ViewCardDescription((MinionCard)localPlayer.playerBoard[boardIndex].CurrentCard);
        }
    }

    public void EndTurn()
    {
        turn++;
        playerTurn = PlayerTurn.Enemy;
    }
    public void ViewHandUI()
    {
       handView.SetActive(true);

        SwitchView(GameView.Cards);
    }

    public enum GameView
    {
        Board,
        Cards
    }
    public enum PlayerTurn
    {
        Player,
        Enemy
    }
    private void SwitchView(GameView view)
    {
        switch (view)
        {
            case GameView.Board:
                AnimController.SetBool("BoardView", true);
                AnimController.SetBool("CardView", false);
                break;
                case GameView.Cards:
                AnimController.SetBool("BoardView", false);
                AnimController.SetBool("CardView", true);
                break;
        }

    }


    #region EnemyAI
    public enum AIDifficulty
    {
        EASY,
        MEDIUM,
        HARD,
        WTF
    }

    #endregion
}
