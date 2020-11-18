using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstController : MonoBehaviour, IUserAction, ISceneController
{
    public PatrolFactory patrol_factory;
    public ScoreRecorder recorder;
    public UserGUI userGUI;
    public PatrolActionManager action_manager;
    public GameObject player;
    private List<GameObject> patrols;

    public int wall_sign = -1;
    public float player_speed = 5f;
    public float rotate_speed = 120f;

    private bool game_over = false;
    private bool gameWin = false;

    void Update()
    {
        for (int i = 0; i < patrols.Count; i++)
        {
            patrols[i].gameObject.GetComponent<PatrolData>().wall_sign = wall_sign;
        }
        if (wall_sign == 10)
        {
            GameWin();
        }
    }
    void Start()
    {
        SSDirector.GetInstance().CurrentScenceController = this;
        gameObject.AddComponent<UserGUI>();
        gameObject.AddComponent<ScoreRecorder>();
        gameObject.AddComponent<PatrolFactory>();
        gameObject.AddComponent<PatrolActionManager>();
        LoadResources();
    }

    public void LoadResources()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Maze"));
        action_manager = Singleton<PatrolActionManager>.Instance;
        patrol_factory = Singleton<PatrolFactory>.Instance;
        userGUI = Singleton<UserGUI>.Instance;
        recorder = Singleton<ScoreRecorder>.Instance;
        player = Instantiate(Resources.Load("Prefabs/Player"), new Vector3(9, 9, -7), Quaternion.identity) as GameObject;
        patrols = patrol_factory.GetPatrols();
    }

    public void MovePlayer(float translationX, float translationZ)
    {
        if (!game_over && !gameWin)
        {
            if (translationX != 0 || translationZ != 0)
            {
                player.GetComponent<Animator>().SetBool("run", true);
            }
            else
            {
                player.GetComponent<Animator>().SetBool("run", false);
            }

            player.transform.Translate(0, 0, translationZ * player_speed * Time.deltaTime);
            player.transform.Rotate(0, translationX * rotate_speed * Time.deltaTime, 0);

            if (player.transform.localEulerAngles.x != 0 || player.transform.localEulerAngles.z != 0)
            {
                player.transform.localEulerAngles = new Vector3(0, player.transform.localEulerAngles.y, 0);
            }
            if (player.transform.position.y != 0)
            {
                player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            }
        }
    }

    public void PatrolMove()
    {
        for (int i = 0; i < patrols.Count; i++)
        {
            action_manager.GoPatrol(patrols[i]);
        }
    }

    public int GetScore()
    {
        return recorder.GetScore();
    }

    public bool GetGameover()
    {
        return game_over;
    }

    public bool GetWin()
    {
        return gameWin;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scenes/MyScene");
    }

    void OnEnable()
    {
        GameEventManager.ScoreChange += AddScore;
        GameEventManager.GameoverChange += Gameover;
    }

    void OnDisable()
    {
        GameEventManager.ScoreChange -= AddScore;
        GameEventManager.GameoverChange -= Gameover;
    }

    void AddScore()
    {
        recorder.AddScore();
    }

    public void Gameover()
    {
        game_over = true;
        patrol_factory.StopPatrol();
        action_manager.DestroyAllAction();
    }

    public void GameWin()
    {
        gameWin = true;
        patrol_factory.StopPatrol();
        action_manager.DestroyAllAction();
    }
}