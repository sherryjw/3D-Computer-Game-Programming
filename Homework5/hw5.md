# Unity-与游戏世界交互
### 3D Computer Game Programming-Note 5

<br/>

1、编写一个简单的鼠标打飞碟（Hit UFO）游戏

- 游戏内容要求：
  - 游戏有 n 个 round，每个 round 都包括 10 次 trial；
  - 每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同。它们由该 round 的 ruler 控制；
  - 每个 trial 的飞碟有随机性，总体难度随 round 上升；
  - 鼠标点中得分，得分规则按色彩、大小、速度不同计算，规则可自由设定。
- 游戏的要求：
  - 使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的！具体实现见参考资源 Singleton 模板类
  - 尽可能使用前面 MVC 结构实现人机交互与游戏模型分离


#### **【游戏设计】**

```
游戏规则：
· 在飞碟飞过时点击鼠标左键，尽可能击中更多的飞碟
· 游戏共分四个轮次，随着游戏的进行难度逐渐增加，具体表现为飞碟飞行速度的提升和飞行角度的变化
· 不同颜色的飞碟得分不同，击中难度也不同
```
<br/>

#### **【结构设计】**

&emsp;&emsp;参考游戏[牧师与魔鬼（动作分离版）](https://www.yuque.com/pijiuwujializijun/acorbw/vur27e)的 MVC 架构，保留SSDirector，SSAction和SSActionManager 等，重复部分略过不表。<br/>
&emsp;&emsp;使用带缓存的工厂模式管理飞碟，要求：
- DiskFactory 类是一个单实例类，用前面场景单实例创建
- DiskFactory 类有工厂方法 GetDisk 产生飞碟，有回收方法 Free（Disk）
- DiskFactory 使用模板模式根据预制和规则制作飞碟
- 对象模板包括飞碟对象与飞碟数据

&emsp;&emsp;借助课程主页的设计图，设计程序的 UML图：<br/>
![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@v1.5.9/image/3d-hw5-002.png)
<br/><br/>

#### **【编程实现】**

- **Singleton.cs:** 模板类，可以为每个 MonoBehaviour 子类创建一个对象的实例
```C#
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    protected static T instance;

    public static T Instance {
        get {
            if (instance == null) {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null) {
                    Debug.LogError("An instance of " + typeof(T) +
                    " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
}
```

- **DiskData.cs:** 记录飞碟数据，包括飞碟的类型、对应得分等，挂载到预制上时可以实现[自定义组件](#1)
```C#
public class DiskData : MonoBehaviour {
    public int type = 1;
    public int score = 1;                               
    public Color color = Color.white;                    
}
```

- **DiskFactory.cs:** 提供工厂方法 GetDisk 产生飞碟以及 FreeDisk 回收飞碟
```C#
public class DiskFactory : MonoBehaviour {
    private List<DiskData> used;
    private List<DiskData> free;

    void Start() {
        used = new List<DiskData>();
        free = new List<DiskData>();
    }

    public GameObject GetDisk(int type) {
        GameObject disk = null;
        if (free.Count > 0) {
            for (int i = 0; i < free.Count; i++) {
                if (free[i].type == type) {
                    disk = free[i].gameObject;
                    free.Remove(free[i]);
                    break;
                }
            }
        }

        float random_y = Random.Range(-3f, 1f);
        float random_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
        if (disk == null)
            disk = Instantiate(Resources.Load<GameObject>("Prefabs/disk"+type), new Vector3(0, -20f, 0), Quaternion.identity);
        disk.transform.position = new Vector3(random_x * 20f, random_y, 0);
        disk.GetComponent<Renderer>().material.color = disk.GetComponent<DiskData>().color;
        used.Add(disk.GetComponent<DiskData>());
        disk.SetActive(true);
        return disk;
    }

    public void FreeDisk(GameObject disk) {
        for (int i = 0; i < used.Count; ++i) {
            if (disk.GetInstanceID() == used[i].gameObject.GetInstanceID()) {
                used[i].gameObject.SetActive(false);
                used.Remove(used[i]);
                free.Add(used[i]);
                break;
            }
        }
    }

    public void Reset() {
        for (int i = 0; i < used.Count; i++) {
            if (used[i].gameObject.transform.position.y <= -20f) {
                free.Add(used[i]);
                used.Remove(used[i]);
            }
        }
    }
}
```

- **FlyActionManager.cs:** 管理飞碟的飞行，场景控制器可以通过该管理器使飞碟按照一定的角度和速度飞行
```C#
public class FlyActionManager : SSActionManager {
    public DiskFlyAction flyAction;  
    public FirstController sceneController;           

    protected void Start() {
        sceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
        sceneController.actionManager = this;     
    }

    public void DiskFly(GameObject disk, float angle, float speed) {
        int direction = (disk.transform.position.x > 0) ? -1 : 1;
        flyAction = DiskFlyAction.GetSSAction(direction, angle, speed);
        this.RunAction(disk, flyAction, this);
    }
}
```

- **DiskFlyAction.cs:** 使用旋转实现飞行。当飞碟到达指定的水平面时动作结束
```C#
public class DiskFlyAction : SSAction {
    public float gravity = -0.2f;
    private Vector3 speed;
    private Vector3 gravityVector = Vector3.zero;
    private Vector3 angle = Vector3.zero;
    private float time;

    private DiskFlyAction() { }
    public static DiskFlyAction GetSSAction(int direction, float angle, float speed) {
        DiskFlyAction action = ScriptableObject.CreateInstance<DiskFlyAction>();
        if (direction == -1) {
            action.speed = Quaternion.Euler(new Vector3(0, 0, -angle)) * Vector3.left * speed;
        }
        else {
            action.speed = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * speed;
        }
        return action;
    }

    public override void Start() { }

    public override void Update() {
        time += Time.deltaTime;
        gravityVector.y = gravity * time;

        transform.position += (speed + gravityVector) * Time.fixedDeltaTime;
        angle.z = Mathf.Atan((speed.y + gravityVector.y) / speed.x) * Mathf.Rad2Deg;
        transform.eulerAngles = angle;

        if (this.transform.position.y < -20f) {
            this.destroy = true;
            this.callback.SSActionEvent(this);      
        }
    }
}
```

- **ScoreRecorder.cs:** 记分员，根据飞碟的数据计分，提供对计分表的读写
```C#
public class ScoreRecorder : MonoBehaviour {
    private int score;

    void Start () {
        score = 0;
    }

    public void Record(GameObject disk) {
        score += disk.GetComponent<DiskData>().score;
    }

    public int GetScore() {
        return score;
    }

    public void Reset() {
        score = 0;
    }
}
```

- **IUserAction.cs:** 提供用户交互事件的接口，包括击中飞碟、重新开始游戏等
```C#
public interface IUserAction
{
    void Hit(Vector3 position);
    void ReStart();
    int GetScore();
    int GetRound();
    bool isGameOver();
    void GameOver();
}
```

- **FirstController.cs:** 场景控制器，管理飞碟和计分表等对象及相关事件
```C#
public class FirstController : MonoBehaviour, ISceneController, IUserAction {
    public FlyActionManager actionManager;
    public DiskFactory diskFactory;
    public ScoreRecorder scoreRecorder;
    
    private int round = 1;                                                  
    private int trial = 0;                                             
    private bool running = false;
    private int count = 0;

    void Start () {
        SSDirector.GetInstance().CurrentSceneController = this;
        diskFactory = Singleton<DiskFactory>.Instance;
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
        gameObject.AddComponent<FlyActionManager>();
        gameObject.AddComponent<UserGUI>();
    }

	void Update () {
        if(running) {
            count++;
            //用户按下鼠标左键时进行hit事件处理
            if (Input.GetButtonDown("Fire1")) {
                Hit(Input.mousePosition);
            }

            //根据游戏轮次设计飞碟的类型和发射速率
            int speed = 300 - round * 50;
            if (count >= speed)
            {
                count = 0;
                float rand;
                switch (round)
                {
                    case 1:
                        SendDisk(1);
                        break;

                    case 2:
                        rand = Random.Range(0, 1f);
                        if (rand < 0.6f)
                            SendDisk(1);
                        else
                            SendDisk(2);
                        break;

                    case 3:
                        rand = Random.Range(0, 1f);
                        if (rand < 0.4f)
                            SendDisk(1);
                        else if (rand < 0.8f)
                            SendDisk(2);
                        else
                            SendDisk(3);
                        break;

                    case 4:
                        rand = Random.Range(0, 1f);
                        if (rand < 0.2f)
                            SendDisk(1);
                        else if (rand < 0.5f)
                            SendDisk(2);
                        else
                            SendDisk(3);
                        break;

                    default:
                        break;
                }
                trial += 1;
                if (trial == 10)
                {
                    if (round == 4)
                    {
                        running = false;
                    }
                    else
                    {
                        round += 1;
                        trial = 0;
                    }
                }
            }
            diskFactory.Reset();
        }
    }

    public void LoadResources() {
        diskFactory.GetDisk(round);
        diskFactory.Reset();
    }

    private void SendDisk(int type) {
        GameObject disk = diskFactory.GetDisk(type);

        float speed = 0;
        float angle = 0;
        if (type == 1)
        {
            speed = Random.Range(5f, 10f);
            angle = Random.Range(10f, 14f);
        }
        else if (type == 2)
        {
            speed = Random.Range(8f, 13f);
            angle = Random.Range(12f, 16f);
        }
        else
        {
            speed = Random.Range(13f, 18f);
            angle = Random.Range(16f, 20f);
        }
        actionManager.DiskFly(disk, angle, speed);
    }

    //处理hit事件
    public void Hit(Vector3 position) {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];
            if (hit.collider.gameObject.GetComponent<DiskData>() != null) {
                scoreRecorder.Record(hit.collider.gameObject);
                hit.collider.gameObject.transform.position = new Vector3(0, -20f, 0);
            }
        }
    }

    public int GetScore() {
        return scoreRecorder.GetScore();
    }
    
    public int GetRound() {
        return round;
    }

    public bool isGameOver()
    {
        return round == 4 && trial == 10;
    }

    public void ReStart() {
        running = true;
        scoreRecorder.Reset();
        diskFactory.Reset();
        round = 1;
        trial = 1;
    }

    public void GameOver() {
        running = false;
    }
}
```

#### **【游戏效果】**

![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw5-003.png)<br/>
![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw5-004.png)<br/>
![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw5-005.png)<br/>


#### **【动态展示】**

&emsp;&emsp;[🔗视频链接](https://www.bilibili.com/video/BV1dr4y1c7JD/)

<br/><br/>


2、编写一个简单的自定义 Component （选做）<a name="1"></a>

- 用自定义组件定义几种飞碟，做成预制
  - 实现自定义组件，编辑并赋予飞碟一些属性

&emsp;&emsp;使用 Sphere 和 Capsule 组合制作预制如下：<br/>
![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw5-006.png)

&emsp;&emsp;在预制上挂载 DiskData.cs，即可编辑飞碟的属性：类型、得分和颜色：<br/>
![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@master/image/3d-hw5-007.png)<br/>
<br/>

&emsp;&emsp;[🔗项目地址](https://github.com/sherryjw/3D-Computer-Game-Programming/tree/master/Homework5)