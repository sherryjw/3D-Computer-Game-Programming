using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    int GetScore();
    bool GetWin();
    bool GetGameover();
    void Restart();
    void Gameover();
    void PatrolMove();
    void MovePlayer(float translationX, float translationZ);
}
