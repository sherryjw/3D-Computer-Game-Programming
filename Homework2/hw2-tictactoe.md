# Unity编程实践——井字棋

#### **【项目说明】**
&emsp;&emsp;该项目使用 Unity 实现玩家对战的井字棋，所有 UI 均使用 [IMGUI](https://docs.unity3d.com/Manual/GUIScriptingGuide.html) 构建。项目所有资源均已上传至 gitee ✈️[传送门]()。<br/><br/>

#### **【游戏简介】**

&emsp;&emsp;井字棋（Tic-Tac-Toe）是一种在3*3格子上进行的连珠游戏，由分别代表 O 和 X 的两个玩家轮流在格子里留下标记，任意三个标记形成一条直线，则为获胜。<br/><br/>

#### **【实现过程】**

&emsp;&emsp;0、创建脚本 TicTacToe.cs，并将该脚本拖到 Unity 场景中的 Main Camera，后续调试均通过运行游戏来完成。<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@latest/image/3d-hw2-008.png)<br/>

&emsp;&emsp;1、确定程序框架。<br/>

```C#
/*渲染棋子的纹理*/
public Texture2D O;//红圈 
public Texture2D X;//红叉
public Texture2D Background;//背景
public Texture2D Grid;//网格底图

private bool menu;//表示是否显示游戏主页
public int turn;//表示当前玩家，0 表示玩家 1，其棋子为红圈；1 代表玩家 2，其棋子为红叉
public bool gameover;//表示游戏是否到达结束状态，当一方获胜或双方平局时游戏结束
public bool draw;//表示游戏结果是否为平局
public int[,] grid = new int[3, 3];//表示 3*3 的棋盘的网格的状态。对于网格[i, j](0<=i<3, 0<=j<3)而言，-1 表示还没有玩家占据此处，0 表示玩家 1 占据此处，1 表示玩家 2 占据此处

void Start(){
    /*游戏初始化*/
}

void NewGame(){
    /*（重新）开始游戏*/
}

void OnGUI(){
    /*处理游戏界面及变换*/
}
```

&emsp;&emsp;2、实现程序框架中的每个函数。<br/>

- 实现游戏的初始化：
```C#
void Start() {
    menu = true;
    NewGame();
}
```
&emsp;&emsp;游戏初始化时应显示游戏主页，同时初始化游戏状态。

- 实现游戏的（重新）开始：
```C#
void NewGame() {
    turn = 0;
    gameover = false;
    draw = false;
    for (int i = 0; i < 3; ++i) {
        for (int j = 0; j < 3; ++j) {
            grid[i, j] = -1;
        }
    }
}
```
&emsp;&emsp;游戏开始时，默认玩家 1 为先手，游戏结束状态与游戏结果平局标志均重置为 false，3*3 的棋盘的所有网格均重置为无人占据状态。

- 实现游戏界面的处理：

```C#
void BackToMenu(){
    GUI.Label(new Rect(0, 0, 1024, 685), Background);

    GUIStyle fontStyle = new GUIStyle();
    fontStyle.normal.textColor = Color.black;
    fontStyle.normal.background = null;
    fontStyle.fontSize = 80;

    GUIStyle fontStyle2 = new GUIStyle();
    fontStyle2.normal.textColor = Color.black;
    fontStyle2.normal.background = null;
    fontStyle2.fontSize = 40;

    GUI.Label(new Rect(Screen.width/10 + 80, Screen.height/10 + 45, 200, 50), "Tic Tac Toe", fontStyle);

    if (GUI.Button(new Rect(480, 250, 100, 50), "Play", fontStyle2)){
        menu = false;
    }
}

void OnGUI() {
    if (menu){
        BackToMenu();
    }
```
&emsp;&emsp;游戏界面分主页和游戏页，通过 menu 变量的值判断显示哪一页。当显示主页时，使用 **IMGUI** 构建包括背景、“Tic Tac Toe”游戏名称和“Start”按钮的 UI。

```C#
    else {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.textColor = Color.black;
        fontStyle.normal.background = null;
        fontStyle.fontSize = 25;

        GUI.Label(new Rect(0, 0, 1024, 685), Background);

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
```
&emsp;&emsp;当显示游戏页时，使用 **IMGUI** 构建包括背景、“Replay”按钮、“Menu”按钮以及棋盘、游戏状态的 UI。游戏过程中需要处理游戏逻辑，根据当前的游戏情况采取相应的动作：当玩家按下“Replay”按钮时，表示想要重新开始游戏，应调用 NewGame() 重置当前的游戏状态；当游戏结束时，需要在游戏界面显示游戏结果，同时**注意在游戏处于结束状态时任一玩家都不能再落子**；如果游戏还在进行中，则需要在游戏界面显示当前轮到哪一位玩家落子。最后更新棋盘。<br/>
&emsp;&emsp;限于篇幅此处省去对上述逻辑函数的实现描述，细节可参看[完整代码及注释]()。<br/><br/>

#### **【游戏效果】**

&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@latest/image/3d-hw2-027.png)<br/>

&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@latest/image/3d-hw2-028.png)<br/>

&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@latest/image/3d-hw2-029.png)<br/>

&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@latest/image/3d-hw2-030.png)<br/>

&emsp;&emsp;游戏成果动态展示见[TicTacToe](https://www.bilibili.com/video/BV1zK411P7Hg)。<br/>