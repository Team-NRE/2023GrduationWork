using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniMap : MonoBehaviour
{
    [Header ("- Camera Setting")]
    [SerializeField]
    private Camera m_camera;
    [SerializeField]
    private Vector3 m_position = new Vector3(0, 65, 9);
    [SerializeField]
    private Quaternion m_rotation = Quaternion.Euler(90, 0, 0);    
    [Space(30.0f)]
    [SerializeField]
    private bool m_haveBackground = true;
    [SerializeField]
    private Color m_backgroundColor = Color.black;

    enum SizeType {absolute, relative}
    enum LocationType {
        Top_Left, Top_Center, Top_Right,
        Middle_Left, Middle_Center, Middle_Right,
        Bottom_Left, Bottom_Center, Bottom_Right
    }
    [Header ("- UI Setting")]
    [SerializeField]
    private SizeType m_sizeType = SizeType.relative;
    [SerializeField]
    private Vector2 m_size = new Vector2(0.5f, 0.5f);
    [Space(10.0f)]
    [SerializeField]
    private LocationType m_location = LocationType.Bottom_Right;
    [SerializeField]
    private float m_margin = 0;

    void OnEnable() 
    {
        /* 
        *   Initial Settings
        */

        m_camera = GetComponentInChildren<Camera>();
        
        backgroundSetting();
        transformSetting();
        sizeLocationSetting();
        
    }

    private void backgroundSetting()
    {
        m_camera.clearFlags = (m_haveBackground) ? CameraClearFlags.SolidColor : CameraClearFlags.Depth;
        m_camera.backgroundColor = m_backgroundColor;
    }

    private void transformSetting()
    {
        m_camera.gameObject.transform.position = m_position;
        m_camera.gameObject.transform.rotation = m_rotation;
    }

    private void sizeLocationSetting()
    {
        Rect m_viewportRect = new Rect();

        if (m_sizeType == SizeType.absolute)
        {
            m_viewportRect.width  = m_size.x / Screen.width;
            m_viewportRect.height = m_size.y / Screen.height;
        }
        else if (m_sizeType == SizeType.relative)
        {
            m_viewportRect.width  = m_size.x;
            m_viewportRect.height = m_size.y;
        }

        switch (m_location)
        {
            case LocationType.Top_Left:
            case LocationType.Middle_Left:
            case LocationType.Bottom_Left:
                m_viewportRect.x = 0 + m_margin / Screen.width;
                break;
            case LocationType.Top_Center:
            case LocationType.Middle_Center:
            case LocationType.Bottom_Center:
                m_viewportRect.x = (1f - m_viewportRect.width) / 2;
                break;
            case LocationType.Top_Right:
            case LocationType.Middle_Right:
            case LocationType.Bottom_Right:
                m_viewportRect.x = 1f - m_viewportRect.width - m_margin / Screen.width;
                break;
        }

        switch (m_location)
        {
            case LocationType.Top_Left:
            case LocationType.Top_Center:
            case LocationType.Top_Right:
                m_viewportRect.y = 1f - m_viewportRect.height - m_margin / Screen.height;
                break;
            case LocationType.Middle_Left:
            case LocationType.Middle_Center:
            case LocationType.Middle_Right:
                m_viewportRect.y = (1f - m_viewportRect.height) / 2;
                break;
            case LocationType.Bottom_Left:
            case LocationType.Bottom_Center:
            case LocationType.Bottom_Right:
                m_viewportRect.y = 0 + m_margin / Screen.height;
                break;
        }

        m_camera.rect = m_viewportRect;
    }
}
