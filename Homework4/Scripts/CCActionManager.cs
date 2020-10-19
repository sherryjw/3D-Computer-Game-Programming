using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager, ISSActionCallback
{
    private bool isMoving = false;
    public CCMoveToAction MoveBoatAction;
    public CCSequenceAction MoveRoleAction;
    public FirstController sceneController;

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competed, int Param = 0, string strParam = null, Object objectParam = null)
    {
        isMoving = false;
    }

    protected new void Start()
    {
        sceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
        sceneController.SetActionManager(this);
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    public void MoveBoat(GameObject obj, Vector3 target, float speed)
    {
        if (isMoving)
        {
            return;
        }
        isMoving = true;
        MoveBoatAction = CCMoveToAction.GetSSAction(target, speed);
        this.RunAction(obj, MoveBoatAction, this);
    }

    public void MoveRole(GameObject role, Vector3 transfer, Vector3 target, float speed)
    {
        if (isMoving)
        {
            return;
        }
        isMoving = true;
        MoveRoleAction = CCSequenceAction.GetSSAction(0, 0, new List<SSAction> { CCMoveToAction.GetSSAction(transfer, speed), CCMoveToAction.GetSSAction(target, speed) });
        this.RunAction(role, MoveRoleAction, this);
    }
}