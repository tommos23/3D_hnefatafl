using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoad : MonoBehaviour {

    // Font used for buttons,input from unity editor
    public Font buttonFont;

    // dynamic button placement for different sized screens
    float buttonWidthOffset = (Screen.width / 2) - 125;
    float buttonHeightOffset = (Screen.height / 2);

    // Static IP address that can be accessed from UDP objects 
    static public string ipAddrString = "127.0.0.1";


    // Use this for initialization
    void Start ()
    {
        //Set Default Game Condition
        Assets.ApplicationModel.gameType = Assets.ApplicationModel.GameType.Player_V_Player;
	}

    void OnGUI()
    {
        // Set up style to be used throughout scene
        GUIStyle style = new GUIStyle();
        style.font = buttonFont;

        // create input field for board ip address
        ipAddrString = GUI.TextField(new Rect(buttonWidthOffset, buttonHeightOffset + 80, 250, 30), ipAddrString);

        // Create game buttons which each load a different mode
        if (GUI.Button(new Rect(buttonWidthOffset, buttonHeightOffset + 40, 250, 30), "Player Vs Player"))
        {
            Assets.ApplicationModel.gameType = Assets.ApplicationModel.GameType.Player_V_Player;
            SceneManager.LoadScene("main");
        }

        if (GUI.Button(new Rect(buttonWidthOffset, buttonHeightOffset, 250, 30), "AI Vs Player"))
        {
            Assets.ApplicationModel.gameType = Assets.ApplicationModel.GameType.AI_V_Player;
            SceneManager.LoadScene("main");
        }

        if (GUI.Button(new Rect(buttonWidthOffset, buttonHeightOffset - 40, 250, 30), "Player Vs AI"))
        {
            Assets.ApplicationModel.gameType = Assets.ApplicationModel.GameType.Player_V_AI;
            SceneManager.LoadScene("main");
        }

        if (GUI.Button(new Rect(buttonWidthOffset, buttonHeightOffset - 80, 250, 30), "AI Vs AI"))
        {
            Assets.ApplicationModel.gameType = Assets.ApplicationModel.GameType.AI_V_AI;
            SceneManager.LoadScene("main");
        }


    }

}
