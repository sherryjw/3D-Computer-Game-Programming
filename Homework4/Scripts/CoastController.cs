using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastController
{
    private CoastModel coast;

    public void CreateCoast(string name, Vector3 position)
    {
        if (coast == null)
            coast = new CoastModel(name, position);
        coast.priestNum = coast.devilNum = 0;
    }

    public Vector3 AddRole(RoleModel roleModel)
    {
        if (roleModel.flag == 0)
            coast.priestNum++;
        else
            coast.devilNum++;
        roleModel.role.transform.parent = coast.obj.transform;
        roleModel.OnBoat = false;
        return PositionModel.roles[roleModel.tag];
    }

    public CoastModel GetCoastModel()
    {
        return coast;
    }

    public void RemoveRole(RoleModel roleModel)
    {
        if (roleModel.flag == 0)
            coast.priestNum--;
        else
            coast.devilNum--;
    }
}