using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapIcon : MonoBehaviour
{
    private SpriteRenderer m_renderer;
    [SerializeField]
    private Sprite m_iconImage;
    [SerializeField]
    private Color m_iconColor = Color.white;
    [SerializeField]
    private string camp = null;

    private void Awake() 
    {
        m_renderer = GetComponent<SpriteRenderer>();

        Invoke("GetCamp", 0.01f);
        Invoke("SetCampColor", 0.01f);
    }

    private void GetCamp()
    {
        GameObject obj = gameObject;

        while (camp != "Cyborg" && camp != "Human" && camp != "Neutral") 
        {
            obj = obj.transform.parent.gameObject;
            camp = LayerMask.LayerToName(obj.layer);

            if (obj == null) break;
        }
    }

    private void SetCampColor()
    {
        if (camp == "Cyborg") m_iconColor = Color.red;
        if (camp == "Human") m_iconColor = Color.blue;
        if (camp == "Neutral") m_iconColor = Color.white;
        
        m_renderer.sprite = m_iconImage;
        m_renderer.color = m_iconColor;
    }
}
