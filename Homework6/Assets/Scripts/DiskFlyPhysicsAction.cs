using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFlyPhysicsAction : SSAction
{
    private Vector3 speed = Vector3.zero;

    private DiskFlyPhysicsAction() { }
    public override void Start() { }

    public static DiskFlyPhysicsAction GetSSAction(GameObject disk, float angle, float power)
    {
        ConstantForce constantForce = disk.GetComponent<ConstantForce>();
        if (constantForce)
        {
            constantForce.enabled = true;
            constantForce.force = new Vector3(0, -power, 0);
        }
        else
        {
            disk.AddComponent<Rigidbody>().useGravity = false;
            disk.AddComponent<ConstantForce>().force = new Vector3(0, -power, 0);
        }

        float x = Random.Range(-15f, 15f), y = Random.Range(15f, 20f), z = Random.Range(5f, 10f);
        disk.transform.position = new Vector3(x, y, z);
        DiskFlyPhysicsAction action = CreateInstance<DiskFlyPhysicsAction>();
        action.speed = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
        return action;
    }

    public override void Update() { }
}
