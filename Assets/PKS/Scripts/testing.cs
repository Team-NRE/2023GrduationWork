using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class testing : MonoBehaviour
{
    public Tilemap t;

    void Update()
    {
        Debug.Log(t.GetTile(t.WorldToCell(transform.position)));
    }
}
