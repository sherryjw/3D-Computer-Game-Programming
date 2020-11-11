# Unity-物理系统与碰撞
### 3D Computer Game Programming-Note 6

<br/>

1、改进飞碟（Hit UFO）游戏：

- 游戏内容要求：
  - 按 adapter 模式设计图修改飞碟游戏
  - 使它同时支持物理运动与运动学（变换）运动

#### **【游戏设计】**

&emsp;&emsp;在[打飞碟(v1.0)](https://www.yuque.com/pijiuwujializijun/acorbw/xh27ue)的基础上增加飞碟的物理力学运动，并提供运动模式的切换。<br/><br/>

#### **【结构设计】**

&emsp;&emsp;adapter 模式，即适配器模式，是一种通过将一个类的接口转换成客户期望的另外一个接口，使得原本由于接口不兼容而不能一起工作的那些类能一起工作的设计模式。简而言之，我们的任务就是运用一个适配器来实现飞碟动作管理器不同类的接口的衔接。
&emsp;&emsp;根据课程主页的 adapter 模式设计图修改程序结构如下：<br/>
![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw6-007.png)<br/>

<br/>

#### **【编程实现】**

- **IUserAction.cs:** 增加对模式转换和当前模式判断的支持的接口
```C#
public interface IUserAction {
    /* ... */
    bool isPhysics();
    void SwitchMode();
}
```

- **FirstController.cs:** 补充实现上述接口，并新增一个人机交互事件——用户可以在游戏过程中随时按下鼠标右键实现模式切换；为了使游戏难度更加自然，需要对发射飞碟的参数进行调整
```C#
public class FirstController : MonoBehaviour, ISceneController, IUserAction {
    public IActionManager actionManager;
    public DiskFactory diskFactory;
    public ScoreRecorder scoreRecorder;
    
    private int round = 1;                                                  
    private int trial = 0;                                             
    private bool running = false;
    private int count = 0;

    private bool isPhysicsMode = false;

    void Start () {
        SSDirector.GetInstance().CurrentSceneController = this;
        diskFactory = Singleton<DiskFactory>.Instance;
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
        gameObject.AddComponent<FlyActionManager>();
        gameObject.AddComponent<DiskFlyPhysicsActionManager>();
        actionManager = Singleton<FlyActionManager>.Instance;
        gameObject.AddComponent<UserGUI>();
    }

    void Update () {
        if(running) {
            count++;
            //用户按下鼠标左键时进行Hit事件处理
            if (Input.GetButtonDown("Fire1")) {
                Hit(Input.mousePosition);
            }

            //用户按下鼠标右键可切换飞碟运动模式
            if (Input.GetMouseButtonDown(1))
            {
                this.SwitchMode();
            }

            //根据游戏轮次设计飞碟的类型和发射速率
            /* ... */
        }
    }

    private void SendDisk(int type) {
        GameObject disk = diskFactory.GetDisk(type);

        float power = 0;
        float angle = 0;
        /* 物理运动模式下 power 不宜过大 */
        if (type == 1) {
            power = isPhysicsMode ? Random.Range(1f, 2f) : Random.Range(5f, 10f);
            angle = Random.Range(10f, 14f);
        }
        else if (type == 2) {
            power = isPhysicsMode ? Random.Range(1f, 2f) : Random.Range(8f, 13f);
            angle = Random.Range(12f, 16f);
        }
        else {
            power = isPhysicsMode ? Random.Range(1f, 2f) : Random.Range(13f, 18f);
            angle = Random.Range(16f, 20f);
        }
        actionManager.DiskFly(disk, angle, power);
    }

    public void FreeDisk(GameObject disk) {
        diskFactory.FreeDisk(disk);
    }

    public bool isPhysics() {
        return isPhysicsMode;
    }

    public void SwitchMode() {
        isPhysicsMode = !isPhysicsMode;
        actionManager = isPhysicsMode ? Singleton<DiskFlyPhysicsActionManager>.Instance : Singleton<FlyActionManager>.Instance as IActionManager;
    }
	
    /* ... */
}
```

- **DiskFlyPhysicsAction.cs:** 为飞碟增加刚体属性及恒定力，通过对其施加一个垂直向下的力使其从某一随机点开始做自由落体运动
```C#
public class DiskFlyPhysicsAction : SSAction {
    private Vector3 speed = Vector3.zero;

    private DiskFlyPhysicsAction() { }
    public override void Start() { }

    public static DiskFlyPhysicsAction GetSSAction(GameObject disk, float angle, float power) {
        ConstantForce constantForce = disk.GetComponent<ConstantForce>();
        if (constantForce) {
            constantForce.enabled = true;
            constantForce.force = new Vector3(0, -power, 0);
        }
        else {
            disk.AddComponent<Rigidbody>().useGravity = false;
            disk.AddComponent<ConstantForce>().force = new Vector3(0, -power, 0);
        }

        float x = Random.Range(-15f, 15f), y = Random.Range(15f, 20f), z = Random.Range(5f, 10f);
        disk.transform.position = new Vector3(x, y, z);
        DiskFlyPhysicsAction action = CreateInstance<DiskFlyPhysicsAction>();
        action.speed = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
        return action;
    }

    public override void Update() { }
}
```

- **IActionManager:** 以 IActionManager 为适配器，实现不同动作管理器类的接口的衔接，使飞行的动作模式能够相互切换
```C#
public interface IActionManager {
    void DiskFly(GameObject disk, float angle, float power);
}
```

- **DiskFlyPhysicsActionManager:** 参照 ``FlyActionManager.cs`` 编写，基本一致

```C#
public class DiskFlyPhysicsActionManager : SSActionManager, IActionManager, ISSActionCallback {
    public DiskFlyPhysicsAction flyAction;
    public FirstController sceneController;

    protected void Start() {
        sceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
        sceneController.actionManager = this;
    }

    public void DiskFly(GameObject disk, float angle, float power) {
        int direction = (disk.transform.position.x > 0) ? -1 : 1;
        flyAction = DiskFlyPhysicsAction.GetSSAction(disk, angle, power);
        this.RunAction(disk, flyAction, this);
    }

    public void SSActionEvent(SSAction source,
    SSActionEventType events = SSActionEventType.Competeted,
    int intParam = 0,
    string strParam = null,
    Object objectParam = null) {
        sceneController.FreeDisk(source.gameobject);
    }
}
```

- **UserGUI.cs:** 在游戏开始界面增加“Mode”按钮，用户可以使用鼠标左键点击该按钮进行模式切换，所选模式会显示在界面左上方；在游戏进行界面左上方同样会显示所选的模式
```C#
public class UserGUI : MonoBehaviour {
    private IUserAction userAction;
    private bool index = true;
    public string result;

    void OnGUI() {
        /* ... */

        if (index) {
            GUI.Label(new Rect(250, 80, 60, 300), "Hit UFO", style1);
            if (GUI.Button(new Rect(320, 200, 60, 50), "Play", style2)) {
                index = false;
                userAction.ReStart();
            }

            if (GUI.Button(new Rect(310, 250, 60, 50), "Mode", style7)) {
                userAction.SwitchMode();
            }

            if (userAction.isPhysics()) {
                GUI.Label(new Rect(20, 20, 200, 50), "PHYSICS", style6);
            }
            else {
                GUI.Label(new Rect(20, 20, 200, 50), "ORIGIN", style6);
            }
        }
        else {
            GUI.Label(new Rect(12, 12, 200, 50), "Round:" + userAction.GetRound(), style3);
            GUI.Label(new Rect(Screen.width - 90, 12, 200, 50), "Score:" + userAction.GetScore(), style3);

            GUI.Label(new Rect(12, 42, 200, 50), "Mode", style3);
            if (userAction.isPhysics()) {
                GUI.Label(new Rect(72, 42, 200, 50), "PHYSICS", style5);
            }
            else {
                GUI.Label(new Rect(72, 42, 200, 50), "ORIGIN", style5);
            }

            /* ... */
        }
    }
}
```
<br/>

#### **【游戏效果】**

![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw6-001.png)<br/>
![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw6-002.png)<br/>
![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw6-003.png)<br/>
![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw6-004.png)<br/>


#### **【动态展示】**

&emsp;&emsp;[🔗视频链接](https://www.bilibili.com/video/BV1Zf4y1i7Ax/)

<br/><br/>
