# Unity-游戏对象与图形基础
### 3D Computer Game Programming-Note 4

1、基本操作演练

- 下载 Fantasy Skybox FREE，构建自己的游戏场景

  - 从 [Asset Store](https://assetstore.unity.com/packages/2d/textures-materials/sky/fantasy-skybox-free-18353) 上下载 Fantasy Skybox FREE：<br/>
  ![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-001.png)
  - 导入包到 Assets 中：<br/>
  ![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-002.png)
  - 在 Main Camera 上添加部件 Rendering -> Skybox，在已导入的 Fantasy Skybox FREE -> Materials 中选择喜欢的素材（.mat 文件）拖放到 Skybox 中：<br/>
  ![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-003.png)<br/>
  运行：<br/>
  ![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-004.png)
  - 在菜单栏选择 GameObject -> 3D Object -> Terrain，新建一个地形对象，同时为了更完整的视觉效果，在其附近创建另外三个地形对象：<br/>
  ![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-005.png)<br/>
  ![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-006.png)
  - 在 Terrain 的 Inspector 窗口中选择合适的工具渲染起伏的地形、花草等，从 Asset Store 中导入水面预制，游戏场景最终效果如下：<br/>
  ![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-007.png)


- 写一个简单的总结，总结游戏对象的使用。

&emsp;&emsp;作为 Unity 中的基本对象，游戏对象可以作为组件的容器使用，即通过在游戏对象上挂载不同的组件来获得相关的属性，从而实现不同的功能。<br/>
&emsp;&emsp;比如，当希望得到一个视觉效果惊艳的游戏对象时，可以挂载 Material 调整颜色透明度等，或是挂载 Texture2D 为其贴上纹理；当希望一个游戏对象可以作为一个音频播放器时，可以在该游戏对象上挂载一个 Audio；当希望游戏对象可以在游戏运行时自动完成某些动作时，可以编写脚本并挂载到游戏对象上，在运行时自动执行脚本指定的逻辑实现。<br/>
&emsp;&emsp;游戏对象既可以通过直接创建的方式实例化，也可以通过预制来实例化。

<br/><br/>

2、实现《牧师与魔鬼》动作分离版

```
 要求：设计一个裁判类，当游戏达到结束条件时，通知场景控制器游戏结束
```
&emsp;&emsp;本次游戏是在[🔗牧师与恶魔](https://www.yuque.com/pijiuwujializijun/acorbw/xh0mcw)的基础上将动作管理与游戏场景分离而实现的。借用课程主页的设计图：<br/>
![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-008.png)<br/>

&emsp;&emsp;设计思路如下：

- 通过门面模式（控制器模式）输出组合好的几个动作，共原来程序调用
  - 这个门面就是 CCActionManager
- 通过组合模式实现动作组合，按组合模式设计方法
  - 必须有一个抽象事物表示该类事物的共性，例如 SSAction，表示动作，不管是基本动作或是组合后的动作
  - 基本动作，用户设计的基本动作类。 例如：CCMoveToAction
  - 组合动作，由（基本或组合）动作组合的类。例如：CCSequenceAction
- 接口回调（函数回调）实现管理者与被管理者解耦
  - 如组合对象实现一个事件抽象接口（ISSCallback），作为监听器（listener）监听子动作的事件
  - 被组合对象使用监听器传递消息给管理者。至于管理者如何处理由实现该监听器的人决定
- 通过模板方法，让使用者减少对动作管理过程细节的要求
  - SSActionManager 作为 CCActionManager 基类

<br/>

&emsp;&emsp;根据该设计思路，修改程序结构，对应的 UML 图如下：<br/>
![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-009.png)<br/>

<br/>

&emsp;&emsp;为了实现动作分离，需要新增以下文件：
- **SSAction.cs:** 继承 ScriptableObject（不需要绑定 GameObject 对象的可编程基类），作为游戏动作的基类
```C#
public class SSAction : ScriptableObject {
    public bool enable = true;
    public bool destory = false;

    public GameObject gameObject { get; set; }
    public Transform transform { get; set; }
    public ISSActionCallback callback { get; set; }

    protected SSAction() { }

    public virtual void Start() {
        throw new System.NotImplementedException();
    }

    public virtual void Update() {
        throw new System.NotImplementedException();
    }
}
```

- **CCMoveToAction.cs:** 实现具体动作，将一个物体移动到目标位置，并通知任务完成
```C#
public class CCMoveToAction : SSAction {
    public Vector3 target;
    public float speed;

    public static CCMoveToAction GetSSAction(Vector3 target, float speed) {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    private CCMoveToAction() { }

    public override void Start() { }

    public override void Update() {
        if (this.gameObject == null || this.transform.localPosition == target)
        {
            this.destory = true;
            this.callback.SSActionEvent(this);
            return;
        }
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, target, speed * Time.deltaTime);
    }
}
```

- **CCSequenceAction.cs:** 实现一个动作组合序列，顺序播放动作。在本游戏中主要服务于角色上下船的运动轨迹（只有一个移动动作时角色会沿直线而非折线运动，出现穿过其他游戏对象的情况）
```C#
public class CCSequenceAction : SSAction, ISSActionCallback {
    public List<SSAction> sequence;
    public int repeat = -1;
    public int start = 0;

    public static CCSequenceAction GetSSAction(int repeat, int start, List<SSAction> sequence) {
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }

    public override void Start() {
        foreach (SSAction action in sequence) {
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }

    public override void Update() {
        if (sequence.Count == 0)
            return;
        if (start < sequence.Count)
            sequence[start].Update();
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competed, int Param = 0, string strParam = null, Object objectParam = null) {
        source.destory = false;
        this.start++;
        if (this.start >= sequence.Count) {
            this.start = 0;
            if (repeat > 0)
                repeat--;
            if (repeat == 0) {
                this.destory = true;
                this.callback.SSActionEvent(this);
            }
        }
    }

    void OnDestory() { }
}
```

- **SSActionManager.cs:** 作为动作对象管理器的基类，实现了所有动作的基本管理
```C#
public class SSActionManager : MonoBehaviour {
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();

    protected void Start() { }

    protected void Update() {
        foreach (SSAction ac in waitingAdd)
            actions[ac.GetInstanceID()] = ac;
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions) {
            SSAction ac = kv.Value;
            if (ac.destory) {
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable) {
                ac.Update();
            }
        }

        foreach (int key in waitingDelete) {
            SSAction ac = actions[key];
            actions.Remove(key);
            Destroy(ac);
        }
        waitingDelete.Clear();
    }

    public void RunAction(GameObject gameObject, SSAction action, ISSActionCallback manager) {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }
}
```

- **CCActionManager.cs:** 封装游戏中的具体动作，提供接口供场景控制器调用，实现动作管理与游戏场景分离
```C#
public class CCActionManager : SSActionManager, ISSActionCallback {
    private bool isMoving = false;
    public CCMoveToAction MoveBoatAction;
    public CCSequenceAction MoveRoleAction;
    public FirstController sceneController;

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competed, int Param = 0, string strParam = null, Object objectParam = null) {
        isMoving = false;
    }

    protected new void Start() {
        sceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
        sceneController.SetActionManager(this);
    }

    public bool GetIsMoving() {
        return isMoving;
    }

    public void MoveBoat(GameObject obj, Vector3 target, float speed) {
        if (isMoving) {
            return;
        }
        isMoving = true;
        MoveBoatAction = CCMoveToAction.GetSSAction(target, speed);
        this.RunAction(obj, MoveBoatAction, this);
    }

    public void MoveRole(GameObject role, Vector3 transfer, Vector3 target, float speed) {
        if (isMoving) {
            return;
        }
        isMoving = true;
        MoveRoleAction = CCSequenceAction.GetSSAction(0, 0, new List<SSAction> { CCMoveToAction.GetSSAction(transfer, speed), CCMoveToAction.GetSSAction(target, speed) });
        this.RunAction(role, MoveRoleAction, this);
    }
}
```

- **ISSActionCallback.cs:** 实现消息通知，避免与动作管理者直接依赖
```C#
public enum SSActionEventType : int { Started, Competed }
public interface ISSActionCallback {
    void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competed,
        int intParam = 0,
        string strParam = null,
        Object objectParam = null);
}
```

- **Judge.cs:** 裁判类，当游戏达到结束条件时，通知场景控制器游戏结束
```C#
public class Judge : MonoBehaviour {
    public FirstController sceneController;
    public CoastModel srcCoastModel;
    public CoastModel desCoastModel;
    public BoatModel boatModel;

    void Start() {
        sceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
        srcCoastModel = sceneController.SrcCoastController.GetCoastModel();
        desCoastModel = sceneController.DesCoastController.GetCoastModel();
        boatModel = sceneController.boatController.GetBoatModel();
    }

    // 参考之前的 Check()
    void Update() {
        if (!sceneController.isRunning)
            return;
            
        if (sceneController.time <= 0) {
            sceneController.JudgeCallback("Game Over!", false);
            return;
        }
        this.gameObject.GetComponent<UserGUI>().result = "";

        if (desCoastModel.priestNum == 3) {
            sceneController.JudgeCallback("You Win!", false);
            return;
        }
        else {
            int leftPriestNum, leftDevilNum, rightPriestNum, rightDevilNum;
            leftPriestNum = srcCoastModel.priestNum + (boatModel.OnRight ? 0 : boatModel.priestNum);
            leftDevilNum = srcCoastModel.devilNum + (boatModel.OnRight ? 0 : boatModel.devilNum);
            if (leftPriestNum != 0 && leftPriestNum < leftDevilNum) {
                sceneController.JudgeCallback("Game Over!", false);
                return;
            }

            rightPriestNum = desCoastModel.priestNum + (boatModel.OnRight ? boatModel.priestNum : 0);
            rightDevilNum = desCoastModel.devilNum + (boatModel.OnRight ? boatModel.devilNum : 0);
            if (rightPriestNum != 0 && rightPriestNum < rightDevilNum) {
                sceneController.JudgeCallback("Game Over!", false);
                return;
            }
        }
    }
}
```
<br/>

&emsp;&emsp;同时，需要更改以下文件：
- **FirstController.cs:** 主要更改与动作管理相关的部分（与上一版相同的部分代码略去）
```C#
public class FirstController : MonoBehaviour, ISceneController, IUserAction {
    /* 增加动作管理器 */
    public CCActionManager ActionManager;

    public CoastController DesCoastController;
    public CoastController SrcCoastController;
    public BoatController boatController;
    public RoleController[] roleModelControllers;
    //private MoveController moveController;

    private RiverModel river;
    public bool isRunning;
    public float time;
    public float speed = 8;

    public void SetActionManager(CCActionManager actionManager) {
        this.ActionManager = actionManager;
    }

    void Awake() {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        director.CurrentSceneController.LoadResources();
        this.gameObject.AddComponent<UserGUI>();
        this.gameObject.AddComponent<CCActionManager>();
        this.gameObject.AddComponent<Judge>();
    }

    public void LoadResources() {
        /*...*/
    }

    /* 使用动作管理器提供的接口实现运动，取代原来的 moveController；下同 */
    public void MoveBoat() {
        if ((!isRunning) || ActionManager.GetIsMoving())
            return;

        if (boatController.GetBoatModel().OnRight)
            ActionManager.MoveBoat(boatController.GetBoatModel().boat, PositionModel.boat_on_left, speed);
        else
            ActionManager.MoveBoat(boatController.GetBoatModel().boat, PositionModel.boat_on_right, speed);

        boatController.GetBoatModel().OnRight = !boatController.GetBoatModel().OnRight;
    }

    public void MoveRole(RoleModel roleModel) {
        if ((!isRunning) || ActionManager.GetIsMoving())
            return;

        Vector3 target, transfer;
        if (roleModel.OnBoat) {
            if (boatController.GetBoatModel().OnRight)
                target = DesCoastController.AddRole(roleModel);
            else
                target = SrcCoastController.AddRole(roleModel);

            /* 设置一个中转点使运动轨迹成为折线；下同 */
            if (roleModel.role.transform.localPosition.y > target.y)
                transfer = new Vector3(target.x, roleModel.role.transform.localPosition.y, target.z);
            else
                transfer = new Vector3(roleModel.role.transform.localPosition.x, target.y, target.z);

            ActionManager.MoveRole(roleModel.role, transfer, target, 5);
            roleModel.OnRight = boatController.GetBoatModel().OnRight;
            boatController.RemoveRole(roleModel);
        }
        else {
            if (boatController.GetBoatModel().OnRight == roleModel.OnRight) {
                if (roleModel.OnRight) {
                    DesCoastController.RemoveRole(roleModel);
                }
                else {
                    SrcCoastController.RemoveRole(roleModel);
                }
                target = boatController.AddRole(roleModel);
                if (roleModel.role.transform.localPosition.y > target.y)
                    transfer = new Vector3(target.x, roleModel.role.transform.localPosition.y, target.z);
                else
                    transfer = new Vector3(roleModel.role.transform.localPosition.x, target.y, target.z);
                ActionManager.MoveRole(roleModel.role, transfer, target, 5);
            }
        }
    }

    public void Restart() {
        /*...*/
    }

    void Update() {
        if (isRunning) {
            time -= Time.deltaTime;
            this.gameObject.GetComponent<UserGUI>().time = (int)time;
        }
    }

    /* 将裁判类的返回信息呈现在游戏场景中 */
    public void JudgeCallback(string result, bool isRunning) {
        this.gameObject.GetComponent<UserGUI>().result = result;
        this.gameObject.GetComponent<UserGUI>().time = (int)time;
        this.isRunning = isRunning;
    }
}
```
<br/>

&emsp;&emsp;此外，由于增加了游戏场景，上一版中由长方体构成的“粗制滥造”的 River 已经不需要了，因此将所有文件中有关 ``RiverModel`` 的部分全部删去。

&emsp;&emsp;【游戏效果图】<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-010.png)<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@v1.5.6/image/3d-hw4-011.png)<br/>

