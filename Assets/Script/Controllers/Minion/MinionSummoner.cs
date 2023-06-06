/// ksPark
/// 
/// 

using UnityEngine;
using UnityEngine.AI;
using Define;
using System.Collections;
using Photon.Pun;

public class MinionSummoner : MonoBehaviourPun
{
    PhotonView _pv; 
    Transform _summonPos;
    public float _summonCycle = 30;
    public float _nowSummonTime = 65;

    void Start()
    {
        //_pv = GetComponent<PhotonView>();
        _summonPos = transform.Find("SummonPos");
    }

    void Update()
    {
        _nowSummonTime -= Time.deltaTime;

        if (_nowSummonTime <= 0)
        {
            StartCoroutine(SummonLine());
            _nowSummonTime = _summonCycle;
        }
    }

    IEnumerator SummonLine()
    {
        for (int i=0; i<3; i++)
        {
            SummonMinion(ObjectType.Melee);
            yield return new WaitForSeconds(0.5f);
        }

        for (int i=0; i<3; i++)
        {
            SummonMinion(ObjectType.Range);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SummonMinion(ObjectType type)
    {
        string objName = LayerMask.LayerToName(gameObject.layer) + type.ToString() + "Robot";

        GameObject upperMinion = PhotonNetwork.Instantiate(objName, this.transform.position, this.transform.rotation);
        upperMinion.transform.position = _summonPos.position + Vector3.forward;
        upperMinion.GetComponent<Minion>().line = ObjectLine.UpperLine;
        upperMinion.GetComponent<NavMeshAgent>().enabled = true;

        GameObject lowerMinion = PhotonNetwork.Instantiate(objName, this.transform.position, this.transform.rotation);
        lowerMinion.transform.position = _summonPos.position + Vector3.back;
        lowerMinion.GetComponent<Minion>().line = ObjectLine.LowerLine;
        lowerMinion.GetComponent<NavMeshAgent>().enabled = true;
    }
}
