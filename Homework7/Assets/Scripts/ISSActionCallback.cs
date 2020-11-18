using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SSActionEventType : int { Started, Computed }
public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, int intParam = 0, GameObject objectParam = null);
}