using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Define;

public class UI_KillLog : UI_Base
{
    [Header ("- Child UI")]
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RawImage attackerIcon;
    [SerializeField] RawImage deadUserIcon;

    [Header ("- Icons")]
    [SerializeField] Texture2D[] icons;

    string attackerIconName = "", deadUserIconName = "";
    string attackerTextName = "", deadUserTextName = "";

    public override void Init()
    {
        
    }

    public override void UpdateInit()
    {
		
    }

    public void Init(GameObject attacker, GameObject deadUser)
    {
        SetKillText(attacker, deadUser);
        SetIconName(attacker, deadUser);
        SetIconTexture();        
    }

    private void SetKillText(GameObject attacker, GameObject deadUser)
    {
        // set *attacker* icon name
        if (attacker.tag == "PLAYER")
		{
            BaseController attackerBaseController = attacker.GetComponent<BaseController>();

            if (LayerMask.LayerToName(attacker.layer) == "Cyborg")
            {
                attackerTextName = $"<color=red>{attackerBaseController._pType}</color>";
            }
            else
            {
                attackerTextName = $"<color=blue>{attackerBaseController._pType}</color>";
            }
        }
        else
        {
            ObjectController attackerObjectController = attacker.GetComponent<ObjectController>();

            if (LayerMask.LayerToName(attacker.layer) == "Cyborg")
            {
                attackerTextName = $"<color=red>{attackerObjectController._type}</color>";
            }
            else
            {
                attackerTextName = $"<color=blue>{attackerObjectController._type}</color>";
            }
        }

        // set *deadUser* icon name
        BaseController deadUserBaseController = deadUser.GetComponent<BaseController>();

        if (LayerMask.LayerToName(deadUser.layer) == "Cyborg")
        {
            deadUserTextName = $"<color=red>{deadUserBaseController._pType}</color>";
        }
        else
        {
            deadUserTextName = $"<color=blue>{deadUserBaseController._pType}</color>";
        }

        text.text = $"{attackerTextName}  killed  {deadUserTextName}";
    }

    private void SetIconName(GameObject attacker, GameObject deadUser)
    {
        // set *attacker* icon name
        if (attacker.tag == "PLAYER")
		{
            BaseController attackerBaseController = attacker.GetComponent<BaseController>();
            attackerIconName = $"{LayerMask.LayerToName(attacker.layer)}Icon_{attackerBaseController._pType}";
        }
        else
        {
            ObjectController attackerObjectController = attacker.GetComponent<ObjectController>();
            attackerIconName = $"{LayerMask.LayerToName(attacker.layer)}Icon_{attackerObjectController._type}";
        }

        // set *deadUser* icon name
        BaseController deadUserBaseController = deadUser.GetComponent<BaseController>();
        deadUserIconName = $"{LayerMask.LayerToName(deadUser.layer)}Icon_{deadUserBaseController._pType}";
    }

    private void SetIconTexture()
    {
        for (int i=0; i<icons.Length; i++)
        {
            if (icons[i].name == attackerIconName) attackerIcon.texture = icons[i];
            if (icons[i].name == deadUserIconName) deadUserIcon.texture = icons[i];
        }
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
