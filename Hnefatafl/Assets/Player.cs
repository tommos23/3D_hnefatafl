using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets
{
    interface Player
    {
        void IPlayerMove();
        void ISendPlayersMove(Vector3[] lastMove);
    }

    class AIPlayer : Player
    {
        private Assets.SyncSocketServer socketServer;
        private GameObject selectedPiece;

        public AIPlayer(System.Int32 _serverPort)
        {
            socketServer = new SyncSocketServer(_serverPort);
        }

        public void IPlayerMove()
        {
            if (!socketServer.messageFlag)
            {
                socketServer.StartListening();
                
                //Read the message
                socketServer.messageFlag = false;
            }
            else if (SyncSocketServer.data != null)
            {
                Regex validInput = new Regex("^([a-k]{1})([1-9]|1[0-1])-([a-k]{1})([1-9]|1[0-1])<EOF>$");
                if(validInput.IsMatch(socketServer.GetData()))
                {
                    var splitInput = validInput.Split(socketServer.GetData());
                    int selectedPieceX = StringConvertedToNumber(splitInput[0]);
                    int selectedPieceZ = Convert.ToInt32(splitInput[1]);

                    selectedPiece = PlayerControls._GameManager.selectPiece(selectedPieceX, selectedPieceZ);
                    if(selectedPiece != null)
                    {
                        int moveToX = StringConvertedToNumber(splitInput[2]);
                        int moveToZ = Convert.ToInt32(splitInput[3]);
                        Vector3 moveToCoord = new Vector3(moveToX, 1, moveToZ);
                        if(!PlayerControls._GameManager.MovePiece(selectedPiece, moveToCoord))
                        {
                            //Invalid move, game lost
                            PlayerControls._GameManager.playerWon(false);
                        }
                    }
                    else
                    {
                        //Wrong piece selected, game lost
                        PlayerControls._GameManager.playerWon(false);
                    }
                }
                else
                {
                    //Pattern does not match, game lost
                    PlayerControls._GameManager.playerWon(false);
                }
                
            }    
        }

        private int StringConvertedToNumber(string column)
        {
            int retVal = 0;
            string col = column.ToUpper();
            for (int iChar = col.Length - 1; iChar >= 0; iChar--)
            {
                char colPiece = col[iChar];
                int colNum = colPiece - 64;
                retVal = retVal + colNum * (int)Math.Pow(26, col.Length - (iChar + 1));
            }
            return retVal;
        }

        private string NumberConvertedToString(int column)
        {
            string columnString = "";
            decimal columnNumber = column;
            while (columnNumber > 0)
            {
                decimal currentLetterNumber = (columnNumber - 1) % 26;
                char currentLetter = (char)(currentLetterNumber + 65);
                columnString = currentLetter + columnString;
                columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
            }
            return columnString;
        }

        public void ISendPlayersMove(Vector3[] lastMove)
        {
            if(lastMove.Length == 2)
            {
                //Final result should be in the format: "a1-a1<EOF>"
                StringBuilder lastMoveString = new StringBuilder();
                //Letter part
                lastMoveString.Append(NumberConvertedToString((int)lastMove[0].x));
                //Number part
                lastMoveString.Append((int)lastMove[0].z);
                //Seperator
                lastMoveString.Append("-");
                //Letter part
                lastMoveString.Append(NumberConvertedToString((int)lastMove[1].x));
                //Number part
                lastMoveString.Append((int)lastMove[1].z);
                //EOF
                lastMoveString.Append("<EOF>");

                //Send over socket
                socketServer.SendData(lastMoveString.ToString());
            }
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


        public void ISendPlayersMove(Vector3[] lastMove)
        {
            
        }
    }
}
