using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    /* Pieces Prefabs */
    public GameObject whitePiece;
    public GameObject whiteKing;
    public GameObject blackPiece;
    public GameObject cornerPiece;
    public GameObject centrePiece;

    public int activePlayer = 1;		// 1 = Player1, 2 = Player2

    private List<GameObject> gamePieces;

    // Use this for initialization
    void Start ()
    {
        gamePieces = new List<GameObject>();
        AddPieces();

        //Assets.AsyncSocketServer.StartListening();
    }

    // Update is called once per frame
    void Update ()
    {
		
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
        gamePieces.Add(Instantiate(whiteKing, new Vector3(5, 1, 5), Quaternion.identity));
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

        //Add 'invisible' corner and centre pieces
        gamePieces.Add(Instantiate(cornerPiece, new Vector3(0, 1, 0), Quaternion.identity));
        gamePieces.Add(Instantiate(cornerPiece, new Vector3(0, 1, 10), Quaternion.identity));
        gamePieces.Add(Instantiate(cornerPiece, new Vector3(10, 1, 0), Quaternion.identity));
        gamePieces.Add(Instantiate(cornerPiece, new Vector3(10, 1, 10), Quaternion.identity));

        gamePieces.Add(Instantiate(centrePiece, new Vector3(5, 1, 5), Quaternion.identity));

    }


    /* Move the SelectedPiece to the inputted coords */
    public bool MovePiece(GameObject _selectedPiece, Vector3 _coordMoveTo)
    {
        bool canMove = false;
        /* First check that the player is only moving along one axis */
        if(_selectedPiece.transform.position.x == _coordMoveTo.x)
        {
            if (_selectedPiece.transform.position.z != _coordMoveTo.z)
            {
                canMove = checkPiece(_selectedPiece, _coordMoveTo);
            }
        }
        else if (_selectedPiece.transform.position.z == _coordMoveTo.z)
        {
            canMove = checkPiece(_selectedPiece, _coordMoveTo);
        }

        if(canMove)
        {
            _selectedPiece.transform.position = _coordMoveTo; // Move the piece
            TakePieces(_coordMoveTo); //Take any pieces after the move
            ChangePlayer();
        }
        _selectedPiece.GetComponent<Renderer>().material.color = Color.white; // Change it's color back
        return canMove;
    }

    private void TakePieces(Vector3 _coordToMove)
    {
        /*
        Find all other players pieces
        check to see if there is a piece next to the selected piece
        see if there is a same player piece on the same axis directly next to that 
        */

        float moveToX = _coordToMove.x;
        float moveToZ = _coordToMove.z;

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
                if (1 == foundMinusMove)
                {
                    //Same colour, two away, same line
                    List<GameObject> adjacentPiece = (from GameObject piece in gamePieces
                                                        where (piece.tag.Contains(currentPlayerColour) 
                                                        || piece.tag.Contains("Hostile"))
                                                        && piece.transform.position.x == moveToX + 2
                                                        && piece.transform.position.z == moveToZ
                                                        select piece).ToList<GameObject>();
                    //There is a piece on the other side
                    if (adjacentPiece.Count() == 1)
                    {
                        //Remove piece from game
                        gamePieces.Remove(oppositeColourPiece);
                        Destroy(oppositeColourPiece);
                    }
                    
                }
                else if (-1 == foundMinusMove)
                {
                    //Same colour, two away, same line
                    List<GameObject> adjacentPiece = (from GameObject piece in gamePieces
                                                        where (piece.tag.Contains(currentPlayerColour)
                                                        || piece.tag.Contains("Hostile"))
                                                        && piece.transform.position.x == moveToX - 2
                                                        && piece.transform.position.z == moveToZ
                                                        select piece).ToList<GameObject>();
                    //There is a piece on the other side
                    if (adjacentPiece.Count() == 1)
                    {
                        //Remove piece from game
                        gamePieces.Remove(oppositeColourPiece);
                        Destroy(oppositeColourPiece);
                    }
                }
            }

            //if they are on the same row
            if (foundPieceX == moveToX)
            {
                float foundMinusMove = foundPieceZ - moveToZ;
                if (1 == foundMinusMove)
                {
                    //Same colour, two away, same line
                    List<GameObject> adjacentPiece = (from GameObject piece in gamePieces
                                                      where piece.tag.Contains(currentPlayerColour)
                                                      && piece.transform.position.z == moveToZ + 2
                                                      && piece.transform.position.x == moveToX
                                                      select piece).ToList<GameObject>();
                    //There is a piece on the other side
                    if (adjacentPiece.Count() == 1)
                    {
                        //Remove piece from game
                        gamePieces.Remove(oppositeColourPiece);
                        Destroy(oppositeColourPiece);
                    }

                }
                else if (-1 == foundMinusMove)
                {
                    //Same colour, two away, same line
                    List<GameObject> adjacentPiece = (from GameObject piece in gamePieces
                                                      where piece.tag.Contains(currentPlayerColour)
                                                      && piece.transform.position.z == moveToZ - 2
                                                      && piece.transform.position.x == moveToX
                                                      select piece).ToList<GameObject>();
                    //There is a piece on the other side
                    if (adjacentPiece.Count() == 1)
                    {
                        //Remove piece from game
                        gamePieces.Remove(oppositeColourPiece);
                        Destroy(oppositeColourPiece);
                    }
                }
            }

        }
    }

    /* Check that no other piece is in the square moving to */
    private bool checkPiece(GameObject _selectedPiece, Vector3 _coordToMove)
    {
        float x1 = _selectedPiece.transform.position.x;
        float x2 = _coordToMove.x;
        float dx = x2 - x1;

        float z1 = _selectedPiece.transform.position.z;
        float z2 = _coordToMove.z;
        float dz = z2 - z1;

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
        return true;
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
}
