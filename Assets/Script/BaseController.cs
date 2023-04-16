using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BaseController : MonoBehaviour
{   
    private void Start() 
    {
        Init();
    }

    private void FixedUpdate()
    {
    }

    public virtual void Init()
    {
        
    }


    private void OnEnable() {
        // Input System의 "action"을 정의
        var inputActionAsset = Resources.Load<InputActionAsset>("Input");
        InputAction inputAction = inputActionAsset.FindAction("Keyfunction");
        inputAction.performed += OnKeyDown;
        inputAction.Enable();  
    }

    public void OnKeyDown(InputAction.CallbackContext context)
    {
        string name = context.control.name;
        KeyDownAction(name);
    }

    public virtual void KeyDownAction(string name)
    {
        
    }

    
}
