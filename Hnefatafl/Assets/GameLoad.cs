using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoad : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        //Set Default Game Condition
        Assets.ApplicationModel.gameType = Assets.ApplicationModel.GameType.Player_V_Player;
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 250, 30), "Player Vs Player"))
        {
            Assets.ApplicationModel.gameType = Assets.ApplicationModel.GameType.Player_V_Player;
            SceneManager.LoadScene("main");
        }

        if (GUI.Button(new Rect(10, 100, 250, 30), "AI Vs Player"))
        {
            Assets.ApplicationModel.gameType = Assets.ApplicationModel.GameType.AI_V_Player;
            SceneManager.LoadScene("main");
        }

        if (GUI.Button(new Rect(10, 130, 250, 30), "AI Vs AI"))
        {
            Assets.ApplicationModel.gameType = Assets.ApplicationModel.GameType.AI_V_AI;
            SceneManager.LoadScene("main");
        }
    }

	// Update is called once per frame
	void Update ()
    {
		
	}
}
