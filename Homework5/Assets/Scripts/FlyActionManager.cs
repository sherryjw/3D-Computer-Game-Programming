using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
