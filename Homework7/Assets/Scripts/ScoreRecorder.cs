using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour
{
    public int score = 0;

    void Start()
    {
        ((FirstController)SSDirector.GetInstance().CurrentScenceController).recorder = this;
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore()
    {
        score++;
    }
}

