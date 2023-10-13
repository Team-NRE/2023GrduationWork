/// 
///
/// Particle - HackingMana
///

using UnityEngine;
using TMPro;

public class Particle_HackingMana : MonoBehaviour
{
    // 메인 카메라 Transform
    Transform cam;
    // 하위 캔버스 (Drag&Drop)
    public Canvas canvas;

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        canvas.transform.LookAt(canvas.transform.position + cam.rotation * Vector3.back, cam.rotation * Vector3.up);
    }
    
    void destroySelf()
    {
        Destroy(gameObject);
    }
}
