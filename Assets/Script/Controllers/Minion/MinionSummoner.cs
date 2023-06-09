/// ksPark
/// 
/// 

using UnityEngine;
using UnityEngine.AI;
using Define;
using System.Collections;

public class MinionSummoner : MonoBehaviour
{
    Transform _summonPos;
    public float _summonCycle = 30;
    public float _nowSummonTime = 65;

    Coroutine summonCoroutine;

    void Start()
    {
        _summonPos = this.transform.Find("SummonPos");
    }

    void OnDisable()
    {

        if (summonCoroutine != null)
            StopCoroutine(summonCoroutine);
    }

    void Update()
    {
        _nowSummonTime -= Time.deltaTime;

        if (_nowSummonTime <= 0)
        {
            summonCoroutine = StartCoroutine(SummonLine());
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

        GameObject obj = Managers.Resource.Load<GameObject>(objName);

        GameObject upperMinion = Instantiate(obj, _summonPos.position + Vector3.forward, this.transform.rotation);
        upperMinion.GetComponent<Minion>().line = ObjectLine.UpperLine;
        upperMinion.GetComponent<NavMeshAgent>().enabled = true;

        GameObject lowerMinion = Instantiate(obj, _summonPos.position + Vector3.back, this.transform.rotation);
        lowerMinion.GetComponent<Minion>().line = ObjectLine.LowerLine;
        lowerMinion.GetComponent<NavMeshAgent>().enabled = true;
    }
}
