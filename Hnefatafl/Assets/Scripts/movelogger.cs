using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class movelogger {

    string currentLogPath = "";
    string baseDir = "c:\\tafl\\";

    // create text file ready to log to
    public void startLogging()
    {
        // get current timestamp (unix format)
        Int32 unixTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        //create logfile name and current path
        String logFileName = "TaflGame-" + unixTime;
        currentLogPath = baseDir + logFileName + ".txt";

        //create logfile
        Debug.Log("Creating log at : " + currentLogPath);
        try
        {
            System.IO.Directory.CreateDirectory(baseDir);
            if (File.Exists(currentLogPath))
            {
                File.Delete(currentLogPath);
            }

            using (FileStream fs = File.Create(currentLogPath))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes("Output log for Hnefatafl board game \r\n");
                fs.Write(info, 0, info.Length);
            }
        }

        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    // add a move everytime a player moves
    public void recordMove(int activePlayer, Vector3[] lastMove )
    {

        if (lastMove.Length == 2)
        {
            //Final result should be in the format: "a1-a1<EOF>"
            StringBuilder lastMoveString = new StringBuilder();

            // add the player number to the move
            if (activePlayer == 1) { lastMoveString.Append("White : "); }
            else { lastMoveString.Append("Black : "); }


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

            // log to file
            Debug.Log("Move recorded : " + lastMoveString.ToString());
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(currentLogPath, true))
            {
                file.WriteLine(lastMoveString.ToString());
            }
        }

    }

    public void recordWin(int gameWinner)
    {

        string winner = "";
        if (gameWinner == 2) { winner = "White wins"; }
        else if (gameWinner == 1){ winner = "Black wins ";  }


        using (System.IO.StreamWriter file = new System.IO.StreamWriter(currentLogPath, true))
        {
            file.WriteLine(winner);
        }
    }


    //helper function 
    // TODO: this is repeated elsewhere and probably needs refactoring to a utility class
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
}
