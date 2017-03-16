using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameRules gameRules;
    public List<GameObject> gamePieces;
    public GameObject WhitePiece;
    public GameObject WhiteKing;
    public GameObject BlackPiece;

    public int activePlayer = 1;		// 1 = Player1, 2 = Player2
    public static int gameWinner = 0;  // used by game over screen, public record of gameWon var.  

    private String moveString = "";
    public static string latestMove = "";
    public Vector3[] lastMove;

    public Font guiFont;

    // Use this for initialization
    void Start ()
    {
        gamePieces = new List<GameObject>();
        gameWinner = 0; //reset last winner of game       
        lastMove = new Vector3[2];
        gameRules = new Fetlar(gamePieces, WhitePiece, WhiteKing, BlackPiece);
    }

    public bool MovePiece(GameObject selectedPiece, Vector3 coordMoveTo)
    {
        bool canMove = false;
        if (gameRules.MovePiece(selectedPiece, coordMoveTo, activePlayer))
        {
            //Update last move to be sent to AI players
            lastMove[0] = new Vector3(selectedPiece.transform.position.x, 1, selectedPiece.transform.position.z);
            lastMove[1] = coordMoveTo;
            ChangePlayer();
            canMove = true;
        }
        gameWinner = gameRules.GameWinner;

        return canMove;
    }

    public GameObject selectPiece(int x, int z)
    {
        Debug.Log("selecting game piece...");
        return (from gamePiece in gamePieces
                where gamePiece.transform.position.x == x && gamePiece.transform.position.z == z
                select gamePiece).FirstOrDefault();
    }

    private void ChangePlayer()
    {
        if(activePlayer == 1)
        {
            activePlayer = 2;
        }
        else
        {
            activePlayer = 1;
        }
    }

    public void PlayerWon(bool currentPlayer)
    {
        if(currentPlayer)
        {
            gameWinner = activePlayer;
        }
        else
        {
            ChangePlayer();
            gameWinner = activePlayer;
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.black;

        float buttonWidthOffset = (Screen.width / 4) - 125;
        float buttonHeightOffset = (Screen.height / 4) + 125;

        style.font = guiFont;


        if (gameWinner != 0)
        {

            GUI.Label(new Rect(buttonWidthOffset, buttonHeightOffset - 80, 100, 30), "Player " + GameManager.gameWinner + " Wins", style);

            if (GUI.Button(new Rect(buttonWidthOffset, buttonHeightOffset, 175, 30), "Main Menu"))
            {
                SceneManager.LoadScene("splash");

            }
        }
        else
        {
            if (PlayerControls.currentPlayer == 1)
            {
                moveString = "Blacks Move";
            }

            if (PlayerControls.currentPlayer == 2)
            {
                moveString = "Whites Move";
            }
            GUI.Label(new Rect(Screen.width - buttonWidthOffset, Screen.height -  120, 100, 50), moveString, style);

            if (GUI.Button(new Rect(Screen.width - buttonWidthOffset, Screen.height - 60, 100, 50), "Quit"))
            {
                Assets.ApplicationModel.runThread = false;
                SceneManager.LoadScene("splash");
            }
        }
    }
}
