using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
