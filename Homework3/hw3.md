# Unity-空间与运动
### 3D Computer Game Programming-Note 3

1、简答并用程序验证

- 游戏对象运动的本质是什么？

&emsp;&emsp;游戏对象运动的本质是**游戏对象的空间属性的变化，通过矩阵变换（平移、旋转、缩放）实现**。

- 请用三种方法以上方法，实现物体的抛物线运动。

&emsp;&emsp;方法一：直接修改物体的 Transform 的属性 [position](https://docs.unity3d.com/ScriptReference/Transform-position.html)。
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sports1 : MonoBehaviour {
    private float Xspeed = 2.0f;
    private float Yspeed = 0;
    private float gravity = 9.8f;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        this.transform.position += Xspeed * Vector3.left * Time.deltaTime;
        this.transform.position += Yspeed * Vector3.down * Time.deltaTime;
        Yspeed += gravity * Time.deltaTime;
    }
}
```

&emsp;&emsp;方法二：使用 Transform 的方法 [Translate](https://docs.unity3d.com/ScriptReference/Transform.Translate.html)。
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sports2 : MonoBehaviour {
    private float Xspeed = 2.0f;
    private float Yspeed = 0;
    private float gravity = 9.8f;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        this.transform.Translate(Xspeed * Time.deltaTime, 0, 0);
        this.transform.Translate(0, -Yspeed * Time.deltaTime, 0, Space.World);
        Yspeed += gravity * Time.deltaTime;
    }
}
```

&emsp;&emsp;方法三：使用 [Vector3](https://docs.unity3d.com/ScriptReference/Vector3-ctor.html) 的方法构造向量。
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sports3 : MonoBehaviour {
    private float Xspeed = 2.0f;
    private float Yspeed = 0;
    private float gravity = 9.8f;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 myVector = new Vector3(Xspeed * Time.deltaTime, -Yspeed * Time.deltaTime, 0);
        this.transform.position += myVector;
        Yspeed += gravity * Time.deltaTime;
    }
}
```

- 写一个程序，实现一个完整的太阳系，其他星球围绕太阳的转速必须不一样，且不在一个法平面上。

  - 在编写脚本前先创建所有天体对应的游戏对象，使八大行星成为太阳的子对象：
  ![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw3-001.png)<br/>

  - 编写脚本 SolarSystem.cs，使用 [RotateAround](https://docs.unity3d.com/ScriptReference/Transform.RotateAround.html)；使用[Rotate](https://docs.unity3d.com/ScriptReference/Transform.Rotate.html)实现天体自转；参数参考真实宇宙大致给出：

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour {
    public Transform Sun;
    public Transform Mercury;
    public Transform Venus;
    public Transform Earth;
    public Transform Mars;
    public Transform Jupiter;
    public Transform Saturn;
    public Transform Uranus;
    public Transform Neptune;

    // Start is called before the first frame update
    void Start() {
        //初始化天体的位置
        Sun.position = Vector3.zero;
        Mercury.position = new Vector3(0.9f, 0, 0);
        Venus.position = new Vector3(1.5f, 0, 0);
        Earth.position = new Vector3(2.2f, 0, 0);
        Mars.position = new Vector3(3.0f, 0, 0);
        Jupiter.position = new Vector3(4.0f, 0, 0);
        Saturn.position = new Vector3(5.2f, 0, 0);
        Uranus.position = new Vector3(6.5f, 0, 0);
        Neptune.position = new Vector3(7.8f, 0, 0);
        //调整天体的大小
        Sun.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        Mercury.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        Venus.localScale -= new Vector3(0.4f, 0.4f, 0.4f);
        Earth.localScale -= new Vector3(0.4f, 0.4f, 0.4f);
        Mars.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        Saturn.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
        Uranus.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
        Neptune.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update() {
        //RotateAround 方法模拟天体绕太阳公转，Rotate 方法模拟天体自转
        Mercury.RotateAround(Sun.position, new Vector3(0, 1, 2), 48 * Time.deltaTime);
        Mercury.Rotate(Vector3.up * 30 * Time.deltaTime);
        Venus.RotateAround(Sun.position, new Vector3(0, 1, -1), 35 * Time.deltaTime);
        Venus.Rotate(Vector3.up * 30 * Time.deltaTime);
        Earth.RotateAround(Sun.position, Vector3.up, 30 * Time.deltaTime);
        Earth.Rotate(Vector3.up * 30 * Time.deltaTime);
        Mars.RotateAround(Sun.position, new Vector3(0, 1, 1), 24 * Time.deltaTime);
        Mars.Rotate(Vector3.up * 30 * Time.deltaTime);
        Jupiter.RotateAround(Sun.position, new Vector3(0, 5, 1), 13 * Time.deltaTime);
        Jupiter.Rotate(Vector3.up * 30 * Time.deltaTime);
        Saturn.RotateAround(Sun.position, new Vector3(0, 4, 1), 10 * Time.deltaTime);
        Saturn.Rotate(Vector3.up * 30 * Time.deltaTime);
        Uranus.RotateAround(Sun.position, new Vector3(0, 6, 1), 7 * Time.deltaTime);
        Uranus.Rotate(Vector3.up * 30 * Time.deltaTime);
        Neptune.RotateAround(Sun.position, new Vector3(0, 2, 1), 5 * Time.deltaTime);
        Neptune.Rotate(Vector3.up * 30 * Time.deltaTime);
    }
}
```

