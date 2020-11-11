using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
    private IUserAction userAction;
    private bool index = true;
    public string result;

    void Start()
    {
        userAction = SSDirector.GetInstance().CurrentSceneController as IUserAction;
    }

    void OnGUI()
    {
        GUIStyle style1 = new GUIStyle();
        GUIStyle style2 = new GUIStyle();
        GUIStyle style3 = new GUIStyle();
        GUIStyle style4 = new GUIStyle();
        GUIStyle style5 = new GUIStyle();
        GUIStyle style6 = new GUIStyle();
        GUIStyle style7 = new GUIStyle();

        style1.normal.textColor = style2.normal.textColor = style5.normal.textColor = Color.white;
        style3.normal.textColor = style4.normal.textColor = new Color(0.72f, 0.25f, 0.25f);
        style6.normal.textColor = new Color(0.56f, 0, 0);
        style7.normal.textColor = new Color(1.0f, 0.81f, 0.06f);
        style1.fontSize = style4.fontSize = 60;
        style2.fontSize = style7.fontSize = 40;
        style6.fontSize = 30;
        style3.fontSize = style5.fontSize = 20;

        if (index)
        {
            GUI.Label(new Rect(250, 80, 60, 300), "Hit UFO", style1);
            if (GUI.Button(new Rect(320, 200, 60, 50), "Play", style2))
            {
                index = false;
                userAction.ReStart();
            }

            if (GUI.Button(new Rect(310, 250, 60, 50), "Mode", style7))
            {
                userAction.SwitchMode();
            }

            if (userAction.isPhysics())
            {
                GUI.Label(new Rect(20, 20, 200, 50), "PHYSICS", style6);
            }
            else
            {
                GUI.Label(new Rect(20, 20, 200, 50), "ORIGIN", style6);
            }
        }
        else
        {
            GUI.Label(new Rect(12, 12, 200, 50), "Round:" + userAction.GetRound(), style3);
            GUI.Label(new Rect(Screen.width - 90, 12, 200, 50), "Score:" + userAction.GetScore(), style3);

            GUI.Label(new Rect(12, 42, 200, 50), "Mode", style3);
            if (userAction.isPhysics())
            {
                GUI.Label(new Rect(72, 42, 200, 50), "PHYSICS", style5);
            }
            else
            {
                GUI.Label(new Rect(72, 42, 200, 50), "ORIGIN", style5);
            }

            if (userAction.isGameOver())
            {
                GUI.Label(new Rect(200, 80, 100, 100), "Game Over", style4);
                GUI.Label(new Rect(288, 170, 50, 50), "Your Score:" + userAction.GetScore().ToString(), style5);
                if (GUI.Button(new Rect(290, Screen.height / 2 + 30, 100, 50), "Replay", style2))
                {
                    userAction.ReStart();
                    return;
                }
                userAction.GameOver();
            }
        }
    }
}
