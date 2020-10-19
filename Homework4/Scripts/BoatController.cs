using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : ClickAction
{
    BoatModel boatModel;
    IUserAction userAction;

    public BoatController()
    {
        userAction = SSDirector.GetInstance().CurrentSceneController as IUserAction;
    }

    public void CreateBoat(Vector3 position)
    {
        if (boatModel != null)
            Object.DestroyImmediate(boatModel.boat);
        boatModel = new BoatModel(position);
        boatModel.boat.GetComponent<Click>().setClickAction(this);
    }

    public Vector3 AddRole(RoleModel roleModel)
    {
        if (boatModel.roles[0] == null)
        {
            boatModel.roles[0] = roleModel;
            roleModel.OnBoat = true;
            roleModel.role.transform.parent = boatModel.boat.transform;
            if (roleModel.flag == 0)
                boatModel.priestNum++;
            else
                boatModel.devilNum++;

            return PositionModel.roles_on_boat[0];
        }

        if (boatModel.roles[1] == null)
        {
            boatModel.roles[1] = roleModel;
            roleModel.OnBoat = true;
            roleModel.role.transform.parent = boatModel.boat.transform;
            if (roleModel.flag == 0)
                boatModel.priestNum++;
            else
                boatModel.devilNum++;

            return PositionModel.roles_on_boat[1];
        }
        return roleModel.role.transform.localPosition;
    }

    public BoatModel GetBoatModel()
    {
        return boatModel;
    }

    public void RemoveRole(RoleModel roleModel)
    {
        if (boatModel.roles[0] == roleModel)
        {
            boatModel.roles[0] = null;
            if (roleModel.flag == 0)
                boatModel.priestNum--;
            else
                boatModel.devilNum--;
        }
        if (boatModel.roles[1] == roleModel)
        {
            boatModel.roles[1] = null;
            if (roleModel.flag == 0)
                boatModel.priestNum--;
            else
                boatModel.devilNum--;
        }
    }

    public void OnClick()
    {
        if (boatModel.roles[0] != null || boatModel.roles[1] != null)
            userAction.MoveBoat();
    }
}