- - 将脚本拖放到 Main Camera，并设置相应的公有变量，运行：
  ![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw3-002.png)<br/>

  - 为所有天体添加部件 Trail Renderer，可以在运行时显示天体运动轨迹：
  ![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw3-005.png)<br/>
  ![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw3-003.png)<br/>
  ![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw3-004.png)<br/><br/>
  游戏成果动态展示[🔗视频链接](https://www.bilibili.com/video/BV1R54y117nB/)

<br/>

2、编程实践

- 阅读以下游戏脚本
```
Priests and Devils

Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are 3 priests and 3 devils at one side of the river. They all want to get to the other side of this river, but there is only one boat and this boat can only carry two persons each time. And there must be one person steering the boat from one side to the other side. In the flash game, you can click on them to move them and click the go button to move the boat to the other direction. If the priests are out numbered by the devils on either side of the river, they get killed and the game is over. You can try it in many ways. Keep all priests alive! Good luck!
```

- 列出游戏中提及的事物（Objects）
  牧师(Priest)、恶魔(Devil)、船(boat)、河(river)、河岸(sides of river)。

- 用表格列出玩家动作表（规则表）
  玩家动作（事件） | 条件 | 结果
  - | - | -
  点击牧师/恶魔 | 牧师/恶魔在岸上且船未满员 | 牧师/恶魔上船
  点击牧师/恶魔 | 牧师/恶魔在船上且船靠岸 | 牧师/恶魔上岸
  点击船 | 船上至少有一个人物 | 船驶向对岸
  | / | 三个牧师均已过河 | 游戏胜利
  | / | 有一侧河岸魔鬼数量多于牧师数量 | 游戏失败

- 编程要求
```
请将游戏中对象做成预制
在场景控制器 LoadResources 方法中加载并初始化长方形、正方形、球及其色彩代表游戏中的对象
使用 C# 集合类型有效组织对象
整个游戏仅主摄像机和一个 Empty 对象，其他对象必须代码动态生成整个游戏不许出现 Find 游戏对象，SendMessage 这类突破程序结构的通讯耦合语句
请使用课件架构图编程，不接受非 MVC 结构程序
注意细节，例如：船未靠岸，牧师与魔鬼上下船运动中，均不能接受用户事件
```

- **编程实现**

&emsp;&emsp;【游戏效果图】<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@v1.5.3/image/3d-hw3-006.png)<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@v1.5.3/image/3d-hw3-007.png)<br/>

&emsp;&emsp;【MVC架构设计】<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@v1.5.3/image/3d-hw3-010.png)<br/>

&emsp;&emsp;【游戏对象预制】<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@v1.5.3/image/3d-hw3-008.png)<br/>

&emsp;&emsp;【脚本实现】<br/>

