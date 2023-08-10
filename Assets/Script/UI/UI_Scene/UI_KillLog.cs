using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_KillLog : UI_Base
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RawImage attackerIcon;
    [SerializeField] RawImage deadUserIcon;

    public override void Init()
    {
        
    }

    public override void UpdateInit()
    {
		
    }

    public void Init(string attackerNickname, string deadUserNickname)
    {
        text.text = $"{attackerNickname}  Killed  {deadUserNickname}";
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
