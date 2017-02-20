using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour {


    //public Font guiFont;
    public static Camera PlayerCam;           // Camera used by the player
    public static GameManager _GameManager;   // GameObject responsible for the management of the game
    private Assets.Player player1;
    private Assets.Player player2;
    public static int currentPlayer = 1;



    private float buttonWidthOffset = (Screen.width / 4) - 100;
    private float buttonHeightOffset = (Screen.height / 4);


    // Use this for initialization
    void Start ()
    {
        PlayerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); // Find the Camera's GameObject from its tag 
        _GameManager = gameObject.GetComponent<GameManager>();

        switch (Assets.ApplicationModel.gameType)
        {
            //TODO add AI connect?
            case Assets.ApplicationModel.GameType.AI_V_AI:
                player1 = new Assets.AIPlayer(11000);
                player2 = new Assets.AIPlayer(11001);
                
                break;
            case Assets.ApplicationModel.GameType.AI_V_Player:
                player1 = new Assets.AIPlayer(11000);
                player2 = new Assets.HumanPlayer(2);
                break;
            case Assets.ApplicationModel.GameType.Player_V_AI:
                player1 = new Assets.HumanPlayer(1);
                player2 = new Assets.AIPlayer(11001);
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
            if (currentPlayer == _GameManager.activePlayer)
            {
                if (_GameManager.activePlayer == 1)
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
                player1.ISendPlayersMove(_GameManager.lastMove);
                player2.ISendPlayersMove(_GameManager.lastMove);

                currentPlayer = _GameManager.activePlayer;
            }
        }
    }

    void OnGUI()
    {


    }

}
