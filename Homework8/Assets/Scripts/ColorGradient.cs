using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGradient : MonoBehaviour
{
    private ParticleSystem ball;
    private float r, g, b;
    private bool flag = true;
    
    void Start()
    {
        ball = this.GetComponent<ParticleSystem>();
        r = 1.0f;
        g = 0.3f;
        b = 0.25f;
    }

    void Update()
    {
        if (g >= 0.95f)
        {
            flag = false;
        }
        else if (g <= 0.3f)
        {
            flag = true;
        }

        if (flag)
        {
            g += 0.001f;
        }
        else
        {
            g -= 0.001f;
        }
        var main = ball.main;
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(r, g, b));
    }
}
