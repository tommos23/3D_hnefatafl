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
        private static Assets.AsyncSocketServer socketServer;
        private GameObject selectedPiece;
        private System.Int32 serverPort;

        public AIPlayer(System.Int32 _serverPort)
        {
            serverPort = _serverPort;
            socketServer = new AsyncSocketServer(serverPort);
        }

        public void IPlayerMove()
        {

            //socketServer = new AsyncSocketServer(serverPort);
            //socketServer.checkConnection();
            AsyncSocketServer.StartListening(serverPort);
            string moveInput = "";
            moveInput = getMoveFromSocket();

            if ((moveInput != "") && (moveInput != "q<EOF>"))
            {

                Regex validInput = new Regex("^([a-k]{1})([1-9]|1[0-1])-([a-k]{1})([1-9]|1[0-1])<EOF>$");
                if (validInput.IsMatch(moveInput))
                {
                    Debug.Log("Pattern Valid");
                    var splitInput = validInput.Split(moveInput);
                    int selectedPieceX = StringConvertedToNumber(splitInput[1]) - 1;
                    int selectedPieceZ = Convert.ToInt32(splitInput[2]);
                    Debug.Log("split performed:" + selectedPieceX.ToString() + " " + selectedPieceZ.ToString());
                    //int selectedPieceX = 7;
                    // int selectedPieceZ = 5;

                    selectedPiece = PlayerControls._GameManager.selectPiece(selectedPieceX, selectedPieceZ);

                    if (selectedPiece != null)
                    {
                        Debug.Log("Piece selected");
                        int moveToX = StringConvertedToNumber(splitInput[3]) - 1;
                        int moveToZ = Convert.ToInt32(splitInput[4]);

                        //int moveToX = 7;
                        //int moveToZ = 4;
                        Vector3 moveToCoord = new Vector3(moveToX, 1, moveToZ);
                        if (!PlayerControls._GameManager.MovePiece(selectedPiece, moveToCoord))
                        {
                            //Invalid move, game lost
                            Debug.Log("invalid move");
                            PlayerControls._GameManager.playerWon(false);
                        }
                    }
                    else
                    {
                        //Wrong piece selected, game lost
                        Debug.Log("wrong piece selected");
                        PlayerControls._GameManager.playerWon(false);
                    }
                }
                else
                {
                    //Pattern does not match, game lost
                    Debug.Log("Pattern does not match");
                    PlayerControls._GameManager.playerWon(false);
                }

            }
            // otherwise no message has been recieved and we repeat
        }
        //}

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

        public void StartPlayer()
        {
            //socketServer.SendData("make a move<EOF>");
        }

        public void ISendPlayersMove(Vector3[] lastMove)
        {
            if (lastMove.Length == 2)
            {
                //Final result should be in the format: "a1-a1<EOF>"
                StringBuilder lastMoveString = new StringBuilder();
                //Letter part
                lastMoveString.Append(NumberConvertedToString((int)lastMove[0].x));
                //Number part
                lastMoveString.Append((int)lastMove[0].z + 1);
                //Seperator
                lastMoveString.Append("-");
                //Letter part
                lastMoveString.Append(NumberConvertedToString((int)lastMove[1].x));
                //Number part
                lastMoveString.Append((int)lastMove[1].z + 1);
                //EOF
                lastMoveString.Append("<EOF>");

                //Send over socket
                //socketServer.SendData(lastMoveString.ToString());
            }
        }

        public string getMoveFromSocket()
        {
            string _input = "";

            if (socketServer.isDataAvailable()) { _input = socketServer.popData(); }

            return _input;

        }
    }

    class HumanPlayer : Player
    {
        private int playerNumber;
        private GameObject SelectedPiece;   // Selected Piece
        public int gameState = 0;           // In this state, the code is waiting for : 0 = Piece selection, 1 = Piece animation

        private static Assets.AsyncSocketServer socketServer;
        private System.Int32 serverPort = 11020;    // socketfor outgoing info

        public HumanPlayer(int _playerNumber)
        {
            playerNumber = _playerNumber;
            socketServer = new AsyncSocketServer(serverPort);
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

        public static string lastPlayerMoveString;

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

            if (lastMove.Length == 2)
            {
                //Final result should be in the format: "a1-a1<EOF>"
                StringBuilder lastMoveString = new StringBuilder();
                //Letter part
                lastMoveString.Append(NumberConvertedToString(((int)lastMove[0].x) + 1));
                //Number part
                lastMoveString.Append((int)lastMove[0].z);
                //Seperator
                lastMoveString.Append("-");
                //Letter part
                lastMoveString.Append(NumberConvertedToString(((int)lastMove[1].x) + 1));
                //Number part
                lastMoveString.Append((int)lastMove[1].z);
                //EOF
                lastMoveString.Append("<EOF>");


                lastPlayerMoveString = lastMoveString.ToString();
                //send over socket
                //AsyncSocketServer.externalSend();

            }

        }
    }
}
