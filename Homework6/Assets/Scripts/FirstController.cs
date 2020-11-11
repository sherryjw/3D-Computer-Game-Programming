using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {
    public IActionManager actionManager;
    public DiskFactory diskFactory;
    public ScoreRecorder scoreRecorder;
    
    private int round = 1;                                                  
    private int trial = 0;                                             
    private bool running = false;
    private int count = 0;

    private bool isPhysicsMode = false;

    void Start () {
        SSDirector.GetInstance().CurrentSceneController = this;
        diskFactory = Singleton<DiskFactory>.Instance;
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
        gameObject.AddComponent<FlyActionManager>();
        gameObject.AddComponent<DiskFlyPhysicsActionManager>();
        actionManager = Singleton<FlyActionManager>.Instance;
        gameObject.AddComponent<UserGUI>();
    }

	void Update () {
        if(running) {
            count++;
            //用户按下鼠标左键时进行Hit事件处理
            if (Input.GetButtonDown("Fire1")) {
                Hit(Input.mousePosition);
            }

            //用户按下鼠标右键可切换飞碟运动模式
            if (Input.GetMouseButtonDown(1))
            {
                this.SwitchMode();
            }

            //根据游戏轮次设计飞碟的类型和发射速率
            int speed = 300 - round * 50;
            if (count >= speed)
            {
                count = 0;
                float rand;
                switch (round)
                {
                    case 1:
                        SendDisk(1);
                        break;

                    case 2:
                        rand = Random.Range(0, 1f);
                        if (rand < 0.6f)
                            SendDisk(1);
                        else
                            SendDisk(2);
                        break;

                    case 3:
                        rand = Random.Range(0, 1f);
                        if (rand < 0.4f)
                            SendDisk(1);
                        else if (rand < 0.8f)
                            SendDisk(2);
                        else
                            SendDisk(3);
                        break;

                    case 4:
                        rand = Random.Range(0, 1f);
                        if (rand < 0.2f)
                            SendDisk(1);
                        else if (rand < 0.5f)
                            SendDisk(2);
                        else
                            SendDisk(3);
                        break;

                    default:
                        break;
                }
                trial += 1;
                if (trial == 10)
                {
                    if (round == 4)
                    {
                        running = false;
                    }
                    else
                    {
                        round += 1;
                        trial = 0;
                    }
                }
            }
            diskFactory.Reset();
        }
    }

    public void LoadResources() {
        diskFactory.GetDisk(round);
        diskFactory.Reset();
    }

    private void SendDisk(int type) {
        GameObject disk = diskFactory.GetDisk(type);

        float power = 0;
        float angle = 0;
        if (type == 1)
        {
            power = isPhysicsMode ? Random.Range(1f, 2f) : Random.Range(5f, 10f);
            angle = Random.Range(10f, 14f);
        }
        else if (type == 2)
        {
            power = isPhysicsMode ? Random.Range(1f, 2f) : Random.Range(8f, 13f);
            angle = Random.Range(12f, 16f);
        }
        else
        {
            power = isPhysicsMode ? Random.Range(1f, 2f) : Random.Range(13f, 18f);
            angle = Random.Range(16f, 20f);
        }
        actionManager.DiskFly(disk, angle, power);
    }

    public void Hit(Vector3 position) {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];
            if (hit.collider.gameObject.GetComponent<DiskData>() != null) {
                scoreRecorder.Record(hit.collider.gameObject);
                hit.collider.gameObject.transform.position = new Vector3(0, -20f, 0);
            }
        }
    }

    public int GetScore() {
        return scoreRecorder.GetScore();
    }
    
    public int GetRound() {
        return round;
    }

    public bool isGameOver()
    {
        return round == 4 && trial == 10;
    }

    public void ReStart() {
        running = true;
        scoreRecorder.Reset();
        diskFactory.Reset();
        round = 1;
        trial = 1;
    }

    public void GameOver() {
        running = false;
    }

    public bool isPhysics()
    {
        return isPhysicsMode;
    }

    public void SwitchMode()
    {
        isPhysicsMode = !isPhysicsMode;
        actionManager = isPhysicsMode ? Singleton<DiskFlyPhysicsActionManager>.Instance : Singleton<FlyActionManager>.Instance as IActionManager;
    }

    public void FreeDisk(GameObject disk)
    {
        diskFactory.FreeDisk(disk);
    }
}
