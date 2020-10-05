using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleController : ClickAction {
    RoleModel roleModel;
    IUserAction userAction;

    public RoleController() {
        userAction = Director.GetInstance().CurrentSenceController as IUserAction;
    }

    public void CreateRole(Vector3 position, int flag, int tag) {
        if (roleModel != null)
            Object.DestroyImmediate(roleModel.role);
        roleModel = new RoleModel(position, flag, tag);
        roleModel.role.GetComponent<Click>().setClickAction(this);
    }

    public RoleModel GetRoleModel() {
        return roleModel;
    }

    public void OnClick() {
        userAction.MoveRole(roleModel);
    }
}