using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {
    private CoastController DesCoastController;
    private CoastController SrcCoastController;
    private BoatController boatController;
    private RoleController[] roleModelControllers;
    private MoveController moveController;

    private RiverModel river;

    private bool isRuning;
    private float time;
       
    void Awake() {
        Director director = Director.GetInstance();
        director.CurrentSenceController = this;
        director.CurrentSenceController.LoadResources();
        this.gameObject.AddComponent<UserGUI>();
    }

    public void LoadResources() {
        SrcCoastController = new CoastController();
        SrcCoastController.CreateCoast("src_coast", PositionModel.src_coast);
        DesCoastController = new CoastController();
        DesCoastController.CreateCoast("des_coast", PositionModel.des_coast);

        boatController = new BoatController();
        boatController.CreateBoat(PositionModel.boat_on_left);

        roleModelControllers = new RoleController[6];
        for (int i = 0; i < 3; i++) {
            roleModelControllers[i] = new RoleController();
            roleModelControllers[i].CreateRole(PositionModel.roles[i], 0, i);
        }
        for (int i = 3; i < 6; i++) {
            roleModelControllers[i] = new RoleController();
            roleModelControllers[i].CreateRole(PositionModel.roles[i], 1, i);
        }

        foreach (RoleController roleModelController in roleModelControllers) {
            roleModelController.GetRoleModel().role.transform.localPosition = SrcCoastController.AddRole(roleModelController.GetRoleModel());
        }

        moveController = new MoveController();

        river = new RiverModel(PositionModel.river);

        isRuning = true;
        time = 120;
    }

    public void MoveBoat() {
        if ((!isRuning) || moveController.GetIsMoving())
            return;

        if (boatController.GetBoatModel().OnRight)
            moveController.SetMove(PositionModel.boat_on_left, boatController.GetBoatModel().boat);
        else
            moveController.SetMove(PositionModel.boat_on_right, boatController.GetBoatModel().boat);

        boatController.GetBoatModel().OnRight = !boatController.GetBoatModel().OnRight;
    }
 
    public void MoveRole(RoleModel roleModel) {
        if ((!isRuning) || moveController.GetIsMoving())
            return;

        if (roleModel.OnBoat) {
            if (boatController.GetBoatModel().OnRight)
                moveController.SetMove(DesCoastController.AddRole(roleModel), roleModel.role);
            else
                moveController.SetMove(SrcCoastController.AddRole(roleModel), roleModel.role);
            roleModel.OnRight = boatController.GetBoatModel().OnRight;
            boatController.RemoveRole(roleModel);
        }
        else {
            if (boatController.GetBoatModel().OnRight == roleModel.OnRight && (boatController.GetBoatModel().roles[0] == null || boatController.GetBoatModel().roles[1] == null)) {
                if (roleModel.OnRight) {
                    DesCoastController.RemoveRole(roleModel);
                }
                else {
                    SrcCoastController.RemoveRole(roleModel);
                }
                moveController.SetMove(boatController.AddRole(roleModel), roleModel.role);
            }
        }
    }

    public void Restart() {
        time = 120;
        SrcCoastController.CreateCoast("src_coast", PositionModel.src_coast);
        DesCoastController.CreateCoast("des_coast", PositionModel.des_coast);
        for (int i = 0; i < 6; i++) {
            roleModelControllers[i].CreateRole(PositionModel.roles[i], 0, i);
            roleModelControllers[i].GetRoleModel().role.transform.localPosition = SrcCoastController.AddRole(roleModelControllers[i].GetRoleModel());
        }
        for (int i = 3; i < 6; i++) {
            roleModelControllers[i].CreateRole(PositionModel.roles[i], 1, i);
            roleModelControllers[i].GetRoleModel().role.transform.localPosition = SrcCoastController.AddRole(roleModelControllers[i].GetRoleModel());
        }
        boatController.CreateBoat(PositionModel.boat_on_left);
        isRuning = true;
    }

    public void Check() {
        if (!isRuning)
            return;
        this.gameObject.GetComponent<UserGUI>().result = "";

        if (DesCoastController.GetCoastModel().priestNum == 3) {
            this.gameObject.GetComponent<UserGUI>().result = "You Win!";
            isRuning = false;
        }
        else {
            int leftPriestNum, leftDevilNum, rightPriestNum, rightDevilNum;
            leftPriestNum = SrcCoastController.GetCoastModel().priestNum + (boatController.GetBoatModel().OnRight ? 0 : boatController.GetBoatModel().priestNum);
            leftDevilNum = SrcCoastController.GetCoastModel().devilNum + (boatController.GetBoatModel().OnRight ? 0 : boatController.GetBoatModel().devilNum);
            if (leftPriestNum != 0 && leftPriestNum < leftDevilNum) {
                this.gameObject.GetComponent<UserGUI>().result = "Game Over!";
                isRuning = false;
            }

            rightPriestNum = DesCoastController.GetCoastModel().priestNum + (boatController.GetBoatModel().OnRight ? boatController.GetBoatModel().priestNum : 0);
            rightDevilNum = DesCoastController.GetCoastModel().devilNum + (boatController.GetBoatModel().OnRight ? boatController.GetBoatModel().devilNum : 0);
            if (rightPriestNum != 0 && rightPriestNum < rightDevilNum) {
                this.gameObject.GetComponent<UserGUI>().result = "Game Over!";
                isRuning = false;
            }
        }
    }

    void Update() {
        if (isRuning) {
            time -= Time.deltaTime;
            this.gameObject.GetComponent<UserGUI>().time = (int)time;
            if (time <= 0) {
                this.gameObject.GetComponent<UserGUI>().time = 0;
                this.gameObject.GetComponent<UserGUI>().result = "Game Over!";
                isRuning = false;
            }
        }
    }
}