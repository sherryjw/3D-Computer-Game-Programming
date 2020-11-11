using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    void ReStart();
    void Hit(Vector3 position);
    int GetScore();
    int GetRound();
    bool isGameOver();
    void GameOver();
    bool isPhysics();
    void SwitchMode();
}
