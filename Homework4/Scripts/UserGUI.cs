using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction userAction;

    public int time;
    public string result;

    void Start()
    {
        time = 120;
        userAction = SSDirector.GetInstance().CurrentSceneController as IUserAction;
    }

    void OnGUI()
    {
        //userAction.Check();

        GUIStyle style = new GUIStyle();
        GUIStyle style2 = new GUIStyle();
        style.normal.textColor = style2.normal.textColor = Color.white;
        style.fontSize = 20;
        style2.fontSize = 50;

        GUI.Label(new Rect(25, 20, 100, 50), "Time: " + time, style);
        GUI.Label(new Rect(190, 50, 50, 200), "Priests and Devils", style2);
        GUI.Label(new Rect(330, 140, 50, 200), result, style);
        if (GUI.Button(new Rect(690, 20, 60, 30), "Restart", style))
        {
            userAction.Restart();
        }
    }
}