using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController {
    private GameObject obj;

    public void SetMove(Vector3 destination, GameObject obj) {
        this.obj = obj;
        Move temp;
        if (!obj.TryGetComponent<Move>(out temp))
            obj.AddComponent<Move>();
        this.obj.GetComponent<Move>().destination = destination;
    }

    public bool GetIsMoving() {
        return (obj != null && obj.GetComponent<Move>().isMoving);
    }
}