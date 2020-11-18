using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction userAction;
    private bool index = true;

    void Start()
    {
        userAction = SSDirector.GetInstance().CurrentScenceController as IUserAction;
    }

    void OnGUI()
    {
        GUIStyle titleStyle = new GUIStyle();
        GUIStyle buttonStyle = new GUIStyle();
        GUIStyle messageStyle = new GUIStyle();
        GUIStyle gameOverStyle = new GUIStyle();

        titleStyle.normal.textColor = messageStyle.normal.textColor = buttonStyle.normal.textColor = Color.white;
        gameOverStyle.normal.textColor = Color.red;
        titleStyle.fontSize = gameOverStyle.fontSize = 60;
        buttonStyle.fontSize = 40;
        messageStyle.fontSize = 20;

        if (index)
        {
            GUI.Label(new Rect(188, 80, 60, 300), "Intelligent Patrol", titleStyle);
            if (GUI.Button(new Rect(370, 220, 60, 50), "Play", buttonStyle))
            {
                index = false;
                userAction.PatrolMove();
            }
        }
        else
        {
            GUI.Label(new Rect(15, 15, 200, 50), "Score:", messageStyle);
            GUI.Label(new Rect(80, 15, 200, 50), userAction.GetScore().ToString(), messageStyle);
            if (userAction.GetGameover())
            {
                GUI.Label(new Rect(240, 80, 60, 300), "Game Over", titleStyle);
                if (GUI.Button(new Rect(340, 220, 60, 50), "Restart", buttonStyle))
                {
                    userAction.Restart();
                    return;
                }
            }
            else if (userAction.GetWin())
            {
                GUI.Label(new Rect(310, 80, 60, 300), "You win", titleStyle);
                if (GUI.Button(new Rect(345, 220, 60, 50), "Restart", buttonStyle))
                {
                    userAction.Restart();
                    return;
                }
            }
        }
    }

    void Update()
    {
        float dx = Input.GetAxis("Horizontal");
        float dz = Input.GetAxis("Vertical");
        userAction.MovePlayer(dx, dz);
    }
}
