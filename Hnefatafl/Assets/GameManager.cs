using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    /* Pieces Prefabs */
    public GameObject whitePiece;
    public GameObject whiteKing;
    public GameObject blackPiece;

    public int gameState = 0;           // In this state, the code is waiting for : 0 = Piece selection, 1 = Piece animation, 2 = Player2/AI movement
    public int activePlayer = 0;		// 0 = Player1, 1 = Player2
    private GameObject SelectedPiece;   // Selected Piece

    private List<GameObject> gamePieces;

    // Use this for initialization
    void Start ()
    {
        gamePieces = new List<GameObject>();
        AddPieces();
	}



    // Update is called once per frame
    void Update ()
    {
		
	}

    private void AddPieces()
    {
        /* Add the white pieces */
        gamePieces.Add(GameObject.Instantiate(whitePiece, new Vector3(3, 1, 5), Quaternion.identity));

        gamePieces.Add(GameObject.Instantiate(whitePiece, new Vector3(4, 1, 6), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(whitePiece, new Vector3(4, 1, 5), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(whitePiece, new Vector3(4, 1, 4), Quaternion.identity));

        gamePieces.Add(GameObject.Instantiate(whitePiece, new Vector3(5, 1, 7), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(whitePiece, new Vector3(5, 1, 6), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(whitePiece, new Vector3(5, 1, 4), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(whitePiece, new Vector3(5, 1, 3), Quaternion.identity));

        gamePieces.Add(GameObject.Instantiate(whitePiece, new Vector3(6, 1, 6), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(whitePiece, new Vector3(6, 1, 5), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(whitePiece, new Vector3(6, 1, 4), Quaternion.identity));

        gamePieces.Add(GameObject.Instantiate(whitePiece, new Vector3(7, 1, 5), Quaternion.identity));
        
        /* Add the black pieces */
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(3, 1, 0), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(4, 1, 0), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(5, 1, 0), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(6, 1, 0), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(7, 1, 0), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(5, 1, 1), Quaternion.identity));

        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(0, 1, 3), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(0, 1, 4), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(0, 1, 5), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(0, 1, 6), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(0, 1, 7), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(1, 1, 5), Quaternion.identity));

        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(3, 1, 10), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(4, 1, 10), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(5, 1, 10), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(6, 1, 10), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(7, 1, 10), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(5, 1, 9), Quaternion.identity));

        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(10, 1, 3), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(10, 1, 4), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(10, 1, 5), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(10, 1, 6), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(10, 1, 7), Quaternion.identity));
        gamePieces.Add(GameObject.Instantiate(blackPiece, new Vector3(9, 1, 5), Quaternion.identity));

    }

    //Update SlectedPiece with the GameObject inputted to this function
    public void SelectPiece(GameObject _PieceToSelect)
    {
        // Change color of the selected piece to make it apparent. Put it back to white when the piece is unselected
        if (SelectedPiece)
        {
            SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
            ChangeState(0);
            SelectedPiece = null;
            return;
        }
        SelectedPiece = _PieceToSelect;
        SelectedPiece.GetComponent<Renderer>().material.color = Color.red;
        ChangeState(1);
    }

    // Move the SelectedPiece to the inputted coords
    public bool MovePiece(Vector3 _coordToMove)
    {
        bool move = false;
        if(SelectedPiece.transform.position.x == _coordToMove.x)
        {
            if (SelectedPiece.transform.position.z != _coordToMove.z)
            {
                move = checkPiece(_coordToMove);
            }
        }
        else if (SelectedPiece.transform.position.z == _coordToMove.z)
        {
            move = checkPiece(_coordToMove);
        }

        if(move)
        {
            SelectedPiece.transform.position = _coordToMove;// Move the piece
            ChangeState(0);
            ChangePlayer();
        }
        SelectedPiece.GetComponent<Renderer>().material.color = Color.white; // Change it's color back
        SelectedPiece = null; // Unselect the Piece
        return true;
    }

    /* Check that no other piece is in the square moving to */
    private bool checkPiece(Vector3 _coordToMove)
    {
        float x1 = SelectedPiece.transform.position.x;
        float x2 = _coordToMove.x;
        float dx = x2 - x1;

        float z1 = SelectedPiece.transform.position.z;
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

    // Change the state of the game
    private void ChangeState(int _newState)
    {
        gameState = _newState;
        Debug.Log("GameState = " + _newState);
    }

    private void ChangePlayer()
    {
        if(activePlayer == 0)
        {
            activePlayer = 1;
        }
        else
        {
            activePlayer = 0;
        }
    }
}
