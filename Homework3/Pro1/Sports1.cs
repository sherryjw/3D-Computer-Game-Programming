using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sports1 : MonoBehaviour
{
    private float Xspeed = 2.0f;
    private float Yspeed = 0;
    private float gravity = 9.8f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += Xspeed * Vector3.left * Time.deltaTime;
        this.transform.position += Yspeed * Vector3.down * Time.deltaTime;
        Yspeed += gravity * Time.deltaTime;
    }
}
