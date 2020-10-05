using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleModel {
    public GameObject role;
    public int tag;
    public int flag;
    public bool OnBoat;
    public bool OnRight;

    public RoleModel(Vector3 position, int flag, int tag) { // 0 represents Priest, 1 represents Devil 
        this.tag = tag;
        this.flag = flag;
        OnBoat = false;
        OnRight = false;

        if (flag == 0) 
            role = GameObject.Instantiate(Resources.Load("Prefabs/Priest", typeof(GameObject))) as GameObject;        
        else
            role = GameObject.Instantiate(Resources.Load("Prefabs/Devil", typeof(GameObject))) as GameObject;        
        
        role.transform.Rotate(0, 180, 0);
        role.name = "role" + tag;
        role.transform.position = position;
        role.AddComponent<Click>();
        role.AddComponent<BoxCollider>();
    }
}