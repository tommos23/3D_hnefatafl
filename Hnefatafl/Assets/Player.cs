using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    interface Player
    {
        void IPlayerMove();

    }

    class AIPlayer : Player
    {
        public void IPlayerMove()
        {
            
        }
    }

    class HumanPlayer : Player
    {
        private int playerNumber;
        private GameObject SelectedPiece;   // Selected Piece
        public int gameState = 0;           // In this state, the code is waiting for : 0 = Piece selection, 1 = Piece animation


        public HumanPlayer(int _playerNumber)
        {
            playerNumber = _playerNumber;
        }

        public void IPlayerMove()
        {
            Ray _ray;
            RaycastHit _hitInfo;

            // Select a piece if the gameState is 0 or 1
            if (gameState == 0)
            {
                // On Left Click
                if (Input.GetMouseButtonDown(0))
                {
                    _ray = PlayerControls.PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click

                    // Raycast and verify that it collided
                    if (Physics.Raycast(_ray, out _hitInfo))
                    {
                        if (playerNumber == 1)
                        {
                            // Select the piece if it has the good Tag
                            if (_hitInfo.collider.gameObject.tag.Contains("PiecePlayer1"))
                            {
                                SelectPiece(_hitInfo.collider.gameObject);
                            }
                        }
                        else if (playerNumber == 2)
                        {
                            // Select the piece if it has the good Tag
                            if (_hitInfo.collider.gameObject.tag.Contains("PiecePlayer2"))
                            {
                                SelectPiece(_hitInfo.collider.gameObject);
                            }
                        }
                    }
                }
            }

            // Move the piece if the gameState is 1
            if (gameState == 1)
            {
                Vector3 selectedCoord;

                // On Left Click
                if (Input.GetMouseButtonDown(0))
                {
                    _ray = PlayerControls.PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click

                    // Raycast and verify that it collided
                    if (Physics.Raycast(_ray, out _hitInfo))
                    {
                        // Select the piece if it has the good Tag
                        if (_hitInfo.collider.gameObject.tag == ("Cube"))
                        {
                            selectedCoord = new Vector3(_hitInfo.collider.gameObject.transform.position.x, 1, _hitInfo.collider.gameObject.transform.position.z);
                            //Try to move the piece
                            PlayerControls._GameManager.MovePiece(SelectedPiece, selectedCoord);
                            SelectedPiece = null;
                            gameState = 0;                            
                        }
                    }
                }
            }
        }

        /* Update SlectedPiece with the GameObject inputted to this function */
        public void SelectPiece(GameObject _PieceToSelect)
        {
            // Change color of the selected piece to make it apparent. Put it back to white when the piece is unselected
            if (SelectedPiece)
            {
                SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
                gameState = 0;
                SelectedPiece = null;
                return;
            }
            SelectedPiece = _PieceToSelect;
            SelectedPiece.GetComponent<Renderer>().material.color = Color.red;
            gameState = 1;
        }


    }
}