&emsp;&emsp;【动态展示】<br/>
&emsp;&emsp;[🔗视频链接](https://www.bilibili.com/video/BV1hK411N7gz/)（新的游戏场景占用较多内存，游戏过程会有明显卡顿）

<br/><br/>

3、材料与渲染联系

- Standard Shader 自然场景渲染器
```
 选择合适内容，如 Albedo Color and Transparency，寻找合适素材，展示相关效果的呈现
```
&emsp;&emsp;创建一个球体和一个 Material，将 Material 拖到球体上。首先调整颜色：<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-012.png)<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-013.png)<br/>

&emsp;&emsp;通过调整 Alpha 值来控制其透明度：当 Rendering Mode 为 Opaque 时调整无效；当 Rendering Mode 为 Cutout 时，Alpha 值为 0~127 时材质为完全透明，而 Alpha 值为 128~255 时材质为完全不透明；当 Rendering Mode 为 Fade 时可以实现任意透明度；当 Rendering Mode 为 Transparent 时可以实现一定范围内的任意透明度：<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-014.png)<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-015.png)<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-016.png)<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-017.png)<br/>

&emsp;&emsp;调整 Metallic 值可以控制材质的金属感：<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-018.png)<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-019.png)<br/>
&emsp;&emsp;（联想到了哈利波特的金色飞贼）<br/>

&emsp;&emsp;调整 Smoothness 值可以控制材质的平滑度：<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-020.png)<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw4-021.png)<br/>
&emsp;&emsp;（质感接近台球了）<br/>
<br/><br/>

- 声音
```
 给出游戏中利用 Reverb Zones 呈现车辆穿过隧道的声效的案例
```
&emsp;&emsp;下载声音素材[🔗Engine](https://assetstore.unity.com/packages/audio/sound-fx/engines-123836)，在 Unity 场景中创建一个空对象，在该空对象上挂载组件 Audio Source 和 Audio Reverb Zone，将素材拖放到 Audio Source 的 AudioClip 作为声音资源，同时开启 Loop，并在 Audio Reverb Zone 中设置 Reverb Preset 为 Cave（隧道声效），运行游戏即可。

&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@v1.5.6/image/3d-hw4-022.png)<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@v1.5.6/image/3d-hw4-023.png)<br/>
