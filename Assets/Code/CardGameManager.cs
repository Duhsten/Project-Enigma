using ProjectEnigma.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("UI")]
    public TMPro.TMP_Text playerHealthUI;
    public TMPro.TMP_Text opponentHealthUI;
    public TMPro.TMP_Text turnNumberUI;
    public TMPro.TMP_Text playersTurnUI;
    public TMPro.TMP_Text gameTimeUI;
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
                playerTurn = PlayerTurn.Player;
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
        HeroCard user = localPlayer.playerBoard[userCardIndex];
        HeroCard opposer = opposerPlayer.playerBoard[opposerCardIndex];
        if (user.Attack > opposer.Defense)
        {
            opposer.Defense = user.Attack - opposer.Defense;
        }
        else if(user.Attack == opposer.Defense)
        {
            opposer.Defense = (int)(user.Attack * modifer_1);
            user.Defense = (int)(user.Attack * modifer_1);
        }
        else if (user.Attack < opposer.Defense)
        {
            user.Defense = user.Defense - opposer.Defense;
        }
        if(user.Defense < 0)
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

   
    
}
