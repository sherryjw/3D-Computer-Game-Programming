using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatModel {
    public GameObject boat;
    public RoleModel[] roles;
    public int priestNum;
    public int devilNum;
    public bool OnRight;

    public BoatModel(Vector3 position) {
        priestNum = devilNum = 0;
        roles = new RoleModel[2];
        boat = GameObject.Instantiate(Resources.Load("Prefabs/Boat", typeof(GameObject))) as GameObject;
        boat.name = "boat";
        boat.transform.position = position;
        boat.transform.localScale = new Vector3(4, 0.6f, 2);
        boat.transform.Rotate(0, 180, 0);
        boat.AddComponent<BoxCollider>();
        boat.AddComponent<Click>();
        OnRight = false;
    }
}