/// ksPark
///
///

using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

[RequireComponent(typeof(LineRenderer))]

public class NavMeshPathRenderer : MonoBehaviour
{
    PhotonView pv;
    NavMeshAgent m_NavMeshAgent;
    LineRenderer m_LineRenderer;

    void Awake()
    {
        pv             = GetComponentInParent<PhotonView>();
        m_NavMeshAgent = GetComponentInParent<NavMeshAgent>();
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        if (!pv.IsMine) return;
        if (!m_NavMeshAgent.enabled) return;
        if (!m_LineRenderer.enabled) return;
        SetLine();
    }

    void SetLine()
    {
        if (m_NavMeshAgent.isStopped)
        {
            if (m_LineRenderer.positionCount <= 0) return;
            m_LineRenderer.positionCount = 0;
            Debug.Log("!!");
        }
        else
        {
            DrawPath();
        }
    }

    void DrawPath()
    {
        if (m_NavMeshAgent.path.corners.Length.Equals(0)) return;

        m_LineRenderer.positionCount = m_NavMeshAgent.path.corners.Length;

        for (int i=0; i<m_LineRenderer.positionCount; i++)
        {
            m_LineRenderer.SetPosition(i, m_NavMeshAgent.path.corners[i] + Vector3.up * 11);
        }
    }
}
