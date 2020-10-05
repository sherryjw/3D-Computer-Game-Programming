using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastModel {
    public GameObject obj;
    public int priestNum, devilNum;
    public CoastModel(string name, Vector3 position) {
        priestNum = devilNum = 0;
        obj = GameObject.Instantiate(Resources.Load("Prefabs/Coast", typeof(GameObject))) as GameObject;
        obj.name = name;
        obj.transform.position = position;
        obj.transform.localScale = new Vector3(10, 1.6f, 16);
    }
}