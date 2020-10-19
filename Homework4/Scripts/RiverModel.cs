using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverModel
{
    private GameObject river;

    public RiverModel(Vector3 position)
    {
        river = Object.Instantiate(Resources.Load("Prefabs/River", typeof(GameObject))) as GameObject;
        river.name = "river";
        river.transform.position = position;
        river.transform.localScale = new Vector3(15, 0.7f, 25);
        river.transform.Rotate(0, 180, 0);
    }
}