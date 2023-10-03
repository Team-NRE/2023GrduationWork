/// ksPark
///
/// Particle - Coin Drop
///

using UnityEngine;
using TMPro;

public class Particle_CoinDrop : MonoBehaviour
{
    // 메인 카메라 Transform
    Transform cam;
    // 하위 캔버스 (Drag&Drop)
    public Canvas canvas;
    // 코인 텍스트 (Drag&Drop)
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        cam = Camera.main.transform;
        Managers.Sound.Play("CoinDrop", Define.Sound.Effect, 1, 1.5f);
    }

    private void Update()
    {
        canvas.transform.LookAt(canvas.transform.position + cam.rotation * Vector3.back, cam.rotation * Vector3.up);
    }

    public void setInit(string value)
    {
        coinText.text = "+" + value;
    }
    
    void destroySelf()
    {
        Destroy(gameObject);
    }
}
