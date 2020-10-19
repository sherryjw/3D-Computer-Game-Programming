using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    /* 增加动作管理器 */
    public CCActionManager ActionManager;

    public CoastController DesCoastController;
    public CoastController SrcCoastController;
    public BoatController boatController;
    public RoleController[] roleModelControllers;
    //private MoveController moveController;

    public RiverModel river;

    public bool isRunning;
    public float time;
    public float speed = 8;

    public void SetActionManager(CCActionManager actionManager)
    {
        this.ActionManager = actionManager;
    }

    void Awake()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        director.CurrentSceneController.LoadResources();
        this.gameObject.AddComponent<UserGUI>();
        this.gameObject.AddComponent<CCActionManager>();
        this.gameObject.AddComponent<Judge>();
    }

    public void LoadResources()
    {
        roleModelControllers = new RoleController[6];
        for (int i = 0; i < 3; i++)
        {
            roleModelControllers[i] = new RoleController();
            roleModelControllers[i].CreateRole(PositionModel.roles[i], 0, i);
        }
        for (int i = 3; i < 6; i++)
        {
            roleModelControllers[i] = new RoleController();
            roleModelControllers[i].CreateRole(PositionModel.roles[i], 1, i);
        }

        SrcCoastController = new CoastController();
        SrcCoastController.CreateCoast("src_coast", PositionModel.src_coast);
        DesCoastController = new CoastController();
        DesCoastController.CreateCoast("des_coast", PositionModel.des_coast);

        foreach (RoleController roleModelController in roleModelControllers)
        {
            roleModelController.GetRoleModel().role.transform.localPosition = SrcCoastController.AddRole(roleModelController.GetRoleModel());
        }

        boatController = new BoatController();
        boatController.CreateBoat(PositionModel.boat_on_left);

        river = new RiverModel(PositionModel.river);

        isRunning = true;
        time = 120;
    }

    /* 使用动作管理器提供的接口实现运动，取代原来的 moveController；下同 */
    public void MoveBoat()
    {
        if ((!isRunning) || ActionManager.GetIsMoving())
            return;

        if (boatController.GetBoatModel().OnRight)
            ActionManager.MoveBoat(boatController.GetBoatModel().boat, PositionModel.boat_on_left, speed);
        else
            ActionManager.MoveBoat(boatController.GetBoatModel().boat, PositionModel.boat_on_right, speed);

        boatController.GetBoatModel().OnRight = !boatController.GetBoatModel().OnRight;
    }

    public void MoveRole(RoleModel roleModel)
    {
        if ((!isRunning) || ActionManager.GetIsMoving())
            return;

        Vector3 target, transfer;
        if (roleModel.OnBoat)
        {
            if (boatController.GetBoatModel().OnRight)
                target = DesCoastController.AddRole(roleModel);
            else
                target = SrcCoastController.AddRole(roleModel);

            /* 设置一个中转点使运动轨迹成为折线；下同 */
            if (roleModel.role.transform.localPosition.y > target.y)
                transfer = new Vector3(target.x, roleModel.role.transform.localPosition.y, target.z);
            else
                transfer = new Vector3(roleModel.role.transform.localPosition.x, target.y, target.z);

            ActionManager.MoveRole(roleModel.role, transfer, target, 5);
            roleModel.OnRight = boatController.GetBoatModel().OnRight;
            boatController.RemoveRole(roleModel);
        }
        else
        {
            if (boatController.GetBoatModel().OnRight == roleModel.OnRight )
            {
                if (roleModel.OnRight)
                {
                    DesCoastController.RemoveRole(roleModel);
                }
                else
                {
                    SrcCoastController.RemoveRole(roleModel);
                }
                target = boatController.AddRole(roleModel);
                if (roleModel.role.transform.localPosition.y > target.y)
                    transfer = new Vector3(target.x, roleModel.role.transform.localPosition.y, target.z);
                else
                    transfer = new Vector3(roleModel.role.transform.localPosition.x, target.y, target.z);
                ActionManager.MoveRole(roleModel.role, transfer, target, 5);
            }
        }
    }

    public void Restart()
    {
        time = 120;
        SrcCoastController.CreateCoast("src_coast", PositionModel.src_coast);
        DesCoastController.CreateCoast("des_coast", PositionModel.des_coast);
        for (int i = 0; i < 6; i++)
        {
            roleModelControllers[i].CreateRole(PositionModel.roles[i], 0, i);
            roleModelControllers[i].GetRoleModel().role.transform.localPosition = SrcCoastController.AddRole(roleModelControllers[i].GetRoleModel());
        }
        for (int i = 3; i < 6; i++)
        {
            roleModelControllers[i].CreateRole(PositionModel.roles[i], 1, i);
            roleModelControllers[i].GetRoleModel().role.transform.localPosition = SrcCoastController.AddRole(roleModelControllers[i].GetRoleModel());
        }
        boatController.CreateBoat(PositionModel.boat_on_left);
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {
            time -= Time.deltaTime;
            this.gameObject.GetComponent<UserGUI>().time = (int)time;
        }
    }

    /* 将裁判类的返回信息呈现在游戏场景中 */
    public void JudgeCallback(string result, bool isRunning)
    {
        this.gameObject.GetComponent<UserGUI>().result = result;
        this.gameObject.GetComponent<UserGUI>().time = (int)time;
        this.isRunning = isRunning;
    }
}