using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class check : MonoBehaviour
{
    public Vector3 Point;

    private void Check()
    {
        Debug.Log("check");

        if (Input.GetMouseButtonDown(0))
        {
            // ray로 마우스 위치 world 좌표로 받기.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity))
            {
                Point = raycastHit.point;

                if (transform.position.x - 1 <= Point.x && transform.position.x + 1 >= Point.x &&
                        transform.position.z - 1 <= Point.z && transform.position.z + 1 >= Point.z)
                {
                    Debug.Log("Shoot");
                    GameObject.Find("Player").SendMessage("Shoot", SendMessageOptions.DontRequireReceiver);
                }

            }
        }
    }
}