using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchHandler : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(Click);
    }

    void Click()
    {
        SceneManager.LoadScene("Scenes/Main");
    }
}
