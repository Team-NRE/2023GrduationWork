using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSummon : MonoBehaviour
{
    [Header ("- Stats Script")]
    [SerializeField] ObjStats stats;

    [Header ("- Summon List")]
    [SerializeField] GameObject[] summonList;
    [SerializeField] GameObject[] summonPos;
    [SerializeField] GameObject[] milestone;

    [Header ("- Variable")]
    [SerializeField] float summonTerm;

    void Start()
    {
        stats = GetComponent<ObjStats>();

        summonTerm = 0.5f;
    }

    public void Summon(string camp)
    {
        if (stats.AttackCoolingTime > 0)
        {
            stats.AttackCoolingTime -= Time.deltaTime;
        }
        else
        {
            StartCoroutine(SummonObject(camp));
            stats.AttackCoolingTime = stats.AttackSpeed;
        }
    }

    IEnumerator SummonObject(string camp)
    {
        for (int i=0; i<summonList.Length; i++)
        {
            for (int j=0; j<summonPos.Length; j++)
            {
                GameObject _Object = Instantiate(summonList[i], summonPos[j].transform.position, Quaternion.identity);
                
                _Object.layer = LayerMask.NameToLayer(camp);

                _Object.GetComponent<ObjController>().camp = camp;

                MinionMove moveScript = _Object.GetComponent<MinionMove>();
                moveScript.milestone = milestone[j];
                moveScript.enemyCamp = GameObject.Find((camp == "Human") ? "Cyborg Camp" : "Human Camp");
            }

            yield return new WaitForSeconds(summonTerm);
        }
    }
}
