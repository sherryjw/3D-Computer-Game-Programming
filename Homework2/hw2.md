# Unity-离散仿真引擎基础
### 3D Computer Game Programming-Note 2

1、简答题
- 解释游戏对象（GameObjects）和资源（Assets）的区别与联系。

&emsp;&emsp;游戏对象（GameObjects）是游戏程序空间中的事物，可以是空对象、2D/3D对象、光线、摄像机等，用于表示人物、道具、风景等。游戏对象本身不能做任何事情，需要通过赋予其属性才能成为角色、环境或特殊效果。

&emsp;&emsp;资源（Assets）是构造游戏对象、装饰游戏对象、配置游戏等的物体和数据，可以是来自 Unity 外部创建的文件，如音频文件、图像等，也可以是 Unity 创建的文件，如渲染纹理等。

&emsp;&emsp;联系：资源可以实例化为具体的游戏对象，也可以成为游戏对象的属性；游戏对象可以整合多种资源，也可以被预设为资源。

---

- 下载几个游戏案例，分别总结资源、对象组织的结构（指资源的目录组织结构与游戏对象树的层次结构）。

&emsp;&emsp;访问 [Unity Asset Store](https://assetstore.unity.com/) 可以下载喜欢的项目，下面以卡丁车游戏为例：<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@latest/image/3d-hw2-020.png)<br/>

&emsp;&emsp;案例资源的目录组织结构如下：<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@latest/image/3d-hw2-021.png)<br/>

&emsp;&emsp;可以看到，资源按照类型来组织目录的，例如在 Karting 目录下，资源根据动画、声音、预制、脚本等不同类型来组织。

&emsp;&emsp;案例主场景中游戏对象树的层次结构如下：<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@latest/image/3d-hw2-022.png)<br/>

&emsp;&emsp;由于案例对象数目过多，此处不展开所有层次讨论。从一二级对象可以看到，对象之间既有互相独立的，也有按照层次关系组合的。比如游戏管理器包括游戏菜单、游戏结果显示等子对象。<br/>

---

- 编写一个代码，使用 debug 语句来验证 [MonoBehaviour](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html) 基本行为或事件触发的条件。
  - 基本行为包括 Awake()、Start()、Update()、FixedUpdate()、LateUpdate()
  - 常用事件包括 OnGUI()、OnDisable()、OnEnable()

&emsp;&emsp;根据 Unity API 手册编写脚本 Test.cs：
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    void Awake()
    {
        Debug.Log("Awake!");
    }

    void Start()
    {
        Debug.Log("Start!");
    }

    void Update()
    {
        Debug.Log("Update!");
    }

    void FixedUpdate()
    {
        Debug.Log("FixedUpdate!");
    }

    void LateUpdate()
    {
        Debug.Log("LateUpdate!");
    }

    void OnGUI()
    {
        Debug.Log("OnGUI!");
    }

    void OnDisable()
    {
        Debug.Log("OnDisable!");
    }

    void OnEnable()
    {
        Debug.Log("OnEnable!");
    }
}
```
&emsp;&emsp;创建一个空对象 GameObject，将 Test.cs 添加为 GameObject 的部件，运行测试，控制台输出如下：<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@latest/image/3d-hw2-005.png)<br/>
&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@latest/image/3d-hw2-007.png)<br/>
&emsp;&emsp;其中，事件 OnEnable() 和 OnDisable() 通过点击 GameObject 的 checkbox 触发。<br/>

---
- 查找脚本手册，了解 [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)，Transform，Component 对象
  - 分别翻译官方对三个对象的描述（Description）

&emsp;&emsp;&emsp;&emsp;&emsp;GameObject：Unity 场景中所有实体的基类。<br/>
&emsp;&emsp;&emsp;&emsp;&emsp;Transform：对象的位置、旋转和比例。<br/>
&emsp;&emsp;&emsp;&emsp;&emsp;Component：所有属于 GameObject 的事物的基类。<br/>

-  - 描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、table 的部件

&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@latest/image/3d-hw2-table.png)<br/>

&emsp;&emsp;查看图中 Inspector 面板可以知道：<br/>
&emsp;&emsp;table 的对象是 GameObject，第一个选择框是 activeSelf 属性，文本框是对象名称，第二个选择框及下拉菜单是 Static GameObjects 属性；第二行包括 Tag 属性（标记对象状态）和 Layer 属性（标记对象层级）；第三行是 Prefabs 属性（对应预制的属性）。<br/>
&emsp;&emsp;table 的 Transform 的属性包括 Position（设置对象的 x、y、z 坐标）、Rotation（设置对象绕 x、y、z 轴的旋转角度） 和 Scale（设置对象在 x、y、z 轴上的大小的比例）<br/>
&emsp;&emsp;table 的部件包括 Transform、Mesh Filter、Box Colider 和 Mesh Renderer。<br/>

-  - 用 UML 图描述三者的关系

&emsp;&emsp;![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@latest/image/3d-hw2-009.png)<br/>

---

- 资源预设（Prefabs）与对象克隆（clone）
  - 预设（Prefabs）有什么好处？
  <br/>&emsp;&emsp;预设可以将特殊配置的对象打包，然后在场景中的多个位置或项目中的多个场景中重复使用，这样可以避免大量重复的创建和配置工作，同时当由预设实例化的对象需要改动时，只需要改动预设而不需要逐一改动所有对应的对象。

  - 预设与对象克隆 (clone or copy or Instantiate of Unity Object) 关系？
  <br/>&emsp;&emsp;预设是从一个模板实例化出对象，改动预设时所有由该预设实例化出的对象都会相应地改动，而克隆则是从一个现有对象复制出另一对象，对象之间相互独立，互不影响。

  - 制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象
  <br/>&emsp;&emsp;table 预制的制作流程参见 [Unity课堂实验杂记——预制]()，创建脚本 Instantiate.cs，编写如下代码：
  ```C#
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Instantiate : MonoBehaviour {
        void Start() {
            GameObject Table = Resources.Load("Table") as GameObject;
            Instantiate(Table);
        }
    }
  ```
  &emsp;&emsp;运行结果：

  ![](https://cdn.jsdelivr.net/gh/sherryjw/StaticResource@latest/image/3d-hw2-010.png)<br/>

  &emsp;&emsp;通过这种方式可以直接**由脚本实例化游戏对象**，而**不需要将预制逐一拖入每个属性框中**，当实例化的对象种类非常多时由脚本直接实例化对象无疑是更便捷的方式。
  
2、编程实践

&emsp;&emsp;实现小游戏：🔗[井字棋]()

<br/>

注：所有代码均已上传至 gitee ✈️[传送门]()。