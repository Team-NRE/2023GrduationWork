using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLookCamera : MonoBehaviour
{
    private Camera m_camera;

    void Start()
    {
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + m_camera.transform.rotation * Vector3.back, m_camera.transform.rotation * Vector3.down);
    }
}
