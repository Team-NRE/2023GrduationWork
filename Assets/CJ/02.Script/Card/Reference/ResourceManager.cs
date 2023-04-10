using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    //Object Pool & Load from Resources Dir
    /*public T Load<T>(string path) where T : Object
    {
        if(typeof(T) == typeof(GameObject))
        {
            string name = path;

            int index = name.LastIndexOf("/");
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go; = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }


    //Check poolable & Instantiate
    public GameObject Instantiate(string path, Transform parent = null)
    {
        //CheckPooling
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if(original == null)
        {
            Debug.Log($"Failed to Load prefab : {path}");
            return null;
        }

        //Not Pooling Object
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
        //Actual spawn on GameManager
    }


    //Check poolable & Destroy or SetActive=false
    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            Debug.Log($"{go} is null!!");
            return;
        }

        Poolable poolable = go.GetComponent<Poolable>();
        if(poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }*/
}