&emsp;&emsp;以下按照 MVC 架构三个部分介绍，受篇幅影响此处省去代码细节，完整代码参见个人 [🔗github](https://github.com/sherryjw/3D-Computer-Game-Programming/tree/master/Homework3)。

&emsp;&emsp;①.模型（Model）：主要处理数据对象及关系（包括游戏对象、空间关系等）<br/>

&emsp;&emsp;**CoastModel.cs**：处理游戏对象——河岸
```C#
public class CoastModel {
    public GameObject obj;
    public int priestNum, devilNum;

    public CoastModel(string name, Vector3 position) {
        
    }
}
```

&emsp;&emsp;**BoatModel.cs**：处理游戏对象——船
```C#
public class BoatModel {
    public GameObject boat;
    public RoleModel[] roles;
    public int priestNum;
    public int devilNum;
    public bool OnRight;

    public BoatModel(Vector3 position) {
        
    }
}
```

&emsp;&emsp;**RiverModel.cs**：处理游戏对象——河
```C#
public class RiverModel {
    private GameObject river;

    public RiverModel(Vector3 position) {
        
    }
}
```

&emsp;&emsp;**RoleModel.cs**：处理游戏对象——角色：牧师、魔鬼
```C#
public class RoleModel {
    public GameObject role;
    public int tag;
    public int flag;
    public bool OnBoat;
    public bool OnRight;

    public RoleModel(Vector3 position, int flag, int tag) { // 0 represents Priest, 1 represents Devil 
        
    }
}
```

&emsp;&emsp;**PositionModel.cs**：处理游戏对象的空间位置
```C#
public class PositionModel
{
    public static Vector3 src_coast = new Vector3(-11, -2, 6);
    public static Vector3 des_coast = new Vector3(11, -2, 6);
    public static Vector3 river = new Vector3(1, -2.4f, 6);

    public static Vector3 boat_on_left = new Vector3(-4, -2, 6);
    public static Vector3 boat_on_right = new Vector3(4, -2, 6);

    public static Vector3[] roles = new Vector3[]{new Vector3(-0.2f, 0.8f, 0), new Vector3(-0.1f, 0.8f, 0), new Vector3(0, 0.8f, 0),
    new Vector3(0.1f, 0.8f,0), new Vector3(0.2f, 0.8f, 0), new Vector3(0.3f, 0.8f, 0)};

    public static Vector3[] roles_on_boat = new Vector3[] { new Vector3(-0.1f, 1.2f, 0), new Vector3(0.2f, 1.2f, 0) };
}
```

&emsp;&emsp;②.控制器（Controller）：接受用户事件，控制模型的变化<br/>
- 一个场景一个主控制器<br/>
- 至少实现与玩家交互的接口<br/>
- 实现或管理运动<br/>


&emsp;&emsp;**Director.cs**：获取当前游戏的场景，控制场景运行、切换、入栈与出栈
```C#
public class Director : System.Object {
    private static Director _instance;

    public ISceneController CurrentSenceController { get; set; }

    public static Director GetInstance() {
        
    }
}
```

&emsp;&emsp;**CoastController.cs**：处理与河岸相关的事件，包括牧师/魔鬼上岸、离岸等
```C#
public class CoastController {
    private CoastModel coast;

    public void CreateCoast(string name, Vector3 position) {
        
    }

    public Vector3 AddRole(RoleModel roleModel) {
        
    }

    public CoastModel GetCoastModel() {
        
    }

    public void RemoveRole(RoleModel roleModel) {
        
    }
}
```

&emsp;&emsp;**BoatController.cs**：处理与船相关的事件，包括牧师/恶魔上船、下船等
```C#
public class BoatController : ClickAction {
    BoatModel boatModel;
    IUserAction userAction;

    public BoatController() {

    }

    public void CreateBoat(Vector3 position) {
        
    }

    public Vector3 AddRole(RoleModel roleModel) {
        
    }

    public BoatModel GetBoatModel() {
        
    }

    public void RemoveRole(RoleModel roleModel) {
        
    }

    public void OnClick() {
        
    }
}
```

&emsp;&emsp;**RoleController.cs**：处理与游戏角色——牧师和魔鬼相关的事件，包括点击角色等
```C#
public class RoleController : ClickAction {
    RoleModel roleModel;
    IUserAction userAction;

    public RoleController() {

    }

    public void CreateRole(Vector3 position, int flag, int tag) {
        
    }

    public RoleModel GetRoleModel() {
        
    }

    public void OnClick() {
        
    }
}
```

&emsp;&emsp;**MoveController.cs**：处理与移动相关的事件，包括船离岸、靠岸等
```C#
public class MoveController {
    private GameObject obj;

    public void SetMove(Vector3 destination, GameObject obj) {
        
    }

    public bool GetIsMoving() {
        
    }
}
```

&emsp;&emsp;**FirstController.cs**：管理本次场景所有的游戏对象，协调游戏对象之间的通讯等
```C#
public class FirstController : MonoBehaviour, ISceneController, IUserAction {
    private CoastController DesCoastController;
    private CoastController SrcCoastController;
    private BoatController boatController;
    private RoleModelController[] roleModelControllers;
    private MoveController moveController;

    private RiverModel river;

    private bool isRuning;
    private float time;
       
    void Awake() {
        
    }

    public void LoadResources() {
        
    }

    public void MoveBoat() {
        
    }
 
    public void MoveRole(RoleModel roleModel) {
        
    }

    public void Restart() {
        
    }

    public void Check() {
        
    }

    void Update() {
        
    }
}
```

&emsp;&emsp;**ISceneController.cs**：提供实现资源加载的接口
```C#
public interface ISceneController {
    void LoadResources();
}
```

&emsp;&emsp;**IUserAction.cs**：提供用户交互事件的接口
```C#
public interface IUserAction {
    void MoveBoat();
    void MoveRole(RoleModel roleModel);
    void Check();
    void Restart();
}
```

&emsp;&emsp;**ClickAction.cs**：提供点击事件的接口
```C#
public interface ClickAction {
    void OnClick();
}
```

&emsp;&emsp;**Move.cs**：处理每一帧的场景变化
```C#
public class Move : MonoBehaviour {
    public bool isMoving = false; 
    public float speed = 8; 
    public Vector3 destination; 

    void Update() {
        
    }
}
```

&emsp;&emsp;③.界面（View）：显示模型，将人机交互事件交给控制器处理
- 处理接收 Input 事件
- 渲染 GUI ，接收事件

&emsp;&emsp;**Click.cs**：接收点击事件并交给控制器处理
```C#
public class Click : MonoBehaviour {
    ClickAction clickAction;

    void OnMouseDown() {

    }

    public void setClickAction(ClickAction clickAction) {

    }
}
```

&emsp;&emsp;**UserGUI.cs**：渲染 GUI，并接收事件
```C#
public class UserGUI : MonoBehaviour {
    private IUserAction userAction;

    public int time;
    public string result;

    void Start() {
        
    }

    void OnGUI() {

    }
}
```

<br/>
&emsp;&emsp;【游戏成果动态展示】

&emsp;&emsp;[🔗视频链接](https://www.bilibili.com/video/BV1NK4y1h7RS/)

<br/><br/>

3、思考题
- 使用向量与变换，实现并扩展 Tranform 提供的方法，如 Rotate、RotateAround 等。

&emsp;&emsp;实现如下形式的 Rotate：
```C#
public void Rotate(Vector3 axis, float angle, Transform t);
```

&emsp;&emsp;可以使用函数``public static Quaternion AngleAxis(float angle, Vector3 axis);``来实现，该函数创建一个绕轴 axis 成 角度 angle 的旋转。为了实现 Rotate 效果的呈现，还需要与 t 点乘：
```C#
public void Rotate(float angle, Vector3 axis, Transform t) {
    var a = Quaternion.AngleAxis(angle, axis);
    t.rotation *= a;
}
```
<br/>

&emsp;&emsp;实现如下形式的 RotateAround：
```C#
public void RotateAround(Vector3 point, Vector3 axis, float angle, Transform t);
```

&emsp;&emsp;同样使用函数``public static Quaternion AngleAxis(float angle, Vector3 axis);``来实现，由于是围绕一个中心点旋转，需要计算物体的位移矢量，对其进行旋转后再计算得到新的位置：
```C#
public void RotateAround(Vector3 point, Vector3 axis, float angle, Transform t) {
    var a = Quaternion.AngleAxis(angle, axis);
    var distance = t.position - point;
    distance *= a;
    t.position = distance + point;
    t.rotation *= a;
}
```
