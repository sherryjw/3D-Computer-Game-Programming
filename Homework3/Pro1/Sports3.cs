using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sports3 : MonoBehaviour
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
        Vector3 myVector = new Vector3(Xspeed * Time.deltaTime, -Yspeed * Time.deltaTime, 0);
        this.transform.position += myVector;
        Yspeed += gravity * Time.deltaTime;
    }
}
