# 第三次作业

该文件夹下所有文件均服务于第三次作业，其中:
- hw3.md为书面作业

为方便下载，工程文件存放在[另一文件夹](https://github.com/sherryjw/3D-Computer-Game-Programming/tree/master/Homework3-Project)。

以下资源和代码为配置项目所需：
- Pro1 目录下 Sports1.cs、Sports2.cs、Sports3.cs 对应书面作业简答题第一题——抛物线运动的代码，只需要创建一个游戏对象并将脚本挂载到该游戏对象上运行即可
- Pro1 目录下 SolarSystem.cs 对应书面作业简答题第一题——实现太阳系的代码，
- Pro2 目录下所有 cs 文件对应书面作业简答题第二题——游戏《牧师与魔鬼》的代码，
- Resources1 目录下所有图像供实现太阳系中的天体预制使用
- Resources2 目录下所有图像供《牧师与魔鬼》中的人物和场景预制使用

**太阳系项目配置如下：**
- 创建九个 Sphere 分别命名为 Sun、Earth、Jupiter、Mars、Mercury、Neptune、Saturn、Uranus、Venus，其中 Sun 为父对象，其他均作为 Sun 的子对象
- 将 Resource1 目录下的图像和 Pro1 目录下 SolarSystem.cs 导入 Assets
- 将 Resources1 目录下的图像一一拖放到对应的游戏对象并生成预制
- 将 SolarSystem.cs 挂载到 MainCamera 上，并设置对应的公有变量，运行

**《牧师与魔鬼》项目配置如下：**
- 创建四个 Cube 分别命名为 River、Coast、Boat、Devil，创建一个 Cylinder 命名为 Priest
- 将 Resources2 目录下的图像和 Pro2 目录下的所有脚本文件导入 Assets
- 将 Resources2 目录下的图像一一拖放到对应的游戏对象并生成预制（场景中原有的游戏对象要删去）
- 创建一个空游戏对象，将 FirstController.cs 挂载到该空对象上，运行

[🔗博客链接](https://www.yuque.com/pijiuwujializijun/acorbw/xh0mcw)
