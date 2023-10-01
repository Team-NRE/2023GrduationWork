/// ksPark
///
///

using UnityEngine;

public class UI_CanvasFader : MonoBehaviour
{
    [SerializeField] private KeyCode fadeToggleKey = KeyCode.U;
    [SerializeField] private float tweenSpeed = 1f;
    
    [SerializeField] private bool isTweening = false;
    private float currentAlpha = 0f;
    private float targetAlpha = .01f;
    private CanvasGroup canvasGroup;
    private bool hideUiButtonTweenNotNull;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup.alpha == 0) HideUI();
        else ShowUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(fadeToggleKey)) TurnUI();
        
        gameObject.SetActive(currentAlpha + targetAlpha != 0);
        if(!isTweening) return;
        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, Time.unscaledDeltaTime * tweenSpeed);
        canvasGroup.alpha = currentAlpha;
        if(targetAlpha == currentAlpha) isTweening = false;
    }

    public void HideUI()
    {
        MakeCanvasInvisibleTween();
        gameObject.SetActive(currentAlpha + targetAlpha != 0);
    }

    public void ShowUI()
    {
        MakeCanvasVisibleTween();
        gameObject.SetActive(currentAlpha + targetAlpha != 0);
    }

    public void TurnUI()
    {
        if(currentAlpha < 0.01f) MakeCanvasVisibleTween();
        else MakeCanvasInvisibleTween();
        gameObject.SetActive(currentAlpha + targetAlpha != 0);
    }

    private void MakeCanvasVisibleTween()
    {
        if (targetAlpha == 1f) return;
        isTweening = true;
        targetAlpha = 1f;
        Managers.UI.isOpenedPopup = true;
    }

    private void MakeCanvasInvisibleTween()
    {
        if (targetAlpha == 0f) return;
        isTweening = true;
        targetAlpha = 0f;
        Managers.UI.isOpenedPopup = false;
    }
}