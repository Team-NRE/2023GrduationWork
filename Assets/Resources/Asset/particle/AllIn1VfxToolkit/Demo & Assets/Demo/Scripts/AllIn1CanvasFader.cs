using UnityEngine;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    public class AllIn1CanvasFader : MonoBehaviour
    {
        [SerializeField] private KeyCode fadeToggleKey = KeyCode.U;
        [SerializeField] private float tweenSpeed = 1f;
        [SerializeField] private AllIn1DemoScaleTween hideUiButtonTween;
        
        [SerializeField] private bool isTweening = false;
        private float currentAlpha = 0f;
        private float targetAlpha = .01f;
        private CanvasGroup canvasGroup;
        private bool hideUiButtonTweenNotNull;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            hideUiButtonTweenNotNull = hideUiButtonTween != null;
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
            if(hideUiButtonTweenNotNull) hideUiButtonTween.ScaleUpTween();
            gameObject.SetActive(currentAlpha + targetAlpha != 0);
        }

        public void ShowUI()
        {
            MakeCanvasVisibleTween();
            if(hideUiButtonTweenNotNull) hideUiButtonTween.ScaleUpTween();
            gameObject.SetActive(currentAlpha + targetAlpha != 0);
        }

        public void TurnUI()
        {
            if(currentAlpha < 0.01f) MakeCanvasVisibleTween();
            else MakeCanvasInvisibleTween();
            if(hideUiButtonTweenNotNull) hideUiButtonTween.ScaleUpTween();
            gameObject.SetActive(currentAlpha + targetAlpha != 0);
        }

        private void MakeCanvasVisibleTween()
        {
            isTweening = true;
            targetAlpha = 1f;
        }

        private void MakeCanvasInvisibleTween()
        {
            isTweening = true;
            targetAlpha = 0f;
        }
    }
}