using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Text text;
    private Button button;
    public RectTransform content;

    private float timer = 50.0f;
    private float oriHeight;

    void Start()
    {
        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(Click);

        oriHeight = text.rectTransform.sizeDelta.y;
    }

    void Click()
    {
        if (text.gameObject.activeSelf)
        {
            StartCoroutine(fold());
        }
        else
        {
            StartCoroutine(unfold());
        }
    }

    IEnumerator fold()
    {
        float rx = 0;
        float curHeight = oriHeight;
        float curContHeight = content.sizeDelta.y;

        for (int i = 0; i < timer; ++i)
        {
            rx += 90.0f / timer;
            curHeight -= oriHeight / timer;
            curContHeight -= oriHeight / timer;

            text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, curHeight);
            text.transform.rotation = Quaternion.Euler(rx, 0, 0);
            content.sizeDelta = new Vector2(content.sizeDelta.x, curContHeight);

            yield return null;
        }
        text.gameObject.SetActive(false);
    }

    IEnumerator unfold()
    {
        text.gameObject.SetActive(true);
        float rx = 90;
        float curHeight = 0;
        float curContHeight = content.sizeDelta.y;

        for (int i = 0; i < timer; ++i)
        {
            rx -= 90.0f / timer;
            curHeight += oriHeight / timer;
            curContHeight += oriHeight / timer;

            text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, curHeight);
            text.transform.rotation = Quaternion.Euler(rx, 0, 0);
            content.sizeDelta = new Vector2(content.sizeDelta.x, curContHeight);

            yield return null;
        }
    }
}