using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFlyPhysicsActionManager : SSActionManager, IActionManager, ISSActionCallback
{
    public DiskFlyPhysicsAction flyAction;
    public FirstController sceneController;

    protected void Start()
    {
        sceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
    }

    public void DiskFly(GameObject disk, float angle, float power)
    {
        int direction = (disk.transform.position.x > 0) ? -1 : 1;
        flyAction = DiskFlyPhysicsAction.GetSSAction(disk, angle, power);
        this.RunAction(disk, flyAction, this);
    }

    public void SSActionEvent(SSAction source,
    SSActionEventType events = SSActionEventType.Competeted,
    int intParam = 0,
    string strParam = null,
    Object objectParam = null)
    {
        sceneController.FreeDisk(source.gameobject);
    }
}
