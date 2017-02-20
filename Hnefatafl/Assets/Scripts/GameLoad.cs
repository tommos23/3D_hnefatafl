using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoad : MonoBehaviour {

    public Font buttonFont;
    float buttonWidthOffset = (Screen.width / 2) - 125;
    float buttonHeightOffset = (Screen.height / 2);
    // Use this for initialization
    void Start ()
    {
        //Set Default Game Condition
        Assets.ApplicationModel.gameType = Assets.ApplicationModel.GameType.Player_V_Player;
	}

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.font = buttonFont;

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
            //SceneManager.LoadScene("main");
        }
    }

	// Update is called once per frame
	void Update ()
    {
		
	}
}
