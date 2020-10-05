using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionModel
{
    public static Vector3 src_coast = new Vector3(-11, -2, 6);
    public static Vector3 des_coast = new Vector3(11, -2, 6);
    public static Vector3 river = new Vector3(1, -2.4f, 6);

    public static Vector3 boat_on_left = new Vector3(-4, -2, 6);
    public static Vector3 boat_on_right = new Vector3(4, -2, 6);

    public static Vector3[] roles = new Vector3[]{new Vector3(-0.2f, 0.8f, 0), new Vector3(-0.1f, 0.8f, 0), new Vector3(0, 0.8f, 0),
    new Vector3(0.1f, 0.8f,0), new Vector3(0.2f, 0.8f, 0), new Vector3(0.3f, 0.8f, 0)};

    public static Vector3[] roles_on_boat = new Vector3[] { new Vector3(-0.1f, 1.2f, 0), new Vector3(0.2f, 1.2f, 0) };
}