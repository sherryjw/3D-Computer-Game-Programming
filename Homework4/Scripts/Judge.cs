using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    public FirstController sceneController;
    public CoastModel srcCoastModel;
    public CoastModel desCoastModel;
    public BoatModel boatModel;

    void Start()
    {
        sceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
        srcCoastModel = sceneController.SrcCoastController.GetCoastModel();
        desCoastModel = sceneController.DesCoastController.GetCoastModel();
        boatModel = sceneController.boatController.GetBoatModel();
    }

    // 参考之前的 Check()
    void Update()
    {
        if (!sceneController.isRunning)
        {
            return;
        }
        if (sceneController.time <= 0)
        {
            sceneController.JudgeCallback("Game Over!", false);
            return;
        }
        this.gameObject.GetComponent<UserGUI>().result = "";

        if (desCoastModel.priestNum == 3)
        {
            sceneController.JudgeCallback("You Win!", false);
            return;
        }
        else
        {
            int leftPriestNum, leftDevilNum, rightPriestNum, rightDevilNum;
            leftPriestNum = srcCoastModel.priestNum + (boatModel.OnRight ? 0 : boatModel.priestNum);
            leftDevilNum = srcCoastModel.devilNum + (boatModel.OnRight ? 0 : boatModel.devilNum);
            if (leftPriestNum != 0 && leftPriestNum < leftDevilNum)
            {
                sceneController.JudgeCallback("Game Over!", false);
                return;
            }

            rightPriestNum = desCoastModel.priestNum + (boatModel.OnRight ? boatModel.priestNum : 0);
            rightDevilNum = desCoastModel.devilNum + (boatModel.OnRight ? boatModel.devilNum : 0);
            if (rightPriestNum != 0 && rightPriestNum < rightDevilNum)
            {
                sceneController.JudgeCallback("Game Over!", false);
                return;
            }
        }
    }
}
