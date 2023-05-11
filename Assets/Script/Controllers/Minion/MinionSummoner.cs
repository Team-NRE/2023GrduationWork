using UnityEngine;
using UnityEngine.AI;
using Define;
using System.Collections;

public class MinionSummoner : MonoBehaviour
{
    Transform _summonPos;
    public float _summonCycle = 30;
    public float _nowSummonTime = 65;

    void Start()
    {
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
        Debug.Log("Start Summon");
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

        var upperMinion = Managers.Pool.Pop(objName);
        upperMinion.transform.position = _summonPos.position + Vector3.forward;
        upperMinion.GetComponent<Minion>().line = ObjectLine.UpperLine;
        upperMinion.GetComponent<NavMeshAgent>().enabled = true;

        var lowerMinion = Managers.Pool.Pop(objName);
        lowerMinion.transform.position = _summonPos.position + Vector3.back;
        lowerMinion.GetComponent<Minion>().line = ObjectLine.LowerLine;
        lowerMinion.GetComponent<NavMeshAgent>().enabled = true;

        Debug.Log(_summonPos.position);
    }
}
