using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour {

    /* objects used by player controller */
    public static Camera PlayerCam;           // Camera used by the player
    public static GameManager GameManager;   // GameObject responsible for the management of the game
    private Assets.Player player1;
    private Assets.Player player2;
    public static int currentPlayer = 1;

    private movelogger moveLogger = new movelogger();
    private bool winRecorded;



    // Use this for initialization
    void Start ()
    {
        PlayerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); // Find the Camera's GameObject from its tag 
        GameManager = gameObject.GetComponent<GameManager>();
        winRecorded = false;
        moveLogger.startLogging();

        switch (Assets.ApplicationModel.gameType)
        {
            case Assets.ApplicationModel.GameType.AI_V_AI:
                player1 = new Assets.AIPlayer(11000, 11001);
                player2 = new Assets.AIPlayer(12000, 12001);
                break;
            case Assets.ApplicationModel.GameType.AI_V_Player:
                player1 = new Assets.AIPlayer(11000, 11001);
                player2 = new Assets.HumanPlayer(2);
                break;
            case Assets.ApplicationModel.GameType.Player_V_AI:
                player1 = new Assets.HumanPlayer(1);
                player2 = new Assets.AIPlayer(12000, 12001);
                break;
            case Assets.ApplicationModel.GameType.Player_V_Player:
                player1 = new Assets.HumanPlayer(1);
                player2 = new Assets.HumanPlayer(2);
                break;
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (GameManager.gameWinner == 0)
        {
            if (currentPlayer == GameManager.activePlayer)
            {
                if (GameManager.activePlayer == 1)
                {
                    player1.IPlayerMove();
                }
                else
                {
                    player2.IPlayerMove();
                }
            }
            else
            {
                //A player has made a turn. Inform everyone
                player1.ISendPlayersMove(GameManager.lastMove);
                player2.ISendPlayersMove(GameManager.lastMove);
               
                moveLogger.recordMove(GameManager.activePlayer, GameManager.lastMove);

                currentPlayer = GameManager.activePlayer;
            }
        }

        if ((GameManager.gameWinner != 0)&&(!winRecorded))
        {
            moveLogger.recordWin(GameManager.gameWinner);
            winRecorded = true;
        }
    }
}
