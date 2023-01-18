using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniMapIcon : MonoBehaviour
{
    private SpriteRenderer m_renderer;
    [SerializeField]
    private Sprite m_iconImage;
    [SerializeField]
    private Color m_iconColor = Color.white;


    private void OnEnable() 
    {
        m_renderer = GetComponent<SpriteRenderer>();
        m_renderer.sprite = m_iconImage;
        m_renderer.color = m_iconColor;
    }
}
