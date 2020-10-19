using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionModel
{
    public static Vector3 src_coast = new Vector3(-8, -0.2f, 3);
    public static Vector3 des_coast = new Vector3(8, -0.2f, 3);
    public static Vector3 river = new Vector3(1, -0.5f, 6);

    public static Vector3 boat_on_left = new Vector3(-3, 0, 1);
    public static Vector3 boat_on_right = new Vector3(3, 0, 1);

    public static Vector3[] roles = new Vector3[]{new Vector3(-0.2f, 0.8f, -0.12f), new Vector3(-0.1f, 0.8f, -0.12f), new Vector3(0, 0.8f, -0.12f),
    new Vector3(0.1f, 0.7f, -0.12f), new Vector3(0.2f, 0.7f, -0.12f), new Vector3(0.3f, 0.7f, -0.12f)};

    public static Vector3[] roles_on_boat = new Vector3[] { new Vector3(-0.1f, 1.1f, 0), new Vector3(0.2f, 1.1f, 0) };
}