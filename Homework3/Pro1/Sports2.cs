using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sports2 : MonoBehaviour
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
        this.transform.Translate(Xspeed * Time.deltaTime, 0, 0);
        this.transform.Translate(0, -Yspeed * Time.deltaTime, 0, Space.World);
        Yspeed += gravity * Time.deltaTime;
    }
}
