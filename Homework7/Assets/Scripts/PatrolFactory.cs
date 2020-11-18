﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFactory : MonoBehaviour
{
    private GameObject patrol = null;
    private List<GameObject> used = new List<GameObject>();
    private Vector3[] vec = new Vector3[9];

    public List<GameObject> GetPatrols()
    {
        int[] pos_x = { -6, 4, 13 };
        int[] pos_z = { -4, 6, -13 };
        int index = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                vec[index] = new Vector3(pos_x[i], 0, pos_z[j]);
                index++;
            }
        }
        for (int i = 0; i < 9; i++)
        {
            patrol = Instantiate(Resources.Load<GameObject>("Prefabs/Patrol"));
            patrol.transform.position = vec[i];
            patrol.GetComponent<PatrolData>().sign = i + 1;
            patrol.GetComponent<PatrolData>().start_position = vec[i];
            used.Add(patrol);
        }
        return used;
    }

    public void StopPatrol()
    {
        for (int i = 0; i < used.Count; i++)
        {
            used[i].gameObject.GetComponent<Animator>().SetBool("run", false);
        }
    }
}
