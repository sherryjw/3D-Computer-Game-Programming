using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;

    public static CCMoveToAction GetSSAction(Vector3 target, float speed)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    private CCMoveToAction() { }

    public override void Start()
    {

    }

    public override void Update()
    {
        if (this.gameObject == null || this.transform.localPosition == target)
        {
            this.destory = true;
            this.callback.SSActionEvent(this);
            return;
        }
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, target, speed * Time.deltaTime);
    }
}