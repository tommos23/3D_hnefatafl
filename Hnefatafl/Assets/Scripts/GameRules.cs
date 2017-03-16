using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GameRules : MonoBehaviour
{
    public abstract bool MovePiece(GameObject selectedPiece, Vector3 coordMoveTo, int activePlayer);
    public int GameWinner = 0;

    /* Pieces Prefabs */
    protected GameObject whitePiece; /* Could be passed in nicer? */
    protected GameObject whiteKing;
    protected GameObject blackPiece;
    protected GameObject king;  // uses passed in whiteKing prefab, however this King is named and accessible from code

    /* Board constants */
    protected List<GameObject> gamePieces;
    protected int BoardSize;
    protected int CentrePiece;

}

public class Fetlar : GameRules
{
    
    public Fetlar(List<GameObject> gamePieces, GameObject whitePiece, GameObject whiteKing, GameObject blackPiece)
    {
        this.gamePieces = gamePieces;
        this.whitePiece = whitePiece;
        this.whiteKing = whiteKing;
        this.blackPiece = blackPiece;
        BoardSize = 10;
        CentrePiece = 5;
        AddPieces();
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

        //gamePieces.Add(Instantiate(whiteKing, new Vector3(5, 1, 5), Quaternion.identity));
        king = Instantiate(whiteKing, new Vector3(5, 1, 5), Quaternion.identity);
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
    public override bool MovePiece(GameObject selectedPiece, Vector3 coordMoveTo, int activePlayer)
    {
        bool canMove = false;
        /* First check that the player is only moving along one axis */
        if (selectedPiece.transform.position.x == coordMoveTo.x)
        {
            if (selectedPiece.transform.position.z != coordMoveTo.z)
            {
                canMove = checkPiece(selectedPiece, coordMoveTo, activePlayer);
            }
        }
        else if (selectedPiece.transform.position.z == coordMoveTo.z)
        {
            canMove = checkPiece(selectedPiece, coordMoveTo, activePlayer);
        }

        if (canMove)
        {
            selectedPiece.transform.position = coordMoveTo; // Move the piece
            TakePieces(coordMoveTo, activePlayer); //Take any pieces after the move           
        }
        selectedPiece.GetComponent<Renderer>().material.color = Color.white; // Change it's color back
        return canMove;
    }

    private bool TakePieces(Vector3 coordToMove, int activePlayer)
    {
        /*
        Find all other players pieces
        check to see if there is a piece next to the selected piece
        see if there is a same player piece on the same axis directly next to that 
        */
        float moveToX = coordToMove.x;
        float moveToZ = coordToMove.z;

        bool canTakePiece = false;

        /* Get the pieces for the current player and opposite player */
        string currentPlayerColour = "PiecePlayer1";
        string oppositePlayerColour = "PiecePlayer2";
        if (activePlayer == 2)
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
            if (foundPieceZ == moveToZ)
            {
                float foundMinusMove = foundPieceX - moveToX;
                if (Math.Abs(foundMinusMove) == 1 && oppositeColourPiece.tag.Contains("King"))
                {
                    if (IsKingSurrounded(oppositeColourPiece, currentPlayerColour))
                    {
                        GameWinner = activePlayer;
                        canTakePiece = true;
                    }
                }
                else if (1 == foundMinusMove)
                {
                    //Same colour, two away, same line
                    var adjacentPiece = getAdjacentPieces(currentPlayerColour, moveToX + 2, moveToZ);
                    TakePiece(adjacentPiece, oppositeColourPiece);
                    canTakePiece = true;
                }
                else if (-1 == foundMinusMove)
                {
                    //Same colour, two away, same line
                    var adjacentPiece = getAdjacentPieces(currentPlayerColour, moveToX - 2, moveToZ);
                    TakePiece(adjacentPiece, oppositeColourPiece);
                    canTakePiece = true;
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
                        GameWinner = activePlayer;
                        canTakePiece = true;
                    }
                }
                else if (1 == foundMinusMove)
                {
                    //Same colour, two away, same line
                    var adjacentPiece = getAdjacentPieces(currentPlayerColour, moveToX, moveToZ + 2);
                    TakePiece(adjacentPiece, oppositeColourPiece);
                    canTakePiece = true;

                }
                else if (-1 == foundMinusMove)
                {
                    //Same colour, two away, same line
                    var adjacentPiece = getAdjacentPieces(currentPlayerColour, moveToX, moveToZ - 2);
                    TakePiece(adjacentPiece, oppositeColourPiece);
                    canTakePiece = true;
                }
            }
        }
        return canTakePiece;
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
            if ((king.transform.position.x == 5) && (king.transform.position.z == 5))
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

        var surroundingPieces = getAdjacentPieces(currentPlayerColour, kingX, kingZ + 1);
        surroundingPieces.AddRange(getAdjacentPieces(currentPlayerColour, kingX, kingZ - 1));
        surroundingPieces.AddRange(getAdjacentPieces(currentPlayerColour, kingX + 1, kingZ));
        surroundingPieces.AddRange(getAdjacentPieces(currentPlayerColour, kingX - 1, kingZ));

        if (surroundingPieces.Count == 4)
        {
            return true;
        }
        if (surroundingPieces.Count == 3)
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
    private bool checkPiece(GameObject selectedPiece, Vector3 coordToMove, int activePlayer)
    {
        float x1 = selectedPiece.transform.position.x;
        float x2 = coordToMove.x;
        float dx = x2 - x1;

        float z1 = selectedPiece.transform.position.z;
        float z2 = coordToMove.z;
        float dz = z2 - z1;

        if (!selectedPiece.tag.Contains("King"))
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
        if (dx == 0)
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
            //Check not moving into corners
            if (x2 == 0 && z2 == 0)
            {
                GameWinner = activePlayer;
            }
            if (x2 == 0 && z2 == BoardSize)
            {
                GameWinner = activePlayer;
            }
            if (x2 == BoardSize && z2 == 0)
            {
                GameWinner = activePlayer;
            }
            if (x2 == BoardSize && z2 == BoardSize)
            {
                GameWinner = activePlayer;
            }
        }

        return true;
    }
}
