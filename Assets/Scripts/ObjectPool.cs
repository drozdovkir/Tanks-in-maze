using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject object_;
    public int amount;

    private List<GameObject> objects = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newObject = Instantiate(object_);
            newObject.SetActive(false);
            objects.Add(newObject);
        }
    }

    public GameObject GetObject()
    {
        for (int i = 0; i < objects.Count; i++)
            if (!objects[i].activeInHierarchy)
                return objects[i];

        return null;
    }
}