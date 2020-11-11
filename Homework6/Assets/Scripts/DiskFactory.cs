using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour
{
    private List<DiskData> used;
    private List<DiskData> free;

    void Start()
    {
        used = new List<DiskData>();
        free = new List<DiskData>();
    }

    public GameObject GetDisk(int type)
    {
        GameObject disk = null;
        if (free.Count > 0)
        {
            for (int i = 0; i < free.Count; i++)
            {
                if (free[i].type == type)
                {
                    disk = free[i].gameObject;
                    free.Remove(free[i]);
                    break;
                }
            }
        }

        float random_y = Random.Range(-3f, 1f);
        float random_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
        if (disk == null)
            disk = Instantiate(Resources.Load<GameObject>("Prefabs/disk"+type), new Vector3(0, -20f, 0), Quaternion.identity);
        disk.transform.position = new Vector3(random_x * 20f, random_y, 0);
        disk.GetComponent<Renderer>().material.color = disk.GetComponent<DiskData>().color;
        used.Add(disk.GetComponent<DiskData>());
        disk.SetActive(true);
        return disk;
    }

    public void FreeDisk(GameObject disk)
    {
        for (int i = 0; i < used.Count; ++i)
        {
            if (disk.GetInstanceID() == used[i].gameObject.GetInstanceID())
            {
                used[i].gameObject.SetActive(false);
                used.Remove(used[i]);
                free.Add(used[i]);
                break;
            }
        }
    }

    public void Reset()
    {
        for (int i = 0; i < used.Count; i++)
        {
            if (used[i].gameObject.transform.position.y <= -20f)
            {
                free.Add(used[i]);
                used.Remove(used[i]);
            }
        }
    }
}
