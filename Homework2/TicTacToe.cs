using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : MonoBehaviour {
    public Texture2D O;
    public Texture2D X;
    public Texture2D Background;
    public Texture2D Grid;

    private bool menu;
    private int turn;//0 for O, 1 for X
    private bool gameover;
    private bool draw;
    private int[,] grid = new int[3, 3];//0 for empty, 1 for O, 2 for X

    /*游戏初始化*/
    void Start() {
        menu = true;
        NewGame();
    }

    /*（重新）开始游戏*/
    void NewGame() {
        turn = 0;
        gameover = false;
        draw = false;
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                grid[i, j] = 0;
            }
        }
    }

    /*处理游戏界面及变换*/
    void OnGUI() {
        if (menu){
           BackToMenu();
        }
        else {
            GUIStyle fontStyle = new GUIStyle();
            fontStyle.normal.textColor = Color.black;
            fontStyle.normal.background = null;
            fontStyle.fontSize = 28;

            GUI.Label(new Rect(0, 0, 2048, 1370), Background);

            if (GUI.Button(new Rect(450, 160, 100, 50), "Replay", fontStyle))
                NewGame();
            if (GUI.Button(new Rect(455, 240, 100, 50), "Menu", fontStyle)){
                menu = true;
                BackToMenu();
                NewGame();
            }

            if ((gameover = Judge())) 
                ShowResult();
            else 
                ShowTurn();
            
            ShowChessboardAndListen();
        }
    }

    void BackToMenu(){
        GUI.Label(new Rect(0, 0, 2048, 1370), Background);

        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.textColor = Color.black;
        fontStyle.normal.background = null;
        fontStyle.fontSize = 80;

        GUIStyle fontStyle2 = new GUIStyle();
        fontStyle2.normal.textColor = Color.black;
        fontStyle2.normal.background = null;
        fontStyle2.fontSize = 40;

        GUI.Label(new Rect(Screen.width/10 + 80, Screen.height/10 + 45, 200, 50), "Tic Tac Toe", fontStyle);

        if (GUI.Button(new Rect(480, 250, 100, 50), "Start", fontStyle2)){
            menu = false;
        }
    }

    bool isVertical() {
        /*判断3*3棋盘中是否存在某一垂直线上的格子被同一玩家占据*/
        for (int i = 0; i < 3; ++i) {
            if (grid[0, i] == 1 || grid[0, i] == 2) {
                if ((grid[0, i] == grid[1, i]) && (grid[1, i] == grid[2, i])) {
                    return true;
                }
            }
        }
        return false;
    }

    bool isHorizontal() {
        /*判断3*3棋盘中是否存在某一水平线上的格子被同一玩家占据*/
        for (int i = 0; i < 3; ++i) {
            if (grid[i, 0] == 1 || grid[i, 0] == 2) {
                if ((grid[i, 0] == grid[i, 1]) && (grid[i, 1] == grid[i, 2])) {
                    return true;
                }
            }
        }
        return false;
    }

    bool isDiagonal() {
        /*判断3*3棋盘中主对角线或副对角线上的格子是否为同一玩家占据*/
        if (grid[0, 0] == 1 || grid[0, 0] == 2) {
            if ((grid[0, 0] == grid[1, 1]) && (grid[1, 1] == grid[2, 2])) {
                return true;
            }
        }
        if (grid[0, 2] == 1 || grid[0, 2] == 2) {
            if ((grid[0, 2] == grid[1, 1]) && (grid[1, 1] == grid[2, 0])) {
                return true;
            }
        }
        return false;
    }

    bool isDraw() {
        /*如果棋盘上存在未被占据的格子，则游戏仍在进行中；否则游戏结果为平局（注意判断有无输赢在判断是否平局之前完成了）*/
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                if (grid[i, j] == 0) {
                    return draw;
                }
            }
        }
        draw = true;
        return draw;
    }

    bool Judge() {
        return isVertical() || isHorizontal() || isDiagonal() || isDraw();
    }

    void ShowResult() {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.textColor = Color.black;
        fontStyle.normal.background = null;
        fontStyle.fontSize = 30;

        if (draw) {
            GUI.Label(new Rect(85, 40, 200, 50), "Game Over! It's a Draw!", fontStyle);
        }
        else if (turn == 1) {
            GUI.Label(new Rect(65, 40, 200, 50), "Game Over! Player 1 Wins!", fontStyle);
        }
        else if (turn == 0) {
            GUI.Label(new Rect(65, 40, 200, 50), "Game Over! Player 2 Wins!", fontStyle);
        }
    }

    void ShowTurn() {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.textColor = Color.black;
        fontStyle.normal.background = null;
        fontStyle.fontSize = 30;

        if (turn == 0) {
            GUI.Label(new Rect(60, 40, 200, 50), "Player 1, now it's your turn!", fontStyle);
        }
        else if (turn == 1) {
            GUI.Label(new Rect(60, 40, 200, 50), "Player 2, now it's your turn!", fontStyle);
        }
    }

    int GetOnesTurn() {
        return 1 - turn;
    }

    void ShowChessboardAndListen() {
        /*对棋盘的更新首先是遍历棋盘中的每一个格子，对于具体的格子来说有三种可能情况：*/
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                /*已被玩家 1 占据。此时格子应显示红圈：*/
                if (grid[i, j] == 1) {
                    GUI.Button(new Rect(80 * (i + 1.5f), 80 * (j + 1.2f), 80, 80), O);
                }
                /*已被玩家 2 占据。此时格子应显示红叉：*/
                else if (grid[i, j] == 2) {
                    GUI.Button(new Rect(80 * (i + 1.5f), 80 * (j + 1.2f), 80, 80), X);
                }
                /*没有玩家占据。如果玩家点击了这个空格子且游戏还在进行中，则切换为另一玩家并设置格子的新状态：*/
                else if (GUI.Button(new Rect(80 * (i + 1.5f), 80 * (j + 1.2f), 80, 80), Grid)) {
                    if (gameover == false) {
                        if (turn == 0) {
                            grid[i, j] = 1;
                        }
                        else if (turn == 1) {
                            grid[i, j] = 2;
                        }
                        turn = GetOnesTurn();
                    }
                }
            }
        }
    }
}
