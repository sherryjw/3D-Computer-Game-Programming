using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < 100; ++i)
        {
            ParticleSystem star = Instantiate(Resources.Load<ParticleSystem>("Prefabs/Star"), new Vector3(Random.Range(-14.0f, 14.0f), Random.Range(3.5f, 7.0f), 0), Quaternion.identity);
        }
    }
}
