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
                                        //private int activePlayer = 0;		// 0 = Player1, 1 = Player2, 2 = AI, to be used later
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

    }

    //Update SlectedPiece with the GameObject inputted to this function
    public void SelectPiece(GameObject _PieceToSelect)
    {
        // Change color of the selected piece to make it apparent. Put it back to white when the piece is unselected
        if (SelectedPiece)
        {
            SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
        }
        SelectedPiece = _PieceToSelect;
        SelectedPiece.GetComponent<Renderer>().material.color = Color.red;
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
        }
        SelectedPiece.GetComponent<Renderer>().material.color = Color.white; // Change it's color back
        SelectedPiece = null; // Unselect the Piece
        return true;
    }

    /* Check that no other piece is in the square moving to */
    private bool checkPiece(Vector3 _coordToMove)
    {
        foreach (GameObject piece in gamePieces)
        {
            if (_coordToMove.x == piece.transform.position.x && _coordToMove.z == piece.transform.position.z)
            {
                return false;
            }
        }
        return true;
    }

    // Change the state of the game
    public void ChangeState(int _newState)
    {
        gameState = _newState;
        Debug.Log("GameState = " + _newState);
    }
}
