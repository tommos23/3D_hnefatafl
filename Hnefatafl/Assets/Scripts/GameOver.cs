using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {


    float buttonWidthOffset = (Screen.width / 2) - 125;
    float buttonHeightOffset = (Screen.height / 2);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 50;
        style.normal.textColor = Color.black;

        GUI.Label(new Rect(buttonWidthOffset, buttonHeightOffset - 80, 100, 30), "Player " + GameManager.gameWinner +  " Wins", style);

        if (GUI.Button(new Rect(buttonWidthOffset, buttonHeightOffset, 250, 30), "Click to return to menu"))
        {
            SceneManager.LoadScene("splash");

        }
    }
}
