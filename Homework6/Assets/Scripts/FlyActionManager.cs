using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyActionManager : SSActionManager {
    /*public DiskFlyAction flyAction;*/
    public SSAction flyAction;
    public FirstController sceneController;           

    protected void Start() {
        sceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
        sceneController.actionManager = this;     
    }

    public void DiskFly(GameObject disk, float angle, float power) {
        int direction = (disk.transform.position.x > 0) ? -1 : 1;
        if (sceneController.isPhysics())
        {
            flyAction = DiskFlyPhysicsAction.GetSSAction(disk, angle, power);
        }
        else
        {
            flyAction = DiskFlyAction.GetSSAction(direction, angle, power);
        }
        this.RunAction(disk, flyAction, this);
    }
}
