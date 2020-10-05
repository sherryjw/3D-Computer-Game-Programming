using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public Transform Sun;
    public Transform Mercury;
    public Transform Venus;
    public Transform Earth;
    public Transform Mars;
    public Transform Jupiter;
    public Transform Saturn;
    public Transform Uranus;
    public Transform Neptune;
    // Start is called before the first frame update
    void Start()
    {
        Sun.position = Vector3.zero;
        Mercury.position = new Vector3(0.9f, 0, 0);
        Venus.position = new Vector3(1.5f, 0, 0);
        Earth.position = new Vector3(2.2f, 0, 0);
        Mars.position = new Vector3(3.0f, 0, 0);
        Jupiter.position = new Vector3(4.0f, 0, 0);
        Saturn.position = new Vector3(5.2f, 0, 0);
        Uranus.position = new Vector3(6.5f, 0, 0);
        Neptune.position = new Vector3(7.8f, 0, 0);
        Sun.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        Mercury.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        Venus.localScale -= new Vector3(0.4f, 0.4f, 0.4f);
        Earth.localScale -= new Vector3(0.4f, 0.4f, 0.4f);
        Mars.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        Saturn.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
        Uranus.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
        Neptune.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        Mercury.RotateAround(Sun.position, new Vector3(0, 1, 2), 48 * Time.deltaTime);
        Mercury.Rotate(Vector3.up * 30 * Time.deltaTime);
        Venus.RotateAround(Sun.position, new Vector3(0, 1, -1), 35 * Time.deltaTime);
        Venus.Rotate(Vector3.up * 30 * Time.deltaTime);
        Earth.RotateAround(Sun.position, Vector3.up, 30 * Time.deltaTime);
        Earth.Rotate(Vector3.up * 30 * Time.deltaTime);
        Mars.RotateAround(Sun.position, new Vector3(0, 1, 1), 24 * Time.deltaTime);
        Mars.Rotate(Vector3.up * 30 * Time.deltaTime);
        Jupiter.RotateAround(Sun.position, new Vector3(0, 5, 1), 13 * Time.deltaTime);
        Jupiter.Rotate(Vector3.up * 30 * Time.deltaTime);
        Saturn.RotateAround(Sun.position, new Vector3(0, 4, 1), 10 * Time.deltaTime);
        Saturn.Rotate(Vector3.up * 30 * Time.deltaTime);
        Uranus.RotateAround(Sun.position, new Vector3(0, 6, 1), 7 * Time.deltaTime);
        Uranus.Rotate(Vector3.up * 30 * Time.deltaTime);
        Neptune.RotateAround(Sun.position, new Vector3(0, 2, 1), 5 * Time.deltaTime);
        Neptune.Rotate(Vector3.up * 30 * Time.deltaTime);
    }
}
