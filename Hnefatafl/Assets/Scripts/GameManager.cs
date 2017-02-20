using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    /* Pieces Prefabs */
    public GameObject whitePiece;
    public GameObject whiteKing;
    public GameObject blackPiece;
    public GameObject cornerPiece;
    public GameObject centrePiece;
    public Vector3[] lastMove;
    public int activePlayer = 1;		// 1 = Player1, 2 = Player2
    public static int gameWinner= 0;  // used by game over screen, public record of gameWon var.  

    private int gameWon = 0;            // 0 = no winner yet, 1 = Player1, 2 = Player2
    private List<GameObject> gamePieces;
    private const int BoardSize = 10;
    private const int CentrePiece = 5;
    private GameObject king;

    private String moveString = "";
    public Font guiFont;

    // Use this for initialization
    void Start ()
    {
        GameManager.gameWinner = 0; //reset last winner of game
        gamePieces = new List<GameObject>();
        lastMove = new Vector3[2];

        AddPieces();

        //Assets.AsyncSocketServer.StartListening();
    }

    // Update is called once per frame
    void Update ()
    {
		if(gameWon != 0)
        {
            //SceneManager.LoadScene("gameOver");
        }


    }

    private void AddPieces()
    {
        /* Add the white pieces */
        gamePieces.Add(Instantiate(whitePiece, new Vector3(3, 1, 5), Quaternion.identity));

        gamePieces.Add(Instantiate(whitePiece, new Vector3(4, 1, 6), Quaternion.identity));
        gamePieces.Add(Instantiate(whitePiece, new Vector3(4, 1, 5), Quaternion.identity));
        gamePieces.Add(Instantiate(whitePiece, new Vector3(4, 1, 4), Quaternion.identity));

        gamePieces.Add(Instantiate(whitePiece, new Vector3(5, 1, 7), Quaternion.identity));
        gamePieces.Add(Instantiate(whitePiece, new Vector3(5, 1, 6), Quaternion.identity));

        gamePieces.Add(Instantiate(whitePiece, new Vector3(5, 1, 6), Quaternion.identity));
        //gamePieces.Add(Instantiate(whiteKing, new Vector3(5, 1, 5), Quaternion.identity));
        king  = Instantiate(whiteKing, new Vector3(5, 1, 5), Quaternion.identity);
        gamePieces.Add(king);

        gamePieces.Add(Instantiate(whitePiece, new Vector3(5, 1, 4), Quaternion.identity));
        gamePieces.Add(Instantiate(whitePiece, new Vector3(5, 1, 3), Quaternion.identity));

        gamePieces.Add(Instantiate(whitePiece, new Vector3(6, 1, 6), Quaternion.identity));
        gamePieces.Add(Instantiate(whitePiece, new Vector3(6, 1, 5), Quaternion.identity));
        gamePieces.Add(Instantiate(whitePiece, new Vector3(6, 1, 4), Quaternion.identity));

        gamePieces.Add(Instantiate(whitePiece, new Vector3(7, 1, 5), Quaternion.identity));

        

        /* Add the black pieces */
        gamePieces.Add(Instantiate(blackPiece, new Vector3(3, 1, 0), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(4, 1, 0), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(5, 1, 0), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(6, 1, 0), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(7, 1, 0), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(5, 1, 1), Quaternion.identity));

        gamePieces.Add(Instantiate(blackPiece, new Vector3(0, 1, 3), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(0, 1, 4), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(0, 1, 5), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(0, 1, 6), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(0, 1, 7), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(1, 1, 5), Quaternion.identity));

        gamePieces.Add(Instantiate(blackPiece, new Vector3(3, 1, 10), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(4, 1, 10), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(5, 1, 10), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(6, 1, 10), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(7, 1, 10), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(5, 1, 9), Quaternion.identity));

        gamePieces.Add(Instantiate(blackPiece, new Vector3(10, 1, 3), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(10, 1, 4), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(10, 1, 5), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(10, 1, 6), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(10, 1, 7), Quaternion.identity));
        gamePieces.Add(Instantiate(blackPiece, new Vector3(9, 1, 5), Quaternion.identity));

    }


    /* Move the SelectedPiece to the inputted coords */
    public bool MovePiece(GameObject selectedPiece, Vector3 coordMoveTo)
    {
        bool canMove = false;
        /* First check that the player is only moving along one axis */
        if(selectedPiece.transform.position.x == coordMoveTo.x)
        {
            if (selectedPiece.transform.position.z != coordMoveTo.z)
            {
                canMove = checkPiece(selectedPiece, coordMoveTo);
            }
        }
        else if (selectedPiece.transform.position.z == coordMoveTo.z)
        {
            canMove = checkPiece(selectedPiece, coordMoveTo);
        }

        if(canMove)
        {
            //Update last move to be sent to AI players
            lastMove[0] = new Vector3(selectedPiece.transform.position.x, 1, selectedPiece.transform.position.z);
            lastMove[1] = coordMoveTo;
            selectedPiece.transform.position = coordMoveTo; // Move the piece
            TakePieces(coordMoveTo); //Take any pieces after the move
            ChangePlayer();
        }
        selectedPiece.GetComponent<Renderer>().material.color = Color.white; // Change it's color back
        return canMove;
    }

    private void TakePieces(Vector3 coordToMove)
    {
        /*
        Find all other players pieces
        check to see if there is a piece next to the selected piece
        see if there is a same player piece on the same axis directly next to that 
        */
        float moveToX = coordToMove.x;
        float moveToZ = coordToMove.z;

        /* Get the pieces for the current player and opposite player */
        string currentPlayerColour = "PiecePlayer1";
        string oppositePlayerColour = "PiecePlayer2";
        if(activePlayer == 2)
        {
            currentPlayerColour = "PiecePlayer2";
            oppositePlayerColour = "PiecePlayer1";
        }

        /* Splits the gamePieces into player pieces (using LINQ) */
        List<GameObject> oppositePlayerPieces = (from GameObject piece in gamePieces
                                                 where piece.tag.Contains(oppositePlayerColour)
                                                 select piece).ToList<GameObject>();


        foreach (GameObject oppositeColourPiece in oppositePlayerPieces)
        {
            float foundPieceX = oppositeColourPiece.transform.position.x;
            float foundPieceZ = oppositeColourPiece.transform.position.z;
            /* Find the other players pieces */

            //if they are on the same row
            if(foundPieceZ == moveToZ)
            {
                float foundMinusMove = foundPieceX - moveToX;
                if(Math.Abs(foundMinusMove) == 1 && oppositeColourPiece.tag.Contains("King"))
                {
                    if(IsKingSurrounded(oppositeColourPiece, currentPlayerColour))
                    {
                        playerWon(true);
                    }
                }
                else if (1 == foundMinusMove)
                {
                    //Same colour, two away, same line
                    var adjacentPiece = getAdjacentPieces(currentPlayerColour, moveToX + 2, moveToZ);
                    TakePiece(adjacentPiece, oppositeColourPiece);
                }
                else if (-1 == foundMinusMove)
                {
                    //Same colour, two away, same line
                    var adjacentPiece = getAdjacentPieces(currentPlayerColour, moveToX - 2, moveToZ);
                    TakePiece(adjacentPiece, oppositeColourPiece);
                }
            }

            //if they are on the same row
            if (foundPieceX == moveToX)
            {
                float foundMinusMove = foundPieceZ - moveToZ;
                if (Math.Abs(foundMinusMove) == 1 && oppositeColourPiece.tag.Contains("King"))
                {
                    if (IsKingSurrounded(oppositeColourPiece, currentPlayerColour))
                    {
                        playerWon(true);
                    }
                }
                else if (1 == foundMinusMove)
                {
                    //Same colour, two away, same line
                    var adjacentPiece = getAdjacentPieces(currentPlayerColour, moveToX, moveToZ + 2);
                    TakePiece(adjacentPiece, oppositeColourPiece);

                }
                else if (-1 == foundMinusMove)
                {
                    //Same colour, two away, same line
                    var adjacentPiece = getAdjacentPieces(currentPlayerColour, moveToX, moveToZ - 2);
                    TakePiece(adjacentPiece, oppositeColourPiece);
                }
            }
        }
    }

    private List<GameObject> getAdjacentPieces(string currentPlayerColour, float xDirection, float zDirection)
    {
        List<GameObject> adjacentPiece = (from GameObject piece in gamePieces
                                          where piece.tag.Contains(currentPlayerColour)
                                          && piece.transform.position.x == xDirection
                                          && piece.transform.position.z == zDirection        
                                          select piece).ToList<GameObject>();

        //Add hostile corners by returning fake piece 
        if (xDirection == 0 && zDirection == 0)
        {
            adjacentPiece.Add(new GameObject());
        }
        if (xDirection == 0 && zDirection == BoardSize)
        {
            adjacentPiece.Add(new GameObject());
        }
        if (xDirection == BoardSize && zDirection == 0)
        {
            adjacentPiece.Add(new GameObject());
        }
        if (xDirection == BoardSize && zDirection == BoardSize)
        {
            adjacentPiece.Add(new GameObject());
        }
        if (xDirection == CentrePiece && zDirection == CentrePiece)
        {

            // get the white king
            if ((king.transform.position.x ==5) && (king.transform.position.z == 5))
            {
              // do nothing
            }

            else
            {
                adjacentPiece.Add(new GameObject());
            }

        }

        return adjacentPiece;
    }

    private void TakePiece(List<GameObject> adjacentPieces, GameObject oppositeColourPiece)
    {
        //There is a piece on the other side
        if (adjacentPieces.Count() == 1)
        {
            //Remove piece from game
            gamePieces.Remove(oppositeColourPiece);
            Destroy(oppositeColourPiece);        
        }
    }

    private bool IsKingSurrounded(GameObject kingPiece, string currentPlayerColour)
    {
        //get pieces in each direction
        float kingX = kingPiece.transform.position.x;
        float kingZ = kingPiece.transform.position.z;

        var surroundingPieces =    getAdjacentPieces(currentPlayerColour, kingX, kingZ + 1);
        surroundingPieces.AddRange(getAdjacentPieces(currentPlayerColour, kingX, kingZ - 1));
        surroundingPieces.AddRange(getAdjacentPieces(currentPlayerColour, kingX + 1, kingZ));
        surroundingPieces.AddRange(getAdjacentPieces(currentPlayerColour, kingX - 1, kingZ));

        if(surroundingPieces.Count == 4)
        {
            return true;
        }
        if(surroundingPieces.Count == 3)
        {
            //Check to see if the king is trapped by the wall
            if (kingX == 0 || kingX == BoardSize)
            {
                return true;
            }
            if (kingZ == 0 || kingZ == BoardSize)
            {
                return true;
            }
        }
        return false;
    }

    /* Check that no other piece is in the square moving to */
    private bool checkPiece(GameObject selectedPiece, Vector3 coordToMove)
    {
        float x1 = selectedPiece.transform.position.x;
        float x2 = coordToMove.x;
        float dx = x2 - x1;

        float z1 = selectedPiece.transform.position.z;
        float z2 = coordToMove.z;
        float dz = z2 - z1;
        
        if(!selectedPiece.tag.Contains("King"))
        {
            //Check not moving into corners
            if (x2 == 0 && z2 == 0)
            {
                return false;
            }
            if (x2 == 0 && z2 == BoardSize)
            {
                return false;
            }
            if (x2 == BoardSize && z2 == 0)
            {
                return false;
            }
            if (x2 == BoardSize && z2 == BoardSize)
            {
                return false;
            }
            if (x2 == CentrePiece && z2 == CentrePiece)
            {
                return false;
            }
        }

        /* Movement on z axis (aka. up down) */
        if(dx == 0)
        {
            foreach (GameObject piece in gamePieces)
            {
                if (x1 == piece.transform.position.x)
                {
                    /* Moving up board */
                    if (dz > 0)
                    {
                        if (z1 < piece.transform.position.z && piece.transform.position.z <= z2)
                        {
                            return false;
                        }
                    }
                    /* Moving down board */
                    else
                    {
                        if (z1 > piece.transform.position.z && piece.transform.position.z >= z2)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        /* Movement on x axis (aka. left right) */
        else if (dz == 0)
        {
            foreach (GameObject piece in gamePieces)
            {
                if (z1 == piece.transform.position.z)
                {
                    /* Moving up board */
                    if (dx > 0)
                    {
                        if (x1 < piece.transform.position.x && piece.transform.position.x <= x2)
                        {
                            return false;
                        }
                    }
                    /* Moving down board */
                    else
                    {
                        if (x1 > piece.transform.position.x && piece.transform.position.x >= x2)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        //Has the king just won
        if (selectedPiece.tag.Contains("King"))
        {
            bool hasWon = false;
            //Check not moving into corners
            if (x2 == 0 && z2 == 0)
            {
                hasWon = true;
            }
            if (x2 == 0 && z2 == BoardSize)
            {
                hasWon = true;
            }
            if (x2 == BoardSize && z2 == 0)
            {
                hasWon = true;
            }
            if (x2 == BoardSize && z2 == BoardSize)
            {
                hasWon = true;
            }
            //Has the king got to the corner?
            if (hasWon)
            {
                playerWon(true);
            }
        }

        return true;
    }

    public GameObject selectPiece(int x, int z)
    {
        Debug.Log("selecting game piece...");
        return (from gamePiece in gamePieces
                where gamePiece.transform.position.x == x && gamePiece.transform.position.z == z
                select gamePiece).FirstOrDefault();
    }

    //True if the active player won, false if the other player won
    public void playerWon(bool isActivePlayer)
    {
        if (isActivePlayer)
        {
            gameWon = activePlayer;
            gameWinner = activePlayer;
        }
        else
        {
            ChangePlayer();
            gameWon = activePlayer;
            gameWinner = activePlayer;
        }
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

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.black;

        float buttonWidthOffset = (Screen.width / 4) - 125;
        float buttonHeightOffset = (Screen.height / 4 + 125);

        style.font = guiFont;


        if (gameWon != 0)
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
            GUI.Label(new Rect(buttonWidthOffset, buttonHeightOffset - 80, 100, 50), moveString, style);

            if (GUI.Button(new Rect(Screen.width - buttonWidthOffset, Screen.height - buttonHeightOffset - 50, 100, 50), "Quit"))
            {
                SceneManager.LoadScene("splash");
            }
        }



    }
}
