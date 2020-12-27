using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventClick : MonoBehaviour
{
    public void OnPointerClick()
    {
        SceneManager.LoadScene("Scenes/Detail");
    }
}